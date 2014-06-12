﻿function escape_regexp(text) {
    return text.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
}

$.expr[':'].textEquals = function (a, i, m) {
    return $(a).text().match(new RegExp("^" + escape_regexp(m[3]) + "$", "i")) != null;
};


$(function () {

    var isCategoryEdit = $("#isCategoryEdit").length == 1;
    var categoryName = "";
    if (isCategoryEdit)
        categoryName = $("#Name").val();

    var nextCatId = 1;
    function addCat() {
        var catId = nextCatId;
        nextCatId++;
        var catText = $("#txtNewRelatedCategory").val();
        $("#CatInputContainer").before(
            "<div class='added-cat' id='cat-" + catId + "' style='display: none;'>" +
                "<a href='/Kategorien/" + catText + "/" + catId + "'>" + catText + "</a>" +
                "<input type='hidden' value='" + catText + "' name='cat-" + catId + "'/>" +
                "<a href='#' id='delete-cat-" + catId + "'><img alt='' src='/Images/Buttons/cross.png' /></a>" +
            "</div> ");
        $("#txtNewRelatedCategory").val('');
        $("#delete-cat-" + catId).click(function (e) {
            e.preventDefault();
            animating = true;
            $("#cat-" + catId).stop(true).animate({ opacity: 0 }, 250, function () {
                $(this).hide("blind", { direction: "horizontal" }, function () {
                    $(this).remove();
                    animating = false;
                });
            });
        });
        $("#cat-" + catId).show("blind", { direction: "horizontal" });
    }

    $("#txtNewRelatedCategory").autocomplete({
        minLength: 0,
        source: '/Api/Category/ByName',
        focus: function (event, ui) {
            $("#txtNewRelatedCategory").data("category-id", ui.item.id);
            $("#txtNewRelatedCategory").val(ui.item.name);
            return false;
        },
        select: function (event, ui) {
            $("#txtNewRelatedCategory").data("category-id", ui.item.id);
            $("#txtNewRelatedCategory").val(ui.item.name);
            
            if ($(".added-cat:textEquals('" + ui.item.name + "')").length > 0) {
                return false;
            }

            addCat();
            return false;
        }
    }).data("ui-autocomplete")._renderItem = function (ul, item) {
        console.log(categoryName + " " + item.name);
            if (isCategoryEdit &&categoryName == item.name)
                return "";

		    return $("<li></li>")
				.data("ui-autocomplete-item", item)
				.append("<a class='CatListItem'><img src='" + item.imageUrl + "'/><div class='CatDescription'><span class='cat-name'>"
				    + item.name + "</span><span class='NumberQuestions'>(" + item.numberOfQuestions + " Fragen)</span></div></a>")
				.appendTo(ul);
		};

    var animating = false;
    function checkText() {
        var text = $("#txtNewRelatedCategory").val();
        var matchesInAutomcompleteList = $(".ui-autocomplete li .cat-name:textEquals('" + text + "')");
        var alreadyAddedCategory = $(".added-cat:textEquals('" + text + "')");

        if (matchesInAutomcompleteList.size() != 0 && alreadyAddedCategory.size() == 0) {
            if ($("#txtNewRelatedCategory").val() != matchesInAutomcompleteList.text()) {
                $("#txtNewRelatedCategory").val(matchesInAutomcompleteList.text());
            }
        }

        if (!animating && alreadyAddedCategory.size() != 0) {
            animating = true;
            alreadyAddedCategory.effect('bounce', null, 'fast', function () { animating = false; });
        }
        setTimeout(checkText, 250);
    }
    checkText();


    var fnCheckTextAndAdd = function (event) {
        checkText();
        if (event.keyCode == 13 && $(".added-cat:textEquals('" + ui.item.name + "')").length == 0) {
            addCat();
        }

        if (event.keyCode == 13) {
            event.preventDefault();
        }
    }
    $("#txtNewRelatedCategory").keydown(fnCheckTextAndAdd);

    $("#txtNewRelatedCategory").bind("initCategoryFromTxt", addCat);
});