
class AnswerBodyLoader {

    private _isInLearningTab: boolean;
    private _sessionConfigDataJson: SessionConfigDataJson;
    private _getCustomSession: boolean = false;
    private _isSkipStep: boolean = false;


    constructor(answerBody: AnswerBody) {
        this._isInLearningTab = $('#LearningTab').length > 0;
        $(() => {
             if ($("#hddIsLearningSession").val() === "True") {

                if ($("#hddIsLearningSession").attr("data-learning-session-id") == "-1") {
                    $("#hddIsLearningSession").attr("data-learning-session-id", "-2");
                    this.loadNewSession();
                }

                var self = this; 
                $("#btnNext, #aSkipStep").click(function(e) {
                    e.preventDefault();
                    var skipstepIdx = this.id === "btnNext" ? -1 : 1;
                    self._isSkipStep = skipstepIdx !== -1;

                    var url = "/AnswerQuestion/RenderAnswerBodyByLearningSession/" +
                        "?skipStepIdx=" + skipstepIdx;
                    AnswerQuestion.LogTimeForQuestionView();
                    self.loadNewQuestion(url);
                });

            }else {
                $("#NextQuestionLink, #btnNext").click((e) => {
                    e.preventDefault();
                    var pager = $("#NextQuestionLink").attr("href").split("?")[1].split("=")[1];
                    var primaryDataUrl = "/AnswerQuestion/RenderAnswerBodyByNextQuestion/?pager=" + pager;
                    AnswerQuestion.LogTimeForQuestionView();
                    this.loadNewQuestion(primaryDataUrl);
                });

                $("#PreviousQuestionLink").click((e) => {
                    e.preventDefault();
                    var pager = $("#PreviousQuestionLink").attr("href").split("?")[1].split("=")[1];
                    var primaryDataUrl = "/AnswerQuestion/RenderAnswerBodyByPreviousQuestion/?pager=" + pager;
                    AnswerQuestion.LogTimeForQuestionView();
                    this.loadNewQuestion(primaryDataUrl);
                });
            }

            if (Modernizr.history) {
                $(window).off("popstate");
                window.onpopstate = (event) => {
                    if ((event.state != null) && (event.state.stateType === "BrowserNavigation"))
                        location.reload();
                };
            }
        });
    }

    public loadNewTestSession() {
        this.loadNewSession();
    }

    public loadNewSession(questionFilter = null, loadedFromVue = false, continueWithNewSession = false) {

            this._sessionConfigDataJson = {
                categoryId: $('#hhdCategoryId').val(),
                isInLearningTab: this._isInLearningTab,
                currentUserId: $("#hddUserId").val(),
                maxQuestionCount: questionFilter != null ? questionFilter.maxQuestionCount : -1,
                minProbability: questionFilter != null ? questionFilter.minProbability : 0,
                maxProbability: questionFilter != null ? questionFilter.maxProbability : 100,
                inWishknowledge: questionFilter != null ? questionFilter.inWishknowledge : false,
                questionOrder: questionFilter != null ? questionFilter.questionOrder : -1,
                isInTestMode: questionFilter != null ? questionFilter.isInTestMode : false,
                isNotInWishKnowledge: questionFilter != null ? questionFilter.isNotQuestionInWishKnowledge : false,
                allQuestions: questionFilter != null ? questionFilter.allQuestions : false,
                createdByCurrentUser: questionFilter != null ? questionFilter.createdByCurrentUser : false,
                safeLearningSessionOptions: questionFilter != null ? questionFilter.safeLearningSessionOptions : false,
                answerHelp: questionFilter != null ? questionFilter.answerHelp : true,
                repititions: questionFilter != null ? questionFilter.repititions : true,
                randomQuestions: questionFilter != null ? questionFilter.randomQuestions : false
            }

        var url = "/AnswerQuestion/RenderNewAnswerBodySessionForCategory";
        this._getCustomSession = true;
        this.loadNewQuestion(url, loadedFromVue, continueWithNewSession);
    }

    public loadNewQuestion(url: string, loadedFromVue: boolean = false, continueWithNewSession: boolean = false) {
        this._isInLearningTab = $('#LearningTab').length > 0;
        if (this._getCustomSession)
            $("#TestSessionHeader").remove();

        if (this._isInLearningTab && this._getCustomSession && loadedFromVue) {
            $("#AnswerBody").fadeOut();
            $("#QuestionDetails").fadeOut();
            $(".FooterQuestionDetails").fadeOut();
            $(".SessionSessionHeading").fadeOut();
            $(".SessionBar").fadeOut();
            $("#spanPercentageDone").html(0 + "%");
            $("#progressPercentageDone").width(0 + "%");
            Utils.ShowSpinner();
            $('html, body').animate({ scrollTop: 0 }, 'fast');
        }
        $.ajax({
            url: url,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(this._sessionConfigDataJson),
            type: 'POST',
            headers: { "cache-control": "no-cache" },
            success: result => {
                eventBus.$emit('suicide');
                result = JSON.parse(result);

                if (!this._isInLearningTab) {
                    this.updateUrl(result.url);
                }

                if (result.LearningSessionResult) {
                    this.showLearningSessionResult(result);
                    $(".ProgressBarSegment .ProgressBarLegend").hide();
                    return;
                }
                $(".FooterQuestionDetails").remove();
                $("#AnswerBody").replaceWith(result.answerBodyAsHtml);
                if (this._isInLearningTab && !this._getCustomSession) {
                    $("#QuestionDetails").empty();
                }

                if ($("#hddIsLearningSession").val() !== "True") 
                    this.updateNavigationBar(result.navBarData);
                else
                    this.updateSessionHeader(result.sessionData);
              
                this.updateMenu(result.menuHtml);
                document.title = $(".QuestionText").html();
                $("div#comments").replaceWith(result.commentsAsHtml);

                new AnswerBody();
                FillSparklineTotals();
                InitTooltips();
                Images.Init();
                InitClickLog("div#LicenseQuestion");
                InitClickLog("div#AnswerBody");
                InitClickLog("div#AnswerQuestionPager");
                InitClickLog("div#answerQuestionDetails");
                InitClickLog("div#comments");
                PreventDropdonwnsFromBeingHorizontallyOffscreen("div#AnswerBody");
                Utils.HideSpinner();
                if (this._getCustomSession)
                    this._getCustomSession = false;
                if ($("div[data-div-type='questionDetails']").length > 1)
                    $("div[data-div-type='questionDetails']").last().remove();
                if ($("div[data-div-type='testSessionHeader']").length > 1)
                    $("div[data-div-type='testSessionHeader']").slice(1).remove();

                if (continueWithNewSession) {
                    $(".SessionSessionHeading").fadeIn();
                    $(".SessionBar").fadeIn();
                    $("#QuestionListApp").fadeIn();
                }
                if (loadedFromVue) {
                    $(".SessionSessionHeading").fadeIn();
                    $(".SessionBar").fadeIn();
                    $("#AnswerBody").fadeIn();
                    $("#QuestionDetails").fadeIn();
                    $(".FooterQuestionDetails").fadeIn();
                }
            }
        });
    }

    private showLearningSessionResult(result) {
        var container = this._isInLearningTab || $("#AnswerBody").length > 0 ? $('#AnswerBody') : $("#MasterMainWrapper");
        container.html(result.LearningSessionResult);
        new LearningSessionResult();
    }

    private updateUrl(url: string) {
        if(Modernizr.history)
            history.pushState({stateType: "BrowserNavigation"}, $(".QuestionText").html(), url);
    }

    private updateMenu(newMenuHtml: string) {
        if(newMenuHtml)
            $("#mainMenuThemeNavigation").replaceWith($(newMenuHtml));
    }

    private updateSessionHeader(sessionStepData) {
        if ($("#hddIsLearningSession").val() === "True") {
            $("#hddIsLearningSession").attr("data-current-step-idx", sessionStepData.currentStepIdx);
            $("#hddIsLearningSession").attr("data-skip-step-index", sessionStepData.skipStepIdx);
            $("#hddIsLearningSession").attr("data-is-last-step", sessionStepData.isLastStep);
            $(".SessionBar .QuestionCount").html(sessionStepData.currentSessionHeader);
        }
    }

    private updateNavigationBar(navBarData: any) {
        $("#AnswerQuestionPager .Current").replaceWith($(navBarData.currentHtml).find(".Current"));

        if (navBarData.nextUrl) {
            if ($("#NextQuestionLink").length === 0)
                $("#AnswerQuestionPager .Next").append($(navBarData.currentHtml).find("#NextQuestionLink"));
            else
                $("#NextQuestionLink").attr("href", navBarData.nextUrl);
        } else {
            $("#NextQuestionLink").remove();
        }

        if (navBarData.previousUrl) {
            if ($("#PreviousQuestionLink").length === 0)
                $("#AnswerQuestionPager .Previous").append($(navBarData.currentHtml).find("#PreviousQuestionLink"));
            else
                $("#PreviousQuestionLink").attr("href", navBarData.previousUrl);
        } else {
            $("#PreviousQuestionLink").remove();
        }

        $("#NextQuestionLink, #PreviousQuestionLink").unbind();
    }
}