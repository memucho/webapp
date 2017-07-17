<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WidgetMenuModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<nav class="navbar navbar-default">
    <div class="container-fluid">
        <div class="navbar-header">
            <a class="navbar-brand" href="/Hilfe/Widget">Widgets</a>
        </div>
        <ul class="nav navbar-nav">
            <li class="<%= Model.CurrentIsExample? "active" : "" %>"><a href="/Widget-Beispiele">Beispiele</a></li>
            <li class="<%= Model.CurrentIsPricing? "active" : "" %>"><a href="/Widget-Angebote-Preisliste">Preise</a></li>
            <li class="<%= Model.CurrentIsHelp ? "active" : "" %> dropdown">
                <a href="/Hilfe/Widget" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Hilfe <span class="caret"></span></a>
                <ul class="dropdown-menu">
                    <li><a href="<%= "/Hilfe/Widget" %>">Hilfe zum Einbetten</a></li>
                    <li role="separator" class="divider"></li>
                    <li><a href="<%= Links.HelpWidgetWordpress() %>">in Wordpress</a></li>
                    <li><a href="<%= Links.HelpWidgetMoodle() %>">in Moodle</a></li>
                    <li><a href="<%= Links.HelpWidgetBlackboard() %>">in Blackboard</a></li>
                </ul>
            </li>
        </ul>
    </div>
</nav>