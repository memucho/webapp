<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerBodyModel>" %>
<%@ Import Namespace="TrueOrFalse" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div id="AnswerBody">

    <input type="hidden" id="hddQuestionViewGuid" value="<%= Model.QuestionViewGuid.ToString() %>" />
    <input type="hidden" id="hddInteractionNumber" value="1" />
    <input type="hidden" id="questionId" value="<%= Model.QuestionId %>" />
    <input type="hidden" id="isLastQuestion" value="<%= Model.IsLastQuestion %>" />
    <input type="hidden" id="ajaxUrl_GetSolution" value="<%= Model.AjaxUrl_GetSolution(Url) %>" />
    <input type="hidden" id="ajaxUrl_CountLastAnswerAsCorrect" value="<%= Model.AjaxUrl_CountLastAnswerAsCorrect(Url) %>" />
    <input type="hidden" id="ajaxUrl_CountUnansweredAsCorrect" value="<%= Model.AjaxUrl_CountUnansweredAsCorrect(Url) %>" />
    <% if (Model.IsTestSession) { %>
        <input type="hidden" id="ajaxUrl_TestSessionRegisterAnsweredQuestion" value="<%= Model.AjaxUrl_TestSessionRegisterAnsweredQuestion(Url) %>" />
        <input type="hidden" id="TestSessionProgessAfterAnswering" value="<%= Model.TestSessionProgessAfterAnswering %>" />
    <% } %>
    
    <% if (Model.IsLearningSession) { %>
        <input type="hidden" id="ajaxUrl_LearningSessionAmendAfterShowSolution" value="<%= Model.AjaxUrl_LearningSessionAmendAfterShowSolution(Url) %>" />
    <% } %>

    <input type="hidden" id="disableAddKnowledgeButton"  value="<%= Model.DisableAddKnowledgeButton %>"/>
    
    <input type="hidden" id="hddTimeRecords" />
    <input type="hidden" id="hddQuestionId" value="<%=Model.QuestionId %>" />
    <input type="hidden" id="isInTestMode" value="<%=Model.IsInTestMode %>"/>
    <div id="QuestionTitle" style="display:none">
        <%= Model.QuestionTitle %>
    </div>
    <div class="AnswerQuestionBodyMenu">

        <% if (!Model.IsInWidget)
           { %>
            <% if (!Model.DisableAddKnowledgeButton)
               { %>
                <div class="Pin" data-question-id="<%= Model.QuestionId %>">
                    <%= Html.Partial("AddToWishknowledgeButtonQuestionDetail", new AddToWishknowledge(Model.IsInWishknowledge, isShortVersion: true)) %>
                </div>
            <% } %>
            <div class="Button dropdown">
                <span class="margin-top-4">
                    <a href="#" class="dropdown-toggle btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu dropdown-menu-right standard-question-drop-down">
                        <% if (Model.IsInLearningTab && (Model.IsCreator || Model.IsInstallationAdmin)){ %>
                            <li>
                                <a data-allowed="logged-in" onclick="eventBus.$emit('open-edit-question-modal', {
                                        questionId: <%= Model.QuestionId %>,
                                        edit: true
                                    })">
                                    <div class="dropdown-icon">
                                        <i class="fa fa-pencil"></i>
                                    </div>
                                    <span>Frage bearbeiten</span>
                                </a>
                            </li>

                        <% } %>
                        
                        <% if (Model.IsInLearningTab && Model.IsInstallationAdmin){ %>
                            <li>
                                <a href="<%=Links.GetUrl(Model.Question) %>">
                                    <div class="dropdown-icon">
                                        <i class="fas fa-file"></i>
                                    </div>
                                    <span>Frageseite anzeigen</span>
                                </a>
                            </li>
                        <% } %>

                        <li>
                            <a href="<%=Links.QuestionHistory(Model.QuestionId) %>" data-allowed="logged-in">
                                <div class="dropdown-icon">
                                    <i class="fa fa-code-fork"></i>
                                </div>
                                <span>Bearbeitungshistorie der Frage</span>
                            </a>
                        </li>
                        <li>
                            <a href="<%=Links.GetUrl(Model.Question) + "#JumpLabel" %>">
                                <div class="dropdown-icon">
                                    <i class="fas fa-comment"></i>
                                </div>
                                <span>Frage kommentieren</span>
                            </a>
                        </li>
                        <% if (Model.IsInLearningTab && (Model.IsCreator || Model.IsInstallationAdmin)){ %>
                            <li>
                                <a data-toggle="modal" data-questionid="<%=Model.QuestionId %>" href="#modalDeleteQuestion">
                                    <div class="dropdown-icon">
                                        <i class="fas fa-trash"></i>
                                    </div>
                                    <span>Frage löschen</span>
                                </a>
                            </li>
                        <% } %>
                    </ul>
                </span>
            </div>
            
         <% } %>
    </div>
    
    <% if (Model.SolutionType != SolutionType.FlashCard.ToString()) { %>
    <h3 class="QuestionText">
        <%= Model.QuestionText %>
    </h3>
    <% } %>

    <div class="row">
        <% if (Model.SolutionType != SolutionType.FlashCard.ToString()){
               if (!string.IsNullOrEmpty(Model.QuestionTextMarkdown)){ %>
                    <div id="MarkdownCol">
                        <div class="RenderedMarkdown"><%= Model.QuestionTextMarkdown %></div>
                    </div>
                <% }
           } %>

        <div id="AnswerAndSolutionCol">
            <div id="AnswerAndSolution">

                <% if (Model.HasSound) { Html.RenderPartial("AudioPlayer", Model.SoundUrl); } %>

                <div class="row">
                    <div id="AnswerInputSection">
                        <input type="hidden" id="hddSolutionMetaDataJson" value="<%: Model.SolutionMetaDataJson %>" />
                        <input type="hidden" id="hddSolutionTypeNum" value="<%: Model.SolutionTypeInt %>" />
                        <%
                            string userControl = "SolutionType" + Model.SolutionType + ".ascx";

                            if (Model.SolutionMetadata.IsDate)
                                userControl = "SolutionTypeDate.ascx";
                            if (Model.SolutionType == "MatchList")
                            {
                                if (Request.Browser.IsMobileDevice)
                                    userControl = "SolutionTypeMatchList_LayoutMobile.ascx";
                                if (Model.IsMobileRequest == true)
                                    userControl = "SolutionTypeMatchList_LayoutMobile.ascx";
                                if (Model.IsMobileRequest == false)
                                    userControl = "SolutionTypeMatchList.ascx";
                            }

                            Html.RenderPartial("~/Views/Questions/Answer/AnswerControls/" + userControl, Model.SolutionModel);

                            if (Model.SolutionType == SolutionType.FlashCard.ToString()){ %>
                                <script type="text/javascript">
                                    var questionText = '<h3 class="QuestionText" style="text-align: center; font-size: 22px; font-family: Open Sans, Arial, sans-serif; line-height: 31px; margin: 0;"><%= Model.QuestionText %></h3>';
                                    var flashCardFrontHTML = questionText + '<%= Model.QuestionTextMarkdown %>';
                                    $("#flashCard-front").append($('<div id="flashCard-frontContent">').append(flashCardFrontHTML));
                                    $('#flashCard-frontContent img').load(function() {
                                        var frontHeight = $('#flashCard-frontContent').outerHeight();
                                        var backHeight = $('.back.flashCardContentSite').outerHeight();
                                        $('#flashCardContent').height(
                                            frontHeight > backHeight
                                                ? frontHeight
                                                : backHeight
                                        );
                                    });
                                </script>
                            <% }

                            if (Model.SolutionType != SolutionType.FlashCard.ToString()) { %>
                                <div class="answerFeedback answerFeedbackCorrect" style="display: none;">
                                    <i class="fa fa-check-circle">&nbsp;</i>Richtig!
                                </div>
                                <div class="answerFeedback answerFeedbackWrong" style="display: none;">
                                    <i class="fa fa-minus-circle">&nbsp;</i>Leider falsch
                                </div>
                            <% } %>
                        </div>
                    <div id="ButtonsAndSolutionCol">
                            <div id="ButtonsAndSolution" class="Clearfix">
                                <div id="Buttons">
                                    <div id="btnGoToTestSession" style="display: none"> 
                                        <% if (Model.HasCategories && !Model.IsInWidget && !Model.IsForVideo && Model.IsLastQuestion) { %>
                                            <a href="<%= Links.CategoryDetailLearningTab(Model.PrimaryCategory) %>" id="btnStartTestSession" class="btn btn-primary show-tooltip" rel="nofollow" data-original-title='<%= Model.IsLoggedIn ? "Lerne alle Fragen im Thema " : "Lerne 5 zufällig ausgewählte Fragen aus dem Thema " %><%= Model.PrimaryCategory.Name  %>'>
                                                <b>Weiterlernen</b>
                                            </a>
                                        <% } %>
                                    </div>
                                    <% if (Model.SolutionType == SolutionType.FlashCard.ToString()) { %>
                                        <a href="#" id="btnFlipCard" class="btn btn-warning memo-button" rel="nofollow">Umdrehen</a>
                                    <% } %>
                             

                                    <% if (Model.SolutionType != SolutionType.FlashCard.ToString()){ %>
                                        <div id="buttons-first-try" class="ButtonGroup">
                                        <a href="#" id="btnCheck" class="btn btn-primary memo-button" rel="nofollow">Antworten</a>
                                        <% if (!Model.IsInTestMode && Model.AnswerHelp){ %>
                                            <a href="#" class="selectorShowSolution SecAction btn btn-link memo-button"><i class="fa fa-lightbulb-o">&nbsp;</i>Lösung anzeigen</a>
                                        <% }%>        
                                    <% } else { %>
                                        <div id="buttons-answer" class="ButtonGroup flashCardAnswerButtons" style="display: none">
                                            <a href="#" id="btnRightAnswer" class="btn btn-warning memo-button" rel="nofollow">Wusste ich!</a>
                                            <a href="#" id="btnWrongAnswer" class="btn btn-warning memo-button" rel="nofollow">Wusste ich nicht!</a>
                                            <% if (!Model.IsInTestMode && Model.AnswerHelp){ %>
                                                <a href="#" id="flashCard-dontCountAnswer" class="selectorShowSolution SecAction btn btn-link memo-button">Nicht werten!</a>
                                            <% } %>
                                            </div>
                                            <div>
                                    <% } %>
                                    <% if (Model.IsLearningSession && Model.IsLoggedIn && Model.NextUrl != null && !Model.IsInTestMode) { %>
                                        <a id="aSkipStep" href="<%= Model.NextUrl(Url) %>" class="SecAction btn btn-link memo-button"><i class="fa fa-step-forward">&nbsp;</i>Frage überspringen</a>
                                    <% } %>
                                </div>

                                <div id="buttons-next-question" class="ButtonGroup" style="display: none;">
                                    <% if (Model.NextUrl != null && !Model.IsLastQuestion) { %>
                                        <a href="<%= Model.NextUrl(Url) %>" id="btnNext" class="btn btn-primary memo-button" rel="nofollow">Nächste Frage</a>
                                    <% }else if(Model.NextUrl == null && Model.IsForVideo){ %> 
                                        <button id="continue"  class="btn btn-primary clickToContinue memo-button" style="display: none">Weiter</button>
                                    <% } %>

                                    <% if (Model.SolutionType != SolutionType.FlashCard.ToString() && !Model.IsInTestMode && Model.AnswerHelp ) { %>
                                        <a href="#" id="aCountAsCorrect" class="SecAction btn btn-link show-tooltip memo-button" title="Drücke hier und die Frage wird als richtig beantwortet gewertet" rel="nofollow" style="display: none;">Hab ich gewusst!</a>
                                    <% } %>
                                </div>

                                <% if (Model.SolutionType != SolutionType.FlashCard.ToString())
                                    { %>
                                <div id="buttons-answer-again" class="ButtonGroup" style="display: none">
                                    <a href="#" id="btnCheckAgain" class="btn btn-warning memo-button" rel="nofollow">Nochmal Antworten</a>
                                    <% if (!Model.IsInTestMode && Model.AnswerHelp){ %>
                                    <a href="#" class="selectorShowSolution SecAction btn btn-link memo-button"><i class="fa fa-lightbulb-o">&nbsp;</i>Lösung anzeigen</a>
                                    <% } %>
                                </div>
                                <% } %>
                            <div style="clear: both"></div>
                            </div>
                                <div id="AnswerFeedbackAndSolutionDetails">
                                <% if (Model.SolutionType != SolutionType.FlashCard.ToString())
                                    { %>
                                <div id="AnswerFeedback">
                                    <div class="" id="divAnsweredCorrect" style="display: none; margin-top: 5px;">
                                        <b style="color: green;">Richtig!</b> <span id="wellDoneMsg"></span>
                                    </div>
                                    <div id="Solution" class="Detail" style="display: none;">
                                        <div class="Label">Richtige Antwort:</div>
                                        <div class="Content body-m"></div>
                                    </div>
                                    <div id="divWrongAnswerPlay" class="Detail" style="display: none; background-color: white;">
                                        <span style="color: #B13A48"><b>Deine Antwort war falsch</b></span>
                                        <div>Deine Eingabe:</div>
                                        <div style="margin-top: 7px;" id="divWrongEnteredAnswer">
                                        </div>
                                    </div>
                                    <div id="divWrongAnswer" class="Detail" style="display: none; background-color: white;">
                                        <span id="spnWrongAnswer" style="color: #B13A48"><b>Falsch beantwortet </b></span>
                                        <a href="#" id="CountWrongAnswers" style="float: right;">(zwei Versuche)</a><br />

                                        <div style="margin-top: 5px;" id="answerFeedbackTry">Du könntest es wenigstens probieren!</div>

                                        <div id="divWrongAnswers" style="margin-top: 7px; display: none;">
                                            <span class="WrongAnswersHeading">Deine bisherigen Antwortversuche:</span>
                                            <ul style="padding-top: 5px;" id="ulAnswerHistory">
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                                <% } %>
                                <% if (Model.AnswerHelp){ %>
                                    <div id="SolutionDetailsSpinner" style="display: none;">
                                        <i class="fa fa-spinner fa-spin" style="color: #b13a48;"></i>
                                    </div>
                                    <div id="SolutionDetails" style="display: none; background-color: white;">


                                        <div id="Description" class="Detail" style="display: none;">
                                            <div class="Label">Ergänzungen zur Antwort:</div>
                                            <div class="Content body-m"></div>
                                        </div>
                                        <div id="References" class="Detail" style="display: none;">
                                            <div class="Label">Quellen:</div>
                                            <div class="Content body-s"></div>
                                        </div>
                                    </div>
                                <% } %>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
        <% if (!Model.IsInWidget  && Model.SolutionType != SolutionType.FlashCard.ToString())
           { %>
            <div id="activityPointsDispaly" class="<%if (Model.IsLoggedIn){%>hideOnBigScreen<%} %>">
                <small>Dein Punktestand</small>
                <span id="activityPoints"><%= Model.TotalActivityPoints %></span>
                <span style="display: inline-block; white-space: nowrap;" class="show-tooltip" data-placement="bottom" title="Du bekommst Lernpunkte für das Beantworten von Fragen">
                    <i class="fa fa-info-circle"></i>
                </span>
            </div>
        <% } %>
        
        <% Html.RenderPartial("~/Views/Questions/Answer/AnswerQuestionDetails.ascx", Model); %>

</div>