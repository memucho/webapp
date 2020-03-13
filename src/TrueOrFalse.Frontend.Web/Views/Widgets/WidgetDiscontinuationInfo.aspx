<%@ Page Title="Spielen" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.Widget.Master" 
    Inherits="ViewPage<WidgetBaseModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
        html {
            height: auto;
        }
        #mainDiv {
            padding: 15px 2px 55px 2px;
            text-align: center;
            color: #203256;
        }
        .titleP {
            margin-bottom: 10px;
            font-weight: bold;
            font-size: 160%;
        }
        .description {
            margin: 10px 5px;
            font-size: 110%;
        }
        .buttonP {
            margin-top: 30px;
        }
    </style>
    
    <% if(Model.IncludeCustomCss){ %>
        <link href="<%= Model.CustomCss %>" rel="stylesheet" />
    <% } %>
    
    <%= Scripts.Render("~/bundles/js/AwesomeIframe") %>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    

    <div id="mainDiv">
        <p class="titleP">
            Der Support für unsere Widgets endet
        </p>
        <p class="description">
            Leider werden wir demnächst unser Widget-Feature abschalten. Dafür konzentrieren wir uns mit voller Kraft auf die Kernfunktionen von memucho als Wissensmanager und Lerntool für offene Bildungsinhalte! <br/>
            <br/>
            <b>Alle Fragen bleiben erhalten</b> und es ist möglich, diese <b>Inhalte zu sichern und zu migrieren</b>. Du hast Fragen? <a href="mailto:christof@memucho.de?Subject=Widgets" target="_top">Schreibe uns</a>, wir helfen Dir gerne.
        </p>

    </div>

</asp:Content>