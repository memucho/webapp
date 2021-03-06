﻿fnAddRegExMethod("IsbnChar", /(^[0-9][-]{0,1}){0,}[xX0-9]$/, "Bitte verwende nur Ziffern und Bindestriche ('X' am Ende möglich).");
fnAddRegExMethod("IsbnLength", /(^(([a-z0-9][-]{0,1}){12})[a-z0-9]$)|(^(([a-z0-9][-]{0,1}){9})[a-z0-9]$)/, "Die ISBN muss genau 10 oder 13 Stellen haben (ohne Bindestriche).");
fnAddRegExMethod("IsbnAll", /(^(([0-9][-]{0,1}){12})[xX0-9]$)|(^(([0-9][-]{0,1}){9})[xX0-9]$)/, "Fies! Dieses Feld hat strenge Regeln: <br/> Verwende nur Ziffern und Bindestriche ('X' am Ende möglich).<br/> Die ISBN muss genau 10 oder 13 Stellen haben (ohne Bindestriche).");
fnAddRegExMethod("Issn", /(^(([0-9][-]{0,1}){7})[xX0-9]$)/, "Fies! Dieses Feld hat strenge Regeln: <br/> Verwende nur Ziffern und Bindestriche ('X' am Ende möglich).<br/> Die ISSN muss genau 8 Stellen haben (ohne Bindestriche).");
fnAddAllOrNothingMethod("DateAllOrNothing", "Bitte trage das Datum vollständig ein.");

var fnEditCatValidation = function (categoryType, resetValidator = false, usePartialCustomSettings = false) {

    //for (var i in validator.settings.rules) {
    //    delete validator.settings.rules[i];
    //}

    var validationBasicSettings = {
        //debug: true,
        rules: {
            Name: {
                required: true,
            },
            Title: {
                required: true,
            },
            TitleVolume: {
                required: true,
            },
            Author: {
                required: true,
            },
            ISBN: {
                //IsbnChar: true,
                //IsbnLength: true,
                IsbnAll: true,
            },
            ISSN: {
                //IsbnChar: true,
                //IsbnLength: true,
                Issn: true,
            },
            No: {
                digits: true,
            },
            PublicationYear: {
                minlength: 4,
                maxlength: 4,
            },
            PublicationDateYear: {
                digits: true,
                minlength: 4,
                maxlength: 4,
                DateAllOrNothing: true,
            },
            PublicationDateMonth: {
                digits: true,
                minlength: 2,
                maxlength: 2,
                range: [1, 12],
                DateAllOrNothing: true,
            },
            PublicationDateDay: {
                digits: true,
                minlength: 2,
                maxlength: 2,
                range: [1, 31],
                DateAllOrNothing: true,
            },
            Url: {
                //url: true, too strict
            },
            WikipediaUrl: {
                //url: true, too strict
            },
        },
        messages: {
            Name: {
            
            },
            PublicationYear: {
                minlength: "Bitte gib das Jahr vierstellig an.",
                maxlength: "Bitte gib das Jahr vierstellig an.",
            },
            PublicationDateYear: {
                minlength: "Bitte gib das Jahr vierstellig an.",
                maxlength: "Bitte gib das Jahr vierstellig an.",
            },
            PublicationDateMonth: {
                range: "Bitte gib für den Monat einen Wert zwischen 01 und 12 an.",
                minlength: "Bitte gib den Monat zweistellig an.",
                maxlength: "Bitte gib den Monat zweistellig an.",
            },
            PublicationDateDay: {
                range: "Bitte gib für den Tag einen Wert zwischen 01 und 31 an.",
                minlength: "Bitte gib den Tag zweistellig an.",
                maxlength: "Bitte gib den Tag zweistellig an.",
            },
        },
    }

    var validator = fnValidateForm("#EditCategoryForm", validationBasicSettings, resetValidator);
    
    //Further custom settings for partials:

    if (usePartialCustomSettings) {

       if (categoryType == "DailyArticle") {
            $('[name="Author"]').rules("add", { required: false, });
        }

        if (categoryType == "DailyIssue") {
            $('[name="PublicationDateDay"]').rules("add", { required: true, });
            $('[name="PublicationDateMonth"]').rules("add", { required: true, });
            $('[name="PublicationDateYear"]').rules("add", { required: true, });
            validator.groups['PublicationDateDay'] = 'DateGroup';//http://stackoverflow.com/questions/2150268/jquery-validate-plugin-how-can-i-add-groups-to-a-validator-after-its-been-initi#answer-9688284
            validator.groups['PublicationDateMonth'] = 'DateGroup';
            validator.groups['PublicationDateYear'] = 'DateGroup';
        }

        if (categoryType == "MagazineArticle") {
            $('[name="Author"]').rules("add", { required: false, });
            $('[name="Title"]').rules("add", { required: true, });
        }

        if (categoryType == "MagazineIssue") {
            $('[name="PublicationDateYear"]').rules("add", { required: true, });
            $('[name="No"]').rules("add", { required: true, });
            $('[name="Title"]').rules("add", { required: false, });

            validator.groups['PublicationDateDay'] = 'DateGroup';//http://stackoverflow.com/questions/2150268/jquery-validate-plugin-how-can-i-add-groups-to-a-validator-after-its-been-initi#answer-9688284
            validator.groups['PublicationDateMonth'] = 'DateGroup';
            validator.groups['No'] = 'IssueGroup';
            validator.groups['PublicationDateYear'] = 'IssueGroup';
        }

        if (categoryType == "VolumeChapter") {
            $('[name="TitleVolume"]').rules("add", { required: true, });
            $('[name="Editor"]').rules("add", { required: true, });
        }

        if (categoryType == "WebsiteArticle") {
            $('[name="Author"]').rules("add", { required: false, });
            $('[name="Url"]').rules("add", { required: true, });
            //$('[name="PublicationDateYear"]').rules("add", { AllOrNothing: true, });
            validator.groups['PublicationDateDay'] = 'DateGroup';//http://stackoverflow.com/questions/2150268/jquery-validate-plugin-how-can-i-add-groups-to-a-validator-after-its-been-initi#answer-9688284
            validator.groups['PublicationDateMonth'] = 'DateGroup';
            validator.groups['PublicationDateYear'] = 'DateGroup';
        }
    }
}
