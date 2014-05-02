$(function() {
    $("[popover-all-sets-for]").click(function (e) {

        e.preventDefault();

        var elem = $(this);

        if (elem.attr("loaded") == "true")
            return;
        
        $.post("/Api/Sets/ForQuestion", {
            "questionId": elem.attr("popover-all-sets-for")
        }, function (data) {

            elem.attr("loaded", "true");

            var content = "";
            for (var i = 5; i < data.length; i++) {
                content += "<a href='"+ data[i].Url +"'><span class='label label-set' style='display:block;'>" + data[i].Name +  "</span></a>&nbsp;";
            }

            content = "<div style=''>" + content + "</div>";

            elem.popover({
                title: 'weitere Frages&#228tze',
                html : true,
                content: content,
                trigger: 'click'
            });

            elem.popover('show');
        });
    });

    /*JULE NOGO AREA*/
    $("#logo").hover(
        function () { $(this).animate({ 'background-size': '100%' }, 250); },
        function () { $(this).animate({ 'background-size': '86%' }, 250); }
    );
    /*JULE NOGO END*/

    $(".sparklineTotals").each(function () {
        $(this).sparkline([parseInt($(this).attr("data-answersTrue")), parseInt($(this).attr("data-answersFalse"))], {
            type: 'pie',
            sliceColors: ['#3e7700', '#B13A48']
        });
    });

    $(".sparklineTotalsUser").each(function () {
        $(this).sparkline([parseInt($(this).attr("data-answersTrue")), parseInt($(this).attr("data-answersFalse"))], {
            type: 'pie',
            sliceColors: ['#3e7700', '#B13A48']
        });
    });

    new MenuMobile();
    new Menu();
});

class Menu {

    constructor() {
        $("#mainMenu .list-group-item").hover(
            function () {
                $(this).find(".show-on-hover").show(150);
            },
            function () {
                $(this).find(".show-on-hover").hide(150);
            }
        );
    }
}

class MenuMobile {

    private _isOpen: boolean = false;
    private _isInProgress: boolean = false;

    constructor() {
        $("#MenuButton").click(() => {
            if (this._isOpen)
                this.closeMenu();
            else
                this.openMenu();
        });

        //close on click outside the menu

        //$(document).mouseup((e) => { //$temp: Review - this does not work
        $(document).mouseup(function (e) {

            if (!this._isOpen)
                return;

            if ($("#mainMenu").has(e.target).length === 0 &&
                $("#MenuButton").has(e.target).length === 0) {
                this.closeMenu();
            }
        });

        //close on ESC
        $(document).keyup((e: any) => {
            if (!this._isOpen)
                return;

            if (e.keyCode == 27) {
                this.closeMenu();
            }
        });
    }

    openMenu() {

        if (this._isInProgress)
            return;

        this._isInProgress = true;
        $("#mainMenu").slideDown();
        this._isOpen = true;
        this._isInProgress = false;
    }

    closeMenu() {

        if (this._isInProgress)
            return;

        this._isInProgress = true;
        $("#mainMenu").slideUp();
        this._isOpen = false;
        this._isInProgress = false;
    }

}