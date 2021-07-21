<%@ Page Title="Kontakt & Anfahrt" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" 
	Inherits="ViewPage<BaseModel>"%>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <meta name="description" content="Kontakt und Anfahrt zum Lerntool und Wissensassistenten memucho">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Welcome/Contact.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/mailto") %>

    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Kontakt", Url = Links.Contact, ToolTipText = "Kontakt"});
       Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="PageHeader">
        <h1><span class="ColoredUnderline GeneralMemucho">Kontakt & Anfahrt</span></h1>
    </div>

    <p class="digitalAddresses">
        E-Mail: <span class="mailme">team at memucho dot de</span> <br />
        Mobil: +49-0178-1866848
    </p>
    <p>
        Unsere Adressen sind: <br/>
    </p>    
    <p class="postalAddress">
        <i class="fa fa-map-marker">&nbsp;</i>&nbsp;memucho <br />
        Am Moorhof <br />
        Nettgendorfer Str. 7<br />  
        14947 Nuthe-Urstromtal<br />
    </p>

    <div id="approachSketch">
        <img src="<%= Links.GoogleMapsPreviewPath("Wildau.png") %>" alt="Wildau Peview" />
    </div>
    <p>
        Deinen Anfahrtsweg kannst du <a href="http://maps.google.com/maps/dir//Nettgendorfer+Str.+7,+14947+Nuthe-Urstromtal" target="blank"><b>hier</b></a> mit GoogleMaps planen.
    </p>
</asp:Content>