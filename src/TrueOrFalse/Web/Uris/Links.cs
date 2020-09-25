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

        public const string AccountController = "Account";
        public const string RegisterAction = "Register";
        public const string RegisterSuccess = "RegisterSuccess";
        public const string Logout = "Logout";
        public const string MembershipAction = "Membership";
        public static string Membership() => GetUrlHelper().Action(MembershipAction, AccountController);
        public static string WidgetStats() => GetUrlHelper().Action("WidgetStats", AccountController);
        public static string BetaInfo() => GetUrlHelper().Action("MemuchoBeta", VariousController);

        public static UrlHelper GetUrlHelper()
        {
            var res = new UrlHelper(HttpContext.Current.Request.RequestContext);
            res.RemoveRoutes(new[] {"version"});
            return res;
        }

        /* Category Footer*/

        public static string LearningSessionFooter(int categoryId, string categoryName) =>
            "/" + categoryName + "/" + categoryId + "/Lernen";
        public static string AnalyticsFooter(int categoryId, string categoryName) =>
            "/" + categoryName + "/" + categoryId + "/Wissensnetz";

        /* About */
        public const string AboutController = "About";

        public static string Team() => GetUrlHelper().Action("Team", WelcomeController);
        public static string AboutMemucho() => GetUrlHelper().Action("AboutMemucho", AboutController);
        public static string WelfareCompany() => GetUrlHelper().Action("WelfareCompany", AboutController);
        public static string Jobs() => GetUrlHelper().Action("Jobs", AboutController);
        public static string ForTeachers() => GetUrlHelper().Action("ForTeachers", AboutController);
        public static string Promoter() => GetUrlHelper().Action("Promoter", WelcomeController);

        /* Welcome */
        public static string Welcome() => GetUrlHelper().Action("Welcome", WelcomeController);
        public static string WelcomeLinks(string name, int Id)  { return "/"+ name + "/" + Id; }

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
        public static string UserDetail(IUserTinyModel user) => UserDetail(user.Name, user.Id);

        public static string UserDetail(string userName, int userId)
        {
            return GetUrlHelper().Action(UserAction, UserController,
                new { name = UriSegmentFriendlyUser.Run(userName), id = userId }, null);
        }

        public static string UserDetailBadges(User user)
        {
            var userTiny = new UserTinyModel(user);
            return GetUrlHelper().Action("Badges", UserController,
                new { name = UriSegmentFriendlyUser.Run(userTiny.Name), id = userTiny.Id }, null);
        }

        public static string Register() => GetUrlHelper().Action(RegisterAction, RegisterController);

        public const string UsersController = "Users";
        public const string UsersAction = "Users";
        public static string Users() => GetUrlHelper().Action(UsersAction, UsersController);

        public const string NetworkAction = "Network";
        public static string Network() => GetUrlHelper().Action(NetworkAction, UsersController);
        /* Images*/
        public const string NoCategoryUrl = "/Images/no-category-picture-350.png";
        public const string NoQuestionUrl = "/Images/no-question-128.png";
        public static string GoogleMapsPreviewPath(string imageName) => "/Images/GoogleMapsPreview/" + imageName;
        public static string CCLicenses(string imageName) => "/Images/Licenses/" + imageName;


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

        public static string QuestionWithCreatorFilter(UrlHelper url, User user) => user != null
            ? "/Fragen/Suche/" + "Ersteller__" + user.Name + "__"
            : "/Fragen/Suche/" + "Ersteller__unbekannt__";





        public static string QuestionSearch(string searchTerm) => "/Fragen/Suche/" + searchTerm;
        public static string CategoriesSearch(string searchTerm) => "/Suche/" + searchTerm;
        public static string SetsSearch(string searchTerm) => "/Fragesaetze/Suche/" + searchTerm;
        public static string UsersSearch(string searchTerm) => "/Nutzer/Suche/" + searchTerm;

        public static string QuestionHistory(int questionId) =>
            GetUrlHelper().Action("List", "QuestionHistory", new { questionId = questionId });

        public static string QuestionHistoryDetail(int questionId, int revisionId) =>
            GetUrlHelper().Action("Detail", "QuestionHistoryDetail", new { questionId = questionId, revisionId = revisionId});

        public static string QuestionRestore(int questionId, int questionChangeId) =>
            GetUrlHelper().Action("Restore", QuestionsController, new { questionId = questionId, questionChangeId = questionChangeId });

        public static string QuestionChangesOverview(int pageToShow) =>
            GetUrlHelper().Action("List", "QuestionChangesOverview", new { pageToShow = pageToShow });

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

        public static string AnswerQuestion(Question question) => 
            HttpContext.Current == null
            ? ""
            : AnswerQuestion(question, -1);

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

        /*Learn*/
        public const string LearningSessionResultController = "LearningSessionResult";

        public static string LearningSession(LearningSessionNew learningSession)
        {
            //return GetUrlHelper().Action("Learn", AnswerQuestionController,
            //    new
            //    {
            //        learningSessionId = 1,
            //        learningSessionName = learningSession.UrlName,
            //    });
            return "#";
        }

        public static string StartCategoryLearningSession(int categoryId) =>
           GetUrlHelper().Action("StartLearningSession", CategoryController, new { categoryId = categoryId });

        public static string StartLearningSessionForSet(int setId)
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

        public static string TestSession(string categoryName, int categoryId) =>
            CategoryDetailLearningTab(categoryName, categoryId);

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
        public static string SetEdit(Set set) => SetEdit(GetUrlHelper(), set.Name, set.Id);
        public static string SetEdit(UrlHelper url, string name, int id) => url.Action("Edit", "EditSet", new { text = UriSanitizer.Run(name), id = id });
        public const string SetController = "Set";
        public const string SetCreateAction = "Create";
        public const string SetEditController = "EditSet";

        public static string SetDetail(UrlHelper url, SetMini setMini) => SetDetail(url, setMini.Name, setMini.Id);
        public static string SetDetail(UrlHelper url, Set set) => SetDetail(url, set.Name, set.Id);
        public static string SetDetail(Set set) => 
            HttpContext.Current == null
            ? ""
            : SetDetail(set.Name, set.Id);
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

        /*Category*/
        public const string CategoriesAction = "Categories";
        public const string CategoriesWishAction = "CategoriesWish";
        public const string CategoriesController = "Categories";
        public const string CategoryController = "Category";
        public const string CategoryNewController = "CategoryNew";
        public const string CategoryEditController = "EditCategory";
        public const string CategoryCreateAction = "Create";
        public static string CategoriesAll() => GetUrlHelper().Action(CategoriesAction, CategoriesController);
        public static string CategoriesWish() => GetUrlHelper().Action("CategoriesWish", CategoriesController);
        public static string CategoryCreate() => GetUrlHelper().Action(CategoryCreateAction, CategoryEditController);
        public static string CategoryCreate(int parentCategoryId) => GetUrlHelper().Action("Create", "EditCategory", new { parent = parentCategoryId });
        //public static string Category(Category category) => UriSanitizer.Run(category.Name) + "/" + category.Id;

        public static string CategoryHistoryDetail(int categoryId, int categoryChangeId) => 
            GetUrlHelper().Action("Detail", "CategoryHistoryDetail", new { categoryId  = categoryId , categoryChangeId = categoryChangeId });

        public static string CategoryHistory(int categoryId) =>
            GetUrlHelper().Action("List", "CategoryHistory", new { categoryId = categoryId });

        public static string CategoryDetail(Category category) =>
            HttpContext.Current == null 
                ? "" 
                : CategoryDetail(category.Name, category.Id);

        public static string CategoryDetail(Category category, int version) =>
            HttpContext.Current == null 
                ? "" 
                : CategoryDetail(category.Name, category.Id, version);

        public static string CategoryDetail(string name, int id) =>
            GetUrlHelper().Action("Category", CategoryController, new { text = UriSanitizer.Run(name), id = id });

        public static string CategoryDetailAnalyticsTab(Category category) =>
            CategoryDetail(category) + "/Wissensnetz";
        public static string CategoryDetailAnalyticsTab(string name, int id) =>
            CategoryDetail(name, id) + "/Wissensnetz";
        public static string CategoryDetailLearningTab(string name, int id) =>
            CategoryDetail(name, id) + "/Lernen";
        public static string CategoryDetailLearningTab(Category category) =>
            CategoryDetail(category) + "/Lernen";
        

        public static string CategoryDetailRedirect(string name, int id) =>
            GetUrlHelper().Action("Category","Category" ,new { text = UriSanitizer.Run(name), id = id });

        public static string CategoryDetail(string name, int id, int version) => 
            GetUrlHelper().Action("Category", CategoryController, new { text = UriSanitizer.Run(name), id = id, version = version }, null);

        public static string CategoryRestore(int categoryId, int categoryChangeId) => 
            GetUrlHelper().Action("Restore", CategoryController, new { categoryId = categoryId, categoryChangeId = categoryChangeId});

        public static string CategoryChangesOverview(int pageToShow) =>
            GetUrlHelper().Action("List", "CategoryChangesOverview", new { pageToShow = pageToShow });

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
        public static string CategoryEdit(string name, int id) => CategoryEdit(GetUrlHelper(), name, id);
        public static string CategoryEdit(UrlHelper url, string name, int id) => url.Action("Edit", "EditCategory", new { text = UriSanitizer.Run(name), id = id });
        

        public static string FAQItem(string itemNameInView) => GetUrlHelper().Action("FAQ", "Help") + "#" + itemNameInView;
        public static string Contact => GetUrlHelper().Action("Contact", "Welcome");

        public static string ErrorNotLoggedIn(string backTo) => GetUrlHelper().Action("_NotLoggedIn", "Error", new {backTo = backTo});

        public static bool IsLinkToWikipedia(string url)
        {
            if (IsNullOrEmpty(url))
                return false;

            return Regex.IsMatch(url, "https?://.{0,3}wikipedia.");
        }


        // Partner

        public static string Tutory = "https://www.tutory.de";
        public static string TutoryImage = "/Images/LogosPartners/Logo_tutory_250px.png";
    }
}
 
