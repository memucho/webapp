using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using TrueOrFalse.Search;

public class QuestionRepo : RepositoryDbBase<Question>
{
    private readonly SearchIndexQuestion _searchIndexQuestion;

    public QuestionRepo(ISession session, SearchIndexQuestion searchIndexQuestion) : base(session)
    {
        _searchIndexQuestion = searchIndexQuestion;
    }

    public void UpdateFieldsOnly(Question question)
    {
        base.Update(question);
    }

    public new void Update(Question question)
    {
        var categoriesIds = _session
            .CreateSQLQuery("SELECT Category_id FROM categories_to_questions WHERE Question_id =" + question.Id)
            .List<int>();

        var query = "SELECT Category_id FROM reference WHERE Question_id=" + question.Id + " AND Category_id is not null";

        var categoriesReferences = _session
            .CreateSQLQuery(query)
            .List<int>();

        var categoriesBeforeUpdateIds = categoriesIds.Union(categoriesReferences);

        _searchIndexQuestion.Update(question);
        base.Update(question);
        Flush();

        var categoriesToUpdateIds = categoriesBeforeUpdateIds
            .Union(question.Categories.Select(c => c.Id))
            .Union(question.References.Where(r => r.Category != null)
            .Select(r => r.Category.Id))
            .Distinct()
            .ToList(); //All categories added or removed have to be updated

        Sl.Resolve<UpdateQuestionCountForCategory>().Run(categoriesToUpdateIds);

        EntityCache.AddOrUpdate(question, categoriesToUpdateIds);

        var aggregatedCategoriesToUpdate =
            CategoryAggregation.GetAggregatingAncestors(Sl.CategoryRepo.GetByIds(categoriesToUpdateIds));

        foreach (var category in aggregatedCategoriesToUpdate)
        {
            category.UpdateCountQuestionsAggregated();
            Sl.CategoryRepo.Update(category);
            KnowledgeSummaryUpdate.ScheduleForCategory(category.Id);
        }
    }

    public override void Create(Question question)
    {
        if (question.Creator == null)
            throw new Exception("no creator");

        base.Create(question);
        Flush();
        Sl.R<UpdateQuestionCountForCategory>().Run(question.Categories);
        foreach (var category in question.Categories.ToList())
        {
            category.UpdateCountQuestionsAggregated();
            Sl.CategoryRepo.Update(category);
            KnowledgeSummaryUpdate.ScheduleForCategory(category.Id);
        }
        if (question.Visibility != QuestionVisibility.Owner)
        {
            UserActivityAdd.CreatedQuestion(question);
            ReputationUpdate.ForUser(question.Creator);
        }
        _searchIndexQuestion.Update(question);
        EntityCache.AddOrUpdate(question);
    }

    public override void Delete(Question question)
    {
        _searchIndexQuestion.Delete(question);
        base.Delete(question);
        EntityCache.Remove(question);
        UserValuationCache.RemoveAllForQuestion(question.Id);
    }

    public IList<Question> GetForCategoryAggregated(int categoryId, int currentUser, int resultCount = -1)
    {
        var category = Sl.CategoryRepo.GetById(categoryId);

        return category.GetAggregatedQuestionsFromMemoryCache();
    }

    public IList<Question> GetForCategoryFromMemoryCache(int categoryId)
    {
        return EntityCache.GetQuestionsForCategory(categoryId);
    }

    public IList<Question> GetForCategory(int categoryId, int currentUser, int resultCount= -1) => 
        GetForCategory(new List<int> {categoryId}, currentUser, resultCount);

    public IList<Question> GetForCategory(IEnumerable<int> categoryIds, int currentUser, int resultCount = -1)
    {
        var query = _session.QueryOver<Question>()
            .OrderBy(q => q.TotalRelevancePersonalEntries).Desc
            .ThenBy(x => x.DateCreated).Desc
            .Where(q => q.Visibility == QuestionVisibility.All || q.Creator.Id == currentUser)
            .JoinQueryOver<Category>(q => q.Categories)
            .Where(Restrictions.In("Id", categoryIds.ToArray()));

        if (resultCount > -1)
            query.Take(resultCount);

        return query.List<Question>();
    }

    public IList<Question> GetForCategory(int categoryId)
    {
        return _session.QueryOver<Question>()
            .Where(q => q.Visibility == QuestionVisibility.All)
            .JoinQueryOver<Category>(q => q.Categories)
            .Where(c => c.Id == categoryId)
            .List<Question>();
    }

    public IList<Question> GetForReference(int categoryId, int currentUser, int resultCount = -1)
    {
        var query = _session.QueryOver<Question>()
            .OrderBy(q => q.TotalRelevancePersonalEntries)
            .Desc
            .ThenBy(q => q.DateCreated)
            .Desc
            .Where(q => q.Visibility == QuestionVisibility.All || q.Creator.Id == currentUser)
            .JoinQueryOver<Reference>(q => q.References)
            .Where(q => q.Category.Id == categoryId);

        if (resultCount > -1)
            query.Take(resultCount);

        return query.List<Question>();
    }

    public PagedResult<Question> GetForCategoryAndInWishCount(int categoryId, int userId, int resultCount)
    {
        var query = _session.QueryOver<QuestionValuation>()
            .Where(q =>
                q.RelevancePersonal != -1 &&
                q.User.Id == userId)
            .JoinQueryOver(q => q.Question)
            .OrderBy(q => q.TotalRelevancePersonalEntries).Desc
            .ThenBy(x => x.DateCreated).Desc
            .JoinQueryOver<Category>(q => q.Categories)
            .Where(c => c.Id == categoryId);

        return new PagedResult<Question>
        {
            PageSize = resultCount,
            Total = query.RowCount(),
            Items = query
                .Take(resultCount)
                .List<QuestionValuation>()
                .Select(qv => qv.Question)
                .ToList()
        };
    }

    public Question GetByIdFromMemoryCache(int questionId) => EntityCache.GetQuestionById(questionId);

    public IList<Question> GetByIds(List<int> questionIds) => 
        GetByIds(questionIds.ToArray());

    public override IList<Question> GetByIds(params int[] questionIds)
    {
        var questions = _session
            .QueryOver<Question>()
            .Fetch(q => q.Categories).Eager
            .Where(Restrictions.In("Id", questionIds))
            .List()
            .Distinct()
            .ToList();

        return questionIds
            .Select(questionId => questions.FirstOrDefault(question => question.Id == questionId))
            .Where(q => q != null)
            .ToList();
    }

    public IList<int> GetByKnowledge(
        int userId, 
        bool isKnowledgeSolidFilter, 
        bool isKnowledgeShouldConsolidateFilter,
        bool isKnowledgeShouldLearnFilter,
        bool isKnowledgeNoneFilter)
    {
        var query = _session
            .QueryOver<QuestionValuation>()
            .Where(q => 
                q.User.Id == userId &&
                q.RelevancePersonal != -1);

        AbstractCriterion knowledgeExpression = null;
        Func<KnowledgeStatus, AbstractCriterion> fnGetCombinedExpression = knowledgeStatus => {
            var knowledgeExpressionToAdd = Restrictions.Eq("KnowledgeStatus", knowledgeStatus);
            return knowledgeExpression != null 
                ? Restrictions.Or(knowledgeExpression, knowledgeExpressionToAdd) 
                : knowledgeExpressionToAdd;
        };

        if (isKnowledgeSolidFilter)
            knowledgeExpression = fnGetCombinedExpression(KnowledgeStatus.Solid);

        if (isKnowledgeShouldConsolidateFilter)
            knowledgeExpression = fnGetCombinedExpression(KnowledgeStatus.NeedsConsolidation);

        if (isKnowledgeShouldLearnFilter)
            knowledgeExpression = fnGetCombinedExpression(KnowledgeStatus.NeedsLearning);

        if (isKnowledgeNoneFilter)
            knowledgeExpression = fnGetCombinedExpression(KnowledgeStatus.NotLearned);

        if (knowledgeExpression != null)
            query.And(knowledgeExpression);

        return query
            .JoinQueryOver(q => q.Question)
            .Select(q => q.Question.Id)
            .List<int>();
    }

    public IEnumerable<Question> GetMostRecent(int amount, QuestionVisibility questionVisibility = QuestionVisibility.All)
    {
        return _session
            .QueryOver<Question>()
            .Where(q => q.Visibility == questionVisibility)
            .OrderBy(q => q.DateCreated).Desc
            .Take(amount)
            .List();
    }

    public IList<Question> GetMostViewed(int amount)
    {
        return _session
            .QueryOver<Question>()
            .OrderBy(q => q.TotalViews).Desc
            .Take(amount)
            .List();
    }

    /// <summary>
    /// Return how often a question is in other peoples WuWi
    /// </summary>
    public int HowOftenInOtherPeoplesWuwi(int userId, int questionId)
    {
        return Sl.R<QuestionValuationRepo>()
            .Query
            .Where(v =>
                v.User.Id != userId &&
                v.Question.Id == questionId &&
                v.RelevancePersonal > -1
            )
            .RowCount();
    }

    /// <summary>
    /// Return how often a question is part of a future date
    /// </summary>
    public int HowOftenInFutureDate(int questionId)
    {
        var query = "SELECT COUNT(*) FROM " +
                    "(SELECT qis.Set_id, dts.Date_id, d.User_id, d.DateTime FROM questioninset as qis LEFT JOIN question as q ON qis.Question_id = q.Id " +
                    "LEFT JOIN date_to_sets as dts ON dts.Set_id = qis.Set_id " +
                    "LEFT JOIN date as d ON d.Id = dts.Date_id " +
                    "WHERE qis.Question_id = {0} AND d.DateTime > NOW() GROUP BY dts.Date_id) " +
                    "AS c; ";
        return (int)Sl.R<ISession>()
            .CreateSQLQuery(String.Format(query, questionId))
            .UniqueResult<long>();

        //Sl.R<ISession>()
        //    .CreateSQLQuery(
        //        "SELECT COUNT(*) FROM " +
        //        "(SELECT qis.Set_id, dts.Date_id, d.User_id, d.DateTime FROM questioninset as qis LEFT JOIN question as q ON qis.Question_id = q.Id " +
        //        "LEFT JOIN date_to_sets as dts ON dts.Set_id = qis.Set_id " +
        //        "LEFT JOIN date as d ON d.Id = dts.Date_id " +
        //        "WHERE qis.Question_id = :questionId AND d.DateTime > NOW() GROUP BY dts.Date_id) " +
        //        "AS c; ")
        //    .SetParameter("questionId", questionId)
        //    .ExecuteUpdate();
    }

    public int HowManyNewPublicQuestionsCreatedSince(DateTime since)
    {
        return _session.QueryOver<Question>()
            .Where(q => q.DateCreated > since)
            .And(q => q.Visibility == QuestionVisibility.All)
            .RowCount();
    }

    public int TotalPublicQuestionCount()
    {
        return _session.QueryOver<Question>()
            .Where(q => q.Visibility == QuestionVisibility.All)
            .RowCount();
    }

    public int GetPercentageQuestionsAnsweredMostlyWrong()
    {
        int moreWrongThanRight = _session
            .Query<Question>()
            .Count(q => q.TotalFalseAnswers + q.TotalTrueAnswers > 0 && q.TotalFalseAnswers >= q.TotalTrueAnswers);
        int moreRightThanWrong = _session
            .Query<Question>()
            .Count(q => q.TotalFalseAnswers < q.TotalTrueAnswers);
        return (100 * moreWrongThanRight / (moreRightThanWrong + moreWrongThanRight));
    }
}