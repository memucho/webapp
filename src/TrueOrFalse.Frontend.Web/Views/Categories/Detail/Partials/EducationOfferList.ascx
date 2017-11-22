<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<EducationOfferListModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<% if (Model.CategoryList.Any()) { %>

    <h2><%: Model.Title %></h2>
    <p><%: Model.Text %></p>

    <div class="topicNavigation row" style= <%= Model.CategoryList.Count == 1 ? " \"justify-content: start;\" " : "" %>>
        <% foreach (var category in Model.CategoryList)
            { %>
                <div class="col-xs-6 topic">
                    <div class="row">
                        <div class="col-xs-3">
                            <div class="ImageContainer">
                                <%= Model.GetCategoryImage(category).RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.CategoryDetail(category.Name, category.Id)) %>
                            </div>
                        </div>
                        <div class="col-xs-9">
                            <a class="topic-name" href="<%= Links.GetUrl(category) %>">
                                <div class="topic-name">
                                    <%--<%= category.Type.GetCategoryTypeIconHtml() %>--%><%: category.Name %>
                                </div>
                            </a>
                            <div class="set-question-count">
                                <%= category.Type.GetName() %> mit <br/>
                                <%: Model.GetTotalSetCount(category) %> Lernset<% if(Model.GetTotalSetCount(category) != 1){ %>s&nbsp;<% } else { %>&nbsp;<% } %> und 
                                <%: Model.GetTotalQuestionCount(category) %> Frage<% if(Model.GetTotalQuestionCount(category) != 1){ %>n<% } %>
                            </div>
                            <div class="KnowledgeBarWrapper">
                                <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(category)); %>
                                <div class="KnowledgeBarLegend">Dein Wissensstand</div>
                            </div>
                        </div>
                    </div>
                </div>
        <% } %>
    </div>

<% } else { %>
    <div class="hidden">&nbsp;</div><% //if empty, templateparser throws error %>
<% } %>