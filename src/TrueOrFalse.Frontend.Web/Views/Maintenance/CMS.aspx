﻿<%@ Page Title="CMS" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<CMSModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Head">
    <%= Scripts.Render("~/bundles/js/MaintenanceCMS") %>
    <meta id="blablabla"/>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <nav class="navbar navbar-default" style="" role="navigation">
        <div class="container">
            <a class="navbar-brand" href="#">Maintenance</a>
            <ul class="nav navbar-nav">
                <li><a href="/Maintenance">Allgemein</a></li>
                <li><a href="/MaintenanceImages/Images">Bilder</a></li>
                <li><a href="/Maintenance/Messages">Nachrichten</a></li>
                <li><a href="/Maintenance/Tools">Tools</a></li>
                <li class="active"><a href="/Maintenance/CMS">CMS</a></li>
                <li><a href="/Maintenance/ContentCreatedReport">Cnt-Created</a></li>
                <li><a href="/Maintenance/ContentStats">Cnt Stats</a></li>
                <li><a href="/Maintenance/Statistics">Stats</a></li>
            </ul>
        </div>
    </nav>
    <% Html.Message(Model.Message); %>
        

    <div>
        <h2>Tools zur Content-Pflege</h2>
        
        <div id="categoryNetworkNavigationWrapper">
            <h4>Themen-Navigation</h4>
            <a href="#" class="networkNavigationUpdate" data-category-id="682"><span class="label label-category">Schule</span></a>
            <a href="#" class="networkNavigationUpdate" data-category-id="687"><span class="label label-category">Studium</span></a>
            <a href="#" class="networkNavigationUpdate" data-category-id="689"><span class="label label-category">Zertifikate</span></a>
            <a href="#" class="networkNavigationUpdate" data-category-id="709"><span class="label label-category">Allgemeinwissen</span></a>

            <div id="categoryNetworkNavigation">
                <% Html.RenderPartial("~/Views/Categories/Navigation/CategoryNetworkNavigation.ascx", new CategoryNetworkNavigationModel(709)); %>
            </div>
        </div>
        

        <div id="showLooseCategories">
            <h4 style="margin-top: 40px;">Lose Themen</h4>
            <p>
                Themen anzeigen, die nicht in eines der vier Oberthemen eingehangen sind: 
                <a href="#" id="btnShowLooseCategories" class="btn btn-default">Themen anzeigen</a>
            </p>
            <div id="showLooseCategoriesResult" style="margin-left: 25px; padding: 0 10px 10px;"></div>
        </div>

        <div id="showCategoriesWithNonAggregatedChildren">
            <h4 style="margin-top: 40px;">Themen mit unbearbeitetem Aggregierungsstatus</h4>
            <p>
                Themen anzeigen, die Unterthemen haben, über deren Aggregierungs-Status noch nicht entschieden ist: 
                <a href="#" id="btnShowCategoriesWithNonAggregatedChildren" class="btn btn-default">Themen anzeigen</a>
            </p>
            <div id="showCategoriesWithNonAggregatedChildrenResult" style="margin-left: 25px; padding: 0 10px 10px;"></div>
        </div>

        <div id="showCategoriesInSeveralRootCategories">
            <h4 style="margin-top: 40px;">Themen in verschiedenen Bäumen</h4>
            <p>
                Themen anzeigen, die in mind. 2 der Root-Kategorien eingehangen sind: 
                <a href="#" id="btnShowCategoriesInSeveralRootCategories" class="btn btn-default">Themen anzeigen</a>
            </p>
            <div id="showCategoriesInSeveralRootCategoriesResult" style="margin-left: 25px; padding: 0 10px 10px;"></div>
        </div>

        <div id="showOvercategorizedSets">
            <h4 style="margin-top: 40px;">Überkategorisierte Lernsets</h4>
            <p>
                Lernsets anzeigen, die mehrere Themen haben, wobei mind. eines ein (direktes oder indirektes) aggregiertes Unterthema von einem anderen ist: 
                <a href="#" id="btnShowOvercategorizedSets" class="btn btn-default">Lernsets anzeigen</a>
            </p>
            <div id="showOvercategorizedSetsResult" style="margin-left: 25px; padding: 0 10px 10px;"></div>
        </div>

        <div id="showSetsWithDifferentlyCategorizedQuestions">
            <h4 style="margin-top: 40px;">Inkongruente Kategorisierung von Fragen in Lernsets</h4>
            <p>
                Lernsets anzeigen, bei denen die Fragen andere Themen zugewiesen haben als die Lernsets, in denen sie sind. 
                Für jedes Lernset wird geschaut, ob es darin mind. 1 Frage gibt, die andere Themen hat als die Lernsets zu denen sie gehört 
                (sortiert nach Erstellungsdatum, Neueste zuerst): 
                <a href="#" id="btnShowSetsWithDifferentlyCategorizedQuestions" class="btn btn-default">Lernsets anzeigen</a>
            </p>
            <div id="showSetsWithDifferentlyCategorizedQuestionsResult" style="margin-left: 25px; padding: 0 10px 10px;"></div>
        </div>
    </div>
    
    <div>
        <h2 style="margin-top: 50px;">Lernsets für Empfehlungen und Spiele</h2>
        <% using (Html.BeginForm("CMS", "Maintenance")){%>
        
            <%= Html.AntiForgeryToken() %>
            <div class="form-group">
                <label class="control-label"><span style="font-weight: bold">Vorgeschlagene Lernsets</span> (Set-Ids kommasepariert)</label>
                <i class="fa fa-info-circle show-tooltip" title="Diese Lernsets werden bei den Inhalteempfehlungen zusätzlich zu allen Lernsets von memucho berücksichtigt.">&nbsp;</i>
                <%= Html.TextBoxFor(m => m.SuggestedSetsIdString, new {@class="form-control"} ) %>
                <% foreach(var set in Model.SuggestedSets) { %>
                    <a href="<%= Links.SetDetail(Url, set) %>"><span class="label label-set"><%: set.Id %>-<%: set.Name %></span></a>
                <% } %>
            </div>
        
            <div class="form-group">
                <label class="control-label"><span style="font-weight: bold">Vorgeschlagene Spiele</span> (Set-Ids kommasepariert)</label>
                <%= Html.TextBoxFor(m => m.SuggestedGames, new {@class="form-control"} ) %>
                <% foreach(var set in Model.SuggestedGameSets) { %>
                    <a href="<%= Links.SetDetail(Url, set) %>"><span class="label label-set"><%: set.Id %>-<%: set.Name %></span></a>
                <% } %>
            </div>
        
            <input type="submit" value="Speichern" class="btn btn-primary" name="btnSave" />

        <% } %>
    </div>


</asp:Content>