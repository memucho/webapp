class AnswerQuestionUserFeedback {

    private _errMsgs = ["Wer einen Fehler gemacht hat und ihn nicht korrigiert, begeht einen zweiten. (Konfuzius)",
        "Es ist ein großer Vorteil im Leben, die Fehler, aus denen man lernen kann, möglichst früh zu begehen. (Churchill)",
        "Weiter, weiter, nicht aufgeben.",
        "Übung macht den Meister. Du bist auf dem richtigen Weg.",
        "Ein ausgeglichener Mensch ist einer, der denselben Fehler zweimal machen kann, ohne nervös zu werden." //Nur Zeigen, wenn der Fehler tatsächlich wiederholt wurde.
    ];

    private _successMsgs = ["Yeah!", "Du bist auf einem guten Weg.", "Sauber!", "Well done!", "Toll!", "Weiter so!", "Genau.", "Absolut.",
                            "Richtiger wird's nicht.", "Fehlerlos!", "Korrrrrekt!", "Einwandfrei", "Mehr davon!", "Klasse.", "Schubidu!",
                            "Wer sagt's denn!", "Exakt.", "So ist es.", "Da kannste nicht meckern.", "Sieht gut aus.", "Oha!", "Rrrrrrichtig!"];

    private _answerQuestion: AnswerQuestion;

    constructor(answerQuestion: AnswerQuestion) {
        this._answerQuestion = answerQuestion;
    }

    public ShowErrorGame() {
        $("#divWrongAnswerPlay").show();
        $("#buttons-first-try").hide();
        $("#buttons-answer-again").hide();

        $("#divWrongEnteredAnswer").html(this._answerQuestion.AnswersSoFar[0]);

        this.AnimateWrongAnswer();
    }

    public ShowError(text = "", forceShow: boolean = false) {

        if (text === "") {
            text = this._errMsgs[Utils.Random(0, this._errMsgs.length - 1)];
        }

        this.UpdateAnswersSoFar();

        if (this._answerQuestion.SolutionType === SolutionType.FlashCard) {
            //MARK:Julian
        } else {
            $("#divWrongAnswer").show();
            $("#buttons-first-try").hide();
            $("#buttons-answer-again").hide();
        }

        if (forceShow || Utils.Random(1, 10) % 4 === 0) {
            $("#answerFeedbackTry").html(text).show();
        } else {
            $("#answerFeedbackTry").html(text).hide();
        }

        this.AnimateWrongAnswer();
    }

    AnimateWrongAnswer() {
        $("#buttons-answer-again").show();
        $(".answerFeedbackWrong").fadeIn(1200, function() {
             $(this).fadeOut(800);
        });
    }
    AnimateCorrectAnswer() {
        $(".answerFeedbackCorrect").fadeIn(1200, function () {
            $(this).fadeOut(800);
        });
    }

    ShowSuccess() {
        var self = this;

        $("#divAnsweredCorrect").show();
        $("#buttons-next-question").show();
        this.AnimateCorrectAnswer();
        $("#divWrongAnswer").hide();

        $("#divAnsweredCorrect").show();
        $("#wellDoneMsg").html("" + self._successMsgs[Utils.Random(0, self._successMsgs.length - 1)]).show();
    }

    ShowSolution() {

        this.ShowNextQuestionLink();
        if (this._answerQuestion.SolutionType !== SolutionType.MatchList &&
            this._answerQuestion.SolutionType !== SolutionType.MultipleChoice &&
            this._answerQuestion.SolutionType !== SolutionType.FlashCard) {
            $("#txtAnswer").attr('disabled', 'true').addClass('disabled');
            if (this._answerQuestion.AnswersSoFar.length === 1) {
                $("#divWrongAnswers .WrongAnswersHeading").html('Deine Antwort:');
                if ($("#txtAnswer").val() !== this._answerQuestion.AnswersSoFar[0]) {
                    $("#divWrongAnswers").show();
                }
            }
            if (this._answerQuestion.AnswersSoFar.length > 1) {
                $("#divWrongAnswers .WrongAnswersHeading").html('Deine Antworten:');
                $("#divWrongAnswers").show();
            }
        }
        this.RenderSolutionDetails();
    }

    RenderSolutionDetails() {
        $('#AnswerInputSection').find('.radio').addClass('disabled').find('input').attr('disabled', 'true');
        $('#AnswerInputSection').find('.checkbox').addClass('disabled').find('input').attr('disabled', 'true');
        if (this._answerQuestion.SolutionType === SolutionType.MatchList) {
            $('#AnswerInputSection .ui-droppable')
                .each((index, matchlistDropElement) => {
                    $(matchlistDropElement).droppable('disable');
                });
            $('#AnswerInputSection .ui-draggable')
                .each((index, matchlistDragElement) => {
                    $(matchlistDragElement).draggable('disable');
                });
            $(".matchlist-mobileselect").addClass('disabled').attr('disabled', 'true');
        }

        $('#Buttons').css('visibility', 'hidden');
        window.setTimeout(() => { $("#SolutionDetailsSpinner").show(); }, 500);

        AnswerQuestion.AjaxGetSolution(result => {

            if (this._answerQuestion.IsTestSession && this._answerQuestion.AnswersSoFar.length === 0) {
                //if solution is shown after answering the question in a TestSession, then this does not need to be registered
                //otherwise, solutionview needs to be registered in the current TestSessionStep
                $.ajax({
                    type: 'POST',
                    url: AnswerQuestion.ajaxUrl_TestSessionRegisterAnsweredQuestion,
                    data: {
                        testSessionId: AnswerQuestion.TestSessionId,
                        questionId: AnswerQuestion.GetQuestionId(),
                        questionViewGuid: $('#hddQuestionViewGuid').val(),
                        answeredQuestion: false
                    },
                    cache: false
                });

                this._answerQuestion.UpdateProgressBar(this._answerQuestion.GetCurrentStep());

                AnswerQuestionUserFeedback.IfLastTestQuestionChangeBtnNextToResult();
            }

            if (this._answerQuestion.IsLearningSession && this._answerQuestion.AnswersSoFar.length === 0) {
                //if is learningSession and user asked to show solution before answering, then queue this question to be answered again
                var self = this;
                this._answerQuestion.ShowedSolutionOnly = true;
                $.ajax({
                    type: 'POST',
                    url: AnswerQuestion.ajaxUrl_LearningSessionAmendAfterShowSolution,
                    data: {
                        learningSessionId: this._answerQuestion.LearningSessionId,
                        stepGuid: this._answerQuestion.LearningSessionStepGuid
                    },
                    cache: false,
                    success(result) {
                        if (self._answerQuestion._isLastLearningStep && !result.newStepAdded) {
                            $('#btnNext').html('Zum Ergebnis');
                        }
                        self._answerQuestion.UpdateProgressBar(result.numberSteps);
                    }
                });
            }


            if (this._answerQuestion.SolutionType === SolutionType.MultipleChoice && !result.correctAnswer) {
                $("#Solution").show().find('.Label').html("Keine der Antworten ist richtig!");
            } else {
                var shownCorrectAnswer = result.correctAnswerAsHTML;
                $("#Solution").show().find('.Content').html("</br>" + shownCorrectAnswer);
            }
            if (this._answerQuestion.SolutionType === SolutionType.MultipleChoice || this._answerQuestion.SolutionType === SolutionType.MultipleChoice_SingleSolution)
                this.HighlightMultipleChoiceSolution(result.correctAnswer);
            if (this._answerQuestion.SolutionType === SolutionType.MatchList)
                this.HighlightMatchlistSoluion(result.correctAnswer);
            
            if (result.correctAnswerDesc) {
                $("#Description").show().find('.Content').html(result.correctAnswerDesc);
            }
            if (result.correctAnswerReferences.length > 0) {
                $("#References").show();
                var indexSuccessfulReferences = 0;
                $(window).on('oneMoreReference', () => {
                    indexSuccessfulReferences++;
                    if (indexSuccessfulReferences === result.correctAnswerReferences.length) {
                        this.ShowAnswerDetails();
                    }
                    if (Utils.IsInWidget()) {
                        this.AddTargetBlankToReferenceLinks();
                    } 
                });
                for (var i = 0; i < result.correctAnswerReferences.length; i++) {
                    var reference = result.correctAnswerReferences[i];
                    var referenceHtml = $('<div class="ReferenceDetails"></div>');
                    referenceHtml.appendTo('#References .Content');

                    var fnRenderReference = function (div, ref) {
                        if (ref.referenceText) {
                            if (ref.referenceType == 'UrlReference') {
                                $('<div class="ReferenceText"><a href="' + ref.referenceText + '" target="_blank">' + ref.referenceText + '</a></div>').appendTo(div);
                            } else
                                $('<div class="ReferenceText">' + ref.referenceText + '</div>').appendTo(div);
                        }
                        if (ref.additionalInfo) {
                            $('<div class="AdditionalInfo">').append(ref.additionalInfo).appendTo(div);
                        }
                    }

                    var fnAjaxCall = function (div, ref) {
                        $.ajax({
                            url: '/Fragen/Bearbeite/ReferencePartial?catId=' + ref.categoryId,
                            type: 'GET',
                            success: function (data) {
                                div.prepend(data);
                                fnRenderReference(div, ref);

                                $('.show-tooltip').tooltip();
                                $(window).trigger('oneMoreReference');
                            }
                        });
                    }

                    if (reference.categoryId != -1) {
                        fnAjaxCall(referenceHtml, reference);
                    } else {
                        fnRenderReference(referenceHtml, reference);
                        $(window).trigger('oneMoreReference');
                    }
                }
            } else {
                this.ShowAnswerDetails();
            }

        });
    }

    static IfLastTestQuestionChangeBtnNextToResult() {
        if (AnswerQuestion.IsLastTestSessionStep) {
            $('#btnNext').html('Zum Ergebnis');
            $("#btnNext").unbind();
        }
    }

    private HighlightMultipleChoiceSolution(correctAnswers: string) {
        var allAnswerElements = $("input[name = 'answer']");
        var correctAnswerArray = correctAnswers.split('</br>');

        for (var i = 0; i < allAnswerElements.length; i++) {
            var currentElement = $(allAnswerElements.get(i));

            if(correctAnswerArray.indexOf(currentElement.val()) !== -1) {
                currentElement.parent().addClass("right-answer");
            } else {
                if (currentElement.prop("checked")) {
                    currentElement.parent().addClass("wrong-answer");      
                }
            }
        }
    }

    HighlightMatchlistSoluion(correctAnswers: string) {
        var correctAnswersArray = correctAnswers.split("%pairseperator%").join("%elementseperator%").split("%elementseperator%");
        correctAnswersArray.splice(0, 1).splice(correctAnswersArray.length - 1, 1);
        var leftElements = [];
        var rightElements = [];
        for (var i = 0; i < correctAnswersArray.length; i++) {
            if (i % 2 !== 0)
                rightElements.push(correctAnswersArray[i]);
            else
                leftElements.push(correctAnswersArray[i]);
        }
        if ($("#matchlist-mobilepairs").length) {
            $(".matchlist-mobilepairrow")
                .each((index, elem) => {
                    var correctRightElementValue = rightElements[$.inArray($(elem).find(".matchlist-elementlabel").html(), leftElements)];
                    if (correctRightElementValue === $(elem).find(".matchlist-mobileselect").val()) {
                        $(elem).find(".matchlist-mobileselect").addClass("right-answer");
                    } else {
                        $(elem).find(".matchlist-mobileselect").addClass("wrong-answer");
                    }
                });
        }
        if ($("#matchlist-pairs").length) {
            $('.matchlist-droppable')
                .each((index, element) => {
                    var correctRightElementValue;
                    if ($(element).attr('id') == null || $(element).attr('id') === "") {
                        correctRightElementValue = rightElements[$.inArray($(element).attr('name'), leftElements)];
                        if (correctRightElementValue === "Keine Zuordnung")
                            $(element).parent().addClass("right-answer");
                        else
                            $(element).parent().addClass("wrong-answer");
                    } else {
                        correctRightElementValue = rightElements[$.inArray($(element).attr('name'), leftElements)];
                        var choosenRightElement = $('#rightElementResponse-' + $(element).attr('id').split("-")[1]);
                        if (choosenRightElement.html() === correctRightElementValue) {
                            $(element).parent().addClass("right-answer");
                            choosenRightElement.addClass("right-answer");
                        } else {
                            $(element).parent().addClass("wrong-answer");
                            choosenRightElement.addClass("wrong-answer");
                        }
                    }
                });
        }
    }

    ShowAnswerDetails() {
        window.setTimeout(() => {
            $("#SolutionDetailsSpinner").remove();
            $("#SolutionDetails").show();
            $('#Buttons').css('visibility', 'visible');
            $('#hddTimeRecords').attr('data-time-of-solution-view', $.now());
        }, 50);
    }

    UpdateAnswersSoFar() {

        var errorTryText;
        var amountOfTriesText = ["0 Versuche", "ein Versuch", "zwei", "drei", "vier", "fünf", "sehr hartnäckig", "Respekt!"];

        switch (this._answerQuestion.AmountOfTries) {
            case 0:
            case 1:
                errorTryText = amountOfTriesText[this._answerQuestion.AmountOfTries]; break;
            case 2:
            case 3:
            case 4:
            case 5:
                errorTryText = amountOfTriesText[this._answerQuestion.AmountOfTries] + " Versuche"; break;
            case 6:
            case 7:
                errorTryText = amountOfTriesText[this._answerQuestion.AmountOfTries]; break;
            default:
                errorTryText = amountOfTriesText[7];
        }
        $("#CountWrongAnswers").html("(" + errorTryText + ")");

        $('#ulAnswerHistory').html("");

        $.each(this._answerQuestion.AnswersSoFar, function (index, val) {
            $('#ulAnswerHistory').append(
                $('<li>' + val + '</li>'));
        });
    }

    private ShowNextQuestionLink() {

        $("#buttons-next-question").show();

        if (!this._answerQuestion.AnsweredCorrectly &&
            !this._answerQuestion.IsGameMode) {
            $("#aCountAsCorrect").show();
        }

        $("#answerFeedbackTry").hide();

        $("#buttons-first-try").hide();
        $("#buttons-answer-again").hide();
    }

    private AddTargetBlankToReferenceLinks() {
        $("#References").find("a").attr("target", "_blank");
    }
} 