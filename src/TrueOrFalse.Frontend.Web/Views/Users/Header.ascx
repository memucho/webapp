﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HeaderModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="boxtainer-header MobileHide">
    <ul class="nav nav-tabs">
        <li class="<%= Html.IfTrue(!Model.IsNetworkTab, "active") %>">
            <a href="<%= Links.Users() %>" >Alle Nutzer (<%= Model.TotalUsers %>)</a>
        </li>
        <li class="<%= Html.IfTrue(Model.IsNetworkTab, "active") %>">
            <a href="<%= Links.Network() %>">
                Mein Netzwerk 
                (<span class="JS-AmountFollowers"><%= Model.TotalIFollow %></span>&nbsp;&ndash;&nbsp;<%= Model.TotalFollowingMe %>)
                <i class="fa fa-question-circle show-tooltip" id="tabInfoMyKnowledge" data-placement="right" title="Hier siehst du, wer dir folgt und wem du folgst."></i>
            </a>
        </li>
    </ul>
</div>