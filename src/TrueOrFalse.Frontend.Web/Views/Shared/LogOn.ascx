﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="TrueOrFalse.Web.Context" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%
    var userSession = new SessionUser();
    if (userSession.IsLoggedIn)
    {
%>
        <img src="/Images/Users/1_20.jpg" /> <span style="vertical-align: middle;">Hallo <b><%= Html.ActionLink(userSession.User.Name, "Profile", "UserProfile", new {name = userSession.User.Name, id = userSession.User.Id} , null)%></b>!
<a href="<%= Url.Action(Links.Logout, Links.AccountController) %>"><i class="icon-off" title="Abmelden"></i></a></span>
<%
    }
    else {
%> 
        <%= Html.ActionLink("Anmelden", "LogOn", "Account")%>
<%
    }
%>
