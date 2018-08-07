﻿<%@ Control Language="C#"  Inherits="System.Web.Mvc.ViewUserControl<BaseModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%  var userSession = new SessionUser();
    var user = userSession.User;
    string userImage = "";

    if (Model.IsLoggedIn)
    {
        var imageSetttings = new UserImageSettings(userSession.User.Id);
        userImage = imageSetttings.GetUrl_30px_square(userSession.User).Url;
    }

     %>

<div id="BreadCrumbContainer" class="container">
  <div id="BreadcrumbContainer" style="display:flex; width:100%; flex-wrap: nowrap;">
    <a href="/" id="BreadcrumbLogoSmall" class="show-tooltip" data-placement="bottom" title="Zur Startseite" style="display:none;">
        <img src="/Images/Logo/LogoSmall.png">
    </a>
    <div style="height: auto;" id="BreadcrumbHome" class="show-tooltip" data-placement="bottom"  title="Zur Startseite">
     <%if(!(Model.TopNavMenu.IsWelcomePage)){ %> 
        <a href="/" class="category-icon">
            <span style="margin-left: 10px">Home</span>
        </a>
        <span><i class="fa fa-chevron-right"></i></span>
     <%}%>
     </div>

<%if(!(Model.TopNavMenu.IsWelcomePage)){ %>  
    <%if(Model.TopNavMenu.IsCategoryBreadCrumb){ %>
        <%= Html.Partial("/Views/Categories/Detail/Partials/BreadCrumbCategories.ascx", Model.TopNavMenu) %>
    <% }else{
            if (Model.TopNavMenu.IsAnswerQuestionOrSetBreadCrumb) { %>
             <%= Html.Partial("/Views/Categories/Detail/Partials/BreadCrumbCategories.ascx", Model.TopNavMenu) %>
          <%}
        
       var i = 0;
       foreach (var breadCrumbItem in Model.TopNavMenu.BreadCrumb) {
            i++;%>
        <div id="<%=i %>BreadCrumbContainer" style="display: flex; height: auto; margin-bottom: 5px" class="show-tooltip" data-placement="bottom" <% if (Model.TopNavMenu.IsAnswerQuestionOrSetBreadCrumb){%>title="Zum Lernset" <% }else{ %> title="<%= breadCrumbItem.ToolTipText%>" <%}%> >                                                                                          
           <%if (breadCrumbItem.Equals(Model.TopNavMenu.BreadCrumb.Last())){%>
              <div style="display: flex; margin-left: 10px; color:#000000; opacity:0.50;"><div><a id="LastBreadcrumb" style="display:block; overflow:hidden; text-overflow:ellipsis;"  href="<%= breadCrumbItem.Url %>"><% if (Model.TopNavMenu.IsAnswerQuestionOrSetBreadCrumb){%>Lernset: <%} %><%= breadCrumbItem.Text %></a></div></div>
            <%} else {%>
               <div style="display: flex; margin-left: 10px;"><div><a id="<%= i %>BreadCrumb" style="display:block; overflow:hidden; text-overflow:ellipsis;"  href="<%= breadCrumbItem.Url %>"><%= breadCrumbItem.Text %></a></div>
               <div><i style="display: inline;" class="fa fa-chevron-right"></i></div>
               </div>  
            <%} %>
        </div>
    <% } %>        
    <%}%>
<%} %>
    <div id="StickyHeaderContainer">    
        <div class="input-group" id="StickyHeaderSearchBoxDiv" style="margin-right:25px">
            <input type="text" class="form-control" placeholder="Suche" id="StickyHeaderSearchBox">
            <div class="input-group-btn">
                <button class="btn btn-default" id="StickySearchButton" onclick="SearchButtonClick()" style="height:34px;" type="submit"><i class="fa fa-search" style="font-size:25px; padding:0px;margin:0px; margin-top:-3px" aria-hidden="true"></i></button>
            </div>
        </div>
        <div style="margin-right:0px;"><a href="<%= Links.Knowledge() %>"><i style="margin-top:6px; font-size:32px;" class="fa fa-dot-circle-o"></i></a></div>
        <div style="margin-right:25px">
           <a class="TextLinkWithIcon dropdown-toggle" id="dLabel" role="button" data-toggle="dropdown" data-target="#" href="#">
            <img class="userImage" style="margin-top:21px; border:none; text-align:center;" src="<%if(Model.IsLoggedIn){ %>"<%= userImage%> <%}%>" />
           </a>   
           <ul id="DropdownMenu" class="dropdown-menu pull-right" role="menu" aria-labelledby="dLabel">
                <li>
                   <a style="white-space:unset; padding:0px;" href="<%= Links.Knowledge()%>">
                       <div id="activity-popover-title">Dein erreichtes Level</div>
                       <div style="padding:3px 20px; margin:0px;">
                        <% Html.RenderPartial("/Views/Shared/ActivityPopupContent.ascx"); %>
                       </div>
                   </a>
                </li>
                <li style="border: solid #707070 1px; margin-left:-1px; width:101%;">
                    <a style="padding:0px;" href="<%= Links.Messages(Url)%>">
                        <div style="white-space:normal; display:flex; padding:22px 0px 24px 22px;">
                            <i style="font-size:24px; padding:0px;" class="fa fa-bell"></i>
                            <span style="display:block;" class="badge dropdown-badge show-tooltip" title="<%= Model.SidebarModel.UnreadMessageCount%> ungelesene Nachrichten" <%if(Model.SidebarModel.UnreadMessageCount != 0){%> style="background-color:#FF001F;" <%}%>><%= Model.SidebarModel.UnreadMessageCount %></span>
                            <span style="display:block;">Du hast <%if(Model.SidebarModel.UnreadMessageCount != 0){ %> <b><%= Model.SidebarModel.UnreadMessageCount %> neue Nachrichten.</b><%}else{ %>keine neuen Benachrichtigungen<%} %></span>
                        </div>
                    </a>
                </li>
               <%if(Model.IsLoggedIn){ %>
                     <li><a href="<%=Url.Action(Links.UserAction, Links.UserController, new {name = userSession.User.Name, id = userSession.User.Id}) %>"><i class="fa fa-user"></i> Deine Profilseite</a></li>
                     <li><a href="<%= Url.Action(Links.UserSettingsAction, Links.UserSettingsController) %>"><i class="fa fa-wrench" title="Einstellungen"></i> Konto-Einstellungen</a></li>
                     <li class="divider"></li>                 
                     <li><a href="#" id="btn-logout" data-url="<%= Url.Action(Links.Logout, Links.WelcomeController) %>" data-is-facebook="<%= user.IsFacebookUser() ? "true" : ""  %>"><i class="fa fa-power-off" title="Ausloggen"></i> Ausloggen</a>  </li>
                     <% if (userSession.IsInstallationAdmin){ %>
                         <li><a href="<%= Url.Action("RemoveAdminRights", Links.AccountController) %>"><i class="fa fa-power-off" title="Ausloggen"></i> Adminrechte abgeben</a>  </li>
                     <% } %>
               <%} %>
            </ul>
        </div>
        <div><a id="StickyMenuButton"><i class="fa fa-bars" style="font-size:inherit; margin-right:0px;"></i></a></div>
    </div>
 </div>
</div> 