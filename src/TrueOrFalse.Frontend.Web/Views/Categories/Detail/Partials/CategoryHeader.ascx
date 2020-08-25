<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="CategoryHeader">
    
    <% var buttonId = Guid.NewGuid(); %>
    <% if (!Model.Category.IsHistoric) { %>
        <div id="ManagementMobile">
            <div class="KnowledgeBarWrapper">
                <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
            </div>
            <div class="Buttons">
                <div class="Button Pin" data-category-id="<%= Model.Id %>">
                    <a href="#" class="noTextdecoration" style="font-size: 22px; height: 10px;">
                        <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge)) %>
                    </a>
                </div>
                <div class="Button dropdown">
                    <a href="#" id="<%= buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%= buttonId %>">
                        <li><a href="<%= Links.CategoryHistory(Model.Id) %>"><i class="fa fa-code-fork"></i>&nbsp;Bearbeitungshistorie</a></li>
                        <li><a href="<%= Links.CategoryEdit(Url, Model.Name, Model.Id) %>" data-allowed="logged-in" ><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a></li>
                        <li><a href="<%= Links.CreateQuestion(categoryId: Model.Id) %>" data-allowed="logged-in" ><i class="fa fa-plus-circle"></i>&nbsp;Frage hinzufügen</a></li>
                        <li><a href="<%= Links.CategoryCreate(Model.Id) %>" data-allowed="logged-in" ><i class="fa fa-plus-circle"></i>&nbsp;Unterthema hinzufügen</a></li>
                    </ul>
                </div>
            </div>
        </div>
    <% } %>

    <div id="HeadingSection">
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.CategoryDetail(Model.Category)) %>
        </div>
        <div id="HeadingContainer">
            <h1 style="margin-bottom: 0"><%= Model.Name %></h1>
            <div>
                <div class="greyed">
                    <%= Model.Category.Type == CategoryType.Standard ? "Thema" : Model.Type %> mit <% if (Model.AggregatedTopicCount == 1)
                                                                                                      { %> 1 Unterthema und <% }
                                                                                                      if (Model.AggregatedTopicCount > 1)
                                                                                                      { %> <%= Model.AggregatedTopicCount %> Unterthemen und <% } %><%= Model.AggregatedQuestionCount %> Frage<%= StringUtils.PluralSuffix(Model.AggregatedQuestionCount, "n") %>
                    <% if (Model.IsInstallationAdmin) { %>
                        <a href="#" id="jsAdminStatistics">
                            <span style="margin-left: 10px; font-size: smaller;" class="show-tooltip" data-placement="right" data-original-title="Nur von admin sichtbar">
                                (<i class="fa fa-user-secret" data-details="<%= Model.GetViewsPerDay() %>">&nbsp;</i><%= Model.GetViews() %> views)
                            </span>
                        </a>
                    
                        <div id="last60DaysViews" style="display: none"></div>
                        
                    <% } %>
                </div>
            </div>
        </div>
    </div>

    <% if (!Model.Category.IsHistoric) { %>
        <div id="TabsBar">
            <div id="CategoryTabsApp" class="Tabs">
                <div id="TopicTab" class="Tab" data-url="<%=Links.CategoryDetail(Model.Name, Model.Id) %>" >
                    <a href="">
                        <%= Model.Category.Type == CategoryType.Standard ? "Thema" : "Übersicht" %>
                    </a>
                </div>
                <div id="LearningTab" class="Tab" data-url="<%=Links.CategoryDetailLearningTab(Model.Name, Model.Id) %>">
                    <a href="" >
                        Lernen
                    </a>
                </div>
                <div id="AnalyticsTab" class="Tab" data-url="<%=Links.CategoryDetailAnalyticsTab(Model.Name, Model.Id) %>" >
                    <a href="">
                        Wissensnetz
                    </a>
                </div>
            </div>
            <div id="Management">
                <div class="Border"></div>
                <div class="KnowledgeBarWrapper">
                    <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
                    <%--<div class="KnowledgeBarLegend">Dein Wissensstand</div>--%>
                </div>
                <div class="Buttons">
                    <div id="ToggleMyWorldBtn">Toggle</div>
                    <div class="Button Pin" data-category-id="<%= Model.Id %>">
                        <a href="#" class="noTextdecoration" style="font-size: 22px; height: 10px;">
                            <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge)) %>
                        </a>
                    </div>
                    <div class="Button dropdown">
                        <% buttonId = Guid.NewGuid(); %>
                        <a href="#" id="<%= buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                            <i class="fa fa-ellipsis-v"></i>
                        </a>
                        <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%= buttonId %>">
                            <li><a href="<%= Links.CategoryHistory(Model.Id) %>"><i class="fa fa-code-fork"></i>&nbsp;Bearbeitungshistorie</a></li>
                            <li><a href="<%= Links.CreateQuestion(categoryId: Model.Id) %>" data-allowed="logged-in"><i class="fa fa-plus-circle"></i>&nbsp;Frage hinzufügen</a></li>
                            <li><a href="<%= Links.CategoryCreate(Model.Id) %>" data-allowed="logged-in"><i class="fa fa-plus-circle"></i>&nbsp;Unterthema hinzufügen</a></li>
                            <li><a href="<%= Links.CategoryEdit(Url, Model.Name, Model.Id) %>" data-allowed="logged-in"><i class="fa fa-pencil"></i>&nbsp;bearbeiten (Expertenmodus)</a></li>
                        </ul>
                    </div>
                </div>
                

            </div>
        </div>
    <% } %>
</div>