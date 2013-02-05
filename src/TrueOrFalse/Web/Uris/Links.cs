﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Uris;

namespace TrueOrFalse.Frontend.Web.Code
{
    public static class Links
    {
        public const string UserProfileController = "UserProfile";
        public const string UserProfile = "Profile";

        /*Question*/
        public const string Questions = "Questions";
        public const string QuestionsController = "Questions";

        public static string AnswerQuestion(UrlHelper url, Question question, int paramElementOnPage){
            return url.Action("Answer", AnswerQuestionController, 
                new { text = UriSegmentFriendlyQuestion.Run(question.Text), id = question.Id, elementOnPage = paramElementOnPage}, null);
        }

        public static string Profile(UrlHelper url, string userName, int userId){
            return url.Action(UserProfile, UserProfileController, 
                new { name = UriSegmentFriendlyUser.Run(userName), id = userId }, null);
        }

        public static string SendAnswer(UrlHelper url, Question question){
            return url.Action("SendAnswer", AnswerQuestionController, new { id = question.Id }, null);
        }   

        public static string GetAnswer(UrlHelper url, Question question){
            return url.Action("GetAnswer", AnswerQuestionController, new { id = question.Id }, null);
        }

        public static string QuestionSetDetail(UrlHelper url, QuestionSet set, int elementOnPage){
            return url.Action("QuestionSet", "QuestionSet", 
                new { text = UriSanitizer.Run(set.Name), id = set.Id, elementOnPage = elementOnPage }, null);
        }

        public const string EditQuestionController = "EditQuestion"; 
        public const string CreateQuestion = "Create";
        public const string EditQuestion = "Edit";

        public const string AnswerQuestionController = "AnswerQuestion";

        /*Category*/
        public const string Categories = "Categories";
        public const string CategoriesController = "Categories";

        /* Category/Edit */
        public const string EditCategoryController = "EditCategory";
        public const string EditCategory = "Edit";

        /**/
        public const string WelcomeController = "Welcome";
        public const string Register = "Register";
        public const string RegisterSuccess = "RegisterSuccess";
        public const string Login = "Login";

        public const string VariousController = "VariousPublic";
        public const string Impressum = "Imprint";
        public const string WelfareCompany = "WelfareCompany";

        public const string KnowledgeController = "Knowledge";
        public const string Knowledge = "Knowledge";

        public const string AccountController = "Account";
        public const string Logout = "Logout";

    }
}
