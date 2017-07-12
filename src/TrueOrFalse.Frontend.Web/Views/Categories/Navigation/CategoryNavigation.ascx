﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryNavigationModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="mainMenuThemeNavigation" class="menu-section">
<% var defaultCategories = Model.DefaultCategoriesList;
   for (int i = 0; i < defaultCategories.Count; i++)
   { %>
       <a class="list-group-item cat primary-point" id="default-category-<%= defaultCategories[i].Id %>" href="<%= Links.CategoryDetail(defaultCategories[i].Name, defaultCategories[i].Id) %>">
    <% switch (i)
       {
           case 0:
                %>
                    <span class="fa-stack fa-fw">
                        <i class="fa fa-circle fa-stack-2x default-category-icon"></i>
                        <i class="fa fa-child fa-stack-1x fa-inverse"></i>
                    </span>
                <%
               break;

           case 1:
                %> 
                    <span class="fa-stack fa-fw">
                        <i class="fa fa-circle fa-stack-2x default-category-icon"></i>
                        <i class="fa fa-graduation-cap fa-stack-1x fa-inverse"></i>
                    </span>
                <%
               break;

           case 2:
                %>
                    <span class="fa-stack fa-fw">
                        <i class="fa fa-circle fa-stack-2x default-category-icon"></i>
                        <i class="fa fa-file-text-o fa-stack-1x fa-inverse"></i>
                    </span>
                <%
               break;

           case 3:
                %>
                    <span class="fa-stack fa-fw">
                        <i class="fa fa-circle fa-stack-2x default-category-icon"></i>
                        <i class="fa fa-lightbulb-o fa-stack-1x fa-inverse"></i>
                    </span>
                <%
               break;
       } %>
            <span class="primary-point-text">
                <%: defaultCategories[i].Name %>
            </span>
       </a>
<% } %>
</div>

<script>
<% if (Model.ActualCategory != null)
   { %>
    var rootCategory = $("#default-category-<%= Model.RootCategory.Id %>");
    rootCategory.addClass("active");

    <% if(Model.ActualCategory != Model.RootCategory)
        { %>

            var actualCategory = $('<a class= "cat sub list-group-item actual-category active" href="<%= Links.CategoryDetail(Model.ActualCategory.Name, Model.ActualCategory.Id) %>">')
                                .append($('<i class="fa fa-caret-right"></i>'))
                                .append($('<span class="actual-sub-category"><%: Model.ActualCategory.Name %></span>'));
            rootCategory.after(actualCategory);

            <% if (Model.CategoryTrail.Count > 0)
               { %>
                    var upperCategory = $('<a class="cat sub list-group-item active" href="<%= Links.CategoryDetail(Model.CategoryTrail.Last().Name, Model.CategoryTrail.Last().Id) %>">')
                                        .append('<span class="sub-category"><%: Model.CategoryTrail.Last().Name %></span>');
                    rootCategory.after(upperCategory);
            <% } %>
    <% } %>
<% } %>
</script>