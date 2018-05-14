﻿<%@ Control Language="C#"  Inherits="System.Web.Mvc.ViewUserControl<TopNavMenu>" %>

<div  style="display: flex; flex-wrap: wrap; width: 100%;">                      
    <div style="height: auto; margin-bottom: 5px;" class="show-tooltip"  title="zur Startseite">
        <i class="fa fa-home"></i>
        <a href="/" class="category-icon">
            <span style="margin-left: 7px">Home</span>
        </a>
        <span><i class="fa fa-chevron-right"></i></span>
    </div>        
    
    <%if(Model.IsCategoryBreadCrumb){ %>
        <%= Html.Partial("/Views/Categories/Detail/Partials/BreadCrumbCategories.ascx", Model) %>
    <% }else{ 
        foreach (var breadCrumbItem in Model.BreadCrumb) { %>
        <i style="display: inline;" class="fa <%= breadCrumbItem.ImageClass %>"></i>
        <div style="display: flex; height: auto; margin-bottom: 5px" class="show-tooltip" title="zur Themenseite">
            <span style="display: inline-table; margin-left: 10px;"><a href="<%= breadCrumbItem.Url %>"><%= breadCrumbItem.Text %></a>
                <i style="display: inline;" class="fa fa-chevron-right"></i>
            </span>          
        </div>
    <% } %>        
    <%}%>
</div> 
