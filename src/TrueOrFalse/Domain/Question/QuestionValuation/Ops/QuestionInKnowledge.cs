using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using TrueOrFalse;

public static class QuestionInKnowledge
{
    public static void Pin(int questionId, User user)
    {
        UpdateRelevancePersonal(questionId, user);
    }

    public static void Pin(IEnumerable<Question> questions, User user, SaveType saveType = SaveType.CacheAndDatabase)
    {
        UpdateRelevancePersonal(questions.ToList(), user, 50, saveType);
    }

    public static void Unpin(int questionId, User user, SaveType saveType = SaveType.CacheAndDatabase)
    {
        UpdateRelevancePersonal(questionId, user, -1, saveType);
    }

    public static void Create(QuestionValuation questionValuation)
    {
        Sl.Resolve<QuestionValuationRepo>().CreateOrUpdate(questionValuation);

        var sb = new StringBuilder();

        sb.Append(GenerateQualityQuery(questionValuation.Question.Id));
        sb.Append(GenerateRelevanceAllQuery(questionValuation.Question.Id));

        sb.Append(GenerateEntriesQuery("TotalRelevancePersonal", "RelevancePersonal", questionValuation.Question.Id));
        sb.Append(GenerateAvgQuery("TotalRelevancePersonal", "RelevancePersonal", questionValuation.Question.Id));
        
        var session = Sl.Resolve<ISession>();
        session.CreateSQLQuery(sb.ToString()).ExecuteUpdate();
        session.Flush();
    }

    private static void ChangeTotalInOthersWishknowledge( bool isIncrement, User user, Question question)
    {
        if (question.Creator == null || question.Creator.Id == user.Id) 
            return; 
           
        var sign = isIncrement ? "+" : "-" ;
        
                Sl.Resolve<ISession>()
                    .CreateSQLQuery(
                @"Update user Set TotalInOthersWishknowledge = TotalInOthersWishknowledge " + sign + " 1 where id = " +
                question.Creator.Id + ";")
                    .ExecuteUpdate();
    }

    private static void UpdateRelevancePersonal(IList<Question> questions, User user, int relevance = 50, SaveType saveType = SaveType.CacheAndDatabase)
    {
        var questionValuations = Sl.QuestionValuationRepo.GetByQuestionIds(questions.GetIds(), user.Id);

        foreach (var question in questions)
        {
            CreateOrUpdateValuation(question, questionValuations.ByQuestionId(question.Id), user, relevance, saveType);
            ChangeTotalInOthersWishknowledge(relevance==50, user, question);

            if (saveType == SaveType.DatabaseOnly || saveType == SaveType.CacheAndDatabase)
                Sl.Session.CreateSQLQuery(GenerateRelevancePersonal(question.Id)).ExecuteUpdate();

            ProbabilityUpdate_Valuation.Run(question, user, saveType);
        }
        UpdateTotalRelevancePersonalInCache(questions);

        if (saveType != SaveType.DatabaseOnly)
            SetUserWishCountQuestions(user);

        var creatorGroups = questions.Select(q => q.Creator).GroupBy(c => c.Id);
        foreach (var creator in creatorGroups)
            ReputationUpdate.ForUser(creator.First());
    }

    private static void UpdateRelevancePersonal(int questionId, User user, int relevance = 50, SaveType saveType = SaveType.CacheAndDatabase)
    {
        var question = EntityCache.GetQuestionById(questionId);
        ChangeTotalInOthersWishknowledge(relevance == 50, user, question);
        CreateOrUpdateValuation(questionId, user, relevance, saveType);

        if(saveType == SaveType.CacheOnly || saveType == SaveType.CacheAndDatabase)
            SetUserWishCountQuestions(user);

        if (saveType == SaveType.DatabaseOnly || saveType == SaveType.CacheAndDatabase)
        {
            var session = Sl.Resolve<ISession>();
            session.CreateSQLQuery(GenerateRelevancePersonal(questionId)).ExecuteUpdate();
            session.Flush();

            ReputationUpdate.ForQuestion(questionId);

            if (relevance != -1)
                ProbabilityUpdate_Valuation.Run(questionId, user.Id);
        }
    }

    public static void SetUserWishCountQuestions(User user)
    {
        var query =
            $@"
            UPDATE user 
            SET WishCountQuestions =
                (SELECT count(id)
                FROM QuestionValuation
                WHERE userId = {user.Id}
                AND RelevancePersonal > 0) 
            WHERE Id = {user.Id}";
        Sl.Resolve<ISession>().CreateSQLQuery(query).ExecuteUpdate();
    }

    private static string GenerateQualityQuery(int questionId)
    {
        return
            GenerateEntriesQuery("TotalQuality", "Quality", questionId) + " " +
            GenerateAvgQuery("TotalQuality", "Quality", questionId) ;
    }

    private static string GenerateRelevancePersonal(int questionId)
    {
        return
            GenerateEntriesQuery("TotalRelevancePersonal", "RelevancePersonal", questionId) + " " +
            GenerateAvgQuery("TotalRelevancePersonal", "RelevancePersonal", questionId);
    }

    private static void UpdateTotalRelevancePersonalInCache(Question question)
    {
        UpdateTotalRelevancePersonalInCache(new List<Question>{ question });
    }

    public static void UpdateTotalRelevancePersonalInCache(IList<Question> questions)
    {
        var questionValuations = Sl.QuestionValuationRepo.GetByQuestionsFromCache(questions);
        foreach (var question in questions)
        {
            var totalRelevancePersonalEntriesCount = questionValuations.Count(v => v.Question.Id == question.Id && v.RelevancePersonal != -1);
            question.TotalRelevancePersonalEntries = totalRelevancePersonalEntriesCount;

            var totalRelevancePersonalAvg = totalRelevancePersonalEntriesCount == 0
                                            ? 0
                                            : questionValuations.Where(v => v.Question.Id == question.Id && v.RelevancePersonal != -1)
                                                  .Select(v => v.RelevancePersonal).Sum() / totalRelevancePersonalEntriesCount;
            question.TotalRelevancePersonalAvg = totalRelevancePersonalAvg;
        }
    }

    private static string GenerateRelevanceAllQuery(int questionId)
    {
        return
            GenerateEntriesQuery("TotalRelevanceForAll", "RelevanceForAll", questionId) + " " +
            GenerateAvgQuery("TotalRelevanceForAll", "RelevanceForAll", questionId);
    }

    private static string GenerateAvgQuery(string fieldToSet, string fieldSource, int questionId)
    {
        return "UPDATE Question SET " + fieldToSet + "Avg = " +
                    "ROUND((SELECT SUM(" + fieldSource + ") FROM QuestionValuation " +
                    " WHERE QuestionId = " + questionId + " AND " + fieldSource + " != -1)/ " + fieldToSet + "Entries) " +
                "WHERE Id = " + questionId + ";";
    }

    private static string GenerateEntriesQuery(string fieldToSet, string fieldSource, int questionId)
    {
        return "UPDATE Question SET " + fieldToSet + "Entries = " +
                    "(SELECT COUNT(Id) FROM QuestionValuation " +
                    "WHERE QuestionId = " + questionId + " AND " + fieldSource + " != -1) " +
                "WHERE Id = " + questionId + ";";
    }

    private static void CreateOrUpdateValuation(int questionId, User user, int relevancePersonal = -2, SaveType saveType = SaveType.CacheAndDatabase)
    {
        var questionValuation = Sl.QuestionValuationRepo.GetBy(questionId, user.Id);
        var question = Sl.QuestionRepo.GetById(questionId);

        CreateOrUpdateValuation(question, questionValuation, user, relevancePersonal, saveType);
    }

    private static void CreateOrUpdateValuation(
        Question question, 
        QuestionValuation questionValuation, 
        User user, 
        int relevancePersonal = -2,
        SaveType saveType = SaveType.CacheAndDatabase)
    {
        var questionValuationRepo = Sl.QuestionValuationRepo;

        if (questionValuation == null)
        {
            var newQuestionVal = new QuestionValuation
            {
                Question = question,
                User = user,
                RelevancePersonal = relevancePersonal,
                CorrectnessProbability = question.CorrectnessProbability
            };

            questionValuationRepo.CreateBySaveType(newQuestionVal, saveType);
        }
        else
        {
            if (relevancePersonal != -2)
                questionValuation.RelevancePersonal = relevancePersonal;

            questionValuationRepo.UpdateBySaveType(questionValuation, saveType);
        }
        questionValuationRepo.Flush();
    }
}