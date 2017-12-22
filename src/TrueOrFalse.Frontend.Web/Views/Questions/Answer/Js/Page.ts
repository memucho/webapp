/// <reference path="../../../../scripts/typescript.defs/lib.d.ts" />
class PageInit {
    constructor() {
        var questionId = $("#hddQuestionId").val();

        var answerEntry = new AnswerEntry();
        answerEntry.Init();

        new AsyncLoading();

        new Pin(PinType.Question);
        new Pin(PinType.Set); //only needed if Set-Cards are presented as content
        new Pin(PinType.Category); //only needed if category catd is presented (e.g. as primary category for unregistered users)

        if ($("#hddIsLearningSessionOnCategoryPage").val() == "true")
            new Pin(PinType.Category, KnowledgeBar.ReloadCategory);

        $('#hddTimeRecords').attr('data-time-on-load', $.now());

        $(window).unload(function () {
            AnswerQuestion.LogTimeForQuestionView();
        });

        new QuestionRowDelete(QuestionRowDeleteSourcePage.QuestionDetail);
        new ShareQuestion(questionId);

        $('[data-toggle=popover]').popover({ html: true }).click(e => { e.preventDefault(); });

        if (!Utils.IsInWidget()) {

            this.ScrollToAnswerQuestionHeaderIfOutsideView();

            //set focus to first possible answer element
            if (document.getElementsByName("answer").length > 0)
                $("[name=answer]")[0].focus();

            $("#txtAnswer:visible").focus();

            $("#row-1:visible").focus();
        }
    }

    ScrollToAnswerQuestionHeaderIfOutsideView() {
        var answerQuestionHeader = $('.SessionHeading, .AnswerQuestionHeader');
        var answerQuestionHeaderTop = answerQuestionHeader.offset().top;
        var scrollY = window.scrollY;

        if (answerQuestionHeader.length > 0 && scrollY > answerQuestionHeaderTop) {
            window.scrollTo(0, answerQuestionHeaderTop);
        }
    }
}

$(() => {
    if ($('#LearningTabContent').length == 0) {
        new PageInit();
    }
});