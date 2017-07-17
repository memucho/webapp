<%@ Page Title="memucho: Schneller lernen, länger wissen" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
	Inherits="ViewPage<BaseModel>"%>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <meta name="description" content="">
    <link href="/Views/Welcome/Drive.css" rel="stylesheet" />
</asp:Content>


<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="title">
        <h1>Anfahrt memucho</h1>
    </div>

    <div id="address">               
        <div>Erkelenzdamm 59 Aufgang 3a</div>
        <div>10999 Berlin</div>
        <div id="description">
            <p>Sie finden memucho im 2. Hinterhof links gegenüber den Fahrrädern im Portal 3A im 4. OG</p>
        </div>
    </div>

    <div id="approachSketch">
        <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2429.026168991007!2d13.4107269157264!3d52.49676587980987!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x47a84fcd1092c5b9%3A0xd784ecc27397c2b6!2sErkelenzdamm+59%2C+10999+Berlin!5e0!3m2!1sde!2sde!4v1499695893799" width="600" height="450" frameborder="0" style="border:0" allowfullscreen></iframe>">
    </div>

    <div id="drivingDirection">    
        <p>Um Ihren Anfahrtsweg zu planen klicken sie bitte <a href="https://maps.google.com/maps?ll=52.496126,13.411542&z=15&t=m&hl=de-DE&gl=DE&mapclient=embed&daddr=Erkelenzdamm%2059%2010999%20Berlin@52.4967659,13.4129156" target="blank"><b>hier</b></a></p>
    </div>
</asp:Content>