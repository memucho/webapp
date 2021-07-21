using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
            routes.MapRoute("Team", "Team", new {controller = "Welcome", action = "Team"});

            routes.MapRoute("Register", "Registrieren", new { controller = "Register", action = "Register" });
            routes.MapRoute("Login", "Login/{action}", new { controller = "Login" });

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

            routes.MapRoute("Question_GetData", "Question/GetData/{id}", new { controller = "Question", action = "GetData" });
            routes.MapRoute("Question_Delete", "Fragen/Loesche/{id}", new { controller = "Question", action = "Delete" }, new[] { "TrueOrFalse" });
            routes.MapRoute("Question_Create", "Fragen/Erstelle/", new { controller = "EditQuestion", action = "Create" });
            routes.MapRoute("Question_Create_Flashcard", "Question/CreateFlashcard/", new { controller = "EditQuestion", action = "CreateFlashcard" });
            routes.MapRoute("Question_StoreImage", "Fragen/Bearbeite/StoreImage", new { controller = "EditQuestion", action = "StoreImage" });
            routes.MapRoute("Question_SolutionEditBody", "Fragen/Bearbeite/SolutionEditBody", new { controller = "EditQuestion", action = "SolutionEditBody" });
            routes.MapRoute("Question_ReferencePartial", "Fragen/Bearbeite/ReferencePartial", new { controller = "EditQuestion", action = "ReferencePartial" });
            routes.MapRoute("Question_Edit", "Fragen/{text}/Bearbeite/{id}", new { controller = "EditQuestion", action = "Edit" });
            routes.MapRoute("Question_AnswerInSet", "Fragen/{text}/{questionId}/im-Fragesatz/{setId}", new { controller = "AnswerQuestion", action = "Answer" });
            routes.MapRoute("Question_Answer", "Fragen/{text}/{id}/{elementOnPage}", new { controller = "AnswerQuestion", action = "Answer", elementOnPage = UrlParameter.Optional });
            routes.MapRoute("GetQuestionEditUrl", "Question/GetEditUrl", new { controller = "AnswerQuestion", action = "GetEditQuestionUrl" });
            routes.MapRoute("Question_Vue_Create", "Question/Create", new { controller = "EditQuestion", action = "VueCreate" });
            routes.MapRoute("Question_Vue_Edit", "Question/Edit", new { controller = "EditQuestion", action = "VueEdit" });


            routes.MapRoute("LearningSession_Result", "Lernen/{learningSessionName}/Ergebnis/{learningSessionId}", new { controller = "LearningSessionResult", action = "LearningSessionResult" });
            routes.MapRoute("Question_Answer_Learn", "Lernen/{learningSessionName}/{learningSessionId}", new { controller = "AnswerQuestion", action = "Learn" });

            routes.MapRoute("TestSession_Result", "Testen/{name}/Ergebnis/{testSessionId}", new { controller = "TestSessionResult", action = "TestSessionResult" });
            routes.MapRoute("Question_Answer_Test", "Testen/{name}/{testSessionId}", new { controller = "AnswerQuestion", action = "Test" });
            /* API */ routes.MapRoute("TestSession_RegisterAnsweredQuestion", "TestSession/RegisterAnsweredQuestion/", new { controller = "TestSession", action = "RegisterQuestionAnswered" });

            /* API */
            routes.MapRoute("Questions_DeleteDetail", "Question/DeleteDetails/{questionId}", new { controller = "Question", action = "DeleteDetails" });
            /* API */ routes.MapRoute("Questions_Delete", "Question/Delete/{questionId}", new { controller = "Question", action = "Delete" });
            /* API */ routes.MapRoute("Question_SendAnswer", "Questions/SendAnswer/{id}", new { controller = "AnswerQuestion", action = "SendAnswer" });
            /* API */ routes.MapRoute("Question_GetSolution", "Questions/GetSolution/{id}", new { controller = "AnswerQuestion", action = "GetSolution" });
            /* API */ routes.MapRoute("Question_CountLastAnswerAsCorrect", "Questions/CountLastAnswerAsCorrect/{id}", new { controller = "AnswerQuestion", action = "CountLastAnswerAsCorrect" });
            /* API */ routes.MapRoute("Question_SaveQuality", "Questions/SaveQuality/{id}/{newValue}", new { controller = "AnswerQuestion", action = "SaveQuality" });
            /* API */ routes.MapRoute("Question_SaveRelevancePersonal", "Questions/SaveRelevancePersonal/{id}/{newValue}", new { controller = "AnswerQuestion", action = "SaveRelevancePersonal" });
            /* API */ routes.MapRoute("Question_SaveRelevanceForAll", "Questions/SaveRelevanceForAll/{id}/{newValue}", new { controller = "AnswerQuestion", action = "SaveRelevanceForAll" });

            routes.MapRoute("Category_Changes_Overview", "Historie/{pageToShow}", new { controller = "CategoryChangesOverview", action = "List" });
            routes.MapRoute("Categories", "Kategorien", new { controller = "Categories", action = "Categories" });
            routes.MapRoute("Categories_Create", "Erstelle/{type}", new { controller = "EditCategory", action = "Create", type = UrlParameter.Optional });
            routes.MapRoute("Categories_Delete", "Loesche/{id}", new { controller = "Categories", action = "Delete" });
            routes.MapRoute("Categories_ById", "Kategorien/{id}", new { controller = "CategoryRedirect", action = "CategoryById" }); // route is used when creating question/questionset (AutocompleteCategories.ts) and adding categories via ajax
            routes.MapRoute("Categories_Edit", "{text}/Bearbeite/{id}", new { controller = "EditCategory", action = "Edit" });
            routes.MapRoute("Category_Detail_Revision", "{text}/{id}/version/{version}", new { controller = "CategoryRedirect", action = "Category" });
            routes.MapRoute("Category_Detail", "Kategorien/{text}/{id}", new { controller = "CategoryRedirect", action = "Category" });
            routes.MapRoute("Category_History", "{categoryName}/{categoryId}/Historie", new { controller = "CategoryHistory", action = "List" });
            routes.MapRoute("Category_History_Detail", "{categoryName}/{categoryId}/Historie/{categoryChangeId}", new { controller = "CategoryHistoryDetail", action = "Detail" });
            routes.MapRoute("Category_Publish_Modal_Data", "GetCategoryPublishModalData", new { controller = "Category", action = "GetCategoryPublishModalData" });
            routes.MapRoute("Category_Authors", "GetAuthorsForHeader", new { controller = "Category", action = "GetCategoryHeaderAuthors" });
            routes.MapRoute("Category_GetMiniItem", "Category/GetMiniCategoryItem", new { controller = "Category", action = "GetMiniCategoryItem" });
            /* API */ routes.MapRoute("Categories_AddSubCategoryRow", "Categories/AddSubCategoryRow", new { controller = "EditCategory", action = "AddSubCategoryRow" });
            /* API */ routes.MapRoute("Categories_EditSubCategoryItems", "Categories/EditSubCategoryItems/{id}", new { controller = "EditSubCategoryItems", action = "Edit" });
            /* API */ routes.MapRoute("Categories_AddSubCategoryItemRow", "Categories/EditSubCategoryItems/{id}/Add", new { controller = "EditSubCategoryItems", action = "AddSubCategoryItemRow" });
            /* API */ routes.MapRoute("Category_StartTestSession", "Kategorie/Testen/{categoryName}/{categoryId}", new { controller = "Category", action = "StartTestSession" });
            /* API */ routes.MapRoute("Category_StartLearningSession", "Category/StartLearningSession", new { controller = "Category", action = "StartLearningSession" });
            /* API */ routes.MapRoute("Category_StartLearningSessionForSets", "Category/StartLearningSessionForSets", new { controller = "Category", action = "StartLearningSessionForSets" });
            /* API */ routes.MapRoute("TopicTabAsync", "Category/GetTopicTabAsync", new { controller = "Category", action = "GetTopicTabAsync" });
            /* API */ routes.MapRoute("DeleteCookie", "Category/DeleteCookie", new { controller = "Category", action = "DeleteCookie" });
            /* API */ routes.MapRoute("Category_Get_Delete_Data", "Categories/GetDeleteData", new { controller = "Categories", action = "GetDeleteData" });

            routes.MapRoute("Knowledge_Learn", "Lernen/Wunschwissen", new { controller = "Knowledge", action = "StartLearningSession" });
            routes.MapRoute("KnowledgeUeberblick", "Wissenszentrale/Ueberblick", new { controller = "Knowledge", action = "Knowledge" });
            routes.MapRoute("KnowledgeTopics", "Wissenszentrale/Themen", new { controller = "Knowledge", action = "Knowledge" });
            routes.MapRoute("KnowledgeFragen", "Wissenszentrale/Fragen", new { controller = "Knowledge", action = "Knowledge" });
            routes.MapRoute("Knowledge_ConfirmEmail", "EmailBestaetigen/{emailKey}", new { controller = "Knowledge", action = "EmailConfirmation" });

            routes.MapRoute("AlgoInsight", "AlgoInsight/{action}", new { controller = "AlgoInsight", action = "AlgoInsight" });

            routes.MapRoute("Maintenance", "Maintenance/{action}", new { controller = "Maintenance", action = "Maintenance" });
            routes.MapRoute("MaintenanceImages", "MaintenanceImages/{action}", new { controller = "MaintenanceImages", action = "Images" });
            routes.MapRoute("Messages", "Nachrichten/{action}", new { controller = "Messages", action = "Messages" });

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
            routes.MapRoute("ApiEduSharing", "Api/EduSharing/{action}", new { controller = "EduSharingApi" });

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

            routes.MapRoute("Übersicht", "Übersicht/Förderer", new { controller = "Welcome", action = "Promoter" });

            

            foreach (var typeName in GetAllControllerNames())
            {
                if (!typeName.Equals("CategoryController"))
                {
                    Debug.WriteLine(typeName.Replace("Controller", ""));
                    var controllerName = typeName.Replace("Controller", "");
                    routes.MapRoute(controllerName + "Generated", controllerName + "/{action}",
                    new {controller = controllerName});
                }
            }

            routes.MapRoute("GetQuestionSets", "Questions/GetQuestionSets", new { controller = "Questions", action = "GetQuestionSets" });
            /*Api*/routes.MapRoute("EditPreview", "Category/RenderContentModule", new { controller = "Category", action = "RenderContentModule" });
            /*Api*/routes.MapRoute("EditSave", "Category/SaveCategoryContent", new { controller = "EditCategory", action = "SaveCategoryContent" });
            /*Api*/routes.MapRoute("EditSegments", "Category/SaveSegments", new { controller = "EditCategory", action = "SaveSegments" });
            /*Api*/routes.MapRoute("GetWishknowledge", "Category/GetWishknowledge", new { controller = "Category", action = "GetWishknowledge" });
            /*Api*/
            routes.MapRoute("Category_KnowledgeBar", "Category/KnowledgeBar", new { controller = "Category", action = "KnowledgeBar" });
            /*Api*/routes.MapRoute("AnalyticsTabApi", "Category/Tab", new { controller = "Category", action = "Tab" });
            /*Api*/routes.MapRoute("SetSetSettingsCookie", "Category/SetSettingsCookie/{text}", new { controller = "Category", action = "SetSettingsCookie", text = UrlParameter.Optional });
            /*Api*/routes.MapRoute("SetMyWorldCookie", "Category/SetMyWorldCookie/{text}", new { controller = "Category", action = "SetMyWorldCookie", text = UrlParameter.Optional });
            /*Api*/routes.MapRoute("GetMyWorldCookie", "Category/GetMyWorldCookie/", new { controller = "Category", action = "GetMyWorldCookie" });

            routes.MapRoute("AnalyticsGetKnowledgeDisplay", "Category/GetKnowledgeGraphDisplay", new { controller = "Category", action = "GetKnowledgeGraphDisplay" });
            routes.MapRoute("RenderNewKnowledgeSummaryBar", "Category/RenderNewKnowledgeSummaryBar", new { controller = "Category", action = "RenderNewKnowledgeSummaryBar" });
            routes.MapRoute("GetQuestion", "Questions/AddToQuestionSet", new { controller = "Questions", action = "AddToQuestionSet" });
            routes.MapRoute("Category_Learning_Button", "{categoryName}/{Id}/Lernen", new { controller = "Category", action = "CategoryLearningTab" });
            routes.MapRoute("Analytics_Tab", "{categoryName}/{Id}/Wissensnetz", new { controller = "Category", action = "CategoryAnalyticsTab" });

            routes.MapRoute("Category", "{text}/{id}", new { controller = "Category", action = "Category" });
            routes.MapRoute("GetSegmentHtml", "Segmentation/GetSegmentHtml", new { controller = "Segmentation", action = "GetSegmentHtml" });
            routes.MapRoute("GetCategoryCard", "Segmentation/GetCategoryCard", new { controller = "Segmentation", action = "GetCategoryCard" });
            
            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Welcome", action = "Welcome", id = UrlParameter.Optional });
            routes.MapRoute("Various", "{action}", new { controller = "VariousPublic"});
            routes.MapRoute("Test", "TestPage", new { controller = "TestPage", action = "TestPage" });
        }

        public static IEnumerable<string> GetAllControllerNames() 
        {
            var assembly = typeof(RouteConfig).Assembly;
            return assembly.GetTypes()
                .Where(t => t.FullName != null && t.FullName.EndsWith("Controller"))
                .Select(t => t.FullName);
        }

    }
}