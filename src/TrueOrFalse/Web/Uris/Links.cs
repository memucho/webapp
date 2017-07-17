using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Uris;
using static System.String;

namespace TrueOrFalse.Frontend.Web.Code
{
    public static class Links
    {
        public const string WelcomeController = "Welcome";
        public const string RegisterController = "Register";

        public const string VariousController = "VariousPublic";
        public static string TermsAndConditions => GetUrlHelper().Action("TermsAndConditions", VariousController);
        public static string Imprint => GetUrlHelper().Action("Imprint", VariousController);

        public const string KnowledgeController = "Knowledge";
        public const string KnowledgeAction = "Knowledge";
        public static string Knowledge() => GetUrlHelper().Action(KnowledgeAction, KnowledgeController);

        public const string HelpController = "Help";
        public const string HelpActionFAQ = "FAQ";
        public static string HelpFAQ() => GetUrlHelper().Action(HelpActionFAQ, HelpController);
        public const string HelpWillkommen = "Willkommen";
        public const string HelpWunschwissen = "Willkommen";
        public static string HelpWidget() => GetUrlHelper().Action("Widget", HelpController);
        public static string HelpWidgetWordpress() => GetUrlHelper().Action("WidgetInWordpress", HelpController);
        public static string HelpWidgetMoodle() => GetUrlHelper().Action("WidgetInMoodle", HelpController);
        public static string HelpWidgetBlackboard() => GetUrlHelper().Action("WidgetInBlackboard", HelpController);
        public static string WidgetPricing() => GetUrlHelper().Action("WidgetPricing", HelpController);
        public static string WidgetExamples() => GetUrlHelper().Action("WidgetExamples", HelpController);

        public const string AccountController = "Account";
        public const string RegisterAction = "Register";
        public const string RegisterSuccess = "RegisterSuccess";
        public const string Logout = "Logout";
        public const string MembershipAction = "Membership";
        public static string Membership() => GetUrlHelper().Action(MembershipAction, AccountController);
        public static string BetaInfo() => GetUrlHelper().Action("MemuchoBeta", VariousController);

        public static UrlHelper GetUrlHelper() => new UrlHelper(HttpContext.Current.Request.RequestContext);

        /* About */
        public const string AboutController = "About";

        public static string AboutMemucho() => GetUrlHelper().Action("AboutMemucho", AboutController);
        public static string WelfareCompany() => GetUrlHelper().Action("WelfareCompany", AboutController);
        public static string Jobs() => GetUrlHelper().Action("Jobs", AboutController);
        public static string ForTeachers() => GetUrlHelper().Action("ForTeachers", AboutController);

        /* AlgoInsight */
        public const string AlgoInsightController = "AlgoInsight";
        public static string AlgoInsightForecast() => GetUrlHelper().Action("Forecast", AlgoInsightController);

        /*Users*/
        public const string UserController = "User";
        public const string UserAction = "User";

        public const string UserSettingsAction = "UserSettings";
        public const string UserSettingsController = "UserSettings";
        public static string UserSettings() => GetUrlHelper().Action(UserSettingsAction, UserSettingsController);

        public static string UserLoginAs(UrlHelper url, int userId) => url.Action("LoginAs", "Users", new {userId = userId});
        public static string UserDetail(User user) => UserDetail(user.Name, user.Id);

        public static string UserDetail(string userName, int userId)
        {
            return GetUrlHelper().Action(UserAction, UserController,
                new { name = UriSegmentFriendlyUser.Run(userName), id = userId }, null);
        }

        public static string UserDetailBadges(User user)
        {
            return GetUrlHelper().Action("Badges", UserController,
                new { name = UriSegmentFriendlyUser.Run(user.Name), id = user.Id }, null);
        }

        public static string Register() => GetUrlHelper().Action(RegisterAction, RegisterController);

        public const string UsersController = "Users";
        public const string UsersAction = "Users";
        public static string Users() => GetUrlHelper().Action(UsersAction, UsersController);

        public const string NetworkAction = "Network";
        public static string Network() => GetUrlHelper().Action(NetworkAction, UsersController);


        /*Question*/
        public const string Questions = "Questions";
        public const string QuestionsController = "Questions";
        public const string QuestionsMineAction = "QuestionsMine";
        public const string QuestionsWishAction = "QuestionsWish";
        public const string EditQuestionController = "EditQuestion";

        public const string AnswerQuestionController = "AnswerQuestion";

        public static string QuestionsAll() => GetUrlHelper().Action(Questions, QuestionsController);
        public static string QuestionsMine() => GetUrlHelper().Action(QuestionsMineAction, QuestionsController);
        public static string QuestionsWish() => GetUrlHelper().Action(QuestionsWishAction, QuestionsController);

        public static string QuestionWithCategoryFilter(UrlHelper url, MenuModelCategoryItem modelCategoryItem){
            return modelCategoryItem.SearchUrl + "Kat__" + modelCategoryItem.Category.Name + "__";
        }

        public static string QuestionWithCategoryFilter(UrlHelper url, Category category){
            return "/Fragen/Suche/Kategorie/" + UriSanitizer.Run(category.Name) + "/" + category.Id;
        }

        public static string QuestionWithCategoryFilter(UrlHelper url, string categoryName, int categoryId){
            return "/Fragen/Suche/Kategorie/" + UriSanitizer.Run(categoryName) + "/" + categoryId;
        }

        public static string QuestionWish_WithCategoryFilter(Category category) => "/Fragen/Wunschwissen/Suche/Kategorie/" + UriSanitizer.Run(category.Name) + "/" + category.Id;

        public static string QuestionWithCreatorFilter(UrlHelper url, User user) => "/Fragen/Suche/" + "Ersteller__" + user.Name + "__";

        public static string QuestionSearch(string searchTerm) => "/Fragen/Suche/" + searchTerm;
        public static string CategoriesSearch(string searchTerm) => "/Kategorien/Suche/" + searchTerm;
        public static string SetsSearch(string searchTerm) => "/Fragesaetze/Suche/" + searchTerm;
        public static string UsersSearch(string searchTerm) => "/Nutzer/Suche/" + searchTerm;


        public static string AnswerQuestion(Question question, Set set) => AnswerQuestion(GetUrlHelper(), question, set);

        public static string AnswerQuestion(UrlHelper url, Question question, Set set){
            return url.Action("Answer", AnswerQuestionController, 
                new { text = UriSegmentFriendlyQuestion.Run(question.Text), questionId = question.Id, setId = set.Id });
        }
        public static string AnswerQuestion(UrlHelper url, string questionText, int questionId, int setId) => 
            url.Action("Answer", AnswerQuestionController,
                new { text = UriSegmentFriendlyQuestion.Run(questionText), questionId, setId});

        public static string AnswerQuestion(QuestionSearchSpec searchSpec) => "/AnswerQuestion/Answer?pager=" + searchSpec?.Key;

        public static string AnswerQuestion(Question question, int paramElementOnPage = 1, string pagerKey = "", string categoryFilter = ""){
            return AnswerQuestion(question.Text, question.Id, paramElementOnPage, pagerKey, categoryFilter);
        }

        public static string AnswerQuestion(Question question) => AnswerQuestion(question, -1);

        public static string AnswerQuestion(string questionText, int questionId, int paramElementOnPage = 1, string pagerKey = "", string categoryFilter = "")
        {
            if (paramElementOnPage == -1)
            {
                return GetUrlHelper().Action("Answer", AnswerQuestionController,
                    new
                    {
                        text = UriSegmentFriendlyQuestion.Run(questionText),
                        id = questionId
                    }, null);
            }

            return GetUrlHelper().Action("Answer", AnswerQuestionController,
                new {
                    text = UriSegmentFriendlyQuestion.Run(questionText), 
                    id = questionId, 
                    elementOnPage = paramElementOnPage, 
                    pager = pagerKey, 
                    category = categoryFilter
                }, null);
        }

        public static string CreateQuestion(int categoryId = -1, int setId = -1)
        {
            var url = GetUrlHelper();

            if (categoryId != -1)
                return url.Action("Create", EditQuestionController, new { categoryId = categoryId });

            if (setId != -1)
                return url.Action("Create", EditQuestionController, new { setId = setId });

            return url.Action("Create", EditQuestionController);
        }

        public static string EditQuestion(Question question) => EditQuestion(GetUrlHelper(), question.Text, question.Id);
        public static string EditQuestion(string questionText, int questionId) => EditQuestion(GetUrlHelper(), questionText, questionId);

        public static string EditQuestion(UrlHelper url, string questionText, int questionId){
            return url.Action("Edit", EditQuestionController, new { text = UriSanitizer.Run(questionText), id = questionId });
        }

        public static string SendAnswer(UrlHelper url, Question question){
            return url.Action("SendAnswer", AnswerQuestionController, new { id = question.Id }, null);
        }

        public static string SendAnswer(UrlHelper url, Question question, 
            Game game, Player player, Round round)
        {
            return url.Action("SendAnswerGame", "Play", 
                new{
                    questionId = question.Id, 
                    gameId = game.Id,
                    userId = player.Id,
                    roundId = round.Id
                }, null);
        }

        public static string SendAnswer(UrlHelper url, Question question,
            LearningSession learningSession, LearningSessionStep learningSessionStep)
        {
            return url.Action("SendAnswerLearningSession", AnswerQuestionController,
                new { id = question.Id, learningSessionId = learningSession.Id, stepGuid = learningSessionStep.Guid }, null);
        }

        public static string GetSolution(UrlHelper url, Question question){
            return url.Action("GetSolution", AnswerQuestionController, new { id = question.Id }, null);
        }

        public static string GetSolution(UrlHelper url, Question question, Round round){
            return url.Action("GetSolution", AnswerQuestionController, new { id = question.Id, roundId = round.Id }, null);
        }

        public static string CountLastAnswerAsCorrect(UrlHelper url, Question question){
            return url.Action("CountLastAnswerAsCorrect", AnswerQuestionController, new { id = question.Id }, null);
        }

        public static string CountUnansweredAsCorrect(UrlHelper url, Question question){
            return url.Action("CountUnansweredAsCorrect", AnswerQuestionController, new { id = question.Id }, null);
        }


        /*Dates*/
        public const string DatesController = "Dates";
        public const string DateCreateAction = "Create";
        public const string DateEditController = "EditDate";
        public static string DateCreate() { return GetUrlHelper().Action(DateCreateAction, DateEditController); }

        public static string Dates() => GetUrlHelper().Action("Dates", "Dates");
        public static object DateEdit(int dateId) => GetUrlHelper().Action("Edit", DateEditController, new { dateId = dateId });
        public static object DateCreateForSet(int setId) => GetUrlHelper().Action("Create", DateEditController, new { setId = setId });
        public static string DateCreateForSets(List<int> setIds, string setListTitle)
        {
            return GetUrlHelper().Action("Create", DateEditController, new { setListTitle })
                + "&setIds="
                + Join("&setIds=", setIds);
        }

        public static object DateCreateForCategory(int categoryId) => GetUrlHelper().Action("Create", "EditDate", new { categoryId = categoryId });


        /*Learn*/
        public const string LearningSessionResultController = "LearningSessionResult";

        public static string LearningSession(LearningSession learningSession)
        {
            return GetUrlHelper().Action("Learn", AnswerQuestionController,
                new
                {
                    learningSessionId = learningSession.Id,
                    learningSessionName = learningSession.UrlName,
                });
        }

        public static string StartLearningSession(LearningSession learningSession)
        {
            if (learningSession.IsSetSession)
                return StartLearningSesssionForSet(learningSession.SetToLearn.Id);

            if (learningSession.IsSetsSession)
                return StartLearningSessionForSets(learningSession.SetsToLearn().Select(s => s.Id).ToList(), learningSession.SetListTitle);

            if (learningSession.IsCategorySession)
                return StartCategoryLearningSession(learningSession.CategoryToLearn.Id);

            if (learningSession.IsDateSession)
                return StartDateLearningSession(learningSession.DateToLearn.Id);

            throw new Exception("unknown type");
        }

        public static string StartDateLearningSession(int dateId) =>
            GetUrlHelper().Action("StartLearningSession", DatesController, new { dateId = dateId });

        public static string StartWishLearningSession() =>
            GetUrlHelper().Action("StartLearningSession", KnowledgeController );

        public static string StartCategoryLearningSession(int categoryId) =>
           GetUrlHelper().Action("StartLearningSession", CategoryController, new { categoryId = categoryId });

        public static string StartLearningSesssionForSet(int setId)
        {
            return GetUrlHelper().Action("StartLearningSession", SetController, new { setId = setId });
        }
        public static string StartLearningSessionForSets(List<int> setIds, string setListTitle)
        {
            return GetUrlHelper().Action("StartLearningSessionForSets", CategoryController, new { setListTitle })
                + "&setIds="
                + Join("&setIds=", setIds);
        }

        /* Testing / TestSession*/
        public const string TestSessionController = "TestSession";
        public const string TestSessionResultController = "TestSessionResult";
        public const string TestSessionResultAction = "TestSessionResult";

        public static string TestSession(string uriName, int testSessionId) => 
            GetUrlHelper().Action("Test", AnswerQuestionController, new { name = uriName, testSessionId = testSessionId });

        public static string TestSessionStartForSet(string setName, int setId) => 
            GetUrlHelper().Action("StartTestSession", SetController, new { setName = UriSanitizer.Run(setName), setId = setId });

        public static string TestSessionStartForCategory(string categoryName, int categoryId) => 
            GetUrlHelper().Action("StartTestSession", CategoryController, new { categoryName = UriSanitizer.Run(categoryName), categoryId = categoryId });

        public static string TestSessionStartForSets(List<int> setIds, string setListTitle)
        {
            return GetUrlHelper().Action("StartTestSessionForSets", SetController, new { setListTitle })
                + "&setIds="
                + Join("&setIds=", setIds);
        }
        public static string TestSessionStartForSetsInCategory(List<int> setIds, string setListTitle, int categoryId)
        {
            return GetUrlHelper().Action("StartTestSessionForSetsInCategory", CategoryController, new { setListTitle, categoryId })
                + "&setIds="
                + Join("&setIds=", setIds);
        }

        public static string TestSessionRegisterQuestionAnswered(UrlHelper url) => url.Action("RegisterQuestionAnswered", TestSessionController);
        public static string LearningSessionAmendAfterShowSolution(UrlHelper url) => url.Action("AmendAfterShowSolution", AnswerQuestionController);

        /*Questionsets / Sets*/
        public const string SetsController = "Sets";
        public const string SetsAction = "Sets";
        public const string SetsWishAction = "SetsWish";
        public const string SetsMineAction = "SetsMine";
        public static string SetsAll() { return GetUrlHelper().Action(SetsAction, SetsController); }
        public static string SetsWish() { return GetUrlHelper().Action(SetsWishAction, SetsController); }
        public static string SetsMine() { return GetUrlHelper().Action(SetsMineAction, SetsController); }
        public static string SetCreate() { return GetUrlHelper().Action(SetCreateAction, SetEditController); }
        public const string SetController = "Set";
        public const string SetCreateAction = "Create";
        public const string SetEditController = "EditSet";

        public static string SetDetail(UrlHelper url, SetMini setMini) => SetDetail(url, setMini.Name, setMini.Id);
        public static string SetDetail(UrlHelper url, Set set) => SetDetail(url, set.Name, set.Id);
        public static string SetDetail(Set set) => SetDetail(set.Name, set.Id);
        public static string SetDetail(string name, int id) => SetDetail(GetUrlHelper(), name, id);

        public static string SetDetail(UrlHelper url, string name, int id){
            return url.Action("QuestionSet", "Set",
                new { text = UriSanitizer.Run(name), id = id}, null);            
        }

        public static string QuestionSetEdit(string name, int questionSetId) => QuestionSetEdit(GetUrlHelper(), name, questionSetId);

        public static string QuestionSetEdit(UrlHelper url, string name, int questionSetId){
            return url.Action("Edit", "EditSet", new { text = UriSanitizer.Run(name), id = questionSetId });
        }

        /* Messages */
        public static string Messages(UrlHelper url) => url.Action("Messages","Messages");
        public static string MessageSetRead(UrlHelper url) => url.Action("SetMessageRead", "Messages");
        public static object MessageSetUnread(UrlHelper url) => url.Action("SetMessageUnread", "Messages");

        /* Games */
        public static string Games() => Games(GetUrlHelper());
        public static string Games(UrlHelper url) => url.Action("Games", "Games");
        public static string GameCreateFromDate(int dateId) => GetUrlHelper().Action("Create", "Game", new {dateId = dateId});
        public static string GameCreateFromSet(int setId) => GetUrlHelper().Action("Create", "Game", new { setId = setId});
        public static string GameCreateFromSets(List<int> setIds) => GetUrlHelper().Action("Create", "Game") + "?setIds="
                + Join("&setIds=", setIds);
        public static string GameCreateFromCategory(int categoryId) => GetUrlHelper().Action("Create", "Game", new { categoryId = categoryId });



        public static string GameCreate() => GetUrlHelper().Action("Create", "Game", null);
        public static string GamePlay(UrlHelper url, int gameId) => GetUrlHelper().Action("Play", "Play", new { gameId = gameId });

        /*Category*/
        public const string CategoriesAction = "Categories";
        public const string CategoriesWishAction = "CategoriesWish";
        public const string CategoriesController = "Categories";
        public const string CategoryController = "Category";
        public const string CategoryEditController = "EditCategory";
        public const string CategoryCreateAction = "Create";
        public static string CategoriesAll() => GetUrlHelper().Action(CategoriesAction, CategoriesController);
        public static string CategoriesWish() => GetUrlHelper().Action("CategoriesWish", CategoriesController);
        public static string CategoryCreate() => GetUrlHelper().Action(CategoryCreateAction, CategoryEditController);
        public static string CategoryDetail(Category category) => CategoryDetail(category.Name, category.Id);

        public static string CategoryDetail(string name, int id) => 
            GetUrlHelper().Action("Category", CategoryController, new { text = UriSanitizer.Run(name), id = id }, null);

        public static string GetUrl(object type)
        {
            if (type == null)
                return "";

            if (type is Category)
                return CategoryDetail((Category) type);

            if (type is Set)
                return SetDetail((Set)type);

            if (type is Question)
                return AnswerQuestion((Question)type);

            throw new Exception("unexpected type");
        }

        public static string CategoryEdit(Category category) => CategoryEdit(GetUrlHelper(), category.Name, category.Id);
        public static string CategoryEdit(UrlHelper url, string name, int id) => url.Action("Edit", "EditCategory", new { text = UriSanitizer.Run(name), id = id });

        public static string FAQItem(string itemNameInView) => GetUrlHelper().Action("FAQ", "Help") + "#" + itemNameInView;
        public static string Directions => GetUrlHelper().Action("Directions", "Welcome");

        public static string ErrorNotLoggedIn(string backTo) => GetUrlHelper().Action("_NotLoggedIn", "Error", new {backTo = backTo});

        public static bool IsLinkToWikipedia(string url)
        {
            if (IsNullOrEmpty(url))
                return false;

            return Regex.IsMatch(url, "https?://.{0,3}wikipedia.");
        }
    }
}
 
