<%@ Page Title="Dein Ergebnis" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<TestSessionResultModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/AnswerQuestion") %>
    <%= Scripts.Render("~/bundles/js/TestSessionResult") %>
    <link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />
    
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% if(Model.TestSession.SessionNotFound) { %>
    
        <h2>Uuups...</h2>
        <p><br />Du hattest hier dein Wissen getestet. Leider kannst du nicht fortsetzen, weil deine Sitzung inzwischen abgelaufen ist.</p>
        <p><a href="/">Zur Startseite</a></p>

    <% } else { %>
    
        <% Html.RenderPartial("~/Views/Questions/Answer/TestSession/TestSessionResultHead.ascx", Model);  %>
    
<%--        <% if (Model.IsLoggedIn)
                Html.RenderPartial("~/Views/Api/ActivityPoints/ActivityLevelProgress.aspx", new ActivityLevelProgressModel(Sl.SessionUser.User));
            else
                Html.RenderPartial("~/Views/Api/ActivityPoints/ActivityLevelProgress.aspx",
                    new ActivityLevelProgressModel(
                        new User {
                            ActivityPoints = Sl.SessionUser.getTotalActivityPoints(),
                            ActivityLevel = UserLevelCalculator.GetLevel(Sl.SessionUser.getTotalActivityPoints())
                        }));
           %>--%>

        <% if (!Model.IsLoggedIn) { %>
            <div class="bs-callout bs-callout-info" id="divCallForRegistration" style="width: 100%; margin-top: 0; text-align: left; opacity: 0; display: none;">
                <div class="row">
                    <div class="col-xs-12">
                        <h3 style="margin-top: 0;">Schneller lernen, länger wissen</h3>
                        <p>
                            Registriere dich bei memucho, um von den vielen Vorteilen personalisierten Lernens zu profitieren. <strong>memucho ist kostenlos!</strong>
                        </p>
                    </div>
                    <div class="col-xs-12 claimsMemucho">
                        <div class="row">
                            <div class="col-xs-4" style="text-align: center;">
                                <i class="fa fa-3x fa-line-chart"></i><br/>
                                Personalisiere dein Lernen
                            </div>
                            <div class="col-xs-4" style="text-align: center;">
                                <i class="fa fa-3x fa-heart"></i><br/>
                                Sammele dein Wunschwissen
                            </div>
                            <div class="col-xs-4" style="text-align: center;">
                                <i class="fa fa-3x fa-lightbulb-o"></i><br/>
                                Entscheide, was du nie vergessen willst
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12" style="text-align: right;">
                        <a href="<%= Links.AboutMemucho() %>" class="btn btn-link">Erfahre mehr über memucho</a>
                        <a href="<%= Url.Action("Register", "Register") %>" class="btn btn-success shakeInInterval" role="button"><i class="fa fa-chevron-circle-right">&nbsp;</i> Jetzt Registrieren</a> <br/>
                    </div>
                </div>
            </div>
        <% } %>


        <div class="row">
            <div class="col-sm-12">
            
                <% Html.RenderPartial("~/Views/Questions/Answer/TestSession/TestSessionResultDetails.ascx", Model);  %>
                     
                <div class="buttonRow">
                    <a href="<%= Url.Action(Links.KnowledgeAction, Links.KnowledgeController) %>" class="btn btn-link" style="padding-right: 10px">
                        Zur Wissenszentrale
                    </a>
                    <a href="<%= Model.LinkForRepeatTest %>" class="btn btn-primary show-tooltip" style="padding-right: 10px"
                            title="Neue Fragen <% if (Model.TestSession.IsSetSession) Response.Write("aus demselben Lernset");
                                                      else if (Model.TestSession.IsSetsSession) Response.Write("aus denselben Lernsets");
                                                      else if (Model.TestSession.IsCategorySession) Response.Write("zum selben Thema");%>
                        " rel="nofollow">
                        <i class="fa fa-play-circle AnswerResultIcon">&nbsp;&nbsp;</i>Weitermachen!
                    </a>
                </div>

                <% if (Model.ContentRecommendationResult != null) { %>
                    <div style="margin-top: 80px;">
                        <h4>Das könnte dich auch interessieren:</h4>
                        <div class="row CardsLandscape" id="contentRecommendation">
                            <% foreach (var set in Model.ContentRecommendationResult.Sets)
                               {
                                    Html.RenderPartial("~/Views/Welcome/WelcomeBoxSingleSet.ascx", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(set.Id));
                               } %>
                            <% foreach (var category in Model.ContentRecommendationResult.Categories)
                               {
                                    Html.RenderPartial("~/Views/Shared/Cards/CardSingleCategory.ascx", CardSingleCategoryModel.GetCardSingleCategoryModel(category.Id));
                               } %>
                            <% foreach (var set in Model.ContentRecommendationResult.PopularSets)
                               { 
                                    Html.RenderPartial("~/Views/Welcome/WelcomeBoxSingleSet.ascx", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(set.Id));
                               } %>
                        </div>
                    </div>
                <% } %>
            </div>
        </div>              
    <% } %>

</asp:Content>