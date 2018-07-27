class EditSet {
    constructor() {
        var self = this;

        $(document).on("click", "div.QuestionText a[data-action=open-details]", function (e) {
           
            self.ExpandSetRow(e, $(this));
        });
        $(document).on("click", "div.QuestionText a[data-action=close-details]",function(e) {
            self.CollapseSetRow(e, $(this));
        });

        $("#ulQuestions input[data-input=video-timecode]").on("click",function() {
            self.SaveTimeCode($(this));
        });
    }

    ExpandSetRow(e : JQueryEventObject, elem : JQuery) {
        e.preventDefault();

        elem.hide();
        elem.parent().css('max-height', 'none');
        elem.parent().find("a[data-action=close-details]").show();
    }

    CollapseSetRow(e: JQueryEventObject, elem: JQuery) {
        e.preventDefault();

        elem.hide();
        elem.parent().css('max-height', '32px');
        elem.parent().find("a[data-action=open-details]").show();
    }

    SaveTimeCode(elem: JQuery) {
        var timeCode = elem.val();
        var questionInSetId = elem.attr("data-in-set-id");

        $.post("/SetVideo/SaveTimeCode/", { timeCode: timeCode, questionInSetId: questionInSetId } );
    }

    static GetSetId() : number { 
        return parseInt($("#questionSetId").attr("data-id"));
    }
}

$(() => {
    new EditSet();
})