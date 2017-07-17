<%@ Page Title="Spielen" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.Widget.Master" 
    Inherits="ViewPage<WidgetSetResultModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
        html { height: auto;}
    </style>
    <%= Scripts.Render("~/bundles/js/TestSessionResult") %>
    <%= Scripts.Render("~/bundles/js/AwesomeIframe") %>
    <link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />

    <% if(Model.IncludeCustomCss){ %>
        <link href="<%= Model.CustomCss %>" rel="stylesheet" />
    <% } %>    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    
    <div id="WidgetSetResult">
        <% if(Model.TestSessionResultModel.TestSession.SessionNotFound) { %>
    
            <h2>Uuups...</h2>
            <p>Du hattest hier dein Wissen getestet. Leider kannst du nicht fortsetzen, weil deine Sitzung inzwischen abgelaufen ist.</p>

        <% } else { %>

            <% Html.RenderPartial("~/Views/Questions/Answer/TestSession/TestSessionResultHead.ascx", Model.TestSessionResultModel);  %>
        
            <div class="row">
                <div class="col-sm-12">
                    <% Html.RenderPartial("~/Views/Questions/Answer/TestSession/TestSessionResultDetails.ascx", Model.TestSessionResultModel);  %>
                </div>
            </div>
    
            <div class="buttonRow">
                <a href="<%= Model.StartSessionUrl %>" class="btn btn-primary show-tooltip" style="padding-right: 10px"
                        title="Neue Fragen <% if (Model.TestSessionResultModel.TestSession.IsSetSession) Response.Write("aus demselben Lernset");
                                                    else if (Model.TestSessionResultModel.TestSession.IsSetsSession) Response.Write("aus denselben Lernsets");
                                                    else if (Model.TestSessionResultModel.TestSession.IsCategorySession) Response.Write("zum selben Thema");%>
                    " rel="nofollow">
                    <i class="fa fa-play-circle AnswerResultIcon">&nbsp;&nbsp;</i>Weitermachen!
                </a>
            </div>
    
        <% } %>
    </div>
</asp:Content>