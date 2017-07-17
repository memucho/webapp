<%@ Page Title="Maintenance: Statistics" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<StatisticsModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(drawChartNewUsers);
        google.setOnLoadCallback(drawChartUsers);
        google.setOnLoadCallback(drawChartQuestionCountStats);
        google.setOnLoadCallback(drawChartNewQuestions);
        google.setOnLoadCallback(drawChartUsageStatsPt1);
        google.setOnLoadCallback(drawChartUsageStatsPt2);

        function drawChartNewUsers() {
            var data = google.visualization.arrayToDataTable([
                [
                    'Datum', '# Registrierungen am Tag', { role: 'annotation' }
                ],
                <%  foreach (var day in Model.UsersNewlyRegisteredPerDay)
                    {
                        Response.Write("[new Date('" + day.DateTime.ToString("yyyy-MM-dd") + "'), " + day.Value + ", '" + day.Value + "'],");
                    } %>
            ]);

            var view = new google.visualization.DataView(data);
            var options = {
                tooltip: { isHtml: true },
                legend: { position: 'top', maxLines: 30 },
                hAxis: {
                    format: 'dd.MM.',
                    gridlines: { count: 8 }
                },
                bar: { groupWidth: '89%' },
                chartArea: { top: 50, left: 50, right: 20, bottom: 35 },
                colors: ['#afd534']
            };
            var chart = new google.visualization.ColumnChart(document.getElementById("chartNewUsers"));
            chart.draw(view, options);
        }

     

        function drawChartUsers() {
            var data = google.visualization.arrayToDataTable([
                [
                    'Datum', 'Registrierte Nutzer'
                ],
                <%  foreach (var day in Model.UsersTotalPerDay)
                    {
                        Response.Write("[new Date('" + day.DateTime.ToString("yyyy-MM-dd") + "'), " + day.Value + "],");
                    } %>
            ]);

            var view = new google.visualization.DataView(data);
            var options = {
                tooltip: { isHtml: true },
                legend: { position: 'top', maxLines: 30 },
                hAxis: {
                    format: 'dd.MM.yyyy'
                },
                bar: { groupWidth: '89%' },
                chartArea: { top: 50, left: 50, right: 20, bottom: 35 },
                colors: ['#afd534'],
                lineWidth: 6,
            };

            var chart = new google.visualization.AreaChart(document.getElementById("chartUsers"));
            chart.draw(view, options);
        }

       

        function drawChartNewQuestions() {
            var data = google.visualization.arrayToDataTable([
                [
                    'Datum', 'Von anderen Nutzern neu erstellte Fragen', 'Von memucho neu erstellte Fragen', { role: 'annotation' }
                ],
                <%  foreach (var day in Model.QuestionsCreatedPerDayResults)
                    {
                        Response.Write("[new Date('" + day.DateTime.ToString("yyyy-MM-dd") + "'), " + day.CountByOthers + ", " + day.CountByMemucho + ", '" + day.TotalCount + "'],");
                    } %>
            ]);

            var view = new google.visualization.DataView(data);

            var options = {
                tooltip: { isHtml: true },
                annotations: { alwaysOutside: true },
                legend: { position: 'top', maxLines: 30 },
                hAxis: {
                    format: 'dd.MM.',
                    gridlines: { count: 8 }
                },
                bar: { groupWidth: '89%' },
                chartArea: { top: 50, left: 50, right: 20, bottom: 35 },
                colors: ['#0000ff', '#afd534'],
                isStacked: true
            };

            var chart = new google.visualization.ColumnChart(document.getElementById("chartNewQuestions"));
            chart.draw(view, options);
        }
        

        function drawChartQuestionCountStats() {
            var data = google.visualization.arrayToDataTable([
                [
                    'Datum', 'Von Nutzern erstellte Fragen', 'Von memucho erstellte Fragen'
                ],
                <%  foreach (var day in Model.QuestionsExistingPerDayResults)
                    {
                        Response.Write("[new Date('" + day.DateTime.ToString("yyyy-MM-dd") + "'), " + day.CountByOthers + ", " + day.CountByMemucho + "],");
                    } %>
            ]);

            var view = new google.visualization.DataView(data);
            var options = {
                tooltip: { isHtml: true },
                legend: { position: 'top', maxLines: 30 },
                hAxis: {
                    format: 'dd.MM.yyyy'
                },
                bar: { groupWidth: '89%' },
                chartArea: { top: 50, left: 50, right: 20, bottom: 35 },
                colors: ['#0000ff', '#afd534'],
                lineWidth: 6,
                isStacked: true
            };

            var chart = new google.visualization.AreaChart(document.getElementById("chartQuestionCountStats"));
            chart.draw(view, options);
        }



        function drawChartUsageStatsPt1() {
            var data = google.visualization.arrayToDataTable([
                [
                    'Datum', '# Fragen beantwortet', '# Fragen gesehen', '# Lernsitzungen gestartet (/10)', '# Termine angelegt (/100)'
                ],
                <%  foreach (var day in Model.UsageStats)
                    {
                        Response.Write("[new Date('" + day.DateTime.ToString("yyyy-MM-dd") + "'), " + 
                            day.QuestionsAnsweredCount + ", " + 
                            day.QuestionsViewedCount + ", " + 
                            day.LearningSessionsStartedCount*10 + ", " + 
                            day.DatesCreatedCount*100 + "],");
                    } %>
            ]);

            var view = new google.visualization.DataView(data);

            var options = {
                tooltip: { isHtml: true },
                legend: { position: 'top', maxLines: 30 },
                hAxis: {
                    format: 'dd.MM.',
                    gridlines: {count: 16},
                    showTextEvery: 2
                },
                bar: { groupWidth: '80%' },
                chartArea: { top: 50, left: 50, right: 20, bottom: 35 },
                colors: ['#afd534', '#003264', '#b13a48', '#1964c8']
            };

            var chart = new google.visualization.ColumnChart(document.getElementById("chartUsageStats1"));
            chart.draw(view, options);
        }

        function drawChartUsageStatsPt2() {
            var data = google.visualization.arrayToDataTable([
                [
                    'Datum', '# Nutzer haben Fragen beantwortet', '# Nutzer haben Fragen gesehen', '# Nutzer haben Lernsitzung gestartet', '# Nutzer haben Termine angelegt'
                ],
                <%  foreach (var day in Model.UsageStats)
                    {
                        Response.Write("[new Date('" + day.DateTime.ToString("yyyy-MM-dd") + "'), " + 
                            day.UsersThatAnsweredQuestionCount + ", " + 
                            day.UsersThatViewedQuestionCount + ", " + 
                            day.UsersThatStartedLearningSessionCount + ", " + 
                            day.UsersThatCreatedDateCount + "],");
                    } %>
            ]);

            var view = new google.visualization.DataView(data);

            var options = {
                tooltip: { isHtml: true },
                legend: {
                    position: 'top',
                    maxLines: 30,
                    textStyle: { fontSize: 11 }
                },
                hAxis: {
                    format: 'dd.MM.',
                    gridlines: { count: 16 },
                    showTextEvery: 2
                },
                bar: { groupWidth: '80%' },
                chartArea: { top: 50, left: 50, right: 20, bottom: 35 },
                colors: ['#afd534', '#003264', '#b13a48', '#1964c8']
            };

            var chart = new google.visualization.ColumnChart(document.getElementById("chartUsageStats2"));
            chart.draw(view, options);
        }

    </script>
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
                <li><a href="/Maintenance/CMS">CMS</a></li>
                <li><a href="/Maintenance/ContentCreatedReport">Cnt-Created</a></li>
                <li><a href="/Maintenance/ContentStats">Cnt Stats</a></li>
                <li class="active"><a href="/Maintenance/Statistics">Stats</a></li>
            </ul>
        </div>
    </nav>
        
    <div class="row">
        <div class="col-xs-12">
            <h2 class="" style="margin-top: 0;">Nutzungsstatistiken</h2>
            <ul>
                <li><a href="#UsersRegistered">Registrierte Nutzer</a></li>
                <li><a href="#QuestionsCreated">Erstellte Fragen</a></li>
                <li><a href="#UsageStats">Nutzung durch eingeloggte Nutzer</a></li>
            </ul>
        </div>
    </div>

    <div class="row">
        <div class="col-xs-12">
            <h2 class="" style="margin-top: 40px;" id="UsersRegistered">Registrierte Nutzer</h2>
            <span class="greyed" style="font-size: 10px;"><a href="#Top">(nach oben)</a></span>
            <p>
                Neu registrierte Nutzer der letzten 31 Tage und Nutzer insgesamt seit <%= Model.SinceGoLive.ToString("dd.MM.yyyy") %>.
            </p>
        </div>

        <div class="col-xs-12" style="margin-top: 20px;">
            <div id="chartNewUsers" style="height: 250px; margin-right: 20px; text-align: left;"></div>
        </div>

        <div class="col-xs-12" style="margin-top: 20px;">
            <div id="chartUsers" style="height: 350px; margin-right: 20px; text-align: left;"></div>
        </div>
    </div>

    <div class="row">
        <div class="col-xs-12">
            <h2 class="" style="margin-top: 60px" id="QuestionsCreated">Erstellte Fragen</h2>
            <span class="greyed" style="font-size: 10px;"><a href="#Top">(nach oben)</a></span>
            <p>
                Neu erstellte Fragen seit <%= Model.Since.ToString("dd.MM.yyyy") %> und insgesamt existierende Fragen seit GoLive (<%= Model.SinceGoLive.ToString("dd.MM.yyyy") %>).
            </p>
        </div>

        <div class="col-xs-12" style="margin-top: 20px;">
            <div id="chartNewQuestions" style="height: 400px; margin-right: 20px; text-align: left;"></div>
        </div>
        <div class="col-xs-12" style="margin-top: 20px;">
            <div id="chartQuestionCountStats" style="height: 400px; margin-right: 20px; text-align: left;"></div>
        </div>
    </div>

    <div class="row">
        <div class="col-xs-12">
            <h1 class="" style="margin-top: 60px" id="UsageStats">Nutzung durch eingeloggte Nutzer (ohne Admins)</h1>
            <span class="greyed" style="font-size: 10px;"><a href="#Top">(nach oben)</a></span>
        </div>

        <div class="col-xs-12" style="margin-top: 20px;">
            <p>
                Wie oft wurden pro Tag durch eingeloggte (!) Nutzer (ohne Admins)... <br />
                (Lernsitzungen und Termine sind mit dem angegebenen Faktor multipliziert, um sie sichtbar zu halten.)
            </p>
            <div id="chartUsageStats1" style="height: 300px; margin-right: 20px; text-align: left;"></div>
        </div>
        <div class="col-xs-12" style="margin-top: 20px;">
            <p>
                Wie viele unterschiedliche eingeloggte Nutzer (ohne Admins) haben an diesem Tag...
            </p>
            <div id="chartUsageStats2" style="height: 300px; margin-right: 20px; text-align: left;"></div>
        </div>
        <div class="col-xs-12" style="margin-top: 30px;">
            <p>
                Zur Anzeige der Nutzung von "WuWi hinzufügen": <a href="https://analytics.google.com/analytics/web/#report/content-event-events/a84537419w87487245p130133725/%3Fexplorer-segmentExplorer.segmentId%3Danalytics.eventLabel%26_r.drilldown%3Danalytics.eventCategory%3AWuWi%26explorer-table.plotKeys%3D%5B%5D%26explorer-table.secSegmentId%3Danalytics.eventAction/" target="_blank">
                    <strong>WuWi-Statistik bei Google Analytics</strong>
                </a>
            </p>
        </div>
    </div>

</asp:Content>