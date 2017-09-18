﻿<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div style="padding-bottom: 15px;">
    <div class="BreadcrumbsMobile DesktopHide">
        <a href="/" class=""><i class="fa fa-home"></i></a>
        <span> <i class="fa fa-chevron-right"></i> </span>
        <% foreach (var item in Model.BreadCrumb){%>
            <a href="<%= Links.CategoryDetail(item) %>" class=""><%= item.Name %></a>
            <span> <i class="fa fa-chevron-right"></i> </span>
        <%}%>
        
        <a href="#" class="current"><%= Model.Category.Name %></a>

    </div>
</div>

<div id="CategoryHeader" style ="margin-bottom: 200px;">
    <div id="HeadingSection">
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.CategoryDetail(Model.Category)) %>
        </div>
        <div id="HeadingContainer">
            <h1 style="margin-bottom: 0"><%= Model.Name %></h1>
            <div>
                <div class="greyed">
                    <%= Model.Category.Type == CategoryType.Standard ? "Thema" : Model.Type %> mit <%= Model.AggregatedSetCount %> Lernset<%= StringUtils.PluralSuffix(Model.AggregatedSetCount, "s") %> und <%= Model.AggregatedQuestionCount %> Frage<%= StringUtils.PluralSuffix(Model.AggregatedQuestionCount, "n") %>
                    <% if(Model.IsInstallationAdmin) { %>
                        <span style="margin-left: 10px; font-size: smaller;" class="show-tooltip" data-placement="right" data-original-title="Nur von admin sichtbar">
                            (<i class="fa fa-user-secret">&nbsp;</i><%= Model.GetViews() %> views)
                        </span>    
                    <% } %>
                </div>
            </div>
        </div>
    </div>
    <div id="TabsBar">
        <div class="Tabs">
            <div class="Tab ">
                Thema
            </div>
            <div class="Tab active">
                Lernen
            </div>
            <div class="Tab">
                Analytics
            </div>
        </div>
        <div id="Management">
            <div class="Border">
                
            </div>
            <div class="KnowledgeBarWrapper">
                <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
                <%--<div class="KnowledgeBarLegend">Dein Wissensstand</div>--%>
            </div>
            <div class="Buttons">
                <div class="Button Facebook">
                    <span class="fa-stack fa-lg">
                        <i class="fa fa-circle fa-stack-2x"></i>
                        <i class="fa fa-facebook fa-stack-1x fa-inverse"></i>
                    </span>
                </div>
                <div class="Button Pin" data-category-id="<%= Model.Id %>">
                    <a href="#" class="noTextdecoration" style="font-size: 22px; height: 10px;">
                        <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge)) %>
                        <span class="Text">Hinzufügen</span>
                    </a>
                </div>
                <div class="Button dropdown">
                    <% var buttonId = Guid.NewGuid(); %>
                    <a href="#" id="<%=buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu" aria-labelledby="<%=buttonId %>">
                        <li><a href="<%= Links.CategoryDetail(Model.Name, Model.Id) %>"> Detailseite zum Thema</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</div>