class LearningSessionResult {
    
    constructor() {
        
        $("#QuestionDetails").remove();

        if ($("#LearningTab").hasClass("active")) {
            $(".SessionBar").hide();
            $("#QuestionListApp").hide();
            $(".SessionHeading").hide();
        }

        $("[data-action=showAllDetails]").click((e) => {
            e.preventDefault();
            $(".answerDetails").show(300);
        });
        $("[data-action=hideAllDetails]").click((e) => {
            e.preventDefault();
            $(".answerDetails").hide(300);
        });

        $("[data-action=showDetailsExceptRightAnswer]").click((e) => {
            e.preventDefault();
            $(".answerDetails").not(".AnsweredRight .answerDetails").show(300);
        });

        $("[data-action=showAnswerDetails]").click(function(e) {
            e.preventDefault();
            $(this).siblings(".answerDetails").toggle(300);
        });

        $("[data-action=toggleDateSets]").click((e) => {
            e.preventDefault();
            $(".dateSets").toggle(300);
        });

        $(".nextLearningTestSession").click((e) => {
            e.preventDefault();
            eventBus.$emit('update-selected-page', 1);
            $(".EduPartnerWrapper").remove();
            $("#QuestionCountCompletSideBar").fadeIn(); 
            var answerBody = new AnswerBody();
            answerBody.Loader.loadNewSession(null, false, true);
            $("#progressPercentageDone").width("0%");
            $("#spanPercentageDone").text("0%");
            $(".ProgressBarSegment .ProgressBarLegend").show();
            Utils.ShowSpinner();
        });

        new LearningSessionResultCharts();
    }
} 