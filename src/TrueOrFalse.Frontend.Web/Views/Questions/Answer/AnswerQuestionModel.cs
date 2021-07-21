using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;
using static System.String;

public class AnswerQuestionModel : BaseModel
{
    public Func<UrlHelper, string> PreviousUrl;
    public Func<UrlHelper, string> NextUrl;
    public CategoryModel CategoryModel;
    public Guid QuestionViewGuid;
    public bool AnswerHelp; 

    public string DescriptionForSearchEngines;
    public string DescriptionForFacebook;

    public int QuestionId;
    public Question Question;
    public UserTinyModel Creator;
    public string CreatorId { get; private set; }
    public string CreatorName { get; private set; }

    public string ImageUrl_250;

    public string PageCurrent;
    public string PagesTotal;
    public string PagerKey;
    public SearchTabType SearchTabOverview;

    public string TotalQualityAvg;
    public string TotalQualityEntries;
    public string TotalRelevanceForAllAvg;
    public string TotalRelevanceForAllEntries;
    public string TotalRelevancePersonalAvg;
    public string TotalRelevancePersonalEntries;
    
    public string SolutionType;
    public QuestionSolution SolutionModel;
    public SolutionMetadata SolutionMetadata;
    public string SolutionMetaDataJson;

    public bool? IsMobileDevice;

    public string ImageUrl_500px;
    public string SoundUrl;
    public int TotalViews;

    public IList<CategoryCacheItem> AllCategoriesParents;
    public IList<CategoryCacheItem> AllCategorysWithChildrenAndParents { get; set; }
    public IList<CategoryCacheItem> ChildrenAndParents; 
    public bool IsOwner;
    public string ImageUrlAddComment;
    public bool HasImage => !IsNullOrEmpty(ImageUrl_500px);
    public string CreationDateNiceText { get; private set; }
    public string CreationDate { get; private set; }
    public string AverageAnswerTime { get; private set; }
    public string QuestionText { get; private set; }
    public string QuestionTitle { get; private set; }
    public string QuestionTextMarkdown { get; private set; }
    public QuestionVisibility Visibility { get; private set; }
    public bool HasPreviousPage;
    public bool HasNextPage;

    public bool SourceIsCategory;
    public Category SourceCategory;

    public IList<Category> Categories;
    public CategoryCacheItem PrimaryCategory;

    public HistoryAndProbabilityModel HistoryAndProbability;

    public bool IsInWishknowledge;

    public IList<CommentModel> Comments;
    public int CommentsSettledCount = 0;

    public bool IsLearningSession => LearningSession != null;
    public LearningSession  LearningSession;
    public LearningSessionStep LearningSessionStep;
    public int CurrentLearningStepIdx;
    public bool IsLastLearningStep;

    public int TestSessionProgessAfterAnswering;

    public bool DisableCommentLink;
    public bool DisableAddKnowledgeButton;
    public bool IsInWidget;
    public bool ShowCategoryList = true;

    public ContentRecommendationResult ContentRecommendationResult;
    public AnalyticsFooterModel AnalyticsFooterModel;
    public bool QuestionHasParentCategories = false;

    public AnswerQuestionModel(Question question, bool? isMobileDevice = null, bool showCategoryList = true, CategoryModel categoryModel= null)
    {
        CategoryModel = categoryModel;
        IsMobileDevice = isMobileDevice;
        HasNextPage = HasPreviousPage = false;
        ContentRecommendationResult = ContentRecommendation.GetForQuestion(question, 6);
        ShowCategoryList = showCategoryList;
        LearningSession = LearningSessionCache.GetLearningSession();  

        Populate(question);
    }

    public AnswerQuestionModel(LearningSession learningSession, bool? isMobileDevice = null)
    {
        this.IsMobileDevice = isMobileDevice;

        LearningSession = learningSession;

        CurrentLearningStepIdx = LearningSession.CurrentIndex;

        LearningSessionStep = LearningSession.Steps[CurrentLearningStepIdx];

        IsLastLearningStep = CurrentLearningStepIdx + 1 == LearningSession.Steps.Count();

        NextUrl = url => url.Action("Learn", Links.AnswerQuestionController, new{ skipStepIdx = learningSession.CurrentIndex +1 });
        QuestionViewGuid = learningSession.QuestionViewGuid;
        AnswerHelp = learningSession.Config.AnswerHelp; 

        Populate(LearningSessionStep.Question);
    }

    public AnswerQuestionModel(Question question, bool isQuestionDetails)
    {
        var valuationForUser = Resolve<TotalsPersUserLoader>().Run(UserId, question.Id);
        var questionValuationForUser = NotNull.Run(Sl.QuestionValuationRepo.GetByFromCache(question.Id, UserId));
        HistoryAndProbability = new HistoryAndProbabilityModel
        {
            AnswerHistory = new AnswerHistoryModel(question, valuationForUser),
            CorrectnessProbability = new CorrectnessProbabilityModel(question, questionValuationForUser),
            QuestionValuation = questionValuationForUser
        };
    }

    //we have no widgets, this can deleted
    public AnswerQuestionModel(Guid questionViewGuid, Question question, QuestionSearchSpec searchSpec, bool? isMobileDevice = null)
    {
        this.IsMobileDevice = isMobileDevice;
        QuestionViewGuid = questionViewGuid;

        PageCurrent = searchSpec.CurrentPage.ToString();
        PagesTotal = searchSpec.PageCount.ToString();
        PagerKey = searchSpec.Key;
        SearchTabOverview = searchSpec.SearchTab;
        HasPreviousPage = searchSpec.HasPreviousPage();
        HasNextPage = searchSpec.HasNextPage();

        NextUrl = url => url.Action("Next", Links.AnswerQuestionController, new {pager = PagerKey});
        PreviousUrl = url => url.Action("Previous", Links.AnswerQuestionController, new {pager = PagerKey});

        if (searchSpec.Filter.HasExactOneCategoryFilter()){
            SourceCategory = Resolve<CategoryRepository>().GetById(searchSpec.Filter.Categories.First());
            if (SourceCategory != null)
                SourceIsCategory = true;
        }

        ContentRecommendationResult = ContentRecommendation.GetForQuestion(question, 6);
        Populate(question);
    }

    private void Populate(Question question)
    {
        Creator = new UserTinyModel(question.Creator);

        if (question.Visibility != QuestionVisibility.All)
            if(Creator.Id != _sessionUser.User.Id )
                throw new Exception("Invalid access to questionId" + question.Id);

        var questionValuationForUser = NotNull.Run(Sl.QuestionValuationRepo.GetByFromCache(question.Id, UserId));

        if(IsLoggedIn)
            ImageUrlAddComment = new UserImageSettings(UserId).GetUrl_128px_square(_sessionUser.User).Url;

        Question = question;

        var comments = Resolve<CommentRepository>().GetForDisplay(question.Id);
        Comments = comments
            .Where(c => !c.IsSettled)
            .Select(c => new CommentModel(c))
            .ToList();
        CommentsSettledCount = comments.Count(c => c.IsSettled);
       
        CreatorId = Creator.Id.ToString();
        CreatorName = Creator.Name;
        CreationDate = question.DateCreated.ToString("dd.MM.yyyy HH:mm:ss");
        CreationDateNiceText = DateTimeUtils.TimeElapsedAsText(question.DateCreated);
        IsOwner = _sessionUser.IsLoggedInUserOrAdmin(Creator.Id);

        var imageResult = new UserImageSettings(Creator.Id).GetUrl_250px(Creator);

        ImageUrl_250 = imageResult.Url;
        QuestionId = question.Id;
        QuestionText = question.Text;
        QuestionTitle = Regex.Replace(QuestionText, "<.*?>", String.Empty);
        QuestionTextMarkdown = question.TextExtended != null ? MarkdownMarkdig.ToHtml(question.TextExtended) : "";
        Visibility = question.Visibility;
        SolutionType = question.SolutionType.ToString();
        SolutionModel = GetQuestionSolution.Run(question);
        SolutionMetadata = new SolutionMetadata {Json = question.SolutionMetadataJson};
        SolutionMetaDataJson = question.SolutionMetadataJson;
        IsInWishknowledge = questionValuationForUser.IsInWishKnowledge;
        TotalViews = question.TotalViews + 1;
        TotalQualityAvg = question.TotalQualityAvg.ToString();
        TotalQualityEntries = question.TotalQualityEntries.ToString();
        TotalRelevanceForAllAvg = question.TotalRelevanceForAllAvg.ToString();
        TotalRelevanceForAllEntries = question.TotalRelevanceForAllEntries.ToString();
        TotalRelevancePersonalAvg = question.TotalRelevancePersonalAvg.ToString();
        TotalRelevancePersonalEntries = question.TotalRelevancePersonalEntries.ToString();
        AverageAnswerTime = "";
        ImageUrl_500px = QuestionImageSettings.Create(question.Id).GetUrl_128px().Url;
        SoundUrl = new GetQuestionSoundUrl().Run(question);
        Categories = question.Categories;
        QuestionHasParentCategories = question.Categories.Any();

        //Find best suited primary category for question
        if (!IsLearningSession && QuestionHasParentCategories)
        {
            PrimaryCategory = GetPrimaryCategory.GetForQuestion(question);
            AnalyticsFooterModel = new AnalyticsFooterModel(PrimaryCategory, true);
            AllCategoriesParents = GraphService.GetAllParentsFromEntityCache(PrimaryCategory.Id);
            var allCategoryChildrens = EntityCache.GetChildren(PrimaryCategory.Id);
            AllCategorysWithChildrenAndParents = EntityCache.GetCategoryCacheItems(
                question.Categories.Select(c => c.Id)
                    .Concat(allCategoryChildrens.Select(c => c.Id))
                    .Concat(AllCategoriesParents.Select(c => c.Id)))
                .ToList();
            ChildrenAndParents = allCategoryChildrens.Concat(AllCategoriesParents).ToList();
        }

        DescriptionForSearchEngines = GetMetaDescriptionSearchEngines();
        DescriptionForFacebook = GetMetaDescriptionsFacebook();
    }

    private string GetMetaDescriptionSearchEngines()
    {
        var result = "";

        if (Question.SolutionType == TrueOrFalse.SolutionType.MultipleChoice_SingleSolution)
        {
            result = $"Antwort: '{SolutionModel.GetAnswerForSEO()}'. {Environment.NewLine}";

            if (result.Length < 100 && !IsNullOrEmpty(Question.Description))
            {
                result += Environment.NewLine;
                result += "Erkärung: ";
                result += Question.Description;
            }

            if (result.Length < 50)
            {
                result += "Alternativen: ";

                if (((QuestionSolutionMultipleChoice_SingleSolution) SolutionModel).Choices.Count < 2)
                    result += "";
                else
                result += ((QuestionSolutionMultipleChoice_SingleSolution)SolutionModel)
                    .Choices
                    .Skip(1)
                    .Aggregate((a, b) => a + ", " + b) + "?  ";

                var test = result; 
            }

        }
        else
        {
            result += Question.Description;
        }

        return result.Truncate(300, addEllipsis: true).Trim().Replace("\"", "'");
    }

    private string GetMetaDescriptionsFacebook()
    {
        var result = "";

        if (Question.SolutionType == TrueOrFalse.SolutionType.MultipleChoice_SingleSolution)
        {
            var solutionModel = (QuestionSolutionMultipleChoice_SingleSolution) SolutionModel;

            if (!solutionModel.Choices.Any())
                return "";

            result = solutionModel
                .Choices
                .Shuffle()
                .Aggregate((a, b) => $"{a} - oder - {Environment.NewLine} {b}");
        }

        return result;
    }
}
