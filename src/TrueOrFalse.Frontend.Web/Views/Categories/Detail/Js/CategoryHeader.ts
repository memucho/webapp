declare var d3;

interface ViewsPerDay {
    Date: string;
    Views: number;
}

class CategoryHeader {

    private _page: CategoryPage;
    private _isLoaded: boolean;

    constructor(page: CategoryPage) {

        $("#SessionConfigReminderHeader>.fa-times-circle").on('click',
            () => {
                $.post("/Category/SetSettingsCookie?name=ShowSessionConfigurationMessageTab");
                $("#SessionConfigReminderHeader").hide(200);
            }); 
        

        $("#jsAdminStatistics").click(() => {
        
            $("#last60DaysViews").toggle();
            
            if (!this._isLoaded) {
                this._isLoaded = true;
                this.getData(page.CategoryId)
                    .done((data) => {
                        this.drawViewsByDayChart(data);
                    });
            };
        });
    }

    private getData(categoryId: number) : JQueryXHR {
        return $.get(`/Api/CategoryStatistics/ViewsByDayByName/?categoryId=${categoryId}&amountOfDays=120`);
    }

    drawViewsByDayChart(data_: ViewsPerDay[]){
        
        var data = data_.map((d) => {
            var date = new Date(parseInt(d.Date.substr(6)));
            return {
                date: date.getFullYear() + "-" + date.getMonth() + "-" + date.getDay(),
                value: d.Views
            };
        });

        var margin = {top: 20, right: 20, bottom: 70, left: 40},
            width = 500 - margin.left - margin.right,
            height = 270 - margin.top - margin.bottom;

        var x = d3.scaleBand().rangeRound([0, width]).padding(0.1),
            y = d3.scaleLinear().rangeRound([height, 0]);

        var g = d3.select("#last60DaysViews")
            .append("svg")
                .attr("width", width + margin.left + margin.right)
                .attr("height", height + margin.top + margin.bottom)
            .append("g")
                .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

        x.domain(data.map(function(d) { return d.date; }));
        y.domain([0, d3.max(data, function(d) { return d.value; })]);

        g.append("g")
            .attr("class", "axis axis--x")
            .attr("transform", "translate(0," + height + ")")
            .call(d3.axisBottom(x))      
                .selectAll("text")	
                .style("text-anchor", "end")
                .attr("dx", "-.8em")
                .attr("dy", ".15em")
                .attr("transform", "rotate(-90) translate(0, -7)");

        g.append("g")
            .attr("class", "axis axis--y")
            .call(d3.axisLeft(y).ticks(10))
         .append("text")
            .attr("transform", "rotate(-90)")
            .attr("y", 6)
            .attr("dy", "0.71em")
            .attr("text-anchor", "end")
            .text("Date");

        g.selectAll(".bar")
        .data(data)
        .enter()
            .append("rect")
            .style("fill", "steelblue")
            .attr("class", "bar")
            .attr("x", function(d) { return x(<any>d.date); })
            .attr("y", function(d) { return y(<any>d.value); })
            .attr("width", x.bandwidth())
            .attr("height", function(d) { return height - y(<any>d.value); });
    }
}