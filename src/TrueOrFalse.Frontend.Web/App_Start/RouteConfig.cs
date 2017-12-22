using System.Web.Mvc;
using System.Web.Routing;

namespace TrueOrFalse
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            // das hier später per Konvention, siehe: http://mvccontrib.codeplex.com/SourceControl/changeset/view/351a6de404cb#src%2fMVCContrib%2fSimplyRestful%2fSimplyRestfulRouteHandler.cs

            routes.MapRoute("Logout", "Ausloggen", new { controller = "Welcome", action = "Logout" });
            routes.MapRoute("CheckUserNameForAvailability", "Registrieren/IsUserNameAvailable", new { controller = "Login", action = "IsUserNameAvailable" });
            routes.MapRoute("CheckEmailForAvailability", "Registrieren/IsEmailAvailable", new { controller = "Login", action = "IsEmailAvailable" });
            routes.MapRoute("PasswordRecovery", "Login/PasswortZuruecksetzen", new { controller = "Welcome", action = "PasswordRecovery" });
            routes.MapRoute("Contact", "Kontakt", new { controller = "Welcome", action = "Contact" });

            routes.MapRoute("Register", "Registrieren", new { controller = "Register", action = "Register" });
            routes.MapRoute("Login", "Login/{action}", new { controller = "Login" });

            routes.MapRoute("Widget", "Hilfe/Widget", new { controller = "Help", action = "Widget" });
            routes.MapRoute("WidgetInWordpress", "Hilfe/memucho-Quiz-in-Wordpress-einbetten", new { controller = "Help", action = "WidgetInWordpress" });
            routes.MapRoute("WidgetInMoodle", "Hilfe/memucho-Quiz-in-Moodle-einbetten", new { controller = "Help", action = "WidgetInMoodle" });
            routes.MapRoute("WidgetInBlackboard", "Hilfe/memucho-Quiz-in-Blackboard-einbetten", new { controller = "Help", action = "WidgetInBlackboard" });
            routes.MapRoute("WidgetPricing", "Widget-Angebote-Preisliste", new { controller = "Help", action = "WidgetPricing" });
            routes.MapRoute("WidgetExamples", "Widget-Beispiele", new { controller = "Help", action = "WidgetExamples" });
            routes.MapRoute("Help", "Hilfe/{action}", new { controller = "Help", action = "FAQ" });

            routes.MapRoute("UsersSearchApi", "Nutzer/SucheApi", new { controller = "Users", action = "SearchApi", searchTerm = UrlParameter.Optional });
            routes.MapRoute("UsersSearch", "Nutzer/Suche/{searchTerm}", new { controller = "Users", action = "Search", searchTerm = UrlParameter.Optional });
            routes.MapRoute("UserSettings", "Nutzer/Einstellungen", new { controller = "UserSettings", action = "UserSettings" });
            routes.MapRoute("UserAccountMembership", "Nutzer/Mitgliedschaft", new { controller = "Account", action = "Membership" });
            routes.MapRoute("UserAccountWidgetStats", "Nutzer/Widget-Statistik", new { controller = "Account", action = "WidgetStats" });
            routes.MapRoute("UserLoginAs", "Nutzer/LoginAs/{userId}", new { controller = "Users", action= "LoginAs" });
            routes.MapRoute("User", "Nutzer/{name}/{id}/{action}", new { controller = "User", action = "User" });
            routes.MapRoute("Users", "Nutzer", new { controller = "Users", action = "Users" }); //included: , page = UrlParameter.Optional 
            routes.MapRoute("Users_Network", "Netzwerk", new { controller = "Users", action = "Network"});

            routes.MapRoute("Questions_MineSearchApi", "Fragen/Meine/SucheApi", new { controller = "Questions", action = "QuestionsMineSearchApi" }, new[] { "TrueOrFalse" });
            routes.MapRoute("Questions_MineSearch", "Fragen/Meine/Suche/{searchTerm}", new { controller = "Questions", action = "QuestionsMineSearch", searchTerm = UrlParameter.Optional }, new[]{"TrueOrFalse"});
            routes.MapRoute("Questions_Mine", "Fragen/Meine", new { controller = "Questions", action = "QuestionsMine" }, new[]{"TrueOrFalse"});
            routes.MapRoute("Questions_WishSearchApi", "Fragen/Wunschwissen/SucheApi", new { controller = "Questions", action = "QuestionsWishSearchApi"}, new[] { "TrueOrFalse" });
            routes.MapRoute("Questions_WishSearch", "Fragen/Wunschwissen/Suche/{searchTerm}", new { controller = "Questions", action = "QuestionsWishSearch", searchTerm = UrlParameter.Optional }, new[] { "TrueOrFalse" });
            routes.MapRoute("Questions_WishSearchFilterCategory", "Fragen/Wunschwissen/Suche/Kategorie/{categoryName}/{categoryId}", new { controller = "Questions", action = "QuestionsWishSearchCategoryFilter" }, new[] { "TrueOrFalse" });
            routes.MapRoute("Questions_Wish", "Fragen/Wunschwissen", new { controller = "Questions", action = "QuestionsWish" }, new[] { "TrueOrFalse" });
            routes.MapRoute("Questions_SearchApi", "Fragen/SucheApi", new { controller = "Questions", action = "QuestionsSearchApi" }, new[] { "TrueOrFalse" });
            routes.MapRoute("Questions_SearchFilterCategory", "Fragen/Suche/Kategorie/{categoryName}/{categoryId}", new { controller = "Questions", action = "QuestionsSearchCategoryFilter"}, new[] { "TrueOrFalse" });
            routes.MapRoute("Questions_Search", "Fragen/Suche/{searchTerm}", new { controller = "Questions", action = "QuestionsSearch", searchTerm = UrlParameter.Optional }, new[] { "TrueOrFalse" });
            routes.MapRoute("Questions", "Fragen", new { controller = "Questions", action = "Questions" }, new[] { "TrueOrFalse" });

            routes.MapRoute("Question_Delete", "Fragen/Loesche/{id}", new { controller = "Questions", action = "Delete" }, new[] { "TrueOrFalse" });
            routes.MapRoute("Question_Create", "Fragen/Erstelle/", new { controller = "EditQuestion", action = "Create" });
            routes.MapRoute("Question_StoreImage", "Fragen/Bearbeite/StoreImage", new { controller = "EditQuestion", action = "StoreImage" });
            routes.MapRoute("Question_SolutionEditBody", "Fragen/Bearbeite/SolutionEditBody", new { controller = "EditQuestion", action = "SolutionEditBody" });
            routes.MapRoute("Question_ReferencePartial", "Fragen/Bearbeite/ReferencePartial", new { controller = "EditQuestion", action = "ReferencePartial" });
            routes.MapRoute("Question_Edit", "Fragen/{text}/Bearbeite/{id}", new { controller = "EditQuestion", action = "Edit" });
            routes.MapRoute("Question_AnswerInSet", "Fragen/{text}/{questionId}/im-Fragesatz/{setId}", new { controller = "AnswerQuestion", action = "Answer" });
            routes.MapRoute("Question_Answer", "Fragen/{text}/{id}/{elementOnPage}", new { controller = "AnswerQuestion", action = "Answer", elementOnPage = UrlParameter.Optional });

            routes.MapRoute("LearningSession_Result", "Lernen/{learningSessionName}/Ergebnis/{learningSessionId}", new { controller = "LearningSessionResult", action = "LearningSessionResult" });
            routes.MapRoute("Question_Answer_Learn", "Lernen/{learningSessionName}/{learningSessionId}", new { controller = "AnswerQuestion", action = "Learn" });

            routes.MapRoute("TestSession_Result", "Testen/{name}/Ergebnis/{testSessionId}", new { controller = "TestSessionResult", action = "TestSessionResult" });
            routes.MapRoute("Question_Answer_Test", "Testen/{name}/{testSessionId}", new { controller = "AnswerQuestion", action = "Test" });
            /* API */ routes.MapRoute("TestSession_RegisterAnsweredQuestion", "TestSession/RegisterAnsweredQuestion/", new { controller = "TestSession", action = "RegisterQuestionAnswered" });

            /* API */
            routes.MapRoute("Questions_DeleteDetails", "Questions/DeleteDetails/{questionId}", new { controller = "Questions", action = "DeleteDetails" });
            /* API */ routes.MapRoute("Questions_Delete", "Questions/Delete/{questionId}", new { controller = "Questions", action = "Delete" });
            /* API */ routes.MapRoute("Question_SendAnswer", "Questions/SendAnswer/{id}", new { controller = "AnswerQuestion", action = "SendAnswer" });
            /* API */ routes.MapRoute("Question_GetSolution", "Questions/GetSolution/{id}", new { controller = "AnswerQuestion", action = "GetSolution" });
            /* API */ routes.MapRoute("Question_CountLastAnswerAsCorrect", "Questions/CountLastAnswerAsCorrect/{id}", new { controller = "AnswerQuestion", action = "CountLastAnswerAsCorrect" });
            /* API */ routes.MapRoute("Question_SaveQuality", "Questions/SaveQuality/{id}/{newValue}", new { controller = "AnswerQuestion", action = "SaveQuality" });
            /* API */ routes.MapRoute("Question_SaveRelevancePersonal", "Questions/SaveRelevancePersonal/{id}/{newValue}", new { controller = "AnswerQuestion", action = "SaveRelevancePersonal" });
            /* API */ routes.MapRoute("Question_SaveRelevanceForAll", "Questions/SaveRelevanceForAll/{id}/{newValue}", new { controller = "AnswerQuestion", action = "SaveRelevanceForAll" });

            routes.MapRoute("Sets_MineSearchApi", "Fragesaetze/Meine/SucheApi", new { controller = "Sets", action = "SetsMineSearchApi" });
            routes.MapRoute("Sets_Mine", "Fragesaetze/Meine", new { controller = "Sets", action = "SetsMine" });
            routes.MapRoute("Sets_Wish", "Fragesaetze/Wunschwissen", new { controller = "Sets", action = "SetsWish" }); //}, new[] { "TrueOrFalse" }); ?
            routes.MapRoute("Sets_WishSearch", "Fragesaetze/Wunschwissen/Suche/{searchTerm}", new { controller = "Sets", action = "SetsWishSearch", searchTerm = UrlParameter.Optional });
            routes.MapRoute("Sets_WishSearchApi", "Fragesaetze/Wunschwissen/SucheApi", new { controller = "Sets", action = "SetsWishSearchApi" });
            routes.MapRoute("Sets_Search", "Fragesaetze/Suche/{searchTerm}", new { controller = "Sets", action = "SetsSearch", searchTerm = UrlParameter.Optional });
            routes.MapRoute("Sets_SearchApi", "FrageSaetze/SucheApi", new { controller = "Sets", action = "SetsSearchApi" });            
            routes.MapRoute("Set_Create", "Fragesaetze/Erstelle/", new { controller = "EditSet", action = "Create" });
            routes.MapRoute("Sets_Edit", "Fragesaetze/{text}/Bearbeite/{id}", new { controller = "EditSet", action = "Edit" });
            routes.MapRoute("Set_Detail_Id", "Fragesaetze/ById/{id}", new { controller = "Set", action = "QuestionSetById" }); //route is used when creating games/dates and adding sets via ajax (AutocompleteSets.ts)
            routes.MapRoute("Set_Detail", "Fragesaetze/{text}/{id}", new { controller = "Set", action = "QuestionSet" });
            routes.MapRoute("Sets", "Fragesaetze/{action}", new { controller = "Sets", action = "Sets" });
            /* API */ routes.MapRoute("Sets_DeleteDetails", "Sets/DeleteDetails/{setId}", new { controller = "Sets", action = "DeleteDetails" });
            /* API */ routes.MapRoute("Sets_Delete", "Sets/Delete/{setId}", new { controller = "Sets", action = "Delete" });
            /* API */ routes.MapRoute("Sets_SaveRelevancePersonal", "Sets/SaveRelevancePersonal/{id}/{newValue}", new { controller = "Sets", action = "SaveRelevancePersonal" });

            routes.MapRoute("Set_StartLearningSession", "Fragesatz/Lernen/{setId}", new { controller = "Set", action = "StartLearningSession" });
            routes.MapRoute("Set_StartTestSession", "Fragesatz/Testen/{setName}/{setId}", new { controller = "Set", action = "StartTestSession" });

            /* API */ routes.MapRoute("Set_ChangeIndicies", "Set/UpdateQuestionsOrder", new { controller = "EditSet", action = "UpdateQuestionsOrder" });
            /* API */ routes.MapRoute("Set_ImageUpload", "Set/UploadImage/{id}", new { controller = "EditSet", action = "UploadImage", id = UrlParameter.Optional });

            routes.MapRoute("Categories_WishSearch", "Kategorien/Wunschwissen/Suche/{searchTerm}", new { controller = "Categories", action = "CategoriesWish", searchTerm = UrlParameter.Optional });
            routes.MapRoute("Categories_WishSearchApi", "Kategorien/Wunschwissen/SucheApi", new { controller = "Categories", action = "SearchApiWish" });
            routes.MapRoute("Categories", "Kategorien", new { controller = "Categories", action = "Categories" });
            routes.MapRoute("Categories_SearchApi", "Kategorien/SucheApi", new { controller = "Categories", action = "SearchApi" });
            routes.MapRoute("Categories_Search", "Kategorien/Suche/{searchTerm}", new { controller = "Categories", action = "Search", searchTerm = UrlParameter.Optional });
            routes.MapRoute("Categories_Create", "Kategorien/Erstelle/{type}", new { controller = "EditCategory", action = "Create", type = UrlParameter.Optional });
            routes.MapRoute("Categories_Delete", "Kategorien/Loesche/{id}", new { controller = "Categories", action = "Delete" });
            routes.MapRoute("Categories_ById", "Kategorien/{id}", new { controller = "Category", action = "CategoryById" }); // route is used when creating question/questionset (AutocompleteCategories.ts) and adding categories via ajax
            routes.MapRoute("Categories_Edit", "Kategorien/{text}/Bearbeite/{id}", new { controller = "EditCategory", action = "Edit" });
            routes.MapRoute("Category_Detail", "Kategorien/{text}/{id}", new { controller = "Category", action = "Category" });
            /* API */ routes.MapRoute("Categories_AddSubCategoryRow", "Categories/AddSubCategoryRow", new { controller = "EditCategory", action = "AddSubCategoryRow" });
            /* API */ routes.MapRoute("Categories_EditSubCategoryItems", "Categories/EditSubCategoryItems/{id}", new { controller = "EditSubCategoryItems", action = "Edit" });
            /* API */ routes.MapRoute("Categories_AddSubCategoryItemRow", "Categories/EditSubCategoryItems/{id}/Add", new { controller = "EditSubCategoryItems", action = "AddSubCategoryItemRow" });

            routes.MapRoute("Category_StartTestSession", "Kategorie/Testen/{categoryName}/{categoryId}", new { controller = "Category", action = "StartTestSession" });

            routes.MapRoute("Knowledge_Learn", "Lernen/Wunschwissen", new { controller = "Knowledge", action = "StartLearningSession" });
            routes.MapRoute("Knowledge", "Wissenszentrale/{action}", new { controller = "Knowledge", action = "Knowledge" });
            routes.MapRoute("Knowledge_ConfirmEmail", "EmailBestaetigen/{emailKey}", new { controller = "Knowledge", action = "EmailConfirmation" });

            routes.MapRoute("AlgoInsight", "AlgoInsight/{action}", new { controller = "AlgoInsight", action = "AlgoInsight" });

            routes.MapRoute("Maintenance", "Maintenance/{action}", new { controller = "Maintenance", action = "Maintenance" });
            routes.MapRoute("MaintenanceImages", "MaintenanceImages/{action}", new { controller = "MaintenanceImages", action = "Images" });
            routes.MapRoute("Messages", "Nachrichten/{action}", new { controller = "Messages", action = "Messages" });

            routes.MapRoute("Dates_StartLearningSession", "Termin/Lernen/{dateId}", new { controller = "Dates", action = "StartLearningSession" });
            routes.MapRoute("Dates", "Termine/{action}", new { controller = "Dates", action = "Dates" });
            routes.MapRoute("DatesCreate", "Termin/Erstellen", new { controller = "EditDate", action = "Create" });
            routes.MapRoute("DatesEdit", "Termin/Bearbeiten/{dateId}", new { controller = "EditDate", action = "Edit" });
            routes.MapRoute("DatesDetails", "Termin/Details/{0}", new { controller = "Date", action = "Edit" });

            routes.MapRoute("Games", "Spielen/{action}", new { controller = "Games", action = "Games" });
            routes.MapRoute("GamesCreate", "Spiel/Erstellen", new { controller = "Game", action = "Create" });
            routes.MapRoute("GamesPlay", "Spiel/{gameId}", new { controller = "Play", action = "Play" });

            routes.MapRoute("AboutMemucho", "Ueber-memucho", new { controller = "About", action = "AboutMemucho" });
            routes.MapRoute("ForTeachers", "Fuer-Lehrer", new { controller = "About", action = "ForTeachers" });
            routes.MapRoute("WelfareCompany", "Gemeinwohlökonomie", new { controller = "About", action = "WelfareCompany" });
            routes.MapRoute("Jobs", "Jobs", new { controller = "About", action = "Jobs" });
            routes.MapRoute("Master", "Master", new { controller = "About", action = "Master" });

            routes.MapRoute("Beta", "Beta", new { controller = "Beta", action = "Beta" });
            routes.MapRoute("MemuchoBeta", "Beta-Phase", new { controller = "VariousPublic", action = "MemuchoBeta" });
            routes.MapRoute("Imprint", "Impressum", new { controller = "VariousPublic", action = "Imprint" });
            routes.MapRoute("TermsAndConditions", "AGB", new { controller = "VariousPublic", action = "TermsAndConditions" });
            routes.MapRoute("ApiExport", "Api/Export/{action}", new { controller = "Export", action = "Export" });
            routes.MapRoute("ApiCategory", "Api/Category/{action}", new { controller = "CategoryApi" });
            routes.MapRoute("ApiCategoryStatistics", "Api/CategoryStatistics/{action}", new { controller = "CategoryStatisticsApi" });
            routes.MapRoute("ApiSets", "Api/Sets/{action}", new { controller = "SetsApi" });
            routes.MapRoute("ApiUserFacebook", "Api/FacebookUsers/{action}", new { controller = "FacebookUsersApi" });
            routes.MapRoute("ApiUserGoogle", "Api/GoogleUsers/{action}", new { controller = "GoogleUsersApi" });
            routes.MapRoute("ApiQuestions", "Api/Questions/{action}", new { controller = "QuestionsApi" });
            routes.MapRoute("ApiSearch", "Api/Search/{action}", new { controller = "SearchApi" });
            routes.MapRoute("ApiActivityPoints", "Api/ActivityPoints/{action}", new { controller = "ActivityPointsApi" });
            routes.MapRoute("ApiLom", "Api/Lom/{action}", new { controller = "LomApi" });

            routes.MapRoute("ImageUpload", "Images/ImageUpload/{action}", new { controller = "ImageUpload" });
            
            routes.MapRoute("Images", "Images/{action}", new { controller = "Images", action = "Images" });

            routes.MapRoute("404", "Fehler/404", new { controller = "Error", action = "_404" });
            routes.MapRoute("500", "Fehler/500", new { controller = "Error", action = "_500" });
            routes.MapRoute("NotLoggedIn", "Fehler/NichtAngemeldet", new { controller = "Error", action = "_NotLoggedIn" });

            routes.MapRoute("RedirectHorseCertificate1", "Pferde-Basispass", new { controller = "Redirect", action = "ToHorseCertificate" });
            routes.MapRoute("Redirect1", "umfrage/{googleCode}", new { controller = "redirect", action = "to", id = UrlParameter.Optional });
            routes.MapRoute("Redirect2", "redirect/to/{googleCode}", new { controller = "redirect", action = "to", id = UrlParameter.Optional });
            routes.MapRoute("Redirect3", "f/{googleCode}", new { controller = "redirect", action = "to", id = UrlParameter.Optional });

            routes.MapRoute("Topic", "Thema/{topicName}", new { controller = "Topics", action = "Topic" });

            routes.MapRoute("WidgetQuestion", "widget/frage/{questionId}", new { controller = "Widget", action = "Question" });
            routes.MapRoute("WidgetSetStart", "widget/fragesatz/start/{setId}", new { controller = "Widget", action = "Set" });
            routes.MapRoute("WidgetSet", "widget/fragesatz/{setId}", new { controller = "Widget", action = "SetStart" });
            routes.MapRoute("WidgetLearningSession", "widget/fragesatz/templateset/{setId}", new { controller = "Widget", action = "SetWithoutStartScreen" });
            routes.MapRoute("WidgetSetVideo", "widget/fragesatz-v/{setId}", new { controller = "Widget", action = "SetVideo" });

            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Welcome", action = "Welcome", id = UrlParameter.Optional });
            routes.MapRoute("Various", "{action}", new { controller = "VariousPublic" });
        }
    }
}