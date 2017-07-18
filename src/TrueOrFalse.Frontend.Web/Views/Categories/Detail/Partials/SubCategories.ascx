﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SubCategoriesModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<h1><%: Model.Title %></h1>
<p><%: Model.Text %></p>

<div id="subCategories" class="row">
    <% foreach (var subCategory in Model.SubCategoryList)
        { %>
            <div class="col-xs-6">
                <div class="row">
                    <div class="col-xs-3">
                        <%= Model.GetCategoryImage(subCategory).RenderHtmlImageBasis(100, false, ImageType.Category) %>
                    </div>
                    <div class="col-xs-9">
                        <a href="<%= Links.GetUrl(subCategory) %>"><%: subCategory.Name %></a>
                        <div class="set-question-count"><%: Model.GetTotalSetCount(subCategory) %> Lernset <%: Model.GetTotalQuestionCount(subCategory) %> Fragen</div>
                        <%-- HIER PROGRESS BAR REIN --%>
                    </div>
                </div>
            </div>
    <% } %>
</div>
