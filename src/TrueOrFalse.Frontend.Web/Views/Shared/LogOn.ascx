<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseModel>" %>
<%@ Import Namespace="FluentNHibernate.Utils" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% 
    var userSession = new SessionUser();
    var user = userSession.User;
%>
<div style="display: flex;">
    <div class="header-item" style="margin-top: -1px; margin-right: 3px;">
        <div class="input-group" id="SmallHeaderSearchBoxDiv">
            <input type="text" class="form-control" placeholder="Suche" id="SmallHeaderSearchBox">
            <div class="input-group-btn" style="height: 34px;">
                <button class="btn btn-default" id="SmallHeaderSearchButton" style="padding-top: 0px; font-size: 29px; align-content: center; display: flex; height: 34px;" onclick="SearchButtonClick()" type="submit"><i class="fa fa-search" style="color: white;" aria-hidden="true"></i></button>
            </div>
        </div>
    </div>
    <div class="header-item" <% if (Model.IsLoggedIn && Model.SidebarModel.UnreadMessageCount == 0){ %> style="margin-right: 15px;"<% } %> >
        <a href="<%=Links.Knowledge() %>" data-btn-login="<%= !userSession.IsLoggedIn %>" class="TextLinkWithIcon KnowledgeLink">
            <i style="font-size: 32px;" class="fa fa-dot-circle-o"></i>
            <span class="primary-point-text TextSpan" style="padding-top: 6px;">Wissenszentrale</span>
        </a>
    </div>
    <div class="header-item"
         <%if (!userSession.IsLoggedIn){%>
            style="margin-top: -2px; padding-left: 10px;" 
         <%} %> id="Login">

        <%if (userSession.IsLoggedIn)
            {
                var imageSetttings = new UserImageSettings(userSession.User.Id);
        %>
        <div style="display: flex; flex-direction: column;">
            <div class="dropdown" id="HeaderUserDropdown" style="display: inline-block;">
                <a class="TextLinkWithIcon dropdown-toggle" id="dLabel" role="button" data-toggle="dropdown" data-target="#" href="#">
                    <div style="display: flex; justify-content: center;">
                        <%if (Model.SidebarModel.UnreadMessageCount != 0)
                            { %>
                        <span id="badgeNewMessages" class="badge show-tooltip badge-header"><%= Model.SidebarModel.UnreadMessageCount %></span>
                        <%}%>
                        <img class="userImage" src="<%= imageSetttings.GetUrl_30px_square(userSession.User).Url %>" />
                        <span id="header-level-display" class="level-display">
                            <span style="display: inline-block; white-space: nowrap; margin-top: -8px;" class="" data-placement="bottom">
                                <span class="half-circle"><span style="height: 31px; width: 31px; flex-direction: column; display: flex; text-align: center; justify-content: center;" class="level-count"><%= userSession.User.ActivityLevel %></span></span>
                            </span>
                        </span>
                    </div>
                    <span class="userName TextSpan" style="font-weight: normal; line-height: normal; padding-top: 4px;">Hallo <b><%= userSession.User.Name%></b><b class="caret"></b></span>
                </a>
                <ul class="dropdown-menu pull-right" id="userDropdown" role="menu" aria-labelledby="dLabel" style="right: 0px;">
                    <li>
                        <a style="white-space: unset; padding: 0px;" href="<%= Links.Knowledge()%>">
                            <div id="activity-popover-title">Deine Lernpunkte</div>
                            <div style="padding: 3px 20px 0px 20px;">
                                <% Html.RenderPartial("/Views/Shared/ActivityPopupContent.ascx"); %>
                            </div>
                        </a>
                    </li>
                    <li class="divider"></li>
                    <li>
                        <a class="<%= Model.UserMenuActive(UserMenuEntry.Messages) %>" href="<%=Links.Messages(Url) %>"  style="display: flex;">Deine Nachrichten                        
                            <% if (Model.SidebarModel.UnreadMessageCount != 0) { %>
                                <svg class="badge" height="100" width="100">
                                    <g>
                                        <circle cx="16" cy="10" r="8" fill="#FF001F"/>
                                        <text class="level-count" x="60%" text-anchor="middle" font-size="10" y="55%" dy=".34em" fill="white"><%= Model.SidebarModel.UnreadMessageCount %></text>
                                    </g>
                                </svg>                
                            <% } %>
                        </a> 
                    </li>
                    <li><a class="<%= Model.UserMenuActive(UserMenuEntry.Network) %>" href="<%=Links.Network() %>">Dein Netzwerk</a></li>
                    <li><a class="<%= Model.UserMenuActive(UserMenuEntry.UserDetail) %>" href="<%=Url.Action(Links.UserAction, Links.UserController, new {name = userSession.User.Name, id = userSession.User.Id}) %>">Deine Profilseite</a></li>
                    <li class="divider"></li>
                    <li><a class="<%= Model.UserMenuActive(UserMenuEntry.UserSettings) %>" href="<%= Url.Action(Links.UserSettingsAction, Links.UserSettingsController) %>">Konto-Einstellungen</a></li>
                    <li><a  <% if (!userSession.IsInstallationAdmin) {%> style="padding-bottom: 15px;" <%}%> href="#" id="btn-logout" data-url="<%= Url.Action(Links.Logout, Links.WelcomeController) %>" data-is-facebook="<%= user.IsFacebookUser ? "true" : ""  %>">Ausloggen</a>  </li>
                    <% if (userSession.IsInstallationAdmin)  { %>
                        <li><a style="padding-bottom: 15px;" href="<%= Url.Action("RemoveAdminRights", Links.AccountController) %>">Adminrechte abgeben</a>  </li>
                    <% } %>
                    <li><a href="#" data-url="<%=Url.Action("ToggleWorld", Links.AccountController) %>">Toggle World</a></li>
                </ul>
            </div>
        </div>
        <%
            }
            else
            {
        %>
        <a class="TextLinkWithIcon" href="#" data-btn-login="true"><i style="font-size: 36px;" class="fa fa-sign-in"></i>
            <span style="padding-top: 4px" class="TextSpan">Einloggen</span></a>
        <%
            }
        %>
    </div>
    <div id="MenuButtonContainer" class="header-item" style="margin-right: 0px;">
        <a id="MenuButton" class="TextLinkWithIcon"><i class="fa fa-bars"></i>
            <span style="padding-top: 7px;" class="TextSpan">Menü</span></a>
    </div>
</div>
