using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHibernate.Mapping;
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
    private const string _viewLocationError = "~/Views/Questions/Answer/AnswerQuestionError.aspx";

    public AnswerQuestionController(QuestionRepo questionRepo, AnswerQuestion answerQuestion)
    {
        _questionRepo = questionRepo;
        _answerQuestion = answerQuestion;
    }

    [SetMainMenu(MainMenuEntry.None)]
    [SetThemeMenu(isQuestionPage: true)]
    public ActionResult Answer(string text, int? id, int? elementOnPage, string pager, int? questionId, string category)
    {
        if (id.HasValue && SeoUtils.HasUnderscores(text))
            return SeoUtils.RedirectToHyphendVersion(RedirectPermanent, id.Value);

        return AnswerQuestion(text, id, elementOnPage, pager, category);
    }

    [SetThemeMenu(isLearningSessionPage: true)]
    public ActionResult Learn(int learningSessionId, string learningSessionName, int skipStepIdx = -1)
    {
        var learningSession = Sl.LearningSessionRepo.GetById(learningSessionId);

        if (learningSession.User != _sessionUser.User)
            throw new Exception("not logged in or not possessing user");

        if (skipStepIdx != -1 && learningSession.Steps.Any(s => s.Idx == skipStepIdx))
        {
            learningSession.SkipStep(skipStepIdx);
            return RedirectToAction("Learn", Links.AnswerQuestionController,
                new {learningSessionId, learningSessionName = learningSessionName});
        }

        var currentLearningStepIdx = learningSession.CurrentLearningStepIdx();

        if (currentLearningStepIdx == -1) //None of the steps is uncompleted
            return RedirectToAction("LearningSessionResult", Links.LearningSessionResultController,
                new {learningSessionId, learningSessionName = learningSessionName});

        var questionViewGuid = Guid.NewGuid();

        Sl.SaveQuestionView.Run(
            questionViewGuid,
            learningSession.Steps[currentLearningStepIdx].Question,
            _sessionUser.User.Id,
            learningSession: learningSession,
            learningSessionStepGuid: learningSession.Steps[currentLearningStepIdx].Guid);

        return View(_viewLocation,
            new AnswerQuestionModel(questionViewGuid, Sl.Resolve<LearningSessionRepo>().GetById(learningSessionId)));
    }

    [SetThemeMenu(isTestSessionPage: true)]
    public ActionResult Test(int testSessionId)
    {
        return TestActionShared(
            testSessionId,
            testSession => RedirectToAction(
                Links.TestSessionResultAction,
                Links.TestSessionResultController,
                new {name = testSession.UriName, testSessionId = testSessionId}
            ),
            (testSession, questionViewGuid, question) => View(
                _viewLocation,
                new AnswerQuestionModel(testSession, questionViewGuid, question))
        );
    }

    public static ActionResult TestActionShared(
        int testSessionId,
        Func<TestSession, ActionResult> redirectToFinalStepFunc,
        Func<TestSession, Guid, Question, ActionResult> resultFunc,
        Func<TestSession, WidgetView> widgetViewFunc = null
    )
    {
        var sessionUser = Sl.SessionUser;

        var sessionCount = sessionUser.TestSessions.Count(s => s.Id == testSessionId);

        if (sessionCount == 0)
        {
            //Logg.r().Error("SessionCount 0");
            //return View(_viewLocation, AnswerQuestionModel.CreateExpiredTestSession());
            throw new Exception("SessionCount is 0. Shoult be 1");
        }

        if (sessionCount > 1)
            throw new Exception(
                $"SessionCount is {sessionUser.TestSessions.Count(s => s.Id == testSessionId)}. Should be not more then more than 1.");

        var testSession = sessionUser.TestSessions.Find(s => s.Id == testSessionId);

        if (testSession.CurrentStepIndex > testSession.NumberOfSteps)
            return redirectToFinalStepFunc(testSession);

        var question = Sl.R<QuestionRepo>().GetById(testSession.Steps.ElementAt(testSession.CurrentStepIndex - 1).QuestionId);
        var questionViewGuid = Guid.NewGuid();

        Sl.SaveQuestionView.Run(questionViewGuid, question, sessionUser.User, widgetViewFunc?.Invoke(testSession));

        return resultFunc(testSession, questionViewGuid, question);
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

            var questionViewGuid2 = Guid.NewGuid();
            Sl.SaveQuestionView.Run(questionViewGuid2, question2, _sessionUser.User);

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
    public JsonResult SendAnswer(int id, string answer, Guid questionViewGuid, int interactionNumber, int millisecondsSinceQuestionView)
    {
        var result = _answerQuestion.Run(id, answer, UserId, questionViewGuid, interactionNumber,
            millisecondsSinceQuestionView);
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
                    : null
            }
        };
    }

    [HttpPost]
    public JsonResult SendAnswerLearningSession(
        int id,
        int learningSessionId,
        Guid questionViewGuid,
        int interactionNumber,
        Guid stepGuid,
        string answer,
        int millisecondsSinceQuestionView,
        bool inTestMode = false
    )
    {
        //var timeOfAnswer = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(timeOfAnswerString));

        var result = _answerQuestion.Run(id, answer, UserId, questionViewGuid, interactionNumber,
            millisecondsSinceQuestionView, learningSessionId, stepGuid, inTestMode);
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
    public JsonResult AmendAfterShowSolution(int learningSessionId, Guid stepGuid, bool isInTestMode = false)
    {
        var learningSessionStep = LearningSession.GetStep(learningSessionId, stepGuid);
        var learningSession = Sl.R<LearningSessionRepo>().GetById(learningSessionId);

        learningSessionStep.AnswerState = StepAnswerState.ShowedSolutionOnly;

        bool newStepAdded = !(learningSession.LimitForThisQuestionHasBeenReached(learningSessionStep) || learningSession.LimitForNumberOfRepetitionsHasBeenReached() || isInTestMode);
        learningSession.UpdateAfterWrongAnswerOrShowSolution(learningSessionStep, isInTestMode);

        return new JsonResult
        {
            Data = new
            {
                numberSteps = learningSession.Steps.Count,
                newStepAdded = newStepAdded
            }
        };
    }

    [HttpPost]
    public JsonResult GetSolution(int id, string answer, Guid questionViewGuid, int interactionNumber, int? roundId,
        int millisecondsSinceQuestionView = -1, int LearningSessionId = -1, string LearningSessionStepGuidString = "")
    {
        var question = _questionRepo.GetById(id);
        var solution = GetQuestionSolution.Run(question);

        if (IsLoggedIn)
            if (roundId == null)
                if (LearningSessionStepGuidString != "" && LearningSessionId != -1)
                {
                    var LearningSessionStepGuid = new Guid(LearningSessionStepGuidString);
                    R<AnswerLog>()
                        .LogAnswerView(question, this.UserId, questionViewGuid, interactionNumber,
                            millisecondsSinceQuestionView, null, LearningSessionId, LearningSessionStepGuid);
                }
                else
                {
                    R<AnswerLog>()
                        .LogAnswerView(question, this.UserId, questionViewGuid, interactionNumber,
                            millisecondsSinceQuestionView);
                }
            else
                R<AnswerLog>()
                    .LogAnswerView(question, this.UserId, questionViewGuid, interactionNumber,
                        millisecondsSinceQuestionView, roundId);

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
            var learningSession = Sl.Resolve<LearningSessionRepo>().GetById((int) learningSessionId);
            ControllerContext.RouteData.Values.Add("learningSessionId", learningSessionId);
            ControllerContext.RouteData.Values.Add("learningSessionName", learningSession.UrlName);
            var learningSessionQuestionViewGuid = Guid.NewGuid();

            answerQuestionModel = new AnswerQuestionModel(learningSessionQuestionViewGuid, learningSession, isMobileDevice);
            if (hideAddToKnowledge.HasValue)
                answerQuestionModel.DisableAddKnowledgeButton = hideAddToKnowledge.Value;

            return ViewRenderer.RenderPartialView(
                "~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx",
                new AnswerBodyModel(answerQuestionModel),
                ControllerContext
            );
        }
        if (testSessionId != null)
        {
            var sessionUser = Sl.SessionUser;
            var testSession = sessionUser.TestSessions.Find(s => s.Id == testSessionId);
            var testSessionQuestion = Sl.QuestionRepo.GetById(testSession.Steps.ElementAt(testSession.CurrentStepIndex - 1).QuestionId);
            var testSessionQuestionViewGuid = Guid.NewGuid();
            var testSessionName = testSession.UriName;

            ControllerContext.RouteData.Values.Add("testSessionId", testSessionId);
            ControllerContext.RouteData.Values.Add("name", testSessionName);

            answerQuestionModel = new AnswerQuestionModel(testSession, testSessionQuestionViewGuid, testSessionQuestion, isMobileDevice);
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

    //AnswerBodyLoader: AnswerBody + NavBar + PageUrl
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
        var model = new AnswerQuestionModel(questionViewGuid, question, activeSearchSpec);

        var currentUrl = Links.AnswerQuestion(question, elementOnPage, activeSearchSpec.Key);
        return GetQuestionPageData(model, currentUrl, new SessionData());
    }

    [HttpPost]
    public string RenderNewAnswerBodySessionForCategory(SessionConfigData sessionConfigData)
    {
        var answerBody = "";
        var categoryId = sessionConfigData.CategoryId;
        var isInLearningTab = sessionConfigData.IsInLearningTab;
        var questionFilter = sessionConfigData.QuestionFilter;
        var sessionUser = _sessionUser;

        var filterQuestions = questionFilter != null;

        if (sessionUser.IsLoggedIn)
            answerBody = RenderAnswerBodyForNewCategoryLearningSession(categoryId, isInLearningTab, questionFilter);
        else
            answerBody = RenderAnswerBodyForNewCategoryTestSession(categoryId, isInLearningTab, filterQuestions, questionFilter);

        return answerBody; 
    }

    public string RenderAnswerBodyForNewCategoryLearningSession(int categoryId, bool isInLearningTab = false, QuestionFilterJson questionFilter = null)
    {
        var learningSession = CreateLearningSession.ForCategory(categoryId, questionFilter);
        var isInTestMode = false;
        if (questionFilter != null)
            isInTestMode = questionFilter.IsTestMode();
        return RenderAnswerBodyByLearningSession(learningSession.Id, isInLearningTab: isInLearningTab, isInTestMode:isInTestMode);
    }

    public string RenderAnswerBodyForNewCategoryTestSession(int categoryId, bool isInLearningTab = false, bool filterQuestions = false, QuestionFilterJson questionFilter = null)
    {   var category =  Sl.CategoryRepo.GetByIdEager(categoryId);

        var sessionuser = new SessionUser();
        var testSession = filterQuestions ? new TestSession(category, questionFilter) : new TestSession(category);

        sessionuser.AddTestSession(testSession);
     
        return RenderAnswerBodyByTestSession(testSession.Id, includeTestSessionHeader:true, isInLearningTab: isInLearningTab);
    }

    
    public string RenderAnswerBodyByLearningSession(int learningSessionId, int skipStepIdx = -1, bool isInLearningTab = false, bool isInTestMode = false)
    {
        var learningSession = Sl.LearningSessionRepo.GetById(learningSessionId);

        var learningSessionName = learningSession.UrlName;

        if (skipStepIdx != -1 && learningSession.Steps.Any(s => s.Idx == skipStepIdx))
            learningSession.SkipStep(skipStepIdx);

        var currentLearningStepIdx = learningSession.CurrentLearningStepIdx();

        if (learningSession.CurrentLearningStepIdx() == -1)
            return RenderLearningSessionResult(learningSessionId, isInTestMode);

        var questionViewGuid = Guid.NewGuid();

        var question = Sl.QuestionRepo.GetById(learningSession.Steps[currentLearningStepIdx].Question.Id);

        Sl.SaveQuestionView.Run(
            questionViewGuid,
            question,
            _sessionUser.User.Id,
            learningSession: learningSession,
            learningSessionStepGuid: learningSession.Steps[currentLearningStepIdx].Guid);

        var model = new AnswerQuestionModel(questionViewGuid, Sl.Resolve<LearningSessionRepo>().GetById(learningSession.Id));

        ControllerContext.RouteData.Values.Add("learningSessionId", learningSession.Id);
        ControllerContext.RouteData.Values.Add("learningSessionName", learningSessionName);

        string currentSessionHeader = "Frage <span id = \"CurrentStepNumber\">" + (model.CurrentLearningStepIdx + 1) + "</span> von <span id=\"StepCount\">" + model.LearningSession.Steps.Count + "</span>";
        int currentStepIdx = currentLearningStepIdx;
        bool isLastStep = model.IsLastLearningStep;
        Guid currentStepGuid = model.LearningSessionStep.Guid;
        string currentUrl = Links.LearningSession(learningSession);

        var sessionData = new SessionData(currentSessionHeader, currentStepIdx, isLastStep, skipStepIdx, currentStepGuid, learningSession.Id);

        return GetQuestionPageData(model, currentUrl, sessionData, isSession: true, isInLearningTab:isInLearningTab, isInTestMode:isInTestMode);
    }

    public string RenderAnswerBodyByTestSession(int testSessionId, bool includeTestSessionHeader = false, bool isInLearningTab = false)
    {
        var sessionUser = Sl.SessionUser;
        var testSession = sessionUser.TestSessions.Find(s => s.Id == testSessionId);

        var question = Sl.R<QuestionRepo>().GetById(testSession.Steps.ElementAt(testSession.CurrentStepIndex - 1).QuestionId);
        var questionViewGuid = Guid.NewGuid();

        Sl.SaveQuestionView.Run(questionViewGuid, question, sessionUser.User);

        var model = new AnswerQuestionModel(testSession, questionViewGuid, question);

        ControllerContext.RouteData.Values.Add("testSessionId", testSessionId);
        ControllerContext.RouteData.Values.Add("name", testSession.UriName);

        string currentSessionHeader = "Frage " + model.TestSessionCurrentStep + " von " + model.TestSessionNumberOfSteps;
        int currentStepIdx = model.TestSessionCurrentStep;
        bool isLastStep = model.TestSessionIsLastStep;
        string currentUrl = Links.TestSession(testSession.UriName, testSessionId);

        var sessionData = new SessionData(currentSessionHeader, currentStepIdx, isLastStep);

        return GetQuestionPageData(model, currentUrl, sessionData, isSession: true, testSessionId: testSessionId, includeTestSessionHeader: includeTestSessionHeader, isInLearningTab: isInLearningTab);
    }

    public string RenderUpdatedQuestionDetails(int questionId, bool showCategoryList = true)
    {
        var question = Sl.QuestionRepo.GetById(questionId);
        var model = new AnswerQuestionModel(question, showCategoryList:showCategoryList);

        return ViewRenderer.RenderPartialView("~/Views/Questions/Answer/AnswerQuestionDetails.ascx", model, ControllerContext);
    }

    private string GetQuestionPageData(
        AnswerQuestionModel model, 
        string currentUrl, 
        SessionData sessionData, 
        bool isSession = false,
        int testSessionId = -1,
        bool includeTestSessionHeader = false,
        bool isInLearningTab = false,
        bool isInTestMode = false)
    {
        string nextPageLink = "", previousPageLink = "";

        if (model.HasNextPage)
            nextPageLink = model.NextUrl(Url);

        if (model.HasPreviousPage)
            previousPageLink = model.PreviousUrl(Url);

        var menuHtml = Empty;
        if (model.Set == null && !isSession)
        {
            Sl.SessionUiData.TopicMenu.PageCategories = ThemeMenuHistoryOps.GetQuestionCategories(model.Question.Id);
            menuHtml = ViewRenderer.RenderPartialView("~/Views/Categories/Navigation/CategoryNavigation.ascx", new CategoryNavigationModel(), ControllerContext);
        }

        var testSessionHeader = "";
        if (includeTestSessionHeader)
            testSessionHeader = ViewRenderer.RenderPartialView("~/Views/Questions/Answer/TestSession/TestSessionHeader.ascx", model, ControllerContext);

        var answerBody = ViewRenderer.RenderPartialView(
            "~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx",
            new AnswerBodyModel(model, isInLearningTab, isInTestMode),
            ControllerContext);

        var serializer = new JavaScriptSerializer();
        var serializedPageData = serializer.Serialize(new
        {
            answerBodyAsHtml = testSessionHeader + answerBody,
            navBarData = new
            {
                nextUrl = nextPageLink,
                previousUrl = previousPageLink,
                currentHtml = isSession ? null : ViewRenderer.RenderPartialView(
                    "~/Views/Questions/Answer/AnswerQuestionPager.ascx",
                    model,
                    ControllerContext
                )
            },
            sessionData = isSession ? new
            {
                currentStepIdx = sessionData.CurrentStepIdx,
                skipStepIdx = sessionData.SkipStepIdx,
                isLastStep = sessionData.IsLastStep,
                currentStepGuid = sessionData.CurrentStepGuid,
                currentSessionHeader = sessionData.CurrentSessionHeader,
                learningSessionId = sessionData.LearningSessionId,
                testSessionId = testSessionId
            } : null,
            url = currentUrl,
            questionDetailsAsHtml = ViewRenderer.RenderPartialView("~/Views/Questions/Answer/AnswerQuestionDetails.ascx", model, ControllerContext),
            commentsAsHtml = ViewRenderer.RenderPartialView("~/Views/Questions/Answer/Comments/CommentsSection.ascx", model, ControllerContext),
            offlineDevelopment = Settings.DevelopOffline(),
            menuHtml,
            isInTestMode = isInTestMode
        });

        return serializedPageData;
    }

    private class SessionData
    {
        public SessionData(string currentSessionHeader = "", int currentStepIdx = -1, bool isLastStep = false, int skipStepIdx = -1, Guid currentStepGuid = new Guid(), int learningSessionId = -1)
        {
            CurrentSessionHeader = currentSessionHeader;
            CurrentStepIdx = currentStepIdx;
            SkipStepIdx = skipStepIdx;
            IsLastStep = isLastStep;
            CurrentStepGuid = currentStepGuid;
            LearningSessionId = learningSessionId;
        }

        public string CurrentSessionHeader { get; private set; }
        public int CurrentStepIdx { get; private set; }
        public int SkipStepIdx { get; private set; }
        public bool IsLastStep { get; private set; }
        public Guid CurrentStepGuid { get; private set; }
        public int LearningSessionId { get; private set; }
    }

    [SetThemeMenu(isLearningSessionPage: true)]
    public string RenderLearningSessionResult(int learningSessionId, bool isInTestMode = false)
    {
        var learningSession = Sl.Resolve<LearningSessionRepo>().GetById(learningSessionId);

        if (learningSession.User != _sessionUser.User)
            throw new Exception("not logged in or not possessing user");

        if (!learningSession.IsCompleted)
        {
            learningSession.CompleteSession();
        }

        if (learningSession.IsDateSession)
        {
            TrainingPlanUpdater.Run(learningSession.DateToLearn.TrainingPlan);
        }

        var currentUrl = Links.LearningSessionResult(learningSession);

        var serializer = new JavaScriptSerializer();
        return serializer.Serialize(
            new
            {
                LearningSessionResult =
                ViewRenderer.RenderPartialView(
                    "~/Views/Questions/Answer/LearningSession/LearningSessionResultInner.ascx",
                    new LearningSessionResultModel(learningSession, isInTestMode: isInTestMode), ControllerContext),
                url = currentUrl,
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
    public string ShareQuestionModal(int questionId) =>
        ViewRenderer.RenderPartialView("~/Views/Questions/Answer/ShareQuestionModal.ascx", new ShareQuestionModalModel(questionId), ControllerContext);

    [HttpPost]
    public int GetQuestionCount(int categoryId, QuestionFilterJson questionFilter)
    {
        return CreateLearningSession.QuestionsInLearningSessionCount(categoryId, questionFilter, IsLoggedIn);
    }
}