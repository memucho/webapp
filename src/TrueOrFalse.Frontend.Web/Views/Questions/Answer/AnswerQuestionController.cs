using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FluentNHibernate.Data;
using Newtonsoft.Json;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Search;
using TrueOrFalse.Web;
using static System.String;

public class AnswerQuestionController : BaseController
{
    private readonly QuestionRepo _questionRepo;
    private readonly AnswerQuestion _answerQuestion;

    private const string _viewLocation = "~/Views/Questions/Answer/AnswerQuestion.aspx";

    public AnswerQuestionController(QuestionRepo questionRepo, AnswerQuestion answerQuestion)
    {
        _questionRepo = questionRepo;
        _answerQuestion = answerQuestion;
    }

    [SetMainMenu(MainMenuEntry.None)]
    [SetThemeMenu(isQuestionPage: true)]
    public ActionResult Answer(string text, int? id, int? elementOnPage, string pager, int? setId, int? questionId, string category)
    {
        
        if (id.HasValue && SeoUtils.HasUnderscores(text))
            return SeoUtils.RedirectToHyphendVersion(RedirectPermanent, id.Value);

        return AnswerQuestion(text, id, elementOnPage, pager, category);
    }


    [SetThemeMenu(isLearningSessionPage: true)]
    public ActionResult Learn( int skipStepIdx = -1)
    {
        var learningSession = LearningSessionCache.GetLearningSession();

        if (learningSession.User != _sessionUser.User)
            throw new Exception("not logged in or not possessing user");

        if (skipStepIdx != -1 && learningSession.CurrentStep != null)
        {
            learningSession.SkipStep();
            return RedirectToAction("Learn", Links.AnswerQuestionController);
        }

        var currentLearningStepIndex = learningSession.CurrentIndex;

        if (currentLearningStepIndex == -1) //None of the steps is uncompleted
            return RedirectToAction("LearningSessionResult", Links.LearningSessionResultController);

        var questionViewGuid = Guid.NewGuid();

        Sl.SaveQuestionView.Run(
            questionViewGuid,
            learningSession.Steps[currentLearningStepIndex].Question,
            _sessionUser.User
          );

        return View(_viewLocation,
            new AnswerQuestionModel(LearningSessionCache.GetLearningSession()));
    }

    public ActionResult AnswerQuestion(string text, int? id, int? elementOnPage, string pager, string category)
    {
        if (IsNullOrEmpty(pager) && (elementOnPage == null || elementOnPage == -1))
        {
            if (id == null)
                throw new Exception("AnswerQuestionController: No id for question provided.");

            var question2 = _questionRepo.GetById((int) id);

            if (question2 == null)
                throw new Exception("question not found");

            _sessionUiData.VisitedQuestions.Add(new QuestionHistoryItem(question2, HistoryItemType.Any));

            return View(_viewLocation, new AnswerQuestionModel(question2));
        }

        var activeSearchSpec = Resolve<QuestionSearchSpecSession>().ByKey(pager);

        if (!IsNullOrEmpty(category))
        {
            var categoryDb = R<CategoryRepository>().GetByName(category).FirstOrDefault();
            if (categoryDb != null)
            {
                activeSearchSpec.Filter.Categories.Clear();
                activeSearchSpec.Filter.Categories.Add(categoryDb.Id);
                activeSearchSpec.OrderBy.PersonalRelevance.Desc();
                activeSearchSpec.PageSize = 1;

                //set total count
                Sl.R<SearchQuestions>().Run(activeSearchSpec);
            }
        }

        if (text == null && id == null && elementOnPage == null)
            return GetViewBySearchSpec(activeSearchSpec);

        var question = _questionRepo.GetById((int) id);

        activeSearchSpec.PageSize = 1;
        if ((int) elementOnPage != -1)
            activeSearchSpec.CurrentPage = (int) elementOnPage;

        _sessionUiData.VisitedQuestions.Add(new QuestionHistoryItem(question, activeSearchSpec));

        var questionViewGuid = Guid.NewGuid();
        Sl.SaveQuestionView.Run(questionViewGuid, question, _sessionUser.User);

        return View(_viewLocation, new AnswerQuestionModel(questionViewGuid, question, activeSearchSpec));
    }

    public ActionResult Next(string pager)
    {
        var activeSearchSpec = Resolve<QuestionSearchSpecSession>().ByKey(pager);
        activeSearchSpec.NextPage(1);
        return GetViewBySearchSpec(activeSearchSpec);
    }

    public ActionResult Previous(string pager)
    {
        var activeSearchSpec = Resolve<QuestionSearchSpecSession>().ByKey(pager);
        activeSearchSpec.PreviousPage(1);
        return GetViewBySearchSpec(activeSearchSpec);
    }

    private ActionResult GetViewBySearchSpec(QuestionSearchSpec searchSpec)
    {

        using (MiniProfiler.Current.Step("GetViewBySearchSpec"))
        {
            var question = AnswerQuestionControllerSearch.Run(searchSpec);

            if (searchSpec.HistoryItem != null)
            {
                if (searchSpec.HistoryItem.Question != null)
                {
                    if (searchSpec.HistoryItem.Question.Id != question.Id)
                    {
                        question = Resolve<QuestionRepo>().GetById(searchSpec.HistoryItem.Question.Id);
                    }
                }

                searchSpec.HistoryItem = null;
            }

            return RedirectToAction("Answer", Links.AnswerQuestionController,
                new
                {
                    text = UriSegmentFriendlyQuestion.Run(question.Text),
                    id = question.Id,
                    elementOnPage = searchSpec.CurrentPage,
                    pager = searchSpec.Key,
                    category = ""
                });
        }
    }

    [HttpPost]
    public JsonResult SendAnswerLearningSession(
        int id,
        int learningSessionId = 0,
        Guid questionViewGuid = new Guid(),
        int interactionNumber = 0 ,
        string answer = "",
        int millisecondsSinceQuestionView = 0,
        bool inTestMode = false,
        bool isLearningSession =false
    )
    {
        if(isLearningSession)
            LearningSessionCache.GetLearningSession().CurrentStep.Answer = answer; 

        var result = _answerQuestion.Run(id, answer, UserId, questionViewGuid, interactionNumber,
            millisecondsSinceQuestionView, learningSessionId, new Guid(), inTestMode);
        var question = _questionRepo.GetById(id);
        var solution = GetQuestionSolution.Run(question);

        return new JsonResult
        {
            Data = new
            {
                correct = result.IsCorrect,
                correctAnswer = result.CorrectAnswer,
                choices = solution.GetType() == typeof(QuestionSolutionMultipleChoice_SingleSolution)
                    ? ((QuestionSolutionMultipleChoice_SingleSolution) solution).Choices
                    : null,
                newStepAdded = result.NewStepAdded,
                numberSteps = result.NumberSteps,
            }
        };
    }

    [HttpPost]
    public JsonResult AmendAfterShowSolution( bool isInTestMode = false)
    {
        var learningSession = LearningSessionCache.GetLearningSession();
        var learningSessionStep = learningSession.CurrentStep;

        learningSessionStep.AnswerState = AnswerState.ShowedSolutionOnly;

        bool newStepAdded = !(learningSession.LimitForThisQuestionHasBeenReached(learningSessionStep) || learningSession.LimitForNumberOfRepetitionsHasBeenReached() || isInTestMode);
        learningSession.ShowSolution();

        return new JsonResult
        {
            Data = new
            {
                numberSteps = learningSession.Steps.Count,
                newStepAdded,
                currentStep = learningSession.CurrentIndex
            }
        };
    }

    [HttpPost]
    public JsonResult GetSolution(int id, Guid questionViewGuid, int interactionNumber, int millisecondsSinceQuestionView = -1, bool isNotAnswered = false)
    {
        var question = _questionRepo.GetById(id);
        var solution = GetQuestionSolution.Run(question);
        if(isNotAnswered)
            R<AnswerLog>().LogAnswerView(question, this.UserId, questionViewGuid, interactionNumber, millisecondsSinceQuestionView);

        EscapeReferencesText(question.References);

        return new JsonResult
        {
            Data = new
            {
                correctAnswerAsHTML = solution.GetCorrectAnswerAsHtml(),
                correctAnswer = solution.CorrectAnswer(),
                correctAnswerDesc = question.Description != null ? MarkdownMarkdig.ToHtml(question.Description) : "",
                correctAnswerReferences = question.References.Select(r => new
                {
                    referenceId = r.Id,
                    categoryId = r.Category?.Id ?? -1,
                    referenceType = r.ReferenceType.GetName(),
                    additionalInfo = r.AdditionalInfo ?? "",
                    referenceText = r.ReferenceText ?? ""
                })
            }
        };
    }

    private static void EscapeReferencesText(IList<Reference> references)
    {
        foreach (var reference in references)
        {
            if(reference.ReferenceText != null)
                reference.ReferenceText = reference.ReferenceText.Replace("\n", "<br/>").Replace("\\n", "<br/>");
            if(reference.AdditionalInfo != null)
                reference.AdditionalInfo = reference.AdditionalInfo.Replace("\n", "<br/>").Replace("\\n", "<br/>");
        }
    }

    [HttpPost]
    public void LogTimeForQuestionView(Guid questionViewGuid, int millisecondsSinceQuestionView) => 
        Sl.SaveQuestionView.LogOverallTime(questionViewGuid, millisecondsSinceQuestionView);

    [HttpPost]
    public void CountLastAnswerAsCorrect(int id, Guid questionViewGuid, int interactionNumber, int? testSessionId, int? learningSessionId, string learningSessionStepGuid) => 
        _answerQuestion.Run(id, _sessionUser.UserId, questionViewGuid, interactionNumber, testSessionId, learningSessionId, learningSessionStepGuid, countLastAnswerAsCorrect: true);

    [HttpPost]
    public void CountUnansweredAsCorrect(int id, Guid questionViewGuid, int interactionNumber, int millisecondsSinceQuestionView, string learningSessionStepGuid, int? testSessionId, int? learningSessionId) => 
        _answerQuestion.Run(id, _sessionUser.UserId, questionViewGuid, interactionNumber, testSessionId, learningSessionId, learningSessionStepGuid, millisecondsSinceQuestionView, countUnansweredAsCorrect: true);

    public ActionResult PartialAnswerHistory(int questionId)
    {
        var question = _questionRepo.GetById(questionId);

        var questionValuationForUser =
            NotNull.Run(Sl.QuestionValuationRepo.GetByFromCache(question.Id, _sessionUser.UserId));
        var valuationForUser = Resolve<TotalsPersUserLoader>().Run(_sessionUser.UserId, question.Id);

        return View("HistoryAndProbability",
            new HistoryAndProbabilityModel
            {
                LoadJs = true,
                AnswerHistory = new AnswerHistoryModel(question, valuationForUser),
                CorrectnessProbability = new CorrectnessProbabilityModel(question, questionValuationForUser),
                QuestionValuation = questionValuationForUser
            }
        );
    }


    //For MatchList Questions
    public string RenderAnswerBody(
        int questionId, 
        string pager, 
        bool? isMobileDevice = null, 
        int? testSessionId = null, 
        int? learningSessionId = null, 
        bool isVideo = false,
        bool? hideAddToKnowledge = false)
    {

        AnswerQuestionModel answerQuestionModel;

        if (learningSessionId != null)
        {
            var learningSession = LearningSessionCache.GetLearningSession();

            answerQuestionModel = new AnswerQuestionModel(learningSession, isMobileDevice);
            if (hideAddToKnowledge.HasValue)
                answerQuestionModel.DisableAddKnowledgeButton = hideAddToKnowledge.Value;

            return ViewRenderer.RenderPartialView(
                "~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx",
                new AnswerBodyModel(answerQuestionModel),
                ControllerContext
            );
        }
        var question = Sl.QuestionRepo.GetById(questionId);
        if (isVideo)
        {
            return ViewRenderer.RenderPartialView(
                "~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx",
                new AnswerBodyModel(new AnswerQuestionModel(question , isMobileDevice)),
                ControllerContext
            );
        }
        //For normal questions
        var activeSearchSpec = Resolve<QuestionSearchSpecSession>().ByKey(pager);
        var questionViewGuid = Guid.NewGuid();

        answerQuestionModel = new AnswerQuestionModel(questionViewGuid, question, activeSearchSpec, isMobileDevice);
        if (hideAddToKnowledge.HasValue)
            answerQuestionModel.DisableAddKnowledgeButton = hideAddToKnowledge.Value;

        return ViewRenderer.RenderPartialView(
            "~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx",
            new AnswerBodyModel(answerQuestionModel),
            ControllerContext
        );
    }

    [HttpPost]
    public string RenderAnswerBody(int questionId)
    {
        var question = EntityCache.GetQuestionById(questionId);
        var answerQuestionModel = new AnswerQuestionModel(question);
        return ViewRenderer.RenderPartialView(
            "~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx",
            new AnswerBodyModel(answerQuestionModel),
            ControllerContext
        );
    }

    public string RenderAnswerBodyByNextQuestion(string pager)
    {
        var activeSearchSpec = Resolve<QuestionSearchSpecSession>().ByKey(pager);
        activeSearchSpec.NextPage(1);
        return GetAnswerBodyBySearchSpec(activeSearchSpec);
    }

    public string RenderAnswerBodyByPreviousQuestion(string pager)
    {
        var activeSearchSpec = Resolve<QuestionSearchSpecSession>().ByKey(pager);
        activeSearchSpec.PreviousPage(1);
        return GetAnswerBodyBySearchSpec(activeSearchSpec);
    }

    private string GetAnswerBodyBySearchSpec(QuestionSearchSpec activeSearchSpec)
    {
        Question question;

        using (MiniProfiler.Current.Step("GetViewBySearchSpec"))
        {
            question = AnswerQuestionControllerSearch.Run(activeSearchSpec);

            if (activeSearchSpec.HistoryItem != null)
            {
                if (activeSearchSpec.HistoryItem.Question != null)
                {
                    if (activeSearchSpec.HistoryItem.Question.Id != question.Id)
                    {
                        question = Resolve<QuestionRepo>().GetById(activeSearchSpec.HistoryItem.Question.Id);
                    }
                }

                activeSearchSpec.HistoryItem = null;
            }
        }

        var elementOnPage = activeSearchSpec.CurrentPage;
        activeSearchSpec.PageSize = 1;
        if (elementOnPage != -1)
            activeSearchSpec.CurrentPage = (int)elementOnPage;
        _sessionUiData.VisitedQuestions.Add(new QuestionHistoryItem(question, activeSearchSpec));

        var questionViewGuid = Guid.NewGuid();
        Sl.SaveQuestionView.Run(questionViewGuid, question, _sessionUser.User);
        var answerQuestionModel = new AnswerQuestionModel(questionViewGuid, question, activeSearchSpec);

        var currentUrl = Links.AnswerQuestion(question, elementOnPage, activeSearchSpec.Key);
        return GetQuestionPageData(answerQuestionModel, currentUrl, new SessionData());
    }

    [HttpPost]
    public string RenderNewAnswerBodySessionForCategory(LearningSessionConfig config)
    {
        var learningSession = IsLoggedIn ? 
            LearningSessionCreator.ForLoggedInUser(config) : 
            LearningSessionCreator.ForAnonymous(config);

        if (config.SafeLearningSessionOptions)
        {
            var user = Sl.UserRepo.GetById(UserId);
            var learningSessionOptionsHelper = new SafeLearningSessionOptionsHelper
            {
                UserIsAuthor = config.CreatedByCurrentUser,
                AllQuestions = config.AllQuestions,
                IsInTestmode = config.IsInTestMode,
                QuestionsInWishknowledge = config.InWishknowledge,
                IsNotQuestionInWishKnowledge = config.IsNotQuestionInWishKnowledge,
                MaxProbability = config.MaxProbability,
                MinProbability = config.MinProbability,
                MaxQuestionCount = config.MaxQuestionCount,
                Repititions = config.Repititions,
                AnswerHelp = config.AnswerHelp
            };

            user.LearningSessionOptions = JsonConvert.SerializeObject(learningSessionOptionsHelper);
            Sl.UserRepo.Update(user);
        }

        LearningSessionCache.AddOrUpdate(learningSession);

        var firstStep = 0; 
        return RenderAnswerBodyByLearningSession(firstStep);
    }

    [HttpPost]
    public string RenderAnswerBodyByLearningSession(int skipStepIdx = -1, int index = -1)
    {
        var learningSession = LearningSessionCache.GetLearningSession();
        if (learningSession.Steps.Count == 0)
            return ""; 

        if (index != -1)
        {
            learningSession.LoadSpecificQuestion(index);
        }
        else
        {
            if (skipStepIdx != -1 && skipStepIdx != 0)
                learningSession.SkipStep();
            else if (skipStepIdx != 0)
                learningSession.NextStep();
        }

        if (learningSession.IsLastStep)
            return RenderLearningSessionResult(learningSession);

       
        learningSession.QuestionViewGuid = Guid.NewGuid(); 
        var question = learningSession.Steps[learningSession.CurrentIndex].Question;

        var sessionUserId = _sessionUser == null ? -1 : _sessionUser.UserId;

        Sl.SaveQuestionView.Run(
            learningSession.QuestionViewGuid,
            question,
            sessionUserId);

        var answerQuestionModel = new AnswerQuestionModel(learningSession, false);

        string currentSessionHeader = "Frage <span id = \"CurrentStepNumber\">" + (answerQuestionModel.CurrentLearningStepIdx + 1) +
                                      "</span> von <span id=\"StepCount\">" + answerQuestionModel.LearningSession.Steps.Count +
                                      "</span>";
        int currentStepIdx = learningSession.CurrentIndex;
        bool isLastStep = answerQuestionModel.IsLastLearningStep;
        string currentUrl = Links.LearningSession(learningSession);

        var sessionData = new SessionData(currentSessionHeader, currentStepIdx, isLastStep, skipStepIdx);
        var config = learningSession.Config;
        return GetQuestionPageData(answerQuestionModel, currentUrl, sessionData, isSession: true,
            isInLearningTab: config.IsInLearningTab, isInTestMode: config.IsInTestMode);
    }

    public string RenderUpdatedQuestionDetails(int questionId, bool showCategoryList = true)
    {
        var question = EntityCache.GetQuestionById(questionId);
        var model = new AnswerQuestionModel(question, showCategoryList:showCategoryList);

        return ViewRenderer.RenderPartialView("~/Views/Questions/Answer/AnswerQuestionDetails.ascx", model, ControllerContext);
    }

    [HttpPost]
    public JsonResult GetQuestionDetails(int questionId)
    {
        var dateNow = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var question = EntityCache.GetQuestionById(questionId);
        var answerQuestionModel = new AnswerQuestionModel(question);
        var correctnessProbability = answerQuestionModel.HistoryAndProbability.CorrectnessProbability;
        var history = answerQuestionModel.HistoryAndProbability.AnswerHistory;
        var json = Json(new
        {
            personalProbability = correctnessProbability.CPPersonal,
            personalColor = correctnessProbability.CPPColor,
            avgProbability = correctnessProbability.CPAll,
            personalAnswerCount = history.TimesAnsweredUser,
            personalAnsweredCorrectly = history.TimesAnsweredUserTrue,
            personalAnsweredWrongly = history.TimesAnsweredUserWrong,
            overallAnswerCount = history.TimesAnsweredTotal,
            overallAnsweredCorrectly = history.TimesAnsweredCorrect,
            overallAnsweredWrongly = history.TimesAnsweredWrongTotal,
            isInWishknowledge = answerQuestionModel.IsInWishknowledge,
            categories = question.Categories.Where(c => c.IsVisibleToCurrentUser()).Select(c => new
            {
                name = c.Name,
                categoryType = c.Type,
                linkToCategory = Links.CategoryDetail(c),
                isPrivate = c.Visibility == CategoryVisibility.Owner,
            }).AsEnumerable().Distinct().ToList(),
            visibility = question.Visibility,
            dateNow,
            endTimer = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        });
        return json;
    }

    public string RenderCategoryList(int questionId)
    {
        var question = EntityCache.GetQuestionById(questionId);

        return ViewRenderer.RenderPartialView("~/Views/Shared/CategoriesOfQuestion.ascx", question, ControllerContext);
    }

    private string GetQuestionPageData(
        AnswerQuestionModel answerQuestionModel, 
        string currentUrl, 
        SessionData sessionData, 
        bool isSession = false,
        int testSessionId = -1,
        bool includeTestSessionHeader = false,
        bool isInLearningTab = false,
        bool isInTestMode = false)
    {
        string nextPageLink = "", previousPageLink = "";

        if (answerQuestionModel.HasNextPage)
            nextPageLink = answerQuestionModel.NextUrl(Url);

        if (answerQuestionModel.HasPreviousPage)
            previousPageLink = answerQuestionModel.PreviousUrl(Url);

        var menuHtml = Empty;

        var answerBody = ViewRenderer.RenderPartialView(
            "~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx",
            new AnswerBodyModel(answerQuestionModel, isInLearningTab, isInTestMode),
            ControllerContext);
        var learningSession = LearningSessionCache.GetLearningSession();
        var serializer = new JavaScriptSerializer();
        
        var serializedPageData = serializer.Serialize(new
        {
            answerBodyAsHtml = answerBody,
            navBarData = new
            {
                nextUrl = nextPageLink,
                previousUrl = previousPageLink,
                currentHtml = isSession ? null : ViewRenderer.RenderPartialView(
                    "~/Views/Questions/Answer/AnswerQuestionPager.ascx",
                    answerQuestionModel,
                    ControllerContext
                )
            },
            sessionData = isSession ? new
            {
                currentStepIdx = learningSession.CurrentIndex,
                skipStepIdx = sessionData.SkipStepIdx,
                isLastStep = sessionData.IsLastStep,
                currentStepGuid = sessionData.CurrentStepGuid,
                currentSessionHeader = sessionData.CurrentSessionHeader,
                learningSessionId = sessionData.LearningSessionId,
                isLearningSession = !learningSession.Config.InWishknowledge,
                stepCount = learningSession.Steps.Count 
            } : null,
            url = currentUrl,
            commentsAsHtml = ViewRenderer.RenderPartialView("~/Views/Questions/Answer/Comments/CommentsSection.ascx", answerQuestionModel, ControllerContext),
            offlineDevelopment = Settings.DevelopOffline(),
            menuHtml,
            isInTestMode
        });

        return serializedPageData;
    }

    private class SessionData
    {
        public SessionData(string currentSessionHeader = "", int currentStepIdx = -1, bool isLastStep = false, int skipStepIdx = -1, int learningSessionId = -1)
        {
            CurrentSessionHeader = currentSessionHeader;
            SkipStepIdx = skipStepIdx;
            IsLastStep = isLastStep;
            LearningSessionId = learningSessionId;
        }

        public string CurrentSessionHeader { get; private set; }
        public int SkipStepIdx { get; private set; }
        public bool IsLastStep { get; private set; }
        public Guid CurrentStepGuid { get; private set; }
        public int LearningSessionId { get; private set; }
    }
    
    [SetThemeMenu(isLearningSessionPage: true)]
    public string RenderLearningSessionResult(LearningSession learningSession, bool isInTestMode = false)
    {
        var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
        return serializer.Serialize(
            new
            {
                LearningSessionResult =
                ViewRenderer.RenderPartialView(
                    "~/Views/Questions/Answer/LearningSession/LearningSessionResultInner.ascx",
                    new LearningSessionResultModel(learningSession, isInTestMode), ControllerContext),
                url = "",
                offlineDevelopment = Settings.DevelopOffline()
            }
        );
    }

    public EmptyResult ClearHistory()
    {
        _sessionUiData.VisitedQuestions = new QuestionHistory();
        return new EmptyResult();
    }

    [HttpPost]
    public int GetQuestionCount(LearningSessionConfig config = null, int categoryId= -1)
    {
        if (config == null)
        {
            var c = new LearningSessionConfig();
            c.AllQuestions = true;
            c.CategoryId = categoryId;
            c.MaxProbability = 100;
            c.CurrentUserId = Sl.CurrentUserId;

            if (c.IsMyWorld())
            {
                c.InWishknowledge = true;
                c.CreatedByCurrentUser = true; 
            }

            return LearningSessionCreator.GetQuestionCount(c);
        }
        config.CurrentUserId = Sl.SessionUser.UserId;

        if (config.IsMyWorld())
        {
            config.IsNotQuestionInWishKnowledge = false;
        }
        return LearningSessionCreator.GetQuestionCount(config);
    }

    [HttpPost]
    public string GetEditQuestionUrl(int id)
    {
        if (!IsLoggedIn)
            return null;

        var question = _questionRepo.GetById(id);
        var isAuthor = question.Creator == _sessionUser.User;
        if (IsInstallationAdmin  || isAuthor)
            return Links.EditQuestion(question.Text, id);

        return null;
    }
}

public class SafeLearningSessionOptionsHelper
{
   public bool IsInTestmode { get; set; }
   public bool AllQuestions { get; set; }
   public bool UserIsAuthor { get; set; }
   public bool IsNotQuestionInWishKnowledge { get; set; }
   public bool QuestionsInWishknowledge { get; set; }
   public int MinProbability { get; set; }
   public int MaxProbability { get; set; }
   public int MaxQuestionCount { get; set; }
   public bool Repititions { get; set;}
   public bool AnswerHelp { get; set;  }
}