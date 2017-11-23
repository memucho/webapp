<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<AnswerQuestionModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="TrueOrFalse" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    
    <% Title = Model.QuestionText; %>
    <% if (Model.IsLearningSession || Model.IsTestSession ) { %>
        <meta name="robots" content="noindex" />
    <%}else { %>
        <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Links.AnswerQuestion(Model.Question) %>" />
    <% } %>
    
    <meta name="description" content="<%= Model.DescriptionForSearchEngines %>"/>
    
    <meta property="og:title" content="<%: Model.QuestionText %>" />
    <meta property="og:url" content="<%= Settings.CanonicalHost %><%= Links.AnswerQuestion(Model.Question) %>" />
    <meta property="og:type" content="article" />
    <meta property="og:image" content="<%= GetQuestionImageFrontendData.Run(Model.Question).GetImageUrl(435, true, imageTypeForDummy: ImageType.Question).Url %>" />
    <meta property="og:description" content="<%= Model.DescriptionForFacebook %>" />
</asp:Content>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    
    <%= Styles.Render("~/bundles/AnswerQuestion") %>
    <%= Scripts.Render("~/bundles/js/AnswerQuestion") %>
    <%= Scripts.Render("~/bundles/js/DeleteQuestion") %>
    
    <% if(Model.IsLearningSession) { %>
        <%= Scripts.Render("~/bundles/js/LearningSessionResult") %>
        <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <% } %>

    <style type="text/css">
         .selectorShowSolution{/* marker class */}
               
        .sparklineTotals{ position: relative;top: 1px; }
        .sparklineTotalsUser{ position: relative;top: 1px; }

        .valRow .valColumn2 .imgDelete{position: relative; left: 10px;top: -3px;  }
        .valRow .valColumn2 .valMine{margin-top: -2px; padding-top: 0;padding-left: 5px; float: left; }
    </style>

    <script type="text/javascript">
        var questionId = "<%= Model.QuestionId %>";
        var qualityAvg = "<%= Model.TotalQualityAvg %>";
        var qualityEntries = "<%= Model.TotalQualityEntries %>";

        var relevancePeronalAvg = "<%= Model.TotalRelevancePersonalAvg %>";
        var relevancePersonalEntries = "<%= Model.TotalRelevancePersonalEntries %>";
        var relevanceForAllAvg = "<%= Model.TotalRelevanceForAllAvg %>";
        var relevanceForAlleEntries = "<%= Model.TotalRelevanceForAllEntries %>";
    </script>

    <link type="text/css" href="/Content/blue.monday/jplayer.blue.monday.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    
    <input type="hidden" id="hddIsLearningSession" value="<%= Model.IsLearningSession %>" 
        data-learning-session-id="<%= Model.IsLearningSession ? Model.LearningSession.Id : -1 %>"
        data-current-step-guid="<%= Model.IsLearningSession ? Model.LearningSessionStep.Guid.ToString() : "" %>"
        data-current-step-idx="<%= Model.IsLearningSession ? Model.LearningSessionStep.Idx : -1 %>"
        data-is-last-step="<%= Model.IsLastLearningStep %>"
        data-skip-step-index = "-1" />
<%--    <input type="hidden" id="hddLearningSessionType" value="<%= LearningSessionType.Testing %>"/>--%>
    <input type="hidden" id="hddIsTestSession" value="<%= Model.IsTestSession %>" 
        data-test-session-id="<%= Model.IsTestSession ? Model.TestSessionId : -1 %>"
        data-current-step-idx="<%= Model.IsTestSession ? Model.TestSessionCurrentStep : -1 %>"
        data-is-last-step="<%= Model.TestSessionIsLastStep %>"/>
    <input type="hidden" id="hddQuestionId" value="<%= Model.QuestionId %>"/>

            <% if (Model.IsLearningSession) { %>
                   <% Html.RenderPartial("~/Views/Questions/Answer/LearningSession/LearningSessionHeader.ascx", Model); %>
            <% }else if (Model.IsTestSession) { %>
                   <% Html.RenderPartial("~/Views/Questions/Answer/TestSession/TestSessionHeader.ascx", Model); %>
            <% }else { %>
                <div class="AnswerQuestionHeader">
                    <% Html.RenderPartial("~/Views/Questions/Answer/Sponsor.ascx", Model); %>
                    <% Html.RenderPartial("~/Views/Questions/Answer/AnswerQuestionPager.ascx", Model); %>
                </div>
           <% } %>

        

    <div class="row">
        <div class="col-xs-12">
            
            <% Html.RenderPartial("~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx",
                   new AnswerBodyModel(Model)); %>

            <% if (!Model.IsLoggedIn && !Model.IsTestSession && !Model.IsLearningSession && Model.SetMinis.Any()) { %>
               <div class="SingleCategoryAttention">
                    
                   <% Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleCategoryFullWidth.ascx", new SingleCategoryFullWidthModel(Model.PrimaryCategory.Id)); %>
                </div>
            <% } %>
            <div class="row">
                <div class="col-sm-6 xxs-stack">
                    <% Html.RenderPartial("~/Views/Questions/Answer/AnswerQuestionDetails.ascx", Model); %>
                </div>
            </div>
            
            <% if (Model.ContentRecommendationResult != null) { %>
                <h4 style="margin-top: 30px;">Das könnte dich auch interessieren:</h4>
                <div class="row CardsLandscapeNarrow" id="contentRecommendation">
                    <% foreach (var set in Model.ContentRecommendationResult.Sets)
                       {
                            Html.RenderPartial("~/Views/Welcome/WelcomeBoxSingleSet.ascx", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(set.Id));
                       } %>
                    <% foreach (var category in Model.ContentRecommendationResult.Categories)
                       {
                            Html.RenderPartial("Cards/CardSingleCategory", CardSingleCategoryModel.GetCardSingleCategoryModel(category.Id));
                       } %>
                    <% foreach (var set in Model.ContentRecommendationResult.PopularSets)
                       { 
                            Html.RenderPartial("~/Views/Welcome/WelcomeBoxSingleSet.ascx", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(set.Id));
                       } %>
                </div>
            <% } %>

            <div class="row" style="margin-top: 30px; color: darkgray; font-weight: bold;">

                <div class="col-xs-4">
                    <h4 style="padding:0; margin:0;">Kommentare<a name="comments"></a></h4>    
                </div>
                
                <div class="col-xs-8 " style="vertical-align: text-bottom; 
                      vertical-align: bottom; margin-top: 3px; text-align: right">
                    <% if (Model.IsLoggedIn)
                       { %>
                        <span style="padding-right: 2px">
                            Die Frage bitte: &nbsp;
                            <a href="#modalQuestionFlagImprove" data-toggle="modal"><i class="fa fa-repeat">&nbsp;</i>verbessern!</a>&nbsp; / 
                            <a href="#modalQuestionFlagDelete" data-toggle="modal"><i class="fa fa-fire">&nbsp;</i>entfernen!</a>
                        </span>
                    <% } %>
                </div>
            </div>  
            
            <% Html.RenderPartial("~/Views/Questions/Answer/Comments/CommentsSection.ascx", Model); %>
                        
            <% if (Model.IsLoggedIn)
               { %>
                <div class="panel panel-default" style="margin-top: 7px;">
                    <div class="panel-heading">Neuen Kommentar hinzufügen</div>
                    <div class="panel-body">
                        <div class="col-xs-2">
                            <img style="width:100%; border-radius:5px;" src="<%= Model.ImageUrlAddComment %>">
                        </div>
                        <div class="col-xs-10">
                            <i class="fa fa-spinner fa-spin hide2" id="saveCommentSpinner"></i>
                            <textarea style="width: 100%; min-height: 82px;" class="form-control" id="txtNewComment" placeholder="Bitte höflich, freundlich und sachlich schreiben :-)"></textarea>
                        </div>
                    
                        <div class="col-xs-12" style="padding-top: 7px;">
                            <a href="#" class="btn btn-default pull-right" id="btnSaveComment">Speichern</a>
                        </div>
                    </div>                
                </div>
            <% } else { %>
                <div class="row" style="margin-bottom: 20px;">
                    <div class="col-xs-12" style="padding-top: 10px; color: darkgray">
                        Um zu kommentieren, musst du eingeloggt sein.
                    </div>                     
                </div>
            <% } %>

        </div>
        
        
    
        <%--MODAL IMPROVE--%>
        <div id="modalQuestionFlagImprove" class="modal fade">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button class="close" data-dismiss="modal">×</button>
                        <h3>Diese Frage verbessern</h3>
                    </div>
                    <div class="modal-body">
                        <div >
                            <p>
                                Ich bitte darum, dass diese Frage verbessert wird, weil: 
                            </p>
                            <ul style="list-style-type: none">
                                <li>
                                    <div class="checkbox">
                                        <label>
                                            <input type="checkbox" name="ckbImprove" value="shouldBePrivate"/> 
                                            <%= ShouldReasons.ByKey("shouldBePrivate") %>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="checkbox">
                                        <label>
                                            <input type="checkbox" name="ckbImprove" value="sourcesAreWrong"/> 
                                            <%= ShouldReasons.ByKey("sourcesAreWrong") %>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="checkbox">
                                        <label>
                                            <input type="checkbox" name="ckbImprove" value="answerNotClear"/> 
                                            <%= ShouldReasons.ByKey("answerNotClear") %>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="checkbox">
                                        <label>
                                            <input type="checkbox" name="ckbImprove" value="improveOtherReason"/>
                                            <%= ShouldReasons.ByKey("improveOtherReason") %>
                                        </label>
                                    </div>
                                </li>
                            </ul>
                        </div>
                        <p style="padding-top: 10px;">
                            Erläuterung zum Verbesserungsvorschlag:
                        </p>
                        <textarea style="width: 500px;" rows="3" id="txtImproveBecause"></textarea>
                        <p style="padding-top: 15px;">
                            Die Verbesserungsanfrage wird als Kommentar veröffentlicht und 
                            als Nachricht an <%= Model.CreatorName %> gesendet.
                        </p>
                    </div>
                    <div class="modal-footer">
                        <a href="#" class="btn" data-dismiss="modal">Schliessen</a>
                        <a href="#" class="btn btn-primary btn-success" id="btnImprove">Absenden</a>
                    </div>
                </div>
            </div>
        </div>
    
        <%--MODAL QUESTION FLAG DELETE--%>
        <div id="modalQuestionFlagDelete" class="modal fade">
             <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button class="close" data-dismiss="modal">×</button>
                        <h3>Diese Frage bitte löschen</h3>
                    </div>
                    <div class="modal-body">
                        <div >
                            <p>
                                Ich bitte darum, dass diese Frage gelöscht wird, weil: 
                            </p>
                            <ul style="list-style-type: none">
                                <li>
                                    <div class="checkbox">
                                        <label>
                                            <input type="checkbox" name="ckbDelete" value="deleteIsOffending"/>
                                            <%= ShouldReasons.ByKey("deleteIsOffending") %>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="checkbox">
                                        <label>
                                            <input type="checkbox" name="ckbDelete" value="deleteIsOffending"/>
                                            <%= ShouldReasons.ByKey("deleteCopyright") %>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="checkbox">
                                        <label>
                                            <input type="checkbox" name="ckbDelete" value="deleteIsSpam"/>
                                            <%= ShouldReasons.ByKey("deleteIsSpam") %>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="checkbox">
                                        <label>
                                            <input type="checkbox" name="ckbDelete" value="deleteOther"/>
                                            <%= ShouldReasons.ByKey("deleteOther") %>
                                        </label>
                                    </div>
                                </li>
                            </ul>
                        </div>
                        <p>
                            Weitere Erläuterung (optional):
                        </p>
                        <textarea style="width: 500px;" rows="3" id="txtDeleteBecause"></textarea>
            
                    </div>
                    <div class="modal-footer">
                        <a href="#" class="btn btn-default" data-dismiss="modal" id="A1">Schließen</a>
                        <a href="#" class="btn btn-primary btn-danger" id="btnShouldDelete">Absenden</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
<% if (Model.IsOwner) Html.RenderPartial("~/Views/Questions/Modals/ModalDeleteQuestion.ascx"); %>

</asp:Content>