﻿var QuestionsSearch = (function () {
    function QuestionsSearch() {
        var _this = this;
        this._elemContainer = $("#JS-SearchResult");
        var _self = this;

        $('#btnSearch').click(function (e) {
            e.preventDefault();
            _this.SubmitSearch();
        });

        $("#txtSearch").keypress(function (e) {
            if (e.keyCode === 13) {
                e.preventDefault();
                _self.SubmitSearch();
            }
        });
    }
    QuestionsSearch.prototype.SubmitSearch = function () {
        var _this = this;
        this._elemContainer.html("<div style='text-align:center; padding-top: 30px;'>" + "<i class='fa fa-spinner fa-spin'></i>" + "</div>");

        $.post($('#txtSearch').attr("formUrl") + "Api", { searchTerm: $('#txtSearch').val() }, function (data) {
            _this._elemContainer.html(data.Html);

            var tabAmount = data.TotalInResult.toString() + " von " + data.TotalInSystem.toString();
            if (data.TotalInResult == data.TotalInSystem) {
                tabAmount = data.TotalInSystem.toString();
            }

            $(".JS-TabsUl").find("li.JS-" + data.Tab).find("span.JS-Amount").html(tabAmount);
        });
    };
    return QuestionsSearch;
})();

$(function () {
    new QuestionsSearch();
});
//# sourceMappingURL=QuestionsSearch.js.map
