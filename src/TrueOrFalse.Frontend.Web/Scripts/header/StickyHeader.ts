﻿

class StickeyHeaderClass {
    private Breadcrumb;
    private RightMainMenu;   
    private Header;
    private OuterHeightBreadCrumb;


    constructor() {
        this.Breadcrumb = $('#Breadcrumb').get(0);
        this.RightMainMenu = $("#RightMainMenu").get(0);
        this.Header = $("#MasterHeader").get(0);
        this.OuterHeightBreadCrumb = $("#Breadcrumb").outerHeight();



        $(window).scroll(() => {
            this.StickyHeader();
        });

        window.onresize = () => {
            this.StickyHeader();
        }

    }

    private StickyHeader() {

        if ($(window).scrollTop() > 80) {
            $('#BreadcrumbLogoSmall').show();
            $('#StickyHeaderContainer').css('display', 'flex');
            $("#Breadcrumb").css("z-index", 100);

            $("#BreadcrumbUserDropdownImage").on("click",
                (e) => {
                    e.preventDefault();
                    setTimeout(() => {                                                     // class open removed from jquery or Bootstrap the Problem is #HeaderUserDropdown is used only once
                        $("#HeaderUserDropdown").addClass("open");
                    }, 10);
                    var t = $("#HeaderUserDropdown").offset().top;
                   
                    $("#userDropdown").css("top", $(window).scrollTop() + $("#Breadcrumb").outerHeight() - $("#userDropdown").parent().offset().top  + "px");  // I have no idea where the 3 pixels come from
                    $("#userDropdown").css("z-index", 1000);
                });



            this.Breadcrumb.style.top = "0";
            this.Breadcrumb.classList.add("ShowBreadcrumb");
            this.Breadcrumb.classList.add("sticky");

            this.RightMainMenu.style.position = "absolute";
            //this.RightMainMenu.style.top = "52px";               // überprüfen, die Größe der Breadcrumb ändert sich doch, also kann man das doch nicht fest verdrahten (fest verdrahten eh bööööse) 

            $('#BreadCrumbTrail').css('max-width', "51%");

        } else {
            //$("#BreadcrumbUserDropdownImage").on("click", () => {
            //        $("#HeaderUserDropdown").addClass("open");
            //    var t = $("#MasterHeader").outerHeight();
            //    var v = $("#Breadcrumb").outerHeight();
            //    var r = t + v;
            //        $("#userDropdown").css("top", $("#MasterHeader").outerHeight() + $("#Breadcrumb").outerHeight() + "px");
            //    });

            this.Breadcrumb.style.top = (80 + this.Header.scrollTop).toString() + "px";
            this.Breadcrumb.style.position = "absolute";
            $("#Breadcrumb").css("z-index", 100);

            if (this.Breadcrumb.classList.contains("ShowBreadcrumb")) this.Breadcrumb.classList.remove("ShowBreadcrumb");

            $('#BreadcrumbLogoSmall').hide();
            $('#StickyHeaderContainer').hide();

           // this.RightMainMenu.style.position = "absolute";
            this.RightMainMenu.style.top = ($("#MasterHeader").outerHeight() + $("#Breadcrumb").outerHeight() + "px");
            this.RightMainMenu.style.position = "absolute";

            $('#BreadCrumbTrail').css("max-width", "");

            if (this.Breadcrumb.classList.contains("sticky")) {
                this.Breadcrumb.classList.remove("sticky");
            }

            if (top.location.pathname === "/") {
                this.Breadcrumb.style.display = "none";
            }

            if (window.innerWidth < 768) {
                this.Breadcrumb.style.top = (50 + this.Header.scrollTop).toString() + "px";
            }
        }

        if (this.countLines(this.Breadcrumb) === 1) {
            this.Breadcrumb.style.height = "55px";
        } else {
            this.Breadcrumb.style.height = "auto";
        }

        this.reorientatedMenu($(window).scrollTop());
    }

   private countLines(target) {

        document.getElementById("Breadcrumb").style.height = "auto";
        var style = window.getComputedStyle(target, null);
        var height = parseInt(style.getPropertyValue("height"));
        var font_size = parseInt(style.getPropertyValue("font-size"));
        var line_height = parseInt(style.getPropertyValue("line-height"));
        var box_sizing = style.getPropertyValue("box-sizing");

        if (isNaN(line_height)) line_height = font_size * 1.2;

        if (box_sizing == 'border-box') {
            var padding_top = parseInt(style.getPropertyValue("padding-top"));
            var padding_bottom = parseInt(style.getPropertyValue("padding-bottom"));
            var border_top = parseInt(style.getPropertyValue("border-top-width"));
            var border_bottom = parseInt(style.getPropertyValue("border-bottom-width"));
            height = height - padding_top - padding_bottom - border_top - border_bottom
        }
        var lines = Math.ceil(height / line_height);
        lines = lines - 1;
        return lines;
    }

   private reorientatedMenu(pos: number):void {
        if (pos > 80) {
            $('#BreadcrumbUserDropdown').css('margin-right', $('#BreadCrumbContainer').css('margin-right'));
        } else {
            $('#BreadcrumbUserDropdown').css('margin-right', '');
        }
    }
}

$(() => {
 new StickeyHeaderClass(); 
});