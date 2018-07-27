<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />

<div class="SessionHeading">
    
    <% Html.RenderPartial("~/Views/Questions/Answer/Sponsor.ascx", Model); %>

    <div class="SessionTitle">
        <% if(Model.LearningSession.IsSetSession) { %>
            <div class="CollectionType">Lernset</div>
            <div class="LabelWrapper">
                <a class="LabelLink" href="<%= Links.SetDetail(Url, Model.LearningSession.SetToLearn) %>">
                    <span class="label label-set"><%: Model.LearningSession.SetToLearn.Name %></span>
                </a>
            </div>
        <% } %>

        <% if(Model.LearningSession.IsSetsSession) { %>   
            <%= Model.LearningSession.SetListTitle %> (<span style="white-space: nowrap"><%= Model.LearningSession.SetsToLearn().Count %> Lernsets</span>)
        <% } %>

        <% if(Model.LearningSession.IsCategorySession) { %>
            <div class="CollectionType">Thema</div>
            <div class="LabelWrapper">
                <a class="LabelLink" href="<%= Links.CategoryDetail(Model.LearningSession.CategoryToLearn.Name, Model.LearningSession.CategoryToLearn.Id) %>">
                    <span class="label label-category"><%: Model.LearningSession.CategoryToLearn.Name %></span>
                </a>
            </div>
        <% } %>
        
        <% if(Model.LearningSession.IsDateSession) { %>
            Dein Termin 
            <a href="<%= Links.Dates() %>"><%= Model.LearningSession.DateToLearn.GetTitle() %></a>
        <% } %>

        <% if(Model.LearningSession.IsWishSession) { %>
            <a href="<%= Links.QuestionsWish() %>">Dein Wunschwissen</a>
        <% } %>
    </div>
</div>

<div class="SessionBar">
    <div class="QuestionCount" style="float: right;">Abfrage <span id="CurrentStepNumber"><%= Model.CurrentLearningStepIdx + 1 %></span> von <span id="StepCount"><%= Model.LearningSession.Steps.Count %></span></div>
    <div class="SessionType">
        <span class="show-tooltip"
        data-original-title="<%= @"<div style='text-align: left;'>In diesem Modus
                <ul>
                    <li>wiederholst du personalisiert die Fragen, die du am dringendsten lernen solltest</li>
                    <li>kannst du dir die Lösung anzeigen lassen</li>
                    <li>werden dir Fragen, die du nicht richtig beantworten konntest, nochmal vorgelegt</li>
                </ul>
            </div>"%>" data-html="true" style="float: left;">
            Lernen
            <span class="fa-stack fa-1x" style="font-size: 10px; top: -1px;">
                <i class="fa fa-circle fa-stack-2x" style="color: #e1efb3;"></i>
                <i class="fa fa-info fa-stack-1x" style=""></i>
            </span>
        </span>
    </div>
    <div class="ProgressBarContainer">
        <div id="progressPercentageDone" class="ProgressBarSegment ProgressBarDone" style="width: <%= Model.CurrentLearningStepPercentage== 0 ? "0" : Model.CurrentLearningStepPercentage + "%" %>;">
            <div class="ProgressBarSegment ProgressBarLegend">
                <span id="spanPercentageDone"><%= Model.CurrentLearningStepPercentage %>%</span>
            </div>
        </div>
        <% if (Model.CurrentLearningStepPercentage < 100) {%>
            <div class="ProgressBarSegment ProgressBarLeft" style="width: 100%;"></div>
        <% } %>
            
    </div>
</div>
