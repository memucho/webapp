﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<WidgetStatsForHostModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<script type="text/javascript" src="https://www.google.com/jsapi"></script>
<script type="text/javascript">
    google.load("visualization", "1", { packages: ["corechart"] });
    google.setOnLoadCallback(drawChartAllWidgets<%= Model.HostOnlyAlphaNumerical %>);
    

    function drawChartAllWidgets<%= Model.HostOnlyAlphaNumerical %>() {
        var data = google.visualization.arrayToDataTable([
            [
                { label: 'Monat', type: 'date' },
                <% foreach (var widgetKey in Model.WidgetKeys) {
                       Response.Write("'" + widgetKey + "', ");
                        Response.Write("{type: 'string', role: 'tooltip', 'p': {'html': true}},");
                   } %>
                { role: 'annotation' }
            ]
            <% foreach (var month in Model.WidgetViewsPerMonthAndKeyResults)
               {
                   Response.Write(", [new Date('" + month.Month.ToString("yyyy-MM-dd") + "')");
                   foreach (var widgetKey in Model.WidgetKeys)
                   {
                       int value = 0;
                       month.ViewsPerWidgetKey.TryGetValue(widgetKey, out value);
                       Response.Write(", " + value);
                       Response.Write(", '<b>" + month.Month.ToString("MMMM yyyy") +"</b><br/>Widget \"" + widgetKey + "\": <b>" + value + "</b> Aufrufe'");
                   }
                   Response.Write(",'Insgesamt: "+ month.ViewsPerWidgetKey.Sum(x => x.Value) + "']");
               } %>
        ]);

        var view = new google.visualization.DataView(data);

        var options = {
            title: "Übersicht über alle Widgets (nach Widget-Identifizierung) auf \"<%= Model.Host %>\"",
            titleTextStyle: {
                fontSize: 18,
                bold: true,
                italic: false
            },
            tooltip: { isHtml: true },
            annotations: { alwaysOutside: true },
            legend: { position: 'top', maxLines: 30 },
            hAxis: {
                format: 'MM/yyyy',
                gridlines: { count: 4 }
            },
            bar: { groupWidth: '89%' },
            chartArea: { top: 50, left: 50, right: 20, bottom: 35 },
            isStacked: true
        };

        var chart = new google.visualization.ColumnChart(document.getElementById("chartAllWidgets<%= Model.HostOnlyAlphaNumerical %>"));
        chart.draw(view, options);
    }

</script>

<div class="allWidgetsViews">
    
    <div id="chartAllWidgets<%= Model.HostOnlyAlphaNumerical %>" class="statsChart" style="height: 600px; margin-right: 20px; text-align: left;">
    </div>
    
    <div style="text-align: center; margin-bottom: 10px;">
        <a class="btn btn-default" data-toggle="collapse" href="#annotation<%= Model.HostOnlyAlphaNumerical %>" aria-expanded="false" aria-controls="annotation<%= Model.HostOnlyAlphaNumerical %>">
            Erläuterungen ein-/ausblenden <i class="fa fa-caret-down"></i>
        </a>
    </div>

    <div id="annotation<%= Model.HostOnlyAlphaNumerical %>" class="collapse">
        <p>
            <strong>Erläuterung Legende:</strong> 
            Unterschiedliche Widgets werden an der Identifizierung (auch Widget-Key) erkannt, die bei der Konfiguration des Widgets angegeben werden kann. 
            Wird keine eigene Identifizierung angegeben, wird die ID des Lernsets (bei einem Lernset-Widget) bzw. der Frage (bei einem Einzelfragen-Widget) als Key verwendet.
        </p>
        <p>
            <strong>Erläuterung Zählweise:</strong> 
            Jede einzelne Seite im Widget zählt. 
            Ruft ein Nutzer die Seite mit dem Widget auf, beantwortet alle sechs Fragen und sieht auch die Ergebnisseite, dann sind 7 Aufrufe entstanden 
            (Startseite + 5 Steps + Ergebnisseite). Rufen drei weitere Nutzer nur die Webseite mit dem Widget auf, ohne im Widget auf "Wissen Testen" zu klicken, 
            sind drei weitere Aufrufe entstanden (3x Startseite). Tooltips zu jedem Widget zeigen im Balkendiagramm die genaue Anzahl der Aufrufe an.
        </p>
    </div>
</div>


<div class="singleWidgetDetails">
    <h3 style="margin: 40px 0 30px;">Details zu einzelnen Widgets</h3>
    <% var divSingleWidgetGuid = Guid.NewGuid(); %>
    <form>
        Zeige Aufrufe zu diesem Widget: <select class="selectWidgetForSingleView" data-host="<%= Model.Host %>" data-target-div="<%= divSingleWidgetGuid %>">
            <% foreach (var widgetKey in Model.WidgetKeys)
               { %>
                   <option value="<%= widgetKey %>"><%= widgetKey %></option>
            <% } %>
        </select>
    </form>
    
    <div id="<%= divSingleWidgetGuid %>">
        <% Html.RenderPartial("~/Views/Users/Account/WidgetStats/Partials/WidgetStatsDetailViews.ascx", new WidgetStatsDetailViewsModel(Model.Host, Model.WidgetKeys.FirstOrDefault())); %>
    </div>
    

    <p></p>

</div>
