<%@ Page Title="Ergebnis Lernsitzung" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<LearningSessionResultModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/AnswerQuestion") %>
    <link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />
    
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>

    <script>
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(drawKnowledgeCharts);

        function drawKnowledgeChartDate(chartElementId, notLearned, needsLearning, needsConsolidation, solid) {

            var chartElement = $("#" + chartElementId);
            if (chartElement.length == 0)
                return;

            //'Sicheres Wissen', 'Sollte gefestigt werden', 'Sollte dringend gelernt werden', 'Noch nie gelernt'
            var data = google.visualization.arrayToDataTable([
                ['Wissenslevel', 'Anteil in %'],
                ['Sicheres Wissen', solid],
                ['Solltest du festigen', needsConsolidation],
                ['Solltest du lernen', needsLearning],
                ['Noch nicht gelernt', notLearned],
            ]);

            var options = {
                pieHole: 0.6,
                legend: { position: 'none' },
                pieSliceText: 'none',
                height: 120,
                backgroundColor: 'transparent',
                chartArea: {
                    width: '90%', height: '90%', top: 6
                },
                slices: {
                    0: { color: '#3e7700' },
                    1: { color: '#fdd648' },
                    2: { color: '#B13A48' },
                    3: { color: '#EFEFEF' },
                },
                pieStartAngle: 180
            };

            var chart = new google.visualization.PieChart(chartElement.get()[0]);
            chart.draw(data, options);
        }

        function drawKnowledgeCharts() {
            $("[data-date-id]").each(function () {
                var $this = $(this);
                var dateId = $this.attr("data-date-id");

                drawKnowledgeChartDate(
                    "chartKnowledgeDate" + dateId,
                    parseInt($this.attr("data-notLearned")),
                    parseInt($this.attr("data-needsLearning")),
                    parseInt($this.attr("data-needsConsolidation")),
                    parseInt($this.attr("data-solid")));
            });
        }

    </script>

    <%= Scripts.Render("~/bundles/js/LearningSessionResult") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    
<%--    <% if (Model.IsLoggedIn)
           Html.RenderPartial("~/Views/Api/ActivityPoints/ActivityLevelProgress.aspx", new ActivityLevelProgressModel(Sl.SessionUser.User)); %>--%>

    <h2 style="margin-bottom: 15px; margin-top: 0px;">
        <span class="<% if (Model.LearningSession.IsDateSession) Response.Write("ColoredUnderline Date");
                        if (Model.LearningSession.IsSetSession) Response.Write("ColoredUnderline Set");
                        if (Model.LearningSession.IsSetsSession) Response.Write("ColoredUnderline Set");
                        if (Model.LearningSession.IsWishSession) Response.Write("ColoredUnderline Knowledge");
                        %>">Ergebnis</span>
    </h2>
    

    <div class="row">
        <div class="col-sm-9 xxs-stack">
            <div class="stackedBarChartContainer">
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
            <div class="buttonRow">
                <% if (Model.LearningSession.IsDateSession) { %>
                    <a href="/Termin/Lernen/<%=Model.LearningSession.DateToLearn.Id %>" class="btn btn-link show-tooltip" style="padding-right: 10px" title="Eine neue Lernsitzung zu diesem Termin beginnen">
                        Weiterlernen
                    </a>
                    <a href="<%= Url.Action(Links.KnowledgeAction, Links.KnowledgeController) %>" class="btn btn-primary" style="padding-right: 10px">
                        Zur Wissenszentrale
                    </a>
                <% } else if (Model.LearningSession.IsSetSession) { %>
                    <a href="<%= Links.SetDetail(Url, Model.LearningSession.SetToLearn) %>" class="btn btn-link" style="padding-right: 10px">Zum Lernset (Übersicht)</a>
                    <a href="<%= Links.StartLearningSession(Model.LearningSession) %>" class="btn btn-primary" style="padding-right: 10px">
                        Weiterlernen
                    </a>
                <% } else if (Model.LearningSession.IsSetsSession) { %>
                    <a href="<%= Links.StartLearningSession(Model.LearningSession) %>" class="btn btn-primary" style="padding-right: 10px">
                        Weiterlernen
                    </a>
                <% } else if (Model.LearningSession.IsCategorySession) { %>
                    <a href="<%= Links.CategoryDetail(Model.LearningSession.CategoryToLearn.Name, Model.LearningSession.CategoryToLearn.Id) %>" class="btn btn-link" style="padding-right: 10px">Zum Thema</a>
                    <a href="<%= Links.StartLearningSession(Model.LearningSession) %>" class="btn btn-primary" style="padding-right: 10px">
                        Weiterlernen
                    </a>   
                <% } else if (Model.LearningSession.IsWishSession) { %>
                    <a href="<%= Links.Knowledge() %>" class="btn btn-link" style="padding-right: 10px">Zur Wissenszentrale</a>
                    <a href="<%= Links.StartWishLearningSession() %>" class="btn btn-primary" style="padding-right: 10px">
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
                <% foreach (var uniqueQuestion in Model.AnsweredStepsGrouped)
                    {
                        if ((uniqueQuestion.First().AnswerState == StepAnswerState.Answered) && uniqueQuestion.First().Answer.AnsweredCorrectly())
                        { %> 
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="QuestionLearned AnsweredRight">
                                        <a href="#" data-action="showAnswerDetails">
                                        <i class="fa fa-check-circle AnswerResultIcon show-tooltip" title="Beim 1. Versuch richtig beantwortet">
                                            &nbsp;&nbsp;
                                        </i><%= uniqueQuestion.First().Question.GetShortTitle(150) %> 
                                        (Details)</a><br/>
                        <% }
                        else if ((uniqueQuestion.Count() > 1) && (uniqueQuestion.Last().AnswerState == StepAnswerState.Answered) && uniqueQuestion.Last().Answer.AnsweredCorrectly())
                        { %> 
                            <div class="row">
                                <div class="col-xs-12">
                                        <a href="#" data-action="showAnswerDetails">
                                    <div class="QuestionLearned AnsweredRightAfterRepetition">
                                        <i class="fa fa-check-circle AnswerResultIcon show-tooltip" title="Beim 2. oder 3. Versuch richtig beantwortet">
                                            &nbsp;&nbsp;
                                        </i><%= uniqueQuestion.First().Question.GetShortTitle(150) %> 
                                        (Details)</a><br/>

                        <% }
                        else if (uniqueQuestion.All(a => a.AnswerState != StepAnswerState.Answered))
                        { %>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="QuestionLearned Unanswered">
                                        <a href="#" data-action="showAnswerDetails">
                                        <i class="fa fa-circle AnswerResultIcon show-tooltip" title="Nicht beantwortet">
                                            &nbsp;&nbsp;
                                        </i><%= uniqueQuestion.First().Question.GetShortTitle(150) %> 
                                        (Details)</a><br/>
                        <% }
                        else if (((uniqueQuestion.Last().AnswerState == StepAnswerState.Answered) && (uniqueQuestion.Last().Answer.AnswerredCorrectly == AnswerCorrectness.False)) ||
                                 ((uniqueQuestion.Last().AnswerState != StepAnswerState.Answered) && (uniqueQuestion.Count() > 1)))
                        { %>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="QuestionLearned AnsweredWrong">
                                        <a href="#" data-action="showAnswerDetails">
                                        <i class="fa fa-minus-circle AnswerResultIcon show-tooltip" title="Falsch beantwortet">
                                            &nbsp;&nbsp;
                                        </i><%= uniqueQuestion.First().Question.GetShortTitle(150) %> 
                                        (Details)</a><br/>
                        <% } %>
                                        <div class="answerDetails" data-questionId=<%= uniqueQuestion.First().QuestionId %>>
                                            <div class="row">
                                                <div class="col-xs-3 col-sm-2 answerDetailImage">
                                                    <div class="ImageContainer ShortLicenseLinkText">
                                                    <%= GetQuestionImageFrontendData.Run(uniqueQuestion.First().Question).RenderHtmlImageBasis(128, true, ImageType.Question, linkToItem: Links.AnswerQuestion(uniqueQuestion.First().Question)) %> 
                                                    </div>
                                                </div>
                                                <div class="col-xs-9 col-sm-10">
                                                    <p class="rightAnswer">Richtige Antwort: <%= GetQuestionSolution.Run(uniqueQuestion.First().Question).GetCorrectAnswerAsHtml() %><br/></p>
                                                    <% int counter = 1;
                                                    foreach (var step in uniqueQuestion)
                                                    {
                                                        if (step.AnswerState == StepAnswerState.NotViewedOrAborted)
                                                        {
                                                            %> <p class="answerTry">Dein <%= counter %>. Versuch: (abgebrochen)</p><%
                                                        }
                                                        else if (step.AnswerState == StepAnswerState.Skipped)
                                                        {
                                                            %> <p class="answerTry">Dein <%= counter %>. Versuch: (übersprungen)</p><%
                                                        }
                                                        else if (step.AnswerState == StepAnswerState.Uncompleted)
                                                        {
                                                            %> <p class="answerTry">Dein <%= counter %>. Versuch: (noch nicht gesehen)</p><%
                                                        }
                                                        else if (step.AnswerState == StepAnswerState.ShowedSolutionOnly)
                                                        {
                                                            %> <p class="answerTry">Dein <%= counter %>. Versuch: (Lösung angezeigt)</p><%
                                                        }
                                                        else
                                                        {
                                                            %> <p class="answerTry">Dein <%= counter %>. Versuch: <%= Question.AnswersAsHTML(step.Answer.AnswerText, step.Question.SolutionType) %></p><%
                                                        }
                                                        counter++;
                                                    } %>
                                                    <p class="answerLinkToQ"><a href="<%= Links.AnswerQuestion(uniqueQuestion.First().Question) %>"><i class="fa fa-arrow-right">&nbsp;</i>Diese Frage einzeln lernen</a></p>
                                                    
                                                </div>
                                                
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                    <% } %>
            </div>
        </div>


        <div class="col-sm-3 xxs-stack">
            <% if(Model.LearningSession.IsSetSession) { %>
                <div class="boxInfo">
                    <div class="boxInfoHeader">
                        Lernset-Info
                    </div>
                    <div class="boxInfoContent">
                        <p>
                            Du hast dieses Lernset gelernt:<br />
                            <a href="<%= Links.SetDetail(Url, Model.LearningSession.SetToLearn) %>" style="display: inline-block;">
                                <span class="label label-set"><%: Model.LearningSession.SetToLearn.Name %></span>
                            </a> (insgesamt <%=Model.LearningSession.TotalPossibleQuestions %> Fragen)
                        </p>
                    </div>
                </div>
            <% } %>

            <% if(Model.LearningSession.IsSetsSession) { %>
                <div class="boxInfo">
                    <div class="boxInfoHeader">
                        <%= Model.LearningSession.SetListTitle %>
                    </div>
                    <div class="boxInfoContent">
                        <p>
                            Du hast dieses Lernset gelernt:
                        </p>
                        <div class="LabelList">
                            <% foreach (var set in Model.SetsToLearn) { %>
                                <div class="LabelItem LabelItem-Set">
                                    <a href="<%= Links.SetDetail(Url, set) %>" style="display: inline-block;">
                                        <span class=""><%: set.Name %></span>
                                    </a> (<%= set.Questions().Count %> Fragen)
                                </div>
                            <% } %>
                        </div>
                    </div>
                </div>
            <% } %>

            <% if(Model.LearningSession.IsWishSession) { %>
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
                            <li><a href="<%= Links.SetsWish() %>"><%= Model.WishCountSets %> Lernset<%= StringUtils.PluralSuffix(Model.WishCountSets,"s") %></a></li>
                        </ul>
                    </div>
                </div>
            <% } %>
            
            <% if(Model.LearningSession.IsCategorySession) { %>
                <div class="boxInfo">
                    <div class="boxInfoHeader">
                        Thema
                    </div>
                    <div class="boxInfoContent">
                        <p>
                            Du hast dieses Thema gelernt:<br />
                            <a href="<%= Links.CategoryDetail(Model.LearningSession.CategoryToLearn.Name, Model.LearningSession.CategoryToLearn.Id) %>" style="display: inline-block;">
                                <span class="label label-category"><%: Model.LearningSession.CategoryToLearn.Name %></span>
                            </a> (insgesamt <%=Model.LearningSession.TotalPossibleQuestions %> Fragen)
                        </p>
                    </div>
                </div>

            <% } %>

            <% if(Model.LearningSession.IsDateSession) { %>
                <div class="boxInfo">
                    <div class="boxInfoHeader">
                        Dein Termin
                    </div>
                    <div class="boxInfoContent">
                        <h3><%= Model.DateToLearn.GetTitle() %></h3>
                        <div class="tableLayout">
                            <div class="tableCellLayout" style="min-width: 18px;"><i class="fa fa-calendar-o">&nbsp;</i></div>
                            <div class="tableCellLayout show-tooltip" title="Dein Termin ist am <%= Model.DateToLearn.DateTime.ToString("dd.MM.yyyy 'um' HH:mm") %>">
                                <% if(Model.DateIsInPast){
                                        Response.Write("Vorbei seit ");
                                    }else { 
                                        Response.Write("Noch ");
                                    }%><%= Model.DateRemainingTimeLabel.Value %> <%= Model.DateRemainingTimeLabel.Label %>
                            </div>
                        </div>
                        <div class="tableLayout">
                            <div class="tableCellLayout" style="min-width: 18px; text-align: center;">
                                <a href="#" data-action="toggleDateSets">
                                    <i class="fa fa-caret-right dateSets">&nbsp;</i><i class="fa fa-caret-down dateSets" style="display: none;">&nbsp;</i>
                                </a>
                            </div>
                            <div class="tableCellLayout">
                                <a href="#" data-action="toggleDateSets">
                                    <%= Model.DateToLearn.CountQuestions() %> Fragen 
                                </a>
                                    
                                <div class="dateSets" style="display: inline; position: relative; display: none;" >
                                    aus <%= Model.DateToLearn.Sets.Count %> Lernset<%= StringUtils.PluralSuffix(Model.DateToLearn.Sets.Count, "s") %>
                                    <%  foreach(var set in Model.DateToLearn.Sets){ %>
                                        <a href="<%= Links.SetDetail(Url, set) %>">
                                            <span class="label label-set"><%= set.Name %></span>
                                        </a>
                                    <% } %>
                                </div>
                            </div>
                        </div>
                                
                        <p style="margin-top: 10px;">
                            Dein Wissensstand:<br/>
                            <div id="chartKnowledgeDate<%=Model.DateToLearn.Id %>" 
                                    data-date-id="<%= Model.DateToLearn.Id %>"
                                    data-notLearned="<%= Model.KnowledgeNotLearned %>"
                                    data-needsLearning="<%= Model.KnowledgeNeedsLearning %>"
                                    data-needsConsolidation="<%= Model.KnowledgeNeedsConsolidation %>"
                                    data-solid="<%= Model.KnowledgeSolid %>">
                            </div>
                        </p>
                        <p style="text-align: right; margin-top: 20px;">
                            <a href="<%= Links.Dates() %>"><i class="fa fa-arrow-right">&nbsp;</i>Zur Terminübersicht</a>
                        </p>
                    </div>
                </div>
                <div class="boxInfo">
                    <div class="boxInfoHeader">
                        Dein Lernplan
                    </div>
                    <div class="boxInfoContent">
                        <% if (Model.TrainingDateCount > 0) { %>
                            <p>
                                Vor dir liegen noch: <br/>
                                ca. <%= Model.TrainingDateCount %> Lernsitzung<%= StringUtils.PluralSuffix(Model.DateToLearn.Sets.Count, "en") %><br />
                                ca. <%= Model.RemainingTrainingTime%> Lernzeit
                            </p>
                            <p style="margin-top: 10px;">
                                Nächste Lernsitzung: <br/><i class="fa fa-bell"></i>&nbsp;
                                <% if(Model.TrainingPlan.HasOpenDates) {
                                    var timeSpanLabel = new TimeSpanLabel(Model.TrainingPlan.TimeToNextDate, showTimeUnit: true);
                                    if (timeSpanLabel.TimeSpanIsNegative) { %>
                                        <a style="display: inline-block;" data-btn="startLearningSession" href="/Termin/Lernen/<%=Model.DateToLearn.Id %>">startet jetzt!</a>
                                    <% } else { %>
                                        in <span class="TPTimeToNextTrainingDate"><%= timeSpanLabel.Full %></span> 
                                    <% } %>
                                    (<span class="TPQuestionsInNextTrainingDate"><%= Model.TrainingPlan.QuestionCountInNextDate %></span> Fragen)<br/>
                                <% } %>
                            </p>
                        <% } else { %>
                            <p>Für diesen Termin sind keine weiteren Lernsitzungen geplant.</p>
                        <% } %>
                    </div>
                </div>
                
            <% } %>
        </div>
    </div>


</asp:Content>