<%@ Page Title="memucho: Schneller lernen, länger wissen" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master"
    Inherits="ViewPage<WelcomeModel>" %>

<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <link rel="canonical" href="<%= Settings.CanonicalHost %>">
    <meta name="description" content="memucho hilft dir beim Lernen. Du sparst Zeit, bist effizienter und es macht mehr Spaß! Entdecke neues Wissen oder füge dein eigenes hinzu.">
</asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    
    <link href="/Views/Welcome/Welcome.css" rel="stylesheet" />

    <script type="text/javascript">

        /* Source http://www.html-advisor.com/javascript/hide-email-with-javascript-jquery/ */
        $(function () {
            var spt = $('span.mailme');
            var at = / at /;
            var dot = / dot /g;
            var addr = $(spt).text().replace(at, "@").replace(dot, ".");
            $(spt).after('<a href="mailto:' + addr + '" title="Send an email">' + addr + '</a>').hover(function () { window.status = "Send a letter!"; }, function () { window.status = ""; });
            $(spt).remove();
        });
    </script>

    <script>
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(function () { drawKnowledgeChart("chartWishKnowledge") });

        function drawKnowledgeChart(chartElementId) {
            if ($("#" + chartElementId).length === 0) {
                return;
            }

            var data = google.visualization.arrayToDataTable([
                ['Wissenslevel', 'link', 'Anteil in %'],
                ['Sicheres Wissen', '/Fragen/Wunschwissen/?filter=solid', <%= Model.KnowledgeSummary.Solid %>],
                ['Solltest du festigen', '/Fragen/Wunschwissen/?filter=consolidate', <%= Model.KnowledgeSummary.NeedsConsolidation %>],
                ['Solltest du lernen', '/Fragen/Wunschwissen/?filter=learn', <%= Model.KnowledgeSummary.NeedsLearning %>],
                ['Noch nicht gelernt', '/Fragen/Wunschwissen/?filter=notLearned', <%= Model.KnowledgeSummary.NotLearned %>],
            ]);

            var options = {
                pieHole: 0.6,
                tooltip: { isHtml: true },
                legend: { position: 'labeled' },
                pieSliceText: 'none',
                chartArea: { 'width': '100%', height: '100%', top: 10 },
                slices: {
                    0: { color: '#afd534' },
                    1: { color: '#fdd648' },
                    2: { color: 'lightsalmon' },
                    3: { color: 'silver' }
                },
                pieStartAngle: 0
            };

            var view = new google.visualization.DataView(data);
            view.setColumns([0, 2]);

            var chart = new google.visualization.PieChart(document.getElementById(chartElementId));
            chart.draw(view, options);

            google.visualization.events.addListener(chart, 'select', selectHandler);

            function selectHandler(e) {
                var urlPart = data.getValue(chart.getSelection()[0].row, 1);
                location.href = urlPart;
            }
        }


    </script>

    <%= Scripts.Render("~/bundles/guidedTourScript") %>
    <%= Styles.Render("~/bundles/guidedTourStyle") %>
    <%= Scripts.Render("~/bundles/Welcome") %>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">

    <div id="welcomeContainer">

        <div id="findYourContent">
            <h1 id="titleFindYourContent">Finde deine Lerninhalte</h1>
            <h3 id="titleFindYourContentSub">und lerne interaktiv & personalisiert</h3>

            <div class="EduCategoryRow row">
                <div class="xxs-stack col-xs-6 col-sm-3">
                    <a href="/Kategorien/Schule/682" class="EduCategory">
                        <span class="EduCategoryIcon">
                            <span class="fa-stack fa-lg">
                                <i class="fa fa-circle fa-stack-2x"></i>
                                <i class="fa fa-child fa-stack-1x fa-inverse IconForeground"></i>
                            </span>
                        </span>
                        <span class="EduCategoryText">Schule
                        </span>
                    </a>
                </div>

                <div class="xxs-stack col-xs-6 col-sm-3">
                    <a href="/Kategorien/Studium/687" class="EduCategory">
                        <span class="EduCategoryIcon">
                            <span class="fa-stack fa-lg">
                                <i class="fa fa-circle fa-stack-2x"></i>
                                <i class="fa fa-graduation-cap fa-stack-1x fa-inverse IconForeground"></i>
                            </span>
                        </span>
                        <span class="EduCategoryText">Studium
                        </span>
                    </a>
                </div>
                <div class="xxs-stack col-xs-6 col-sm-3">
                    <a href="/Kategorien/Zertifikate/689" class="EduCategory show-tooltip" title="Sportbootführerscheine, Einbürgerungstest, Pferdebasispass etc.">
                        <span class="EduCategoryIcon">
                            <span class="fa-stack fa-lg">
                                <i class="fa fa-circle fa-stack-2x"></i>
                                <i class="fa fa-file-text-o fa-stack-1x fa-inverse IconForeground"></i>
                            </span>
                        </span>
                        <span class="EduCategoryText">Zertifikate
                        </span>
                    </a>
                </div>
                <div class="xxs-stack col-xs-6 col-sm-3">
                    <a href="/Kategorien/Allgemeinwissen/709" class="EduCategory">
                        <span class="EduCategoryIcon">
                            <span class="fa-stack fa-lg">
                                <i class="fa fa-circle fa-stack-2x"></i>
                                <i class="fa fa-lightbulb-o fa-stack-1x fa-inverse IconForeground"></i>
                            </span>
                        </span>
                        <span class="EduCategoryText">Allgemeinwissen
                        </span>
                    </a>
                </div>
            </div>
        </div>


        <% if (Model.IsLoggedIn)
            { %>
        <div id="WelcomeDashboard">
            <div class="row">
                <div class="col-sm-6" id="dashboardPoints">
                    <h2>Deine Lernpunkte</h2>
                    <div style="text-align: center; margin-bottom: 25px; margin-top: 15px;">
                        <span class="level-display">
                            <span style="display: inline-block; white-space: nowrap;">
                                <svg class="large">
                                <circle cx="50%" cy="50%" r="50%" />
                                <text class="level-count" x="50%" y="50%" dy = ".34em" ><%= Model.ActivityLevel %></text>
                            </svg>
                            </span>
                        </span>
                        <p style="margin-top: 10px;">
                            Mit <b><%= Model.ActivityPoints.ToString("N0") %> Lernpunkten</b> bist du <span style="white-space: nowrap"><b>Level <%= Model.ActivityLevel %></b>.</span>
                        </p>
                    </div>

                    <div class="NextLevelContainer">
                        <div class="ProgressBarContainer">
                        <div id="NextLevelProgressPercentageDone" class="ProgressBarSegment ProgressBarDone CountUpProgress" data-number="<%= Model.ActivityPointsPercentageOfNextLevel %>" data-character="%" style="width: 95px;">
                                <div class="ProgressBarSegment ProgressBarLegend">
                                <span id="NextLevelProgressSpanPercentageDone" class="CountUp" data-number="<%= Model.ActivityPointsPercentageOfNextLevel %>" data-character ="%">0</span>
                                </div>
                            </div>
                            <div class="ProgressBarSegment ProgressBarLeft" style="width: 100%;"></div>

                        </div>
                    </div>
                    <div class="greyed" style="text-align: center; margin-bottom: 15px;">Noch <%= Model.ActivityPointsTillNextLevel.ToString("N0") %> Punkte bis Level <%= Model.ActivityLevel + 1 %></div>
                </div>

                <div class="col-sm-6" id="dashboardKnowledgeWheel">
                    <h2>Dein Wissensstand</h2>
                    <% if (Model.KnowledgeSummary.Total == 0)
                        { %>
                    <div class="alert alert-info" style="min-height: 180px; margin-bottom: 54px;">
                        <p>
                            memucho kann deinen Wissensstand nicht zeigen, da du noch kein Wunschwissen hast.
                        </p>
                        <p>
                            Um dein Wunschwissen zu erweitern, suche dir interessante Lerninhalte aus und klicke auf den Hinzufügen-Button oder auf das Herzsymbol:
                            <ul style="list-style-type: none">
                                <li>
                                    <i class="fa fa-heart" style="color: #b13a48;"></i>
                                    In deinem Wunschwissen
                                </li>
                                <li>
                                    <i class="fa fa-heart-o" style="color: #b13a48;"></i>
                                    <i>Nicht</i> in deinem Wunschwissen.
                                </li>
                            </ul>

                        </p>
                    </div>
                    <% }
                        else
                        { %>
                    <div id="chartWishKnowledge" <%= !Model.IsLoggedIn ? "style='pointer-events:none;'" : "" %>></div>
                    <% } %>
                </div>
            </div>

            <div class="separator">
            </div>

            <div id="dashboardFooter">
                <% if (Model.KnowledgeSummary.Total > 0)
                    { %>
                <a href="<%= Links.StartWishLearningSession() %>" data-type="learn-wishknowledge" class="btn btn-lg btn-primary show-tooltip" title="Startet eine persönliche Lernsitzung. Du wiederholst die Fragen aus deinem Wunschwissen, die am dringendsten zu lernen sind.">
                    <i class="fa fa-line-chart">&nbsp;</i>Jetzt Wunschwissen lernen
                </a>
                <% } %>
                <span class="float-right-sm-up"><a class="btn btn-lg btn-link" href="<%= Links.Knowledge() %>">Mehr auf deiner<span style="text-decoration: none;">&nbsp;&nbsp;</span><i class="fa fa-heart" style="color: #b13a48;">&nbsp;</i>Wissenszentrale</a></span>
            </div>
        </div>
        <% }
            else
            { %>
        <div id="WelcomeDashboard">
            <img src="/Images/Illustrations/PreviewFullResponsiveScreens.jpg" />
            <div class="separator">
            </div>
            <div class="row" style="text-align: center;">
                <div class="col-sm-12 col-md-7 align-left-md-up">
                    <p>
                        <b>Registriere dich jetzt</b>, um personalisiert und interaktiv zu lernen.
                        <br />
                        Deinen Wissensstand hast du immer im Blick und mit deinen Lernpunkten erreichst du immer neue Level.
                    </p>
                </div>
                <div class="col-sm-12 col-md-5 align-right-md-up">
                    <div class="" style="text-align: center; display: inline-block;">
                        <a href="<%= Url.Action(Links.RegisterAction, Links.RegisterController) %>" class="btn btn-lg btn-primary" role="button"><i class="fa fa-chevron-circle-right">&nbsp;</i> Jetzt kostenlos registrieren</a>
                    </div>

                </div>
            </div>
        </div>

        <% } %>

        <div id="memuchoInfo">
            <h2>memucho ist dein Lernassistent
            </h2>
            <div class="row infoItemRow">
                <div class="col-sm-4 infoItemColumn">
                    <div class="infoIcon">
                        <i class="fa fa-heart"></i>
                    </div>
                    <div class="infoCatchWord">
                        Sammeln
                    </div>
                    <div class="infoExplanationSnippet">
                        Entscheide, was du wissen möchtest.
                    </div>
                </div>

                <div class="col-sm-4 infoItemColumn">
                    <div class="infoIcon">
                        <i class="fa fa-line-chart"></i>
                    </div>
                    <div class="infoCatchWord">
                        Lernen
                    </div>
                    <div class="infoExplanationSnippet">
                        Algorithmen helfen dir, zum idealen Zeitpunkt zu lernen.
                    </div>
                </div>

                <div class="col-sm-4 infoItemColumn">
                    <div class="infoIcon">
                        <i class="fa fa-lightbulb-o"></i>
                    </div>
                    <div class="infoCatchWord">
                        Nicht vergessen
                    </div>
                    <div class="infoExplanationSnippet">
                        Wir erinnern dich, bevor du vergisst.
                    </div>
                </div>
            </div>

            <div class="separator"></div>
            <div id="memuchoInfoFooter">
                <%--                <% if (!Model.IsLoggedIn) { %>
                    <div style="text-align: center; display: inline-block;">
                        <a href="<%= Url.Action(Links.RegisterAction, Links.RegisterController) %>" class="btn btn-lg btn-primary" role="button"><i class="fa fa-chevron-circle-right">&nbsp;</i> Jetzt kostenlos registrieren</a>
                    </div>
                <% } %>--%>
                <a href="<%= Links.AboutMemucho() %>" class="btn btn-lg btn-link">Erfahre mehr...</a>
            </div>
        </div>

        <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>

        <div class="infoBox container-fluid">
            <div class="row">
                <div class="col-md-3 vertical-center">
                    <img alt="Hier sollte der Oer Award sein" style="width: 300px" src="/Images/LogosPartners/badge_nominiert_2017_250real.png"/>
                </div>
                <div class="col-md-9">
                <h3>memucho 2x nominiert für OER-Award 2017</h3>
                <p>
                    Bei den OER-Awards 2017 wurde memucho gleich zweimal nominiert:
                    <ul class="ul">
                        <li>In der Kategorie OER-Infrastruktur und</li>
                        <li>speziell für unsere freien Lerninhalte für den <a href="<%= Links.CategoryDetail("Basispass Pferdekunde",343) %>">Basispass Pferdekunde</a>.</li>
                    </ul>
                </p>
                <p>Darüber freuen wir uns sehr! Die <a href="https://open-educational-resources.de/veranstaltungen/17/award/" target="_blank">OER-Awards <i class="fa fa-external-link" style="font-size: smaller;"></i></a>
                    zeichnen jährlich die besten Angebote im Bereich freie Bildungsinhalte (OER) aus.
                    Die Preis-Verleihung fand am 27. November 2017 in Berlin im Rahmen des 
                    <a href="https://open-educational-resources.de/veranstaltungen/17/" target="_blank">OER-Festivals <i class="fa fa-external-link" style="font-size: smaller;"></i>
                    </a>statt.
                </p>
                </div>
            </div>
        </div>

        <%-- <% Html.RenderPartial("Partials/TopicOfWeek", new TopicOfWeekModel(DateTime.Now)); %>--%>

        <div id="ContentAvailable">
            <h2>Interaktive Lerninhalte zu <%= Model.TotalCategoriesCountRound10 %>+ Themen</h2>
            <p class="ShortParagraph">
                Bei memucho findest du interaktive Lerninhalte zu vielen Themen und kannst sie personalisiert lernen.
            Dein Thema ist nicht dabei? Kein Problem! Du kannst Inhalte leicht übernehmen, ergänzen oder ganz neu erstellen.
            </p>

            <!-- School Content -->
            <div class="row CardsMiniPortrait" style="padding-top: 0;">
                <div class="CardMiniColumn col-xs-4 col-sm-3 col-lg-2">
                    <div class="Card SingleItem Category EduCategoryLinkCard">
                        <div class="ContentContainer">
                            <div class="CardContent">
                                <a href="/Kategorien/Schule/682" class="EduCategory" style="">
                                    <span class="EduCategoryIcon">
                                        <span class="fa-stack fa-lg">
                                            <i class="fa fa-circle fa-stack-2x"></i>
                                            <i class="fa fa-child fa-stack-1x fa-inverse IconForeground"></i>
                                        </span>
                                    </span>
                                    <span class="EduCategoryText">Schule
                                    </span>
                                    <span class="EduCategoryTextSub">Alle Schulfächer anzeigen
                                    </span>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
                <% foreach (var categoryId in Model.CategoriesSchool)
                    { %>
                <div class="CardMiniColumn col-xs-4 col-sm-3 col-lg-2">
                    <% Html.RenderPartial("WelcomeCardMiniCategory", new WelcomeCardMiniCategoryModel(categoryId)); %>
                </div>
                <% } %>
            </div>

            <!-- University Content -->
            <div class="row CardsMiniPortrait" style="padding-top: 0;">
                <div class="CardMiniColumn col-xs-4 col-sm-3 col-lg-2">
                    <div class="Card SingleItem Category EduCategoryLinkCard">
                        <div class="ContentContainer">
                            <div class="CardContent">
                                <a href="/Kategorien/Studium/687" class="EduCategory">
                                    <span class="EduCategoryIcon">
                                        <span class="fa-stack fa-lg">
                                            <i class="fa fa-circle fa-stack-2x"></i>
                                            <i class="fa fa-graduation-cap fa-stack-1x fa-inverse IconForeground"></i>
                                        </span>
                                    </span>
                                    <span class="EduCategoryText">Studium
                                    </span>
                                    <span class="EduCategoryTextSub">Alle Studienfächer anzeigen
                                    </span>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
                <% foreach (var categoryId in Model.CategoriesUniversity)
                    { %>
                <div class="CardMiniColumn col-xs-4 col-sm-3 col-lg-2">
                    <% Html.RenderPartial("WelcomeCardMiniCategory", new WelcomeCardMiniCategoryModel(categoryId)); %>
                </div>
                <% } %>
            </div>

            <!-- Certificate Content -->
            <div class="row CardsMiniPortrait" style="padding-top: 0;">
                <div class="CardMiniColumn col-xs-4 col-sm-3 col-lg-2">
                    <div class="Card SingleItem Category EduCategoryLinkCard">
                        <div class="ContentContainer">
                            <div class="CardContent">
                                <a href="/Kategorien/Zertifikate/689" class="EduCategory">
                                    <span class="EduCategoryIcon">
                                        <span class="fa-stack fa-lg">
                                            <i class="fa fa-circle fa-stack-2x"></i>
                                            <i class="fa fa-file-text fa-stack-1x fa-inverse IconForeground"></i>
                                        </span>
                                    </span>
                                    <span class="EduCategoryText">Zertifikate
                                    </span>
                                    <span class="EduCategoryTextSub">Alle Zertifikate & Spezialwissen anzeigen
                                    </span>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
                <% foreach (var categoryId in Model.CategoriesCertificate)
                    { %>
                <div class="CardMiniColumn col-xs-4 col-sm-3 col-lg-2">
                    <% Html.RenderPartial("WelcomeCardMiniCategory", new WelcomeCardMiniCategoryModel(categoryId)); %>
                </div>
                <% } %>
            </div>

            <!-- General Knowledge Content -->
            <div class="row CardsMiniPortrait" style="padding-top: 0;">
                <div class="CardMiniColumn col-xs-4 col-sm-3 col-lg-2">
                    <div class="Card SingleItem Category EduCategoryLinkCard">
                        <div class="ContentContainer">
                            <div class="CardContent">
                                <a href="/Kategorien/Allgemeinwissen/709" class="EduCategory">
                                    <span class="EduCategoryIcon">
                                        <span class="fa-stack fa-lg">
                                            <i class="fa fa-circle fa-stack-2x"></i>
                                            <i class="fa fa-lightbulb-o fa-stack-1x fa-inverse IconForeground"></i>
                                        </span>
                                    </span>
                                    <span class="EduCategoryText">Allgemeinwissen
                                    </span>
                                    <span class="EduCategoryTextSub">Alle Allgemeinwissen-Themen anzeigen
                                    </span>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
                <% foreach (var categoryId in Model.CategoriesGeneralKnowledge)
                    { %>
                <div class="CardMiniColumn col-xs-4 col-sm-3 col-lg-2">
                    <% Html.RenderPartial("WelcomeCardMiniCategory", new WelcomeCardMiniCategoryModel(categoryId)); %>
                </div>
                <% } %>
            </div>

        </div>


        <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>


        <div id="awards">
            <h2>Auszeichnungen</h2>
            <div class="row">
                <div class="col-sm-4">
                    <div class="logo-box">
                        <div class="img-logo">
                            <a href="https://www.land-der-ideen.de/ausgezeichnete-orte/preistraeger/memucho-online-plattform-zum-faktenlernen" target="_blank">
                                <img src="/Images/LogosPartners/landderideen_ausgezeichnet-2017_w190c.jpg" alt="memucho ist ein ausgezeichneter Ort im Land der Ideen 2017" />
                            </a>
                        </div>
                        <%--                        <p>
                        memucho ist ein Ausgezeichneter Ort im Land der Ideen 2017.
                    </p>
                    <p class="logo-box-link">
                        <a href="https://www.land-der-ideen.de/ausgezeichnete-orte/preistraeger/memucho-online-plattform-zum-faktenlernen" target="_blank">
                            <span style="white-space: nowrap">Zum Wettbewerb <i class="fa fa-external-link"></i></span>
                        </a>
                    </p>--%>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="logo-box">
                        <div class="img-logo">
                            <a href="http://www.innovationspreis.de/news/aktuelles/zehn-nominierungen-f%C3%BCr-den-innovationspreis-berlin-brandenburg-2016.html" target="_blank">
                                <img src="/Images/LogosPartners/innovationspreis-nominiertButton2016.png" alt="Nominiert 2016 für den Innovationspreis Berlin Brandenburg" width="170" height="110" />
                            </a>
                        </div>
                        <%--                        <p>
                        memucho wurde für den Innovationspreis Berlin Brandenburg nominiert.
                    </p>
                    <p class="logo-box-link">
                        <a href="http://www.innovationspreis.de/news/aktuelles/zehn-nominierungen-f%C3%BCr-den-innovationspreis-berlin-brandenburg-2016.html" target="_blank">
                            <span style="white-space: nowrap">Zur Jury-Entscheidung <i class="fa fa-external-link"></i></span>
                        </a>
                    </p>--%>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="logo-box">
                        <div class="img-logo">
                            <a href="https://www.netzsieger.de/p/memucho" target="_blank">
                                <img src="/Images/LogosPartners/Logo_netzsieger_170905-memucho-small.png" alt="" width="165" height="126" />
                            </a>
                        </div>
                        <%--                        <p>
                        memucho erhält beim Einzeltest 4,7/5 Punkten.
                    </p>
                    <p class="logo-box-link">
                        <a href="https://www.netzsieger.de/p/memucho" target="_blank">
                            <span style="white-space: nowrap">Zum Testbericht <i class="fa fa-external-link"></i></span>
                        </a>
                    </p>--%>
                    </div>
                </div>
            </div>
        </div>

        <div id="partner">
            <h2>Partner</h2>
            <div class="row">
                <div class="col-sm-4">
                    <div class="logo-box">
                        <div class="img-logo">
                            <a href="http://lernox.de/" target="_blank">
                                <img style="margin-top: -35px;" src="/Images/LogosPartners/Logo_lernox.png" alt="Logo lernox.de" />
                            </a>
                        </div>
                        <p>
                            Ankommen durch Sprache. DaF-/DaZ-Material finden und sammeln.
                        </p>
                        <p class="logo-box-link">
                            <a href="http://lernox.de/" target="_blank">
                                <span style="white-space: nowrap">lernox.de <i class="fa fa-external-link"></i></span>
                            </a>
                        </p>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="logo-box">
                        <div class="img-logo">
                            <a href="/Kategorien/Learning-Level-Up/722">
                                <img style="margin-top: 24px;" src="/Images/LogosPartners/Logo_LearningLevelUp.png" alt="Learning Level Up und memucho kooperieren!" />
                            </a>
                        </div>
                        <p>
                            Learning Level Up bietet Animationen, Grafiken und Videos zum Lernen. Wir freuen uns über die Kooperation!
                        </p>
                        <p class="logo-box-link">
                            <a href="/Kategorien/Learning-Level-Up/722">Zur Themenseite</a>
                        </p>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="logo-box">
                        <div class="img-logo">
                            <a href="https://www.tutory.de/" target="_blank">
                                <img src="/Images/LogosPartners/Logo_tutory_250px.png" alt="tutory.de" />
                            </a>
                        </div>
                        <p>
                            Mit tutory.de lassen sich im Handumdrehen tolle Arbeitsblätter direkt online erstellen.
                        </p>
                        <p class="logo-box-link">
                            <a href="https://www.tutory.de/" target="_blank">
                                <span style="white-space: nowrap">tutory.de <i class="fa fa-external-link"></i></span>
                            </a>
                        </p>
                    </div>
                </div>
                <%--<div class="col-sm-4">
                <div class="logo-box">
                    <div class="img-logo">
                        <a href="http://www.unesco.org/new/en/communication-and-information/access-to-knowledge/open-educational-resources/" target="_blank">
                            <img style="margin-top: -35px;" src="/Images/LogosPartners/oer_logo_EN_2_w400.png" alt="Logo Open Educational Resources"/>
                        </a>
                    </div>
                    <p>
                        Freie Bildungsmaterialien demokratisieren Bildung! Wir machen mit.
                    </p>
                    <p class="logo-box-link">
                        <a href="http://www.unesco.org/new/en/communication-and-information/access-to-knowledge/open-educational-resources/" target="_blank">
                            <span style="white-space: nowrap">Zur UNESCO-Seite <i class="fa fa-external-link"></i></span>
                        </a>
                    </p>
                </div>        
                </div>--%>
            </div>
        </div>

        <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>

        <% if (Model.MemuchoBlogPosts.Any())
            { %>
        <div id="blogposts">
            <h3>Neues von unserem Blog</h3>
            <% Html.RenderPartial("Partials/BlogPostCarousel", Model.MemuchoBlogPosts); %>
        </div>
        <% } %>

        <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>

        <div id="memuchoInfoExtended">
            <h2>Was ist memucho?</h2>
            <h3>Wir helfen dir, Interessantes zu lernen, nie wieder zu vergessen
                <br class="visible-lg" />
                und dein Wissen zu organisieren.
            </h3>

            <div class="row infoItemRow">
                <div class="col-sm-6 col-xs-12 infoItemColumn">
                    <div class="infoIcon">
                        <i class="fa fa-heart"></i>
                    </div>
                    <div class="infoCatchWord">
                        Wunschwissen sammeln
                    </div>
                    <div class="infoExplanationSnippet">
                        Stelle dir dein  Wunschwissen zusammen und entscheide, was du dir merken möchtest. 
                    Bei memucho findest du interaktive Fragen zu vielen Themen.
                    </div>
                </div>

                <div class="col-sm-6 col-xs-12 infoItemColumn">
                    <div class="infoIcon">
                        <i class="fa fa-line-chart"></i>
                    </div>
                    <div class="infoCatchWord">
                        Interaktiv & optimiert lernen
                    </div>
                    <div class="infoExplanationSnippet">
                        Unsere Algorithmen sagen dir immer, was du am dringendsten lernen musst. 
                    So sparst du Zeit und gewinnst an Sicherheit.
                    </div>
                </div>

                <div class="Clearfix"></div>

                <div class="col-sm-6 col-xs-12 infoItemColumn">
                    <div class="infoIcon">
                        <i class="fa fa-bar-chart"></i>
                    </div>
                    <div class="infoCatchWord">
                        Wissensstand im Blick
                    </div>
                    <div class="infoExplanationSnippet">
                        Du möchtest dir gern 50, 500, 5.000 oder mehr Fragen merken? 
                    Behalte den Überblick und habe deinen Wissensstand immer im Blick.
                    </div>
                </div>

                <div class="col-sm-6 col-xs-12 infoItemColumn">
                    <div class="infoIcon">
                        <i class="fa fa-share-alt"></i>
                    </div>
                    <div class="infoCatchWord">
                        Wissen teilen und gemeinsam lernen
                    </div>
                    <div class="infoExplanationSnippet">
                        Nutze die vorhandenen Lerninhalte und teile dein eigenes Wissen. Du kannst selbst eigene Fragen und Lernsets erstellen.
                    </div>
                </div>


            </div>
            <div class="separator"></div>
            <div id="memuchoInfoExtendedFooter">
                <%--                <% if (!Model.IsLoggedIn) { %>
                <div style="text-align: center; display: inline-block;">
                    <a href="<%= Url.Action(Links.RegisterAction, Links.RegisterController) %>" class="btn btn-lg btn-primary" role="button"><i class="fa fa-chevron-circle-right">&nbsp;</i> Jetzt kostenlos registrieren</a>
                </div>
            <% } %>--%>
                <a href="<%= Links.AboutMemucho() %>" class="btn btn-lg btn-link">Erfahre mehr...</a>
            </div>
        </div>



        <div id="principles">
            <h2>Unsere Prinzipien
            </h2>
            <div class="row infoItemRow">
                <div class="col-sm-6 col-xs-12 infoItemColumn">
                    <div class="infoIcon">
                        <i class="fa fa-bullhorn"></i>
                    </div>
                    <div class="infoCatchWord">
                        Freie Bildungsinhalte
                    </div>
                    <div class="infoExplanationSnippet">
                        Alle Lerninhalte bei uns sind frei und rechtssicher lizenziert. 
                    Du kannst sie nutzen, weiterverwenden und ergänzen, wie es für dich am besten passst.
                    </div>
                </div>

                <div class="col-sm-6 col-xs-12 infoItemColumn">
                    <div class="infoIcon">
                        <i class="fa fa-leaf"></i>
                    </div>
                    <div class="infoCatchWord">
                        Gemeinwohlorientierung
                    </div>
                    <div class="infoExplanationSnippet">
                        Wir möchten unser Unternehmen auf <a href="http://www.gemeinwohl-oekonomie.org/de" target="_blank;">gemeinwohlfördernden Werten <i class="fa fa-external-link"></i></a>aufbauen.
                    Wir sind überzeugt, dass Unternehmen eine ethische, soziale und ökologische Verantwortung haben.
                    </div>
                </div>

                <div class="Clearfix"></div>

                <div class="col-sm-6 col-xs-12 infoItemColumn">
                    <div class="infoIcon">
                        <i class="fa fa-lock"></i>
                    </div>
                    <div class="infoCatchWord">
                        Datenschutz
                    </div>
                    <div class="infoExplanationSnippet">
                        Wir nutzen deine Daten, damit du besser lernen kannst und um memucho besser zu machen. 
                    Aber wir werden deine Daten niemals verkaufen. (<a class="helpLink" href="<%= Links.FAQItem("DataPrivacy") %>">Mehr zum Datenschutz</a>)
                    </div>
                </div>

                <div class="col-sm-6 col-xs-12 infoItemColumn">
                    <div class="infoIcon">
                        <i class="fa fa-search-plus"></i>
                    </div>
                    <div class="infoCatchWord">
                        Open-Source und Transparenz
                    </div>
                    <div class="infoExplanationSnippet">
                        Wir entwickeln memucho als Open Source, die Quelltexte sind frei verfügbar. Du findest sie 
                    auf <a href="https://github.com/TrueOrFalse/TrueOrFalse" target="_blank"><i class="fa fa-github">&nbsp;</i>Github <i class="fa fa-external-link"></i></a>. 
                    In Zukunft möchten wir eine Gemeinwohlbilanz und wichtige Unternehmenszahlen regelmäßig veröffentlichen.
                    </div>
                </div>

            </div>

            <div class="separator"></div>
            <div id="memuchoPrinciplesFooter">
                <% if (!Model.IsLoggedIn)
                    { %>
                <div style="text-align: center; display: inline-block;">
                    <a href="<%= Url.Action(Links.RegisterAction, Links.RegisterController) %>" class="btn btn-lg btn-primary" role="button"><i class="fa fa-chevron-circle-right">&nbsp;</i> Jetzt kostenlos registrieren</a>
                </div>
                <% } %>
                <span class="float-right-sm-up"><a href="<%= Links.AboutMemucho() %>#principles" class="btn btn-lg btn-link">Erfahre mehr...</a></span>
            </div>
        </div>


        <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>


        <div id="team">
            <h2>Team
            </h2>
            <div class="row infoItemRow">
                <div class="col-xs-4 infoItemColumn">
                    <div class="TeamPic">
                        <img src="/Images/Team/team_robert201509_155.jpg" />
                    </div>
                    <div class="infoCatchWord">
                        Robert
                    </div>
                    <div class="infoExplanationSnippet">
                        (Gründer)
                    </div>
                </div>

                <div class="col-xs-4 infoItemColumn">
                    <div class="TeamPic">
                        <img src="/Images/Team/team_jule201509-2_155.jpg" />
                    </div>
                    <div class="infoCatchWord">
                        Jule
                    </div>
                    <div class="infoExplanationSnippet">
                        (Gründer)
                    </div>
                </div>

                <div class="col-xs-4 infoItemColumn">
                    <div class="TeamPic">
                        <img src="/Images/Team/team_christof_20170404_P3312344_155.jpg" />
                    </div>
                    <div class="infoCatchWord">
                        Christof
                    </div>
                    <div class="infoExplanationSnippet">
                        (Gründer)
                    </div>
                </div>

                <div class="col-xs-4 infoItemColumn">
                    <div class="TeamPic">
                        <img src="/Images/Team/team_lisa_sq_155.jpg" />
                    </div>
                    <div class="infoCatchWord">
                        Lisa
                    </div>
                    <div class="infoExplanationSnippet">
                        (Kommunikation)
                    </div>
                </div>

                <div class="col-xs-4 infoItemColumn">
                    <div class="TeamPic">
                        <img src="/Images/Team/team_julian20170404_P3312327_155.jpg" />
                    </div>
                    <div class="infoCatchWord">
                        Julian
                    </div>
                    <div class="infoExplanationSnippet">
                        (Entwicklung)
                    </div>
                </div>
            </div>

            <div class="TeamText">
                <p class="ShortParagraph">
                    Wir möchten freie Bildungsinhalte fördern und dich beim Lernen unterstützen. 
                Auf dieser Idee werden wir ein stabiles gemeinwohlorientiertes Unternehmen aufbauen. 
                Wir konzipieren, gestalten und programmieren memucho gemeinsam.
                </p>
                <p class="ShortParagraph">
                    Wenn du Fragen oder Anregungen hast, schreibe uns eine E-Mail an <span class="mailme">team at memucho dot de</span> oder rufe uns an: +49 - 30 - 616 566 26.
                </p>
            </div>

        </div>

        <%--<div class="row">
        <div class="col-xs-6 col-md-3">
            <h4>Neueste Lernsets</h4>
            <div class="LabelList">
                <% Html.RenderPartial("WelcomeBoxTopSets", WelcomeBoxTopSetsModel.CreateMostRecent(5)); %>
            </div>
        </div>
        <div class="col-xs-6 col-md-3">
            <h4>Neueste Themen:</h4>
            <div class="LabelList">
                <% Html.RenderPartial("WelcomeBoxTopCategories", WelcomeBoxTopCategoriesModel.CreateMostRecent(5)); %>
            </div>
        </div>
        <div class="col-xs-6 col-md-3">
            <h4>Neueste Fragen</h4>
            <div class="LabelList">
                <% Html.RenderPartial("WelcomeBoxTopQuestions", WelcomeBoxTopQuestionsModel.CreateMostRecent(8)); %>
            </div>
        </div>
        <div class="col-xs-6 col-md-3">
            <h4>Top-Themen nach Anzahl Fragen</h4>
            <div class="LabelList">
                <% Html.RenderPartial("WelcomeBoxTopCategories", WelcomeBoxTopCategoriesModel.CreateTopCategories(5)); %>
            </div>
        </div>
    </div>--%>

        <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>

        <%--    <div class="col-md-12">
        <% if (!Model.IsLoggedIn)
           { %>
            <div class="well" id="newsletterSignUp" style="padding: 20px; ">
                <h4>Newsletter</h4>
                <p>Du möchtest bei wichtigen Neuigkeiten benachrichtigt werden? Melde dich hier an:</p>
                <div class="alert alert-danger" role="alert" id="msgInvalidEmail" style="display:none">
                    Keine gültige E-Mail-Adresse.
                </div>
            
                <div class="alert alert-success" role="alert" id="msgEmailSend" style="display:none">
                    Deine Adresse wurde für den Newsletter angemeldet.
                </div>

                <form class="form-inline" style="color: white;">
                    <div class="">
                        <input type="email" class="form-control" id="txtNewsletterRequesterEmail" placeholder="deine@email.de" style="width: 100%;">
                    </div>
                    <div class="" style="text-align: center; margin-top: 10px;">
                        <a class="btn btn-primary" href="#" id="btnNewsletterRequest" style="">
                            <i class="fa fa-envelope-o">&nbsp;</i>Anmelden
                        </a>
                    </div>
                </form> 
            </div>
        <% } %>        
    </div>--%>
    </div>

</asp:Content>
