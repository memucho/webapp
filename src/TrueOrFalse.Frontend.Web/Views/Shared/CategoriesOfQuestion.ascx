﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Question>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div style="display: flex; flex-wrap: wrap;">

    <% foreach(var category in Model.Categories){ %>
        <% if (category.IsVisibleToCurrentUser()){%>

            <% if(category.IsSpoiler(Model)){ %>

                <div class="category-chip-container">
                    <a href="#" onclick="location.href='<%= Links.CategoryDetail(category)%>'">
                        <div class="category-chip show-tooltip" title="Das Thema entspricht der Antwort.">
                            <span>Spoiler</span>
                            <span class="remove-category-chip"></span>
                        </div>
                    </a>
                </div>
            <% }else{ %>
            <% Html.RenderPartial("CategoryLabel", EntityCache.GetCategoryCacheItem(category.Id)); %>
            <%} %>
        
        <%} %>


    <% } %>
</div>