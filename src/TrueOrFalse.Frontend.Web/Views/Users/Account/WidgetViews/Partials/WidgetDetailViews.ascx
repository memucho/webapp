﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<WidgetDetailViewsModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<script type="text/javascript">
    google.setOnLoadCallback(drawChartSingleWidgetDetail_<%= Model.HostOnlyAlphaNumerical %>);

    function drawChartSingleWidgetDetail_<%= Model.HostOnlyAlphaNumerical %>() {
        var data = google.visualization.arrayToDataTable([
            [
                { label: 'Monat', type: 'date' },
                <% foreach (var widgetType in Model.WidgetTypes) {
                       Response.Write("'" + WidgetView.GetDescriptionForWidgetType(widgetType) + "', ");
                   } %>
                { role: 'annotation' }
            ]
            <% foreach (var month in Model.WidgetDetailViewsPerMonthAndType)
               {
                   Response.Write(", [new Date('" + month.Month.ToString("yyyy-MM-dd") + "')");
                   foreach (var widgetType in Model.WidgetTypes)
                   {
                       int value = 0;
                       month.ViewsPerWidgetType.TryGetValue(widgetType, out value);
                       Response.Write(", " + value);
                   }
                   Response.Write(",'Insgesamt: "+ month.ViewsPerWidgetType.Sum(x => x.Value) + "']");
               } %>
        ]);

        var view = new google.visualization.DataView(data);

        var options = {
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

        var chart = new google.visualization.ColumnChart(document.getElementById("chartSingleWidgetDetail_<%= Model.HostOnlyAlphaNumerical %>"));
        chart.draw(view, options);
    }
</script>


<div id="chartSingleWidgetDetail_<%= Model.HostOnlyAlphaNumerical %>" style="height: 400px; margin-right: 20px; text-align: left;">
</div>

