<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<TopicNavigationModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
                
<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperStart.ascx") %>

    <% if (Model.CategoryList.Any()) {
            if(!String.IsNullOrEmpty(Model.Title)){%>
                <h2><%: Model.Title %></h2>
            <% }
            if(!String.IsNullOrEmpty(Model.Text)){%>
                 <p><%: Model.Text %></p>
            <% } %>
    
        <div class="topicNavigation row" style= <%= Model.CategoryList.Count == 1 ? " \"justify-content: start;\" " : "" %> @click.stop="">
            <% var counter = 0; %>
            <% foreach (var category in Model.CategoryList)
                { %>
                
                    <% if(Model.ShowCategory(category) && (Model.GetTotalTopicCount(category) > 0 || Model.GetTotalQuestionCount(category) > 0 || Model.IsInstallationAdmin))   
                       { %>
                        <div class="col-xs-6 topic">
                            <div class="row">
                                <div class="col-xs-3">
                                    <div class="ImageContainer">
                                        <%= Model.GetCategoryImage(category).RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.CategoryDetail(category.Name, category.Id)) %>
                                    </div>
                                </div>
                                <div class="col-xs-9">
                                    <a class="topic-name" href="<%= Links.CategoryDetail(category) %>">
                                        <div class="topic-name">
                                            <% if (Model.GetTotalTopicCount(category) < 1 && Model.GetTotalQuestionCount(category) < 1 && Model.IsInstallationAdmin) { %>
                                                <i class="fa fa-user-secret show-tooltip" data-original-title="Thema ist leer und wird daher nur Admins angezeigt"></i>
                                            <% } %>
                                            <%= category.Type.GetCategoryTypeIconHtml() %><%: category.Name %>
                                        </div>
                                    </a>
                                    <div class="set-question-count">
                                        <span>
                                            <span class="Button Pin" data-category-id="<%= category.Id %>">
                                                <a href="#" class="noTextdecoration" style="font-size: 18px; height: 10px;padding-right:4px">
                                                    <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(category.IsInWishknowledge(), displayAdd:false)) %>
                                                </a>
                                            </span>
                                        </span>
                                        
                                        <% if (Model.GetTotalTopicCount(category) == 1)
                                           { %>1 Unterthema <% } %>
                                        <% if(Model.GetTotalTopicCount(category) > 1 && Model.GetTotalTopicCount(category) > 0)
                                           { %><%= Model.GetTotalTopicCount(category)  %> Unterthemen <% } 
                                           else { %><% } %><%=Model.GetTotalQuestionCount(category) %> Frage<% if(Model.GetTotalQuestionCount(category) != 1){ %>n<% } %>
                                    </div>
                                    <% if (Model.CategoryList[counter].CountQuestionsAggregated != 0)
                                       { %>  
                                        <div class="KnowledgeBarWrapper">
                                            <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(category)); %>
                                            <div class="KnowledgeBarLegend">Dein Wissensstand</div>
                                        </div>
                                        <% } %>
                                </div>
                            </div>
                        </div>
                    <% } %>
                <% counter++; %>
            <% } %>
        </div>
    <% } else { %>
        <div class="hidden">&nbsp;</div><% //if empty, templateparser throws error %>
    <% } %>
            
<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperEnd.ascx") %>