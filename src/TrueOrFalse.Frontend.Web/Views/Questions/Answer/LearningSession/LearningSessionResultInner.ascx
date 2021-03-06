﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<LearningSessionResultModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<%--    <% if (Model.IsLoggedIn)
        Html.RenderPartial("~/Views/Api/ActivityPoints/ActivityLevelProgress.aspx", new ActivityLevelProgressModel(Sl.SessionUser.User)); %>--%>

<link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />

<input type="hidden" id="hddSolutionTypeNum" value="1" />
<input type="hidden" id="hddCategoryId" value="682" />
<input type="hidden" id="hddIsResultSite" value="true"/>

<h2 style="margin-bottom: 15px; margin-top: 0px;">
    <span class="<%
                    if (Model.LearningSession.Config.InWishknowledge) Response.Write("ColoredUnderline Knowledge");
                 %>">Ergebnis</span>
</h2>
    

<div class="row">
    <div class="col-sm-9 xxs-stack" id="ResultMainColumn">
        <div class="stackedBarChartContainer"             
             <% if (!Model.ShowSummaryText){ %>
             style="margin-bottom: 0;"
             <% } %>>
            <% if (Model.NumberCorrectPercentage>0) {%>
                <div class="stackedBarChart chartCorrectAnswer" style="width: <%=Model.NumberCorrectPercentage %>%;">
                    <%=Model.NumberCorrectPercentage %>% 
                </div>
            <% } %>                
            <% if (Model.NumberCorrectAfterRepetitionPercentage>0) {%>
                <div class="stackedBarChart chartCorrectAfterRepetitionAnswer" style="width: <%=Model.NumberCorrectAfterRepetitionPercentage %>%;">
                    <%=Model.NumberCorrectAfterRepetitionPercentage %>% 
                </div>
            <% } %>                
            <% if (Model.NumberWrongAnswersPercentage>0) {%>
                <div class="stackedBarChart chartWrongAnswer" style="width: <%=Model.NumberWrongAnswersPercentage %>%;">
                    <%=Model.NumberWrongAnswersPercentage %>% 
                </div>
            <% } %>                
            <% if (Model.NumberNotAnsweredPercentage>0) {%>
                <div class="stackedBarChart chartNotAnswered" style="width: <%=Model.NumberNotAnsweredPercentage %>%;">
                    <%=Model.NumberNotAnsweredPercentage %>% 
                </div>
            <% } %>                
        </div>
        
        <% if (Model.ShowSummaryText) {%>
        <div class="SummaryText" style="clear: left;">
            <p style="margin-bottom: 20px;">In dieser Lernsitzung hast du <%= Model.NumberUniqueQuestions %> Fragen gelernt und dabei</p>
            <div class="row">
                <div class="col-xs-12">
                    <div class="row">
                        <div class="col-xs-2 col-sm-offset-1 sumPctCol"><div class="sumPct sumPctRight"><span class="sumPctSpan"><%=Model.NumberCorrectPercentage %>%</span></div></div>
                        <div class="col-xs-10 col-sm-9 sumExpl">beim 1. Versuch gewusst (<%=Model.NumberCorrectAnswers %> Fragen)</div>
                    </div>
                    <div class="row">
                        <div class="col-xs-2 col-sm-offset-1 sumPctCol"><div class="sumPct sumPctRightAfterRep"><span class="sumPctSpan"><%=Model.NumberCorrectAfterRepetitionPercentage %>%</span></div></div>
                        <div class="col-xs-10 col-sm-9 sumExpl">beim 2. oder 3. Versuch gewusst (<%=Model.NumberCorrectAfterRepetitionAnswers %> Fragen)</div>
                    </div>
                    <div class="row">
                        <div class="col-xs-2 col-sm-offset-1 sumPctCol"><div class="sumPct sumPctWrong"><span class="sumPctSpan"><%=Model.NumberWrongAnswersPercentage %>%</span></div></div>
                        <div class="col-xs-10 col-sm-9 sumExpl">nicht gewusst (<%=Model.NumberWrongAnswers %> Fragen)</div>
                    </div>
                    <div class="row">
                        <div class="col-xs-2 col-sm-offset-1 sumPctCol"><div class="sumPct sumPctNotAnswered"><span class="sumPctSpan"><%=Model.NumberNotAnsweredPercentage %>%</span></div></div>
                        <div class="col-xs-10 col-sm-9 sumExpl">nicht beantwortet (<%=Model.NumberNotAnswered %> Fragen)</div>
                    </div>
                </div>
            </div>
        </div>
        <% } else {
               var tooltip = string.Format("Der Durchschnitt aller Nutzer beantwortete {0}% richtig", Model.PercentageAverageRightAnswers); %>
        <div id="divIndicatorAverageWrapper" style="width: 100%">
            <div id="divIndicatorAverage" style="margin-left: <%= Model.PercentageAverageRightAnswers %>%">
                <i class="fa fa-caret-up fa-4x show-tooltip" style="margin-left: -16px;" title="<%= tooltip %>"></i>
            </div>
            <div id="divIndicatorAverageText">
                <p class="show-tooltip" title="<%= tooltip %>">
                    Nutzerdurchschnitt (<span id="avgPercentageCorrect"><%= Model.PercentageAverageRightAnswers %></span>% richtig)
                </p>
            </div>
        </div>
        
        <% }%> 


        
        <div class="buttonRow">
            <% if (!Model.LearningSession.Config.InWishknowledge || Model.LearningSession.Config.IsAnonymous()) { %>
                <a href="<%= Links.CategoryDetail(Model.LearningSession.Config.Category.Name, Model.LearningSession.Config.CategoryId) %>" class="btn btn-link " style="padding-right: 10px">Zum Thema</a>
                <a href="<%= Links.StartLearningSession(Model.LearningSession) %>" class="btn btn-primary nextLearningSession" style="padding-right: 10px">
                    Weiterlernen
                </a>   
            <% } else if (Model.LearningSession.Config.InWishknowledge) { %>
                <a href="<%=Links.StartLearningSession(Model.LearningSession)  %>" class="btn btn-primary nextLearningSession" style="padding-right: 10px">
                    Neue Lernsitzung
                </a>
            <% } else {
                throw new Exception("buttons for this type of learning session not specified");
            } %>
        </div>
            
        <div id="detailedAnswerAnalysis">
            <h3 style="margin-bottom: 25px;">Auswertung deiner Antworten</h3>
            <p class="greyed fontSizeSmall">
                <a href="#" data-action="showAllDetails">Alle Details einblenden</a> | <a href="#" data-action="hideAllDetails">Alle Details ausblenden</a> | <a href="#" data-action="showDetailsExceptRightAnswer">Details zu allen nicht korrekten Fragen einblenden</a>
            </p>
            <% foreach (var learningSessionStepNew in Model.AnsweredStepsGrouped)
                {
                    if ((learningSessionStepNew.First().AnswerState != AnswerState.Unanswered) && learningSessionStepNew.First().AnswerState == AnswerState.Correct)
                    { %> 
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="QuestionLearned AnsweredRight">
                                    <a href="#" data-action="showAnswerDetails">
                                    <i class="fa fa-check-circle AnswerResultIcon show-tooltip" title="Beim 1. Versuch richtig beantwortet">
                                        &nbsp;&nbsp;
                                    </i><%= learningSessionStepNew.First().Question.GetShortTitle(150) %> 
                                    (Details)</a><br/>
                    <% }
                    else if ((learningSessionStepNew.Count() > 1) && (learningSessionStepNew.Last().AnswerState != AnswerState.Unanswered) && learningSessionStepNew.Last().AnswerState == AnswerState.Correct)
                    { %> 
                        <div class="row">
                            <div class="col-xs-12">
                                    <a href="#" data-action="showAnswerDetails">
                                <div class="QuestionLearned AnsweredRightAfterRepetition">
                                    <i class="fa fa-check-circle AnswerResultIcon show-tooltip" title="Beim 2. oder 3. Versuch richtig beantwortet">
                                        &nbsp;&nbsp;
                                    </i><%= learningSessionStepNew.First().Question.GetShortTitle(150) %> 
                                    (Details)</a><br/>

                    <% }
                    else if (learningSessionStepNew.All(a => a.AnswerState == AnswerState.Unanswered))
                    { %> 
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="QuestionLearned Unanswered">
                                    <a href="#" data-action="showAnswerDetails">
                                    <i class="fa fa-circle AnswerResultIcon show-tooltip" title="Nicht beantwortet">
                                        &nbsp;&nbsp;
                                    </i><%= learningSessionStepNew.First().Question.GetShortTitle(150) %> 
                                    (Details)</a><br/>
                    <% }
                    else if (((learningSessionStepNew.Last().AnswerState != AnswerState.Unanswered) && (learningSessionStepNew.Last().AnswerState == AnswerState.Correct)) ||
                                ((learningSessionStepNew.Last().AnswerState == AnswerState.Unanswered) && (learningSessionStepNew.Count() > 1)))
                    { %>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="QuestionLearned AnsweredWrong">
                                    <a href="#" data-action="showAnswerDetails">
                                    <i class="fa fa-minus-circle AnswerResultIcon show-tooltip" title="Falsch beantwortet">
                                        &nbsp;&nbsp;
                                    </i><%= learningSessionStepNew.First().Question.GetShortTitle(150) %> 
                                    (Details)</a><br/>
                    <% }
                    else { // fall-back-option, prevents layout bugs (missing opened divs) in case some answer-case isn't dealt with above  %>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="QuestionLearned">
                                    <a href="#" data-action="showAnswerDetails">
                                        <i class="fa fa-question-circle AnswerResultIcon show-tooltip" title="Status unbekannt (Fehler)">
                                            &nbsp;&nbsp;
                                        </i><%= learningSessionStepNew.First().Question.GetShortTitle(150) %> 
                                        (Details)</a><br/>
                             
                    <% }%>
                                    <div class="answerDetails" data-questionId=<%= learningSessionStepNew.First().Question.Id %>>
                                        <div class="row">
                                            <div class="col-xs-3 col-sm-2 answerDetailImage">
                                                <div class="ImageContainer ShortLicenseLinkText">
                                                <%= GetQuestionImageFrontendData.Run(learningSessionStepNew.First().Question).RenderHtmlImageBasis(128, true, ImageType.Question, linkToItem: Links.AnswerQuestion(learningSessionStepNew.First().Question)) %> 
                                                </div>
                                            </div>
                                            <div class="col-xs-9 col-sm-10">
                                                <p class="rightAnswer">Richtige Antwort: <%= GetQuestionSolution.Run(learningSessionStepNew.First().Question).GetCorrectAnswerAsHtml() %><br/></p>

                                                <% int counter = 1;
                                                foreach (var step in learningSessionStepNew)
                                                {
                                                    if (step.AnswerState == AnswerState.Skipped)
                                                    {
                                                        %> <p class="answerTry">Dein <%= counter %>. Versuch: (übersprungen)</p><%
                                                    }
                                                    else if (step.AnswerState == AnswerState.Unanswered)
                                                    {
                                                        %> <p class="answerTry">Dein <%= counter %>. Versuch: (noch nicht gesehen)</p><%
                                                    }
                                                    else
                                                    {
                                                        %> <p class="answerTry">Dein <%= counter %>. Versuch: <%= Question.AnswersAsHtml(step.Answer, step.Question.SolutionType) %></p><%
                                                    }
                                                    counter++;
                                                } %>
                                                <p class="answerLinkToQ"><a href="<%= Links.AnswerQuestion(learningSessionStepNew.First().Question) %>"><i class="fa fa-arrow-right">&nbsp;</i>Diese Frage einzeln lernen</a></p>
                                                    
                                            </div>
                                                
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                <% if (Model.CounterSteps >= 300)
                       break;

                   Model.CounterSteps++; 
                } %>
        </div>
        <% if (Model.CounterSteps >= 300)
           { %>
                <div>Es werden nicht mehr als 300 Fragen in der Auswertung angezeigt</div>
           <% } %>
    </div>


    <div id="ResultSideColumn" class="col-sm-3 xxs-stack">

        <% if(Model.LearningSession.Config.InWishknowledge) { %>
            <div class="boxInfo">
                <div class="boxInfoHeader">
                    Wunschwissen
                </div>
                <div class="boxInfoContent">
                    <p>
                        Du hast dein Wunschwissen gelernt. Dein Wunschwissen enthält
                    </p>
                    <ul>
                        <li><a href="<%= Links.QuestionsWish() %>"><%= Model.WishCountQuestions %> Fragen</a></li>
                    </ul>
                </div>
            </div>
        <% } %>
            
        <% if(!Model.LearningSession.Config.InWishknowledge) { %>
            <div class="boxInfo">
                <div class="boxInfoHeader">
                    Thema
                </div>
                <div class="boxInfoContent">
                    <p>
                        Du hast dieses Thema gelernt:<br />
                        <a href="<%= Links.CategoryDetail(Model.LearningSession.Config.Category.Name, Model.LearningSession.Config.CategoryId) %>" style="display: inline-block;">
                            <span class="label label-category"><%: Model.LearningSession.Config.Category.Name %></span>
                        </a> (insgesamt <%=Model.LearningSession.TotalPossibleQuestions() %> Fragen)
                    </p>
                </div>
            </div>

        <% } %>
    </div>
</div>

                                                            
