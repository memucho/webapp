<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>



<input type="hidden" id="hddIsLearningSession" value="True" 
       data-learning-session-id="-1"
       data-current-step-guid=""
       data-current-step-idx=""
       data-is-last-step=""
       data-skip-step-index="" />

<input type="hidden" id="hddQuestionId" value="1"/>
<input type="hidden" id="hddCategoryId" value="<%= Model.Category.Id %>"/>
<input type="hidden" id="hddLearningSessionStarted" value="False"/>


    
<% if (Model.Category.CountQuestionsAggregated > 0)
   {
       var dummyQuestion = Sl.QuestionRepo.GetById(Model.Category.GetAggregatedQuestionsFromMemoryCache().Where(q => q.IsVisibleToCurrentUser()).Select(q => q.Id).FirstOrDefault()); // why not take Question from Cache directly?

       Html.RenderPartial("~/Views/Questions/Answer/LearningSession/LearningSessionHeader.ascx", new AnswerQuestionModel(dummyQuestion.Id));

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
