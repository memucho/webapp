using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;
using static System.String;

public class AnswerQuestionModel : BaseModel
{
    public Func<UrlHelper, string> PreviousUrl;
    public Func<UrlHelper, string> NextUrl;

    public Guid QuestionViewGuid;

    public string DescriptionForSearchEngines;
    public string DescriptionForFacebook;

    public int QuestionId;
    public Question Question;
    public User Creator;
    public string CreatorId { get; private set; }
    public string CreatorName { get; private set; }

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

    public bool? isMobileDevice;

    public string ImageUrl_500px;
    public string SoundUrl;
    public int TotalViews;

    public int TimesAnsweredUser;
    public int TimesAnsweredUserTrue;
    public int TimesAnsweredUserWrong;

    public bool IsOwner;

    public string ImageUrlAddComment;

    public bool HasImage => !IsNullOrEmpty(ImageUrl_500px);
    public bool HasSound => !IsNullOrEmpty(SoundUrl);

    public string CreationDateNiceText { get; private set; }
    public string CreationDate { get; private set; }

    public string AverageAnswerTime { get; private set; }

    public string QuestionText { get; private set; }
    public string QuestionTextMarkdown { get; private set; }

    public QuestionVisibility Visibility { get; private set; }

    public bool HasPreviousPage;
    public bool HasNextPage;

    public bool SourceIsTabAll;
    public bool SourceIsTabMine;
    public bool SourceIsTabWish;

    public bool SourceIsSet;
    public bool SourceIsCategory;
    public Category SourceCategory;

    public Set Set;
    public IList<Category> Categories;
    public Category PrimaryCategory;
    public IList<SetMini> SetMinis;
    public int SetCount;

    public HistoryAndProbabilityModel HistoryAndProbability;

    public bool IsInWishknowledge;

    public IList<CommentModel> Comments;
    public int CommentsSettledCount = 0;

    public bool IsLearningSession => LearningSession != null;
    public LearningSession LearningSession;
    public LearningSessionStep LearningSessionStep;
    public int CurrentLearningStepIdx;
    public int CurrentLearningStepPercentage;
    public bool IsLastLearningStep = false;

    public TestSession TestSession;
    public bool IsTestSession;
    public int TestSessionId;
    public int TestSessionCurrentStep;
    public int TestSessionNumberOfSteps;
    public int TestSessionCurrentStepPercentage;
    public bool TestSessionIsLastStep = false;
    public int TestSessionProgessAfterAnswering;
    public bool ShowErrorExpiredTestSession;

    public bool DisableCommentLink;
    public bool DisableAddKnowledgeButton;
    public bool IsInWidget;

    public ContentRecommendationResult ContentRecommendationResult;

    public AnswerQuestionModel()
    {
    }

    public AnswerQuestionModel(Question question, bool? isMobileDevice = null): this()
    {
        if(this.QuestionViewGuid == Guid.Empty)
            QuestionViewGuid = Guid.NewGuid();

        this.isMobileDevice = isMobileDevice;
        HasNextPage = HasPreviousPage = false;
        SourceIsTabAll = true;
        ContentRecommendationResult = ContentRecommendation.GetForQuestion(question, 6);

        Populate(question);
    }

    public AnswerQuestionModel(Guid questionViewGuid, LearningSession learningSession, bool? isMobileDevice = null) : this()
    {
        this.isMobileDevice = isMobileDevice;
        QuestionViewGuid = questionViewGuid;

        LearningSession = learningSession;

        CurrentLearningStepIdx = LearningSession.CurrentLearningStepIdx();

        LearningSessionStep = LearningSession.Steps[CurrentLearningStepIdx];
        LearningSessionStep.Question = Sl.QuestionRepo.GetById(LearningSessionStep.Question.Id);//Prevents nhibernate lazy load exception

        IsLastLearningStep = CurrentLearningStepIdx + 1 == LearningSession.Steps.Count();

        CurrentLearningStepPercentage = CurrentLearningStepIdx == 0
            ? 0
            : (int)Math.Round(CurrentLearningStepIdx/(float)LearningSession.Steps.Count()*100);

        NextUrl = url => url.Action("Learn", Links.AnswerQuestionController,
            new {
                learningSessionId = learningSession.Id,
                learningSessionName = learningSession.UrlName
            });

        Populate(LearningSessionStep.Question);
    }

    public AnswerQuestionModel(int dummyQuestionId) : this()
    {
        var dummyQuestion = Sl.QuestionRepo.GetById(dummyQuestionId);

        LearningSession = new LearningSession{Steps = new List<LearningSessionStep>()};

        for (var i = 0; i < LearningSession.DefaultNumberOfSteps; i++)
        {
            LearningSession.Steps.Add(new LearningSessionStep{Idx = i, Question = dummyQuestion});
        }

        Populate(dummyQuestion);
    }


    public static AnswerQuestionModel CreateExpiredTestSession()
    {
        var model = new AnswerQuestionModel()
        {
            IsTestSession = true,
            ShowErrorExpiredTestSession = true
        };
        return model;
    }

    public AnswerQuestionModel(TestSession testSession, Guid questionViewGuid, Question question, bool? isMobileDevice = null) : this()
    {
        this.isMobileDevice = isMobileDevice;

        QuestionViewGuid = questionViewGuid;
        TestSession = testSession;
        IsTestSession = true;
        TestSessionId = testSession.Id;
        TestSessionCurrentStep = testSession.CurrentStepIndex;
        TestSessionNumberOfSteps = testSession.NumberOfSteps;
        TestSessionIsLastStep = testSession.CurrentStepIndex == testSession.NumberOfSteps;
        TestSessionCurrentStepPercentage = TestSessionCurrentStep == 0
            ? 0
            : (int) Math.Round((TestSessionCurrentStep-1)/(float) TestSessionNumberOfSteps*100);
        TestSessionProgessAfterAnswering = (int)Math.Round((TestSessionCurrentStep) / (float)TestSessionNumberOfSteps * 100);
        NextUrl = url => url.Action("Test", Links.AnswerQuestionController);
        HasNextPage = true;
        Populate(question);
    }

    public AnswerQuestionModel(Guid questionViewGuid, Question question, QuestionSearchSpec searchSpec, bool? isMobileDevice = null) : this()
    {
        this.isMobileDevice = isMobileDevice;
        QuestionViewGuid = questionViewGuid;

        PageCurrent = searchSpec.CurrentPage.ToString();
        PagesTotal = searchSpec.PageCount.ToString();
        PagerKey = searchSpec.Key;
        SearchTabOverview = searchSpec.SearchTab;
        HasPreviousPage = searchSpec.HasPreviousPage();
        HasNextPage = searchSpec.HasNextPage();

        NextUrl = url => url.Action("Next", Links.AnswerQuestionController, new {pager = PagerKey});
        PreviousUrl = url => url.Action("Previous", Links.AnswerQuestionController, new {pager = PagerKey});

        SourceIsTabAll = SearchTabType.All == searchSpec.SearchTab;
        SourceIsTabMine = SearchTabType.Mine == searchSpec.SearchTab;
        SourceIsTabWish = SearchTabType.Wish == searchSpec.SearchTab;

        if (searchSpec.Filter.HasExactOneCategoryFilter()){
            SourceCategory = Resolve<CategoryRepository>().GetById(searchSpec.Filter.Categories.First());
            if (SourceCategory != null)
                SourceIsCategory = true;
        }

        ContentRecommendationResult = ContentRecommendation.GetForQuestion(question, 6);
        Populate(question);
    }

    public AnswerQuestionModel(Guid questionViewGuid, Set set, Question question) : this()
    {
        QuestionViewGuid = questionViewGuid;

        int pageCurrent = set.QuestionsInSet.GetIndex(question.Id) + 1;
        int pagesTotal = set.QuestionsInSet.Count;
        PageCurrent = pageCurrent.ToString();
        PagesTotal = pagesTotal.ToString();

        HasPreviousPage = pageCurrent > 1;
        HasNextPage = pageCurrent < pagesTotal;

        //NextUrl = url => url.Action("Next", Links.AnswerQuestionController, new { setId = set.Id, questionId = question.Id });
        //PreviousUrl = url => url.Action("Previous", Links.AnswerQuestionController, new { setId = set.Id, questionId = question.Id });
        if (HasNextPage)
            NextUrl = url => Links.AnswerQuestion(url, set.QuestionsInSet.GetNextTo(question.Id).Question, set);

        if (HasPreviousPage)
            PreviousUrl = url => Links.AnswerQuestion(url, set.QuestionsInSet.GetPreviousTo(question.Id).Question, set);

        SourceIsSet = true;
        Set = set;

        ContentRecommendationResult = ContentRecommendation.GetForQuestion(question, 6);
        Populate(question);
    }

    private void Populate(Question question)
    {
        if (question.Visibility != QuestionVisibility.All)
            if(question.Creator.Id != _sessionUser.User.Id)
                throw new Exception("Invalid access to questionId" + question.Id);

        var questionValuationForUser = NotNull.Run(Sl.QuestionValuationRepo.GetByFromCache(question.Id, UserId));
        var valuationForUser = Resolve<TotalsPersUserLoader>().Run(UserId, question.Id);

        if(IsLoggedIn)
            ImageUrlAddComment = new UserImageSettings(UserId).GetUrl_128px_square(_sessionUser.User).Url;

        Question = question;

        var comments = Resolve<CommentRepository>().GetForDisplay(question.Id);
        Comments = comments
            .Where(c => !c.IsSettled)
            .Select(c => new CommentModel(c))
            .ToList();
        CommentsSettledCount = comments.Count(c => c.IsSettled);

        Creator = question.Creator;
        CreatorId = question.Creator.Id.ToString();
        CreatorName = question.Creator.Name;
        CreationDate = question.DateCreated.ToString("dd.MM.yyyy HH:mm:ss");
        CreationDateNiceText = DateTimeUtils.TimeElapsedAsText(question.DateCreated);

        IsOwner = _sessionUser.IsLoggedInUserOrAdmin(question.Creator.Id);

        QuestionId = question.Id;
        QuestionText = question.Text;
        QuestionTextMarkdown = MarkdownInit.Run().Transform(question.TextExtended);
        Visibility = question.Visibility;
        SolutionType = question.SolutionType.ToString();
        SolutionModel = GetQuestionSolution.Run(question);

        SolutionMetadata = new SolutionMetadata {Json = question.SolutionMetadataJson};
        SolutionMetaDataJson = question.SolutionMetadataJson;

        HistoryAndProbability = new HistoryAndProbabilityModel
        {
            AnswerHistory = new AnswerHistoryModel(question, valuationForUser),
            CorrectnessProbability = new CorrectnessProbabilityModel(question, questionValuationForUser),
            QuestionValuation = questionValuationForUser
        };

        IsInWishknowledge = questionValuationForUser.IsInWishKnowledge();
        
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
        SetMinis = question.SetTop5Minis;
        SetCount = question.SetsAmount;

        //Find best suited primary category for question
        if (!IsLoggedIn && !IsTestSession && !IsLearningSession)
        {
            PrimaryCategory = GetPrimaryCategory.GetForQuestion(question);
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
                result += ((QuestionSolutionMultipleChoice_SingleSolution)SolutionModel)
                    .Choices
                    .Skip(1)
                    .Aggregate((a, b) => a + ", " + b) + "?  ";
            }

        }
        else
        {
            result += Question.Description;
        }

        return result.Truncate(300, addEllipsis: true).Trim();
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