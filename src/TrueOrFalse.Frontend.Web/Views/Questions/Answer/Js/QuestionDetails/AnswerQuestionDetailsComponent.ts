﻿declare var d3: any;
declare var Vue: any;

Vue.component('question-details-component', {

    template: '#question-details-component',

    data() {
        return {
            questionId: 0,
            personalProbability: 0,
            personalProbabilityText: "Nicht im Wunschwissen",
            personalColor: "#DDDDDD",
            avgProbability: 0,
            personalAnswerCount: 0,
            personalAnsweredCorrectly: 0,
            overallAnswerCount: 0,
            overallAnsweredCorrectly: 0,
            isLoggedIn: IsLoggedIn.Yes,
            isInWishknowledge: false,
            showTopBorder: false,
            arcSvg: {},
            personalCounterSvg: {},
            overallCounterSvg: {},

            baseArcData: {
                startAngle: -0.55 * Math.PI,
                endAngle: 0.55 * Math.PI,
                innerRadius: 45,
                outerRadius: 50,
                fill: "#DDDDDD",
            },

            personalArcData: {},

            avgArcData: {},

            baseArcPath: "",
            personalArcPath: "",
            avgArcPath: "",

            arc: d3.arc(),
            testData: "",
            arcLoaded: false,
            percentageLabelWidth: 0,
            avgAngle: 0,
            dxAvgLabel: 0,
            dyAvgLabel: 0,
            avgLabelAnchor: "middle",
            avgProbabilityLabelWidth: 0,
            arcSvgWidth: 0,
            showPersonalArc: false,
            categoryList: "",
            personalStartAngle: 0,
            overallStartAngle: 0,

            baseCounterData: {
                startAngle: 0,
                endAngle: 2 * Math.PI,
                innerRadius: 20,
                outerRadius: 25,
                fill: "#DDDDDD",
            },

            personalWrongAnswerCountData: {},
            personalCorrectAnswerCountData: {},

            overallWrongAnswerCountData: {},
            overallCorrectAnswerCountData: {},
        };
    },

    created() {
        eventBus.$on('set-question-id', (id) => this.questionId = id);
        eventBus.$on('update-question-details', () => this.loadData());
    },

    watch: {
        questionId: function(id) {
            if (id > 0) {
                this.loadCategoryList();
                this.loadData();
            }
        },

        personalProbability: function (val) {
            this.setPersonalArcData();
            if (this.arcLoaded)
                this.updateArc();
        },

        personalAnswerCount: async function(val) {
            if (val > 0)
                this.showPersonalArc = true;
            this.personalStartAngle = 100 - (100 / this.personalAnswerCount * this.personalAnsweredCorrectly);
            await this.setPersonalCounterData();
            this.updateArc();
        },

        personalAnsweredCorrectly: async function() {
            this.personalStartAngle = 100 - (100 / this.personalAnswerCount * this.personalAnsweredCorrectly);
            await this.setPersonalCounterData();
            this.updateArc();
        },

        overallAnswerCount: function () {
            this.overallStartAngle = 100 - (100 / this.overallAnswerCount * this.overallAnsweredCorrectly);
        },

        overAllAnsweredCorrectly: async function () {
            this.overallStartAngle = 100 - (100 / this.personalAnswerCount * this.personalAnsweredCorrectly);
            await this.setOverallCounterData();
            this.updateArc();
        },

        avgProbability: async function () {
            await this.setAvgArcData();
            await this.setAvgArcData();
            if (this.arcLoaded) {
                await this.setAvgLabelPos();
                await this.updateArc();
            }
        },
    },

    computed: {
    },

    mounted: function () {
    },

    methods: {

        loadCategoryList() {
            $.ajax({
                url: "/AnswerQuestion/RenderCategoryList/",
                data: { questionId: this.questionId },
                type: "Post",
                success: categoryListView => {
                    this.categoryList = "'" + categoryListView + "'";
                }
            });
        },

        loadData() {
            $.ajax({
                url: "/AnswerQuestion/GetQuestionDetails/",
                data: { questionId: this.questionId },
                type: "Post",
                success: async data => {
                    this.personalProbability = data.personalProbability;
                    this.isInWishknowledge = data.isInWishknowledge;
                    this.avgProbability = data.avgProbability;
                    this.personalAnswerCount = data.personalAnswerCount;
                    this.personalAnsweredCorrectly = data.personalAnsweredCorrectly;
                    this.overallAnswerCount = data.overallAnswerCount;
                    this.overallAnsweredCorrectly = data.overallAnsweredCorrectly;
                    this.personalColor = data.personalColor;
                    await this.setPersonalProbability();
                    await this.setPersonalArcData();
                    await this.setAvgArcData();
                    await this.setPersonalCounterData();
                    await this.setOverallCounterData();
                    if (!this.arcLoaded) {
                        this.drawArc();
                        this.drawCounterArcs();
                    }
                    else
                        this.updateArc();
                }
            });
        },

        setPersonalProbability() {
            if (this.isInWishknowledge) {
                if (this.personalAnswerCount <= 0) {
                    this.personalProbabilityText = "Nicht gelernt";
                    this.personalColor = "#999999";
                }
                else if (this.personalProbability >= 80)
                    this.personalProbabilityText = "Sicheres Wissen";
                else if (this.personalProbability < 80 && this.personalProbability >= 50)
                    this.personalProbabilityText = "Zu festigen";
                else if (this.personalProbability < 50 && this.personalProbability >= 0)
                    this.personalProbabilityText = "Zu lernen";
            } else {
                this.personalColor = "#DDDDDD";
                this.personalProbabilityText = "Nicht im Wunschwissen";
            }

        },

        setPersonalArcData() {
            this.personalArcData = {
                startAngle: -0.55 * Math.PI,
                endAngle: (-0.55 + this.personalProbability / 100 * 1.1) * Math.PI,
                innerRadius: 40,
                outerRadius: 55,
                fill: this.personalColor,
                class: "personalArc",
            };
            return true;
        },
        setAvgArcData() {
            var avgInnerRadius = 37.5;
            var avgOuterRadius = 57.5;

            if (this.personalProbability < this.avgProbability) {
                avgInnerRadius = 42.5;
                avgOuterRadius = 52.5;
            }

            this.avgArcData = {
                startAngle: (-0.55 + this.avgProbability / 100 * 1.1) * Math.PI - 0.01,
                endAngle: (-0.55 + this.avgProbability / 100 * 1.1) * Math.PI + 0.01,
                innerRadius: avgInnerRadius,
                outerRadius: avgOuterRadius,
                fill: "#707070",
                class: "avgArc"
            };

            this.avgAngle = (-0.55 + this.avgProbability / 100 * 1.1) * Math.PI;
        },

        drawArc() {

            var width = 200;
            var height = 130;
            var self = this;

            var semiPieRef = self.$refs.semiPie;

            self.arcSvg = d3.select(semiPieRef).append("svg")
                .attr("width", width)
                .attr("height", height)
                .append("g").attr("transform", "translate(" + width / 2 + "," + (height - 50) + ")");

            var arc = d3.arc();

            var data = [
                this.baseArcData,
                this.personalArcData,
                this.avgArcData
            ];

            self.arcSvg.selectAll("path").data(data).enter()
                .append("path")
                .style("fill", function (d) { return d.fill })
                .attr("class", function (d) { return d.class })
                .attr("d", arc);

            self.arcSvg.selectAll(".personalArc")
                .style("visibility", function() {
                    return self.showPersonalArc ? "visible" : "hidden";
                });

            this.drawProbabilityLabel();
            this.setAvgLabel();

            this.arcLoaded = true;
        },

        setAvgLabelPos() {
            var self = this;
            var probabilityData = [self.avgProbability];

            self.arcSvg.append('g')
                .selectAll('.dummyAvgProbability')
                .data(probabilityData)
                .enter()
                .append("text")
                .attr("font-family", "font-family:Lato")
                .attr("font-weight", "regular")
                .attr("font-size", "12")
                .text(function (d) { return "∅ " + d + "%" })
                .each(function () {
                    var thisWidth = this.getComputedTextLength();
                    self.avgProbabilityLabelWidth = thisWidth;
                    this.remove();
                });

            var el = (self.avgProbability - 50) / 10;
            self.dyAvgLabel = (0.20 * Math.pow(el, 2) - 5) * 2 + .25 * (Math.pow(el, 2));

            self.dxAvgLabel = 0;

            if (self.avgProbability > 50) {
                if (self.avgProbability < 80)
                    self.dxAvgLabel = -(80 - self.avgProbability) / 100 * self.avgProbabilityLabelWidth;
                else
                    self.dxAvgLabel = - (20 - self.avgProbability) * 6 / 100;
                self.avgLabelAnchor = "start";
            }
            else if (self.avgProbability < 50) {
                if (self.avgProbability > 20)
                    self.dxAvgLabel = (self.avgProbability - 20) / 100 * self.avgProbabilityLabelWidth;
                else
                    self.dxAvgLabel = (self.avgProbability - 80) * 6 / 100;
                self.avgLabelAnchor = "end";
            }
            else if (self.avgProbability == 50) {
                self.avgLabelAnchor = "middle";
            }
        },

        setAvgLabel() {
            var self = this;

            this.setAvgLabelPos();

            var pos = d3.arc()
                .innerRadius(55)
                .outerRadius(55)
                .startAngle(self.avgAngle)
                .endAngle(self.avgAngle);

            self.arcSvg.append("svg:text")
                .attr("transform", function (d) {
                    return "translate(" + pos.centroid(d) + ")";
                })
                .attr("dx", self.dxAvgLabel)
                .attr("dy", self.dyAvgLabel)
                .attr("text-anchor", self.avgLabelAnchor)
                .attr("style", "font-family:Lato")
                .attr("font-size", "12")
                .attr("font-weight", "regular")
                .style("fill", "#555555")
                .style("opacity", 1.0)
                .attr("class", "avgProbabilityLabel")
                .text("∅ " + self.avgProbability + "%");

        },

        drawProbabilityLabel() {
            var self = this;
            var labelWidth = this.calculateLabelWidth();

            self.arcSvg.append("svg:text")
                .attr("dy", ".1em")
                .attr("dx", -(labelWidth / 2) + "px")
                .attr("text-anchor", "left")
                .attr("style", "font-family:Lato")
                .attr("font-size", "30")
                .attr("font-weight", "bold")
                .style("fill", () => self.showPersonalArc ? self.personalColor : "#DDDDDD")
                .attr("class", "personalProbabilityLabel")
                .text(() => self.personalAnswerCount > 0 ? self.personalProbability : self.avgProbability);

            self.arcSvg.append("svg:text")
                .attr("dy", "-.35em")
                .attr("dx", (labelWidth / 2) - self.percentageLabelWidth - 1 + "px")
                .attr("style", "font-family:Lato")
                .attr("text-anchor", "left")
                .attr("font-size", "18")
                .attr("font-weight", "medium")
                .attr("class", "percentageLabel")
                .style("fill", () => self.showPersonalArc ? self.personalColor : "#DDDDDD")
                .text("%");

            self.arcSvg.append("svg:rect")
                .attr("class", "personalProbabilityChip")
                .attr("rx", 10)
                .attr("ry", 10)
                .attr("y", 20)
                .attr("height", 20)
                .style("fill", self.personalColor)
                .style("visibility", function () {
                    return self.isLoggedIn ? "visible" : "hidden";
                })
                .attr("transform", "translate(" + 0 + "," + 0 + ")");

            var textWidth = 0;
            self.arcSvg
                .append("svg:text")
                .attr("dy", "33.5")
                .attr("style", "font-family:Open Sans")
                .attr("text-anchor", "middle")
                .attr("font-size", "10")
                .attr("font-weight", "medium")
                .attr("class", "personalProbabilityText")
                .style("fill", () => self.personalColor == "#999999" ? "white" : "#555555")
                .attr("transform", "translate(" + 0 + "," + 0 + ")")
                .text(self.personalProbabilityText)
                .each(function () {
                    textWidth = this.getComputedTextLength();
                });

            self.arcSvg.selectAll(".personalProbabilityChip")
                .attr("x", - textWidth / 2 - 11)
                .attr("width", textWidth + 22);

            self.arcSvg.selectAll(".personalProbabilityChip,.personalProbabilityText")
                .style("visibility", () => (self.isLoggedIn && self.overallAnswerCount > 0) ? "visible" : "hidden");

        },

        calculateLabelWidth() {
            var self = this;
            var probabilityLabelWidth = 0;

            var probabilityAsText = [self.personalProbability];

            self.arcSvg.append('g')
                .selectAll('.dummyProbability')
                .data(probabilityAsText)
                .enter()
                .append("text")
                .attr("font-family", "font-family:Lato")
                .attr("font-weight", "bold")
                .attr("font-size", "30px")
                .text(self.personalProbability)
                .each(function () {
                    var thisWidth = this.getComputedTextLength();
                    probabilityLabelWidth = thisWidth;
                    this.remove();
                });

            self.arcSvg.append('g')
                .selectAll('.dummyPercentage')
                .data("%")
                .enter()
                .append("text")
                .attr("font-family", "font-family:Lato")
                .attr("font-weight", "medium")
                .attr("font-size", "18px")
                .text(function (d) { return d })
                .each(function () {
                    var thisWidth = this.getComputedTextLength();
                    self.percentageLabelWidth = thisWidth;
                    this.remove();
                });

            return probabilityLabelWidth + self.percentageLabelWidth + 1;
        },

        setPersonalCounterData() {
            var self = this;

            self.personalWrongAnswerCountData = {
                startAngle: 0,
                endAngle: (self.personalStartAngle / 100 * 1) * Math.PI * 2,
                innerRadius: 20,
                outerRadius: 25,
                fill: "#FFA07A",
                class: "personalWrongAnswerCounter",
            };

            self.personalCorrectAnswerCountData = {
                startAngle: (self.personalStartAngle / 100 * 1) * Math.PI * 2,
                endAngle: 2 * Math.PI,
                innerRadius: 20,
                outerRadius: 25,
                fill: "#AFD534",
                class: "personalCorrectAnswerCounter",
            };
        },

        setOverallCounterData() {
            var self = this;

            self.overallWrongAnswerCountData = {
                startAngle: 0,
                endAngle: (self.overallStartAngle / 100 * 1) * Math.PI * 2,
                innerRadius: 20,
                outerRadius: 25,
                fill: "#FFA07A",
                class: "overallWrongAnswerCounter",
            };

            self.overallCorrectAnswerCountData = {
                startAngle: (self.overallStartAngle / 100 * 1) * Math.PI * 2,
                endAngle: 2 * Math.PI,
                innerRadius: 20,
                outerRadius: 25,
                fill: "#AFD534",
                class: "overallCorrectAnswerCounter",
            };
        },

        drawCounterArcs() {

            var self = this;

            var arc = d3.arc();

            var personalCounterData = [
                this.baseCounterData,
                this.personalWrongAnswerCountData,
                this.personalCorrectAnswerCountData
            ];

            var personalCounter = self.$refs.personalCounter;

            self.personalCounterSvg = d3.select(personalCounter).append("svg")
                .attr("width", 50)
                .attr("height", 50)
                .append("g").attr("transform", "translate(" + 25 + "," + 25 + ")");

            self.personalCounterSvg.selectAll("path")
                .data(personalCounterData)
                .enter()
                .append("path")
                .style("fill", function (d) { return d.fill })
                .attr("class", function (d) { return d.class })
                .attr("d", arc);

            self.personalCounterSvg.selectAll(".personalWrongAnswerCounter,.personalCorrectAnswerCounter")
                .style("visibility", () => self.personalAnswerCount > 0 ? "visible" : "hidden");

            self.personalCounterSvg
                .append('svg:foreignObject')
                .attr('height', '16px')
                .attr('width', '14px')
                .attr('x', -7)
                .attr('y', -8)
                .html(function () {
                    var fontColor = self.personalAnswerCount > 0 ? "#999999" : "#DDDDDD";
                    return "<i class='fas fa-user' style='font-size:16px; color:" + fontColor + "'> </i>";
                });

            var overallCounterData = [
                this.baseCounterData,
                this.overallWrongAnswerCountData,
                this.overallCorrectAnswerCountData
            ];

            var overallCounter = self.$refs.overallCounter;

            self.overallCounterSvg = d3.select(overallCounter).append("svg")
                .attr("width", 50)
                .attr("height", 50)
                .append("g").attr("transform", "translate(" + 25 + "," + 25 + ")");

            self.overallCounterSvg.selectAll("path")
                .data(overallCounterData)
                .enter()
                .append("path")
                .style("fill", function (d) { return d.fill })
                .attr("class", function (d) { return d.class })
                .attr("d", arc);

            self.overallCounterSvg.selectAll(".overallCounter")
                .style("visibility", () => self.overallAnswerCount > 0 ? "visible" : "hidden");

            self.overallCounterSvg
                .append('svg:foreignObject')
                .attr('height', '16px')
                .attr('width', '20px')
                .attr('x', -10)
                .attr('y', -8)
                .html(function () {
                    var fontColor = self.overallAnswerCount > 0 ? "#999999" : "#DDDDDD";
                    return "<i class='fas fa-users' style='font-size:16px; color:" + fontColor + "'> </i>";
                });
        },

        updateArc() {
            var self = this;
            var labelWidth = this.calculateLabelWidth();

            self.arcSvg.selectAll(".personalProbabilityLabel")
                .transition()
                .duration(800)
                .attr("dx", -(labelWidth / 2) + "px")
                .style("fill", () => self.showPersonalArc ? self.personalColor : "#DDDDDD")
                .tween("text",
                    function () {
                        var selection = d3.select(this);
                        var start = d3.select(this).text();
                        var end = self.personalProbability;
                        var interpolator = d3.interpolateNumber(start, end);

                        return function(t) { selection.text(Math.round(interpolator(t))); };
                    });

            var pos = d3.arc()
                .innerRadius(55)
                .outerRadius(55)
                .startAngle(self.avgAngle)
                .endAngle(self.avgAngle);

            self.arcSvg.select(".avgProbabilityLabel")
                .transition()
                .duration(400)
                .style("opacity", 0.0);

            self.arcSvg.select(".avgProbabilityLabel")
                .transition()
                .delay(400)
                .duration(400)
                .style("opacity", 1.0)
                .attr("transform",
                    function (d) {
                        return "translate(" + pos.centroid(d) + ")";
                    })
                .attr("dx", self.dxAvgLabel)
                .attr("dy", self.dyAvgLabel)
                .attr("text-anchor", self.avgLabelAnchor)
                .tween("text",
                    function () {
                        var selection = d3.select(this);
                        var text = d3.select(this).text();
                        var numbers = text.match(/(\d+)/);
                        var end = self.avgProbability;
                        var interpolator = d3.interpolateNumber(numbers[0], end);

                        return function (t) {
                            selection.text("∅ " + Math.round(interpolator(t)) + "%");
                        };
                    });;

            self.arcSvg.selectAll(".percentageLabel").transition()
                .duration(800)
                .attr("dx", (labelWidth / 2) - self.percentageLabelWidth + "px")
                .style("fill", () => self.showPersonalArc ? self.personalColor : "#DDDDDD");

            self.arcSvg.selectAll(".personalArc")
                .transition()
                .duration(800)
                .style("fill", self.personalColor)
                .style("visibility", function () {
                    return self.showPersonalArc ? "visible" : "hidden";
                })
                .attrTween("d", function(d) {
                    return self.arcTween(d,
                        self.personalArcData.startAngle,
                        self.personalArcData.endAngle,
                        self.personalArcData.innerRadius,
                        self.personalArcData.outerRadius);
                });

            self.arcSvg.selectAll(".avgArc")
                .transition()
                .duration(800)
                .attrTween("d", function (d) {
                    return self.arcTween(d,
                        self.avgArcData.startAngle,
                        self.avgArcData.endAngle,
                        self.avgArcData.innerRadius,
                        self.avgArcData.outerRadius);
                });

            self.arcSvg.selectAll(".personalProbabilityChip,.personalProbabilityText")
                .style("visibility", () => (self.isLoggedIn && self.overallAnswerCount > 0) ? "visible" : "hidden");


            self.personalCounterSvg.selectAll(".personalWrongAnswerCounter,.personalCorrectAnswerCounter")
                .style("visibility", function () {
                    return self.personalAnswerCount > 0 ? "visible" : "hidden";
                });


            self.personalCounterSvg.selectAll(".personalWrongAnswerCounter")
                .transition()
                .duration(800)
                .attrTween("d", function (d) {
                    return self.arcTween(d,
                        self.personalWrongAnswerCountData.startAngle,
                        self.personalWrongAnswerCountData.endAngle,
                        20,
                        25);
                });

            self.personalCounterSvg.selectAll(".personalCorrectAnswerCounter")
                .transition()
                .duration(800)
                .attrTween("d", function (d) {
                    return self.arcTween(d,
                        self.personalCorrectAnswerCountData.startAngle,
                        self.personalCorrectAnswerCountData.endAngle,
                        20,
                        25);
                });


            self.overallCounterSvg.selectAll(".overallWrongAnswerCounter,.overallCorrectAnswerCounter")
                .style("visibility", function () {
                    return self.overallAnswerCount > 0 ? "visible" : "hidden";
                });

            self.overallCounterSvg.selectAll(".overallWrongAnswerCounter")
                .transition()
                .duration(800)
                .attrTween("d", function (d) {
                    return self.arcTween(d,
                        self.overallWrongAnswerCountData.startAngle,
                        self.overallWrongAnswerCountData.endAngle,
                        20,
                        25);
                });

            self.overallCounterSvg.selectAll(".overallCorrectAnswerCounter")
                .transition()
                .duration(800)
                .attrTween("d", function (d) {
                    return self.arcTween(d,
                        self.overallCorrectAnswerCountData.startAngle,
                        self.overallCorrectAnswerCountData.endAngle,
                        20,
                        25);
                });

            self.personalCounterSvg.selectAll("text")
                .transition()
                .duration(800)
                .style("fill", () => self.personalAnswerCount > 0 ? "#999999" : "#DDDDDD");

            self.overallCounterSvg.selectAll("text")
                .transition()
                .duration(800)
                .style("fill", () => self.overallAnswerCount > 0 ? "#999999" : "#DDDDDD");
        },

        arcTween(d, newStartAngle, newEndAngle, newInnerRadius, newOuterRadius) {
            var arc = d3.arc();

            var interpolateStart = d3.interpolate(d.startAngle, newStartAngle);
            var interpolateRadiusStart = d3.interpolate(d.innerRadius, newInnerRadius);
            var interpolateEnd = d3.interpolate(d.endAngle, newEndAngle);
            var interpolateRadiusEnd = d3.interpolate(d.outerRadius, newOuterRadius);
            return function (t) {
                d.innerRadius = interpolateRadiusStart(t);
                d.outerRadius = interpolateRadiusEnd(t);
                d.startAngle = interpolateStart(t);
                d.endAngle = interpolateEnd(t);
                return arc(d);
            }
        },
    },
});