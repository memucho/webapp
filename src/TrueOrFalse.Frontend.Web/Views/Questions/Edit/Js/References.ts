﻿class ReferenceJson {
    CategoryId: number;
    ReferenceId: number;
    ReferenceType: string;
    AdditionalText: string;
    ReferenceText : string;
}

class Reference {
    FilterType = AutoCompleteFilterType.None;
    LabelText = "";
    SearchFieldPlaceholder = "";
}

class ReferenceBook extends Reference {

    FilterType = AutoCompleteFilterType.Book;
    LabelText = "Buch suchen";
    SearchFieldPlaceholder = "Suche nach Buchtitel oder ISBN";
}

class ReferenceArticle extends Reference {

    FilterType = AutoCompleteFilterType.Article;
    LabelText = "Artikel suchen";
    SearchFieldPlaceholder = "Suche nach Titel / Zeitschrift / Zeitung";
}

class ReferenceVolumeChapter extends Reference {

    FilterType = AutoCompleteFilterType.VolumeChapter;
    LabelText = "Beitrag in Sammelband suchen";
    SearchFieldPlaceholder = "Suche nach Titel oder Autor";
}

class ReferenceWebsiteArticle extends Reference {

    FilterType = AutoCompleteFilterType.WebsiteArticle;
    LabelText = "Online-Artikel suchen";
    SearchFieldPlaceholder = "Suche nach Titel oder Autor";
}

//enum ReferenceType {
//    MediaCategoryReference = 1,
//    FreeTextReference = 2,
//    UrlReference = 3,
//}

class ReferenceUi
{
    constructor() {
        $("#AddReference").click((e) => {
            e.preventDefault();

            $('#JS-ReferenceSearch').show();
            $('#AddReferenceControls').hide();

            $("#ReferenceType").change(() => {

                var referenceSearchType = $('#ReferenceType option:selected').attr('value');

                if(referenceSearchType == "Book")
                    this.AddReferenceSearch(new ReferenceBook());
                if(referenceSearchType == "Article")
                    this.AddReferenceSearch(new ReferenceArticle());
                if(referenceSearchType == "VolumeChapter")
                    this.AddReferenceSearch(new ReferenceVolumeChapter());
                if(referenceSearchType == "WebsiteArticle")
                    this.AddReferenceSearch(new ReferenceWebsiteArticle());
                if(referenceSearchType == "FreeText")
                    this.AddFreetextReference();
                if(referenceSearchType == "Url")
                    this.AddUrlReference();
            });

            $("#ReferenceType").trigger('change');

        });

        $('#JS-HideReferenceSearch').click((e) => {
            e.preventDefault();
            $('#JS-ReferenceSearch').hide();
            $('#AddReferenceControls').show();
        });
    }

    public AddReferenceSearch(reference: Reference) {
        $('#AddFreeTextReference, #AddUrlReference').hide();
        $('#ReferenceSearchInput').closest('.JS-CatInputContainer').show();
        $('#ReferenceSearchInput').attr('placeholder', reference.SearchFieldPlaceholder).data('referenceType', 'MediaCategoryReference');
        new AutocompleteCategories(
            "#ReferenceSearchInput",
            true,
            reference.FilterType,
            "",
            true
        );
    }

    public AddFreetextReference() {
        $('#ReferenceSearchInput').closest('.JS-CatInputContainer').hide();
        $('#AddUrlReference').hide();
        $('#AddFreeTextReference').show();
    }

    public AddUrlReference() {
        $('#ReferenceSearchInput').closest('.JS-CatInputContainer').hide();
        $('#AddFreeTextReference').hide();
        $('#AddUrlReference').show();
    }

    public static ReferenceToJson() : string {

        var jsonReferences: ReferenceJson[] = $('.JS-ReferenceContainer:not(#JS-ReferenceSearch)').map(function (idx, elem): ReferenceJson {
            var elemJ = $(elem);
            var result = new ReferenceJson();

            result.CategoryId = parseInt(elemJ.attr("data-cat-id"));
            result.ReferenceId = parseInt(elemJ.attr("data-ref-id"));
            result.ReferenceType = elemJ.attr("data-ref-type");
            result.AdditionalText = elemJ.find("[name^='AdditionalInfo']").val();
            result.ReferenceText = elemJ.find("[name^='ReferenceText']").val();

            return result;
        }).toArray();

        return JSON.stringify(jsonReferences);
    }
}

class OnSelectForReference implements IAutocompleteOnSelect {
    
    OnSelect(autocomplete: AutocompleteCategories, referenceId: number, referenceType: string) {
        var existingReferences = $('.JS-ReferenceContainer:not(#JS-ReferenceSearch)');
        var refIdxes = new Array;
        for (var i = 0; i < existingReferences.length; i++) {
            refIdxes.push(parseInt($(existingReferences[i]).attr('data-ref-idx')));
        }
        var nextRefIdx = 1;
        if (existingReferences.length != 0) {
            nextRefIdx = Math.max.apply(Math, refIdxes) + 1;
        }
        $(
            "<div id='Ref-" + nextRefIdx + "' " +
                    "class='JS-ReferenceContainer well'" +
                    "data-ref-idx='" + nextRefIdx + "'" +
                    "data-ref-id='" + autocomplete._referenceId + "'" + 
                    "data-ref-type='" + referenceType + "'" + 
                    "data-cat-id='" + autocomplete._catId + "'>" + 
            "<a id='delete-ref-" + nextRefIdx + "' class='close show-tooltip' href ='#' data-toggle='tooltip' title = 'Quellenangabe löschen' data-placement = 'top'>×</a>" +
            "</div>").insertBefore('#JS-ReferenceSearch');
        $("#delete-ref-" + nextRefIdx).click(function (e) {
            e.preventDefault();
            $("#delete-ref-" + nextRefIdx).closest('.JS-ReferenceContainer').remove();
            $(window).trigger('referencesChanged');
        });

        autocomplete._elemInput.val("");
        $('#JS-ReferenceSearch').hide();
        $('#AddReferenceControls').show();

        if (referenceType == "MediaCategoryReference") {
            $.ajax({
                url: '/Fragen/Bearbeite/ReferencePartial?catId=' + autocomplete._catId,
                type: 'GET',
                success: function (data) {
                    $('#Ref-' + nextRefIdx)
                        .append(data)
                        .append(
                        "<div class='form-group' style='margin-bottom: 0;'>" +
                        "<label class='columnLabel control-label' for='AdditionalInfo-" + nextRefIdx + "'>Ergänzungen zur Quelle</label>" +
                        "<div class='columnControlsFull'>" +
                        "<input class='InputRefAddition form-control input-sm' name='AdditionalInfo-" + nextRefIdx + "' type='text' placeholder='Seitenangaben, Zugriffsdatum etc.'/>" +
                        "</div>" +
                        "</div>");

                    $(window).trigger('referenceAdded' + referenceId);
                    $(window).trigger('referencesChanged');
                    $('.show-tooltip').tooltip();
                }
            });
        } else { /* No Category */

            var fnInitReferenceTextValidation = function() {
                $('.ReferenceText').each(function () {//neccessary if several elements by one selector
                    $(this).rules('add', {
                        required: true,
                        messages: {
                            required: "Bitte fülle dieses Pflichtfeld aus (oder lösche diese Quelle)."
                        }
                    });
                });
            }

            if (referenceType == "FreeTextReference") {
                $('#Ref-' + nextRefIdx)
                    .append(
                    "<div class='form-group' style='margin-bottom: 0;'>" +
                    "<label class='RequiredField columnLabel control-label' for='ReferenceText-" + nextRefIdx + "'>Freitextquelle</label>" +
                    "<div class='columnControlsFull'>" +
                    "<textarea class='ReferenceText form-control input-sm' name='ReferenceText-" + nextRefIdx + "' type='text' placeholder='Quellenangabe'></textarea>" +
                    "</div>" +
                    "</div>");
                fnInitReferenceTextValidation();

            }
            if (referenceType == "UrlReference") {
                $('#Ref-' + nextRefIdx)
                    .append(
                    "<div class='form-group' style='margin-bottom: 0;'>" +
                        "<label class='RequiredField columnLabel control-label' for='ReferenceText-" + nextRefIdx + "'>Url</label>" +
                        "<div class='columnControlsFull'>" +
                            "<input class='ReferenceText form-control input-sm' name='ReferenceText-" + nextRefIdx + "' type='text' placeholder='Bitte hier nur die Url eingeben'/>" +
                            "<a href='#' id='TestLink-" + nextRefIdx + "' style='display: none;' target='_blank'>Link testen (in neuem Tab öffnen)</a>" +
                        "</div>" +
                    "</div>"+
                    "<div class='form-group' style='margin-bottom: 0;'>" +
                    "<label class='columnLabel control-label' for='AdditionalInfo-" + nextRefIdx + "'>Ergänzungen zur Quelle</label>" +
                        "<div class='columnControlsFull'>" +
                            //"<input class='UrlReference form-control input-sm' name='AdditionalInfo-" + nextRefIdx + "' type='text' placeholder='Zugriffsdatum etc.'/>" +
                            "<textarea class='AdditionalInfo form-control input-sm' name='AdditionalInfo-" + nextRefIdx + "' type='text' placeholder='Zugriffsdatum etc.'></textarea>" +
                        "</div>" +
                    "</div>");
                fnInitReferenceTextValidation();

                var inputReferenceText = $('[name=ReferenceText-' + nextRefIdx + ']');
                inputReferenceText.bind('input blur', function (e) {
                    if ($(this).val() == "") {
                        $('#TestLink-' + nextRefIdx).hide();
                    } else if (e.type == "blur") {
                        var urlValue = inputReferenceText.val();
                        if (inputReferenceText.val().substring(0, 7) != "http://" && inputReferenceText.val().substring(0, 8) != "https://") {
                            urlValue = "http://" + urlValue;
                        }
                        inputReferenceText.val(urlValue);
                        $('#TestLink-' + nextRefIdx).show().attr('href', urlValue);
                    }
                });
            }
            $('#ReferenceSearchInput').data('referenceType', '');
            $(window).trigger('referenceAdded' + referenceId);
            $(window).trigger('referencesChanged');
            $('.show-tooltip').tooltip();
        }
    }
}

$(function () {
    new ReferenceUi();
    $('#AddFreeTextReference button').click(function(e) {
        e.preventDefault();
        $("#ReferenceSearchInput")
            .data('category-id', '-1')
            .data('referenceType', 'FreeTextReference')
            .trigger('initCategoryFromTxt');
    });
    $('#AddUrlReference button').click(function (e) {
        e.preventDefault();
        $("#ReferenceSearchInput")
            .data('category-id', '-1')
            .data('referenceType', 'UrlReference')
            .trigger('initCategoryFromTxt');
    });
});