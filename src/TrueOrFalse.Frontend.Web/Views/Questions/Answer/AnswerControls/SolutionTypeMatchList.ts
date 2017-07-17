class SolutionTypeMatchList
    extends AnswerEntryBase
    implements IAnswerEntry
{
    public SolutionType = SolutionType.MatchList;
    constructor(answerEntry: AnswerEntry) {
        super(answerEntry);

        //get pager from url parameters
        var isMobile = false;
        if ($(document).width() < 780)
            isMobile = true;
        var isCurrentAnswerBodyMobile = false;
        if ($('#matchlist-mobilepairs').length)
            isCurrentAnswerBodyMobile = true;
        if (isMobile !== isCurrentAnswerBodyMobile) {
            var url = "/AnswerQuestion/RenderAnswerBody";

            var isTestSession = $('#hddIsTestSession').val();
            if(isTestSession !== undefined)
                if (isTestSession.toLowerCase() === "true")
                    var testSessionId = parseInt($('#hddIsTestSession').attr('data-test-session-id'));

            var isLearningSession = $('#hddIsLearningSession').val();
            if (isLearningSession !== undefined)
                if (isLearningSession.toLowerCase() === "true")
                    var learningSessionId = $("#hddIsLearningSession").attr("data-learning-session-id");

            var isVideo = $("#hhdHasVideo").val() ? $("#hhdHasVideo").val().toLowerCase() === "true" : false;


            var urlParams = Utils.GetQueryString();

            jQuery.ajax({
                url: url,
                data: {
                    questionId: $("#questionId").val(),
                    pager: urlParams.pager,
                    isMobileDevice: isMobile,
                    testSessionId: testSessionId,
                    learningSessionId: learningSessionId,
                    isVideo: isVideo
                },
                success: htmlResult => {
                    $("div#LicenseQuestion").remove();
                    $("#AnswerBody")
                        .replaceWith(htmlResult);
                },
                async: false
            });
        }
        this.AnswerQuestion = new AnswerQuestion(this);
    }

    static GetChosenAnswers(): string {
        if ($('#matchlist-mobilepairs').length) {
            var answerRowsMobile: Pair[] = [];
            $('#matchlist-mobilepairs .matchlist-mobilepairrow')
                .each((index, element) => {
                    var leftPairValueMobile = $('.matchlist-mobilepairrow #matchlist-elementlabel-' + index).html();
                    var rightPairValueMobile = $('.matchlist-mobilepairrow #matchlist-select-' + index).val();
                    if (rightPairValueMobile === "Keine Zuordnung") {
                        return;
                    }
                    answerRowsMobile.push(new Pair());
                        answerRowsMobile[answerRowsMobile.length - 1].ElementLeft = new ElementLeft();
                        answerRowsMobile[answerRowsMobile.length - 1].ElementLeft.Text = leftPairValueMobile;
                        answerRowsMobile[answerRowsMobile.length - 1].ElementRight = new ElementRight();
                        answerRowsMobile[answerRowsMobile.length - 1].ElementRight.Text = rightPairValueMobile;
                });
            return JSON.stringify(answerRowsMobile);
        } else {
            var answerRows: Pair[] = [];
            var answerCount = $("#matchlist-pairs [id*='leftElementResponse-']")
                .each((index, elem) => {
                    var rightPairValue = $("#rightElementResponse-" + $(elem).attr('id').split("-")[1]).attr('name');
                    var leftPairValue = $(elem).attr('name');
                    answerRows.push(new Pair());
                    answerRows[index].ElementLeft = new ElementLeft();
                    answerRows[index].ElementLeft.Text = leftPairValue;
                    answerRows[index].ElementRight = new ElementRight();
                    answerRows[index].ElementRight.Text = rightPairValue;
                });
            return JSON.stringify(answerRows);
        }
    }

    GetAnswerText(): string {
        return "falsche Antwort";
    }

    GetAnswerData(): {} {
        return { answer: '{ "Pairs": ' + SolutionTypeMatchList.GetChosenAnswers() + '}'};
    }
}

class Pair {
    ElementLeft: ElementLeft;
    ElementRight: ElementRight;
}

class ElementLeft {
    Text: string;
}

class ElementRight {
    Text: string;
}