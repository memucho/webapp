using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using static System.String;

public class SetModel : BaseModel
{
    public string MetaTitle;
    public string MetaDescription;

    public int Id;
    public string Name;
    public string Text;

    public KnowledgeSummary KnowledgeSummary;

    public Set Set;

    public IList<SetQuestionRowModel> QuestionsInSet;
    public int QuestionCount;

    public User Creator;
    public string CreatorName;
    public string CreationDate;
    public string CreationDateNiceText;

    public ImageFrontendData ImageFrontendData;

    public bool IsOwner;

    public Func<UrlHelper, string> DetailLink;

    public int AnswersAllCount;
    public int AnswersAllPercentageTrue;
    public int AnswersAllPercentageFalse;

    public int AnswerMeCount;
    public int AnswerMePercentageTrue;
    public int AnswerMePercentageFalse;

    public bool IsInWishknowledge;

    public string TotalPins;

    public SetActiveMemory ActiveMemory;

    public ContentRecommendationResult ContentRecommendationResult;

    public bool HasPreviousCategoy => !IsNullOrEmpty(PreviousCategoryName);
    public string PreviousCategoryUrl;
    public string PreviousCategoryName;

    public SetModel(Set set)
    {
        MetaTitle = set.Name;
        MetaDescription = SeoUtils.ReplaceDoubleQuotes(set.Text).Truncate(250, true);

        Id = set.Id;
        Name = set.Name;
        Text = set.Text;

        Set = set;

        FillPreviousCategoryData();

        KnowledgeSummary = KnowledgeSummaryLoader.Run(UserId, set);

        ImageMetaDataCache.WarmupRequestCache(set);

        //var foo = R<ISession>().SessionFactory.Statistics.QueryExecutionCount;

        IsOwner = _sessionUser.IsLoggedInUser(set.Creator.Id);

        Creator = set.Creator;
        CreatorName = set.Creator.Name;
        CreationDate = set.DateCreated.ToString("dd.MM.yyyy HH:mm:ss");
        CreationDateNiceText = DateTimeUtils.TimeElapsedAsText(set.DateCreated);

        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(set.Id, ImageType.QuestionSet);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        //foo = R<ISession>().SessionFactory.Statistics.QueryExecutionCount;

        var questionValutionsForCurrentUser = Sl.QuestionValuationRepo
            .GetActiveInWishknowledgeFromCache(set.QuestionsInSet.Select(x => x.Question.Id).ToList(), _sessionUser.UserId);

        var questions = set.QuestionsInSetPublic.Select(x => x.Question).ToList();
        var totalsPerUser = Resolve<TotalsPersUserLoader>().Run(_sessionUser.UserId, questions);

        QuestionsInSet = set.QuestionsInSetPublic.OrderBy(q => q.Sort).ThenBy(q => q.Id).Select(
            x => new SetQuestionRowModel(
                x.Question, 
                x.Set,
                totalsPerUser.ByQuestionId(x.Question.Id),
                questionValutionsForCurrentUser.ByQuestionId(x.Question.Id)))
            .ToList();

        QuestionCount = QuestionsInSet.Count;

        //foo = R<ISession>().SessionFactory.Statistics.QueryExecutionCount;

        AnswersAllCount = questions.Sum(q => q.TotalAnswers());
        AnswersAllPercentageTrue = questions.Sum(q => q.TotalTrueAnswersPercentage());
        AnswersAllPercentageFalse = questions.Sum(q => q.TotalFalseAnswerPercentage());

        AnswerMeCount = totalsPerUser.Sum(q => q.Total());
        AnswerMePercentageTrue = totalsPerUser.Sum(q => q.TotalTrue);
        AnswerMePercentageFalse = totalsPerUser.Sum(q => q.TotalFalse);

        //foo = R<ISession>().SessionFactory.Statistics.QueryExecutionCount;

        IsInWishknowledge = Sl.SetValuationRepo.IsInWishKnowledge(Id, UserId);

        //foo = R<ISession>().SessionFactory.Statistics.QueryExecutionCount;

        TotalPins = set.TotalRelevancePersonalEntries.ToString();

        ActiveMemory = SetActiveMemoryLoader.Run(set, questionValutionsForCurrentUser);

        ContentRecommendationResult = ContentRecommendation.GetForSet(Set, 6);
    }

    private void FillPreviousCategoryData()
    {
        if (!IsNullOrEmpty(HttpContext.Current.Request.UrlReferrer?.LocalPath))
        {
            if (HttpContext.Current.Request.UrlReferrer.LocalPath.Contains("Kategorien"))
            {
                if (new SessionUiData().VisitedCategories.Any())
                {
                    var visitedCategory = GetLastVisitedCategoryOrDefault(Set);

                    PreviousCategoryUrl = Links.CategoryDetail(visitedCategory.Name, visitedCategory.Id);
                    PreviousCategoryName = visitedCategory.Name;
                }
            }
        }
    }

    private Category GetLastVisitedCategoryOrDefault(Set currentSet)
    {
        foreach (var visitedCategoryItem in Sl.SessionUiData.VisitedCategories)
        {
            var visitedCategory = EntityCache.GetCategory(visitedCategoryItem.Id);
            var visitedCategoryAggregatedSets = visitedCategory.GetAggregatedSetsFromMemoryCache();
            if (visitedCategoryAggregatedSets.Contains(currentSet))
            {
                return visitedCategory;
            }
        }

        return currentSet.Categories.Count > 0 
                ? Set.Categories.First()
                : Sl.CategoryRepo.Allgemeinwissen;
    }

    public string GetViews() => Sl.SetViewRepo.GetViewCount(Id).ToString();
}
