<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="System.Web.Mvc.ViewPage<AnswerQuestionModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="TrueOrFalse" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <% Title = Model.QuestionTitle; %>
    <% if (Model.IsLearningSession ) { %>
        <meta name="robots" content="noindex" />
    <%}else { %>
        <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Links.AnswerQuestion(Model.Question) %>" />
    <% } %>
    
    <meta name="description" content="<%= Model.DescriptionForSearchEngines %>"/>
    
    <meta property="og:title" content="<%: Model.QuestionTitle %>" />
    <meta property="og:url" content="<%= Settings.CanonicalHost %><%= Links.AnswerQuestion(Model.Question) %>" />
    <meta property="og:type" content="article" />
    <meta property="og:image" content="<%= GetQuestionImageFrontendData.Run(Model.Question).GetImageUrl(435, true, imageTypeForDummy: ImageType.Question).Url %>" />
    <meta property="og:description" content="<%= Model.DescriptionForFacebook %>" />
</asp:Content>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">

    <%= Styles.Render("~/bundles/AnswerQuestion") %>
    <%= Scripts.Render("~/bundles/js/AnswerQuestion") %>
    <%= Scripts.Render("~/bundles/js/DeleteQuestion") %>
    <%= Scripts.Render("~/bundles/js/Vue")%>
    <%= Scripts.Render("~/bundles/js/d3") %>
    <%= Scripts.Render("~/bundles/js/tiptap") %>
    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/EditQuestionModalLoader.ascx") %>

    <script type="text/x-template" id="question-details-component">
        <%: Html.Partial("~/Views/Questions/Answer/AnswerQuestionDetailsComponent.vue.ascx") %>
    </script>
    <%= Scripts.Render("~/bundles/js/QuestionDetailsComponent") %>

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
        var relevanceForAllEntries = "<%= Model.TotalRelevanceForAllEntries %>";
    </script>
    <%
        if(Model.IsLearningSession)
        {
            if (!Model.LearningSession.Config.InWishknowledge)
            {
                if (Model.LearningSession.Config.Category != null)
                {
                    Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem {Text = Model.LearningSession.Config.Category.Name, Url = Links.CategoryDetail( Model.LearningSession.Config.Category)});
                }
            }
        }

        Model.TopNavMenu.IsCategoryBreadCrumb = false;
        Model.TopNavMenu.QuestionBreadCrumb = true;
        Model.SidebarModel.AuthorCardLinkText = Model.Creator.Name;
        Model.SidebarModel.AuthorImageUrl = Model.ImageUrl_250;

    %>    
    <link type="text/css" href="/Content/blue.monday/jplayer.blue.monday.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">  
<div class="" style="width: 100%; padding: 0 20px;">                             
<input type="hidden" id="hddIsLandingPage" value="<%=Model.PageCurrent == null ? "2" : Model.PageCurrent %>"/>  <%-- value "1" is Questionsite , value 2 is LandingPage, Test or Learningsession is this input not available--%> 


<% if (Model.IsLearningSession) { %>
           <% Html.RenderPartial("~/Views/Questions/Answer/LearningSession/LearningSessionHeader.ascx", Model); %>
<% }else { %>
      <%-- value "1" is Questionsite , value 2 is LandingPage, Test or Learningsession is this input not available--%> 
        <div class="AnswerQuestionHeader">
            <% Html.RenderPartial("~/Views/Questions/Answer/AnswerQuestionPager.ascx", Model); %>
        </div>
   <% } %>

    <div id="FirstRow"class="row">
        <% Html.RenderPartial("~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx", new AnswerBodyModel(Model)); %>
    </div>
    <div id="Topics"class="row ">
        <div style="display: none" id="GreaterThen767">
        <% Html.RenderPartial("~/Views/Questions/Answer/TopicToConinueLearning.ascx", new TopicToContinueLearningModel(Model, Model.ChildrenAndParents)); %>
        </div>
        <div style="display: none" id="SlowerThen768">
            <% Html.RenderPartial("~/Views/Questions/Answer/TopicToConinueLearning.ascx", new TopicToContinueLearningModel(Model, Model.AllCategorysWithChildrenAndParents)); %>
        </div>
    </div>
    <div class="row">
        <div class="">

           <div class= "col-sm-12">
                <% if (Model.QuestionHasParentCategories &&!Model.IsLearningSession && Model.ContentRecommendationResult.Categories.Count != 0)
                   {
                       Html.RenderPartial("~/Views/Shared/AnalyticsFooter.ascx", Model.AnalyticsFooterModel);
                   } %>
            
           </div>
        <div class="row" style="margin-top: 30px; color: darkgray; font-weight: bold;">
            <div id="JumpLabel"></div>
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
                        <a href="#" class="btn memo-button" data-dismiss="modal">Schliessen</a>
                        <a href="#" class="btn btn-primary btn-success memo-button" id="btnImprove">Absenden</a>
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
                        <a href="#" class="btn btn-default memo-button" data-dismiss="modal" id="A1">Schließen</a>
                        <a href="#" class="btn btn-primary btn-danger memo-button" id="btnShouldDelete">Absenden</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<% if (Model.IsOwner) Html.RenderPartial("~/Views/Questions/Modals/ModalDeleteQuestion.ascx"); %>
</div>
               
</asp:Content>