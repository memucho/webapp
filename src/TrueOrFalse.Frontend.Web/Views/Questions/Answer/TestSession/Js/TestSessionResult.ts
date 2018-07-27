class TestSessionResult {

    constructor() {
        var self = this;
      

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

        if ($("#isSetSession").val() !== "True" && $("#LearningTab").hasClass("active")) {

        $(".nextLearningTestSession").click((e) => {
            e.preventDefault();
            $(".EduPartnerWrapper").remove();
            $("#QuestionCountCompletSideBar").fadeIn();
            $(".ProgressBarSegment .ProgressBarLeft").width("0%");
            var answerBody = new AnswerBody();
            answerBody.Loader.loadNewTestSession();
            });
        }
    }

    public PositionIndicatorAverageText() {
        let availableWidth : number = $("#divIndicatorAverageWrapper").width();
        let textFrameWidth : number = $("#divIndicatorAverageText").width();
        let idealMiddlePosition: number = (+($("#avgPercentageCorrect").html()) / 100 * availableWidth);
        let marginLeft : string;
        if ((textFrameWidth / 2) > idealMiddlePosition) {
            $("#divIndicatorAverageText").css("margin-left", 0);
            $("#divIndicatorAverageText").css("text-align", "left");
            return;
        }
        if ((availableWidth - idealMiddlePosition) < (textFrameWidth / 2)) {
            marginLeft = (availableWidth - textFrameWidth) + "px";
            $("#divIndicatorAverageText").css("margin-left", marginLeft);
            $("#divIndicatorAverageText").css("text-align", "right");
            return;
        }
        marginLeft = (idealMiddlePosition - (textFrameWidth / 2)) + "px";
        $("#divIndicatorAverageText").css("margin-left", marginLeft);
        $("#divIndicatorAverageText").css("text-align", "center");
    }
}