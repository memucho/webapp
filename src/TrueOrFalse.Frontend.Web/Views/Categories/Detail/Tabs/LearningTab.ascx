﻿<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>

<input type="hidden" id="hddIsLearningSession" value="<%= Model.IsLearningSession %>"
    data-learning-session-id="-1"
    data-current-step-guid=""
    data-current-step-idx=""
    data-is-last-step=""
    data-skip-step-index="" />
 
<input type="hidden" id="hddQuestionId" value="1" />
<input type="hidden" id="hddCategoryId" value="<%= Model.Category.Id %>" />
<input type="hidden" id="hddLearningSessionStarted" value="False" />
<input type="hidden" id="hddIsLearningSessionOnCategoryPage" value="true" />




<% 
    if (Model.Category.CountQuestionsAggregated > 0)
    {
        var questionId = Model.Category
            .GetAggregatedQuestionsFromMemoryCache()
            .Where(q => q.IsVisibleToCurrentUser())
            .Select(q => q.Id)
            .FirstOrDefault();

        var dummyQuestion = Sl.QuestionRepo.GetById(questionId); // why not take Question from Cache directly?

        if (Model.IsLoggedIn)
        {
            Html.RenderPartial("~/Views/Questions/Answer/LearningSession/LearningSessionHeader.ascx", new AnswerQuestionModel(dummyQuestion.Id));
        }
        else
        {
            Html.RenderPartial("~/Views/Questions/Answer/TestSession/TestSessionHeader.ascx", Model.LearningTabModel.AnswerQuestionModel);
            Html.RenderPartial("~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx", new AnswerBodyModel(Model.LearningTabModel.AnswerQuestionModel));
        }
    }
    else
    { %>
        <div class="NoQuestions" style="margin-top: 40px;">
            Es sind leider noch keine Fragen zum Lernen in diesem Thema enthalten.
        </div>
    <% } %>

<div id="AnswerBody">
    <input type="hidden" id="hddSolutionTypeNum" value="1" />
</div>
