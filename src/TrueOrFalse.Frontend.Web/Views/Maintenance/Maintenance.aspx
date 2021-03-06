﻿<%@ Page Title="" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.Sidebar.Master" 
    Inherits="System.Web.Mvc.ViewPage<MaintenanceModel>"
    ValidateRequest="false"
    EnableSessionState="ReadOnly" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Administrativ", Url = "/Maintenance", ToolTipText = "Administrativ"});
       Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
   <% Html.RenderPartial("AntiForgeryToken"); %>

    <div style="margin:0 0 0 -10px; position: relative;" class="container-fluid">
        <nav class="navbar navbar-default" style="" role="navigation">
            <div class="container">
                <a class="navbar-brand" href="#">Maintenance</a>
                <ul class="nav navbar-nav">
                    <li class="active"><a href="/Maintenance">Allgemein</a></li>
                    <li><a href="/MaintenanceImages/Images">Bilder</a></li>
                    <li><a href="/Maintenance/Messages">Nachrichten</a></li>
                    <li><a href="/Maintenance/Tools">Tools</a></li>
                    <li><a href="/Maintenance/CMS">CMS</a></li>
                    <li><a href="/Maintenance/ContentCreatedReport">Cnt-Created</a></li>
                    <li><a href="/Maintenance/Statistics">Stats</a></li>
                </ul>
            </div>
        </nav>
    </div>
        
    <% Html.Message(Model.Message); %>
    
    <div class="row">
        <div class="col-md-6 MaintenanceSection">
            <h4>Fragen</h4>
            <a href="<%= Url.Action("RecalculateAllKnowledgeItems", "Maintenance") %>" data-url="toSecurePost">
                <i class="fa fa-retweet"></i>
                Alle Antwortwahrscheinlichkeiten neu berechnen
            </a><br/>
            <a href="<%= Url.Action("CalcAggregatedValuesQuestions", "Maintenance") %>" data-url="toSecurePost">
                <i class="fa fa-retweet"></i>
                Aggregierte Zahlen aktualisieren
            </a>
        </div>
        
        <div class="col-md-6 MaintenanceSection">
            <h4>Cache</h4>
            <a href="<%= Url.Action("ClearCache", "Maintenance") %>" data-url="toSecurePost">
                <i class="fa fa-retweet"></i>
                Cache leeren
            </a><br/>
        </div>
    </div>

    <div class="row">
        <div class="col-md-6 MaintenanceSection">
            <h4>Themen</h4>
            <a href="<%= Url.Action("UpdateFieldQuestionCountForCategories", "Maintenance") %>" data-url="toSecurePost">
                <i class="fa fa-retweet"></i>
                Feld: AnzahlFragen pro Thema aktualisieren
            </a><br/>
            <a href="<%= Url.Action("CreateAggregationsForAll", "Maintenance") %>" data-url="toSecurePost">
                <i class="fa fa-retweet"></i>
                Aggregat-Relationen akualisieren / anlegen
            </a>
        </div>
        <div class="col-md-6 MaintenanceSection">
            <h4>Suche</h4>
            Alle für Suche neu indizieren: <br/>
            <a href="<%= Url.Action("ReIndexAllQuestions", "Maintenance") %>" data-url="toSecurePost">
                <i class="fa fa-retweet"></i> Fragen 
            </a> /
            <a href="<%= Url.Action("ReIndexAllCategories", "Maintenance") %>" data-url="toSecurePost">
                <i class="fa fa-retweet"></i> Themen
            </a> /
            <a href="<%= Url.Action("ReIndexAllUsers", "Maintenance") %>" data-url="toSecurePost">
                <i class="fa fa-retweet"></i> Nutzer
            </a>
        </div>
    </div>

    <div class="row">
        <div class="col-md-6 MaintenanceSection">
            <h4>Nutzer</h4>
            <a href="<%= Url.Action("UpdateUserReputationAndRankings", "Maintenance") %>" data-url="toSecurePost">
                <i class="fa fa-retweet"></i>
                Rankings und Reputation + Aggregates
            </a><br />
            <a href="<%= Url.Action("UpdateUserWishCount", "Maintenance") %>" data-url="toSecurePost">
                <i class="fa fa-retweet"></i>
                Aggregates
            </a>
        </div>
        <div class="col-md-6 MaintenanceSection">
            <h4>Sonstige</h4>
            <a href="<%= Url.Action("CheckForDuplicateInteractionNumbers", "Maintenance") %>" data-url="toSecurePost" style="">
                <i class="fa fa-retweet"></i> Auf Antworten mit selber Guid und InteractionNr checken
            </a><br />
            <a href="<%= Url.Action("MigrateDefaultTemplates", "Maintenance") %>" data-url="toSecurePost" style="display: none;">
                <i class="fa fa-retweet"></i> Themen ohne topicMarkdown mit Templates migrieren
            </a><br />
            <a href="<%= Url.Action("MigrateDescriptionToTemplates", "Maintenance") %>" data-url="toSecurePost" style="display: none;">
                <i class="fa fa-retweet"></i> Themenbeschreibung migrieren
            </a>
        </div>
    </div>
    <br/><br/>
</asp:Content>