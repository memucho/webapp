<%@ Page Title="Förderer" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master"
    Inherits="ViewPage<BaseModel>" %>



<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <meta name="description" content="Förderer für Lerntool und Wissensassistenten memucho">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Welcome/Promoter.css" rel="stylesheet" />

</asp:Content>


<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-xs-12">
            <h1 class="PageHeader" style="margin-bottom: 25px; margin-top: 0px;">Förderer</h1>
        </div>
    </div>

    <div class="promoter-overview">
        <div class="row">
            <div class="col-xs-12 imgLogoDiv">
                <img class="partnerImage" style="padding-left: 40px;" src="/Images/LogosPartners/efre.png" />
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12">
                <p>
                    memucho wird seit Juni 2018 im Rahmen des Programms „Gründung innovativ“ des Europäischen Fonds für regionale Entwicklung (EFRE) von der Investitionsbank
                    des Landes Brandenburg gefördert. Der EFRE wird zur Schaffung nachhaltiger und selbsttragender Wirtschaftsstrukturen genutzt.<br/>
                    <a href="http://www.efre.brandenburg.de" target="_blank">www.efre.brandenburg.de</a>
                </p>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12 imgLogoDiv">
                <img class="partnerImage" src="/Images/LogosPartners/Logo-EXIST-eps.png" />
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <p>
                    memucho wurde von Oktober 2016 bis September 2017 mit einem EXIST-Gründerstipendium gefördert. 
                    EXIST ist ein Förderprogramm des Bundesministeriums für Wirtschaft und Energie (BMWi).<br/>
                    <a href="http://www.exist.de" target="_blank">www.exist.de</a> 
                </p>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 imgLogoDiv">
                <img class="partnerImage" src="/Images/LogosPartners/profund-innovation-logo-t.png" />
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 lastParagraph">
                <p>
                    Profund Innovation ist die Service-Einrichtung für die Förderung von Unternehmensgründungen
                    und Innovationen in der Abteilung Forschung der Freien Universität.<br/>
                    <a href="http://www.fu-berlin.de/sites/profund" target="_blank">www.fu-berlin.de/sites/profund</a>
                </p>
            </div>
        </div>
    </div>
</asp:Content>
