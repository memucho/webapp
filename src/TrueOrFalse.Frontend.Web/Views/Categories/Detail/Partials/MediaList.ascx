<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<MediaListModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% if (Model.MediaList.Any()) { %>

    <h2><%: Model.Title %></h2>
    <p><%: Model.Text %></p>

    <div class="mediaList row" style= <%= Model.MediaList.Count == 1 ? " \"justify-content: start;\" " : "" %>>
        <% foreach (var category in Model.MediaList)
           {
               var questionCount = Model.GetTotalQuestionCount(category); %>
                <% if(Model.GetTotalSetCount(category) > 0 || questionCount > 0 || Model.IsInstallationAdmin)
                   { %>
                    <div class="col-xs-6 mediaReference">
                        <a href="<%= Links.GetUrl(category) %>">
                            <div class="topic-name">
                                <% Html.RenderPartial("~/Views/Categories/Detail/Partials/MediaListReferenceTitle.ascx", category); %>
                            </div>
                        </a>
                        <div class="set-question-count">
                            <% if (Model.GetTotalSetCount(category) < 1 && questionCount < 1 && Model.IsInstallationAdmin) { %>
                                <i class="fa fa-user-secret show-tooltip" style="color: #afd534;" data-original-title="Thema ist leer und wird daher nur Admins angezeigt"></i>
                            <% } %>
                            <%= category.Type.GetCategoryTypeIconHtml() %><%= category.Type.GetShortName() %> mit 
                            <%= questionCount %> Frage<%= StringUtils.PluralSuffix(questionCount,"n") %>
                        </div>
    <%--                    <div class="KnowledgeBarWrapper">
                            <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(category)); %>
                            <div class="KnowledgeBarLegend">Dein Wissensstand</div>
                        </div>--%>
                    </div>
                <% } %>
        <% } %>
    </div>
<% } else { %>
    <div class="hidden">&nbsp;</div><% //if empty, templateparser throws error %>
<% } %>