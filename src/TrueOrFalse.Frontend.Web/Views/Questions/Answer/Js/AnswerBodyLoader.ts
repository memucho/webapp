class AnswerBodyLoader {

    private _answerBody: AnswerBody;
    private _isInLearningTab: boolean;

    constructor(answerBody: AnswerBody) {

        this._answerBody = answerBody;

        if (Utils.IsInWidget())
            return;

        $().ready(() => {
            this._isInLearningTab = $('#LearningTab').length > 0;

            if (window.location.pathname.split("/")[4] === "im-Fragesatz") {
                $("#NextQuestionLink, #btnNext").click((e) => {
                    e.preventDefault();
                    var NextQuestionLinkArgs = $("#NextQuestionLink").attr("href").split("/");
                    var setId = NextQuestionLinkArgs[5];
                    var questionId = NextQuestionLinkArgs[3];
                    var primaryDataUrl = "/AnswerQuestion/RenderAnswerBodyBySet/?questionId=" +
                        questionId +
                        "&setId=" +
                        setId;
                    this.loadNewQuestion(primaryDataUrl);
                });

                $("#PreviousQuestionLink").click((e) => {
                    e.preventDefault();
                    var NextQuestionLinkArgs = $("#PreviousQuestionLink").attr("href").split("/");
                    var setId = NextQuestionLinkArgs[5];
                    var questionId = NextQuestionLinkArgs[3];
                    var primaryDataUrl = "/AnswerQuestion/RenderAnswerBodyBySet/?questionId=" +
                        questionId +
                        "&setId=" +
                        setId;
                    this.loadNewQuestion(primaryDataUrl);
                });

            } else if ($("#hddIsLearningSession").val() === "True") {

                if ($("#hddIsLearningSession").attr("data-learning-session-id") == "-1")
                    this.loadNewLearningSession();

                $("#btnNext, #aSkipStep").click((e) => {
                    
                    e.preventDefault();
                    var learningSessionId = $("#hddIsLearningSession").attr("data-learning-session-id");
                    var skipStepIdx = $("#hddIsLearningSession").attr("data-skip-step-index");
                    var url = "/AnswerQuestion/RenderAnswerBodyByLearningSession/?learningSessionId=" +
                        learningSessionId +
                        "&skipStepIdx=" +
                        skipStepIdx;
                    this.loadNewQuestion(url);
                });

            } else if (this._answerBody.IsTestSession()) {

                $("#btnNext").click((e) => {
                    e.preventDefault();
                    var testSessionId = $("#hddIsTestSession").attr("data-test-session-id");
                    var url = "/AnswerQuestion/RenderAnswerBodyByTestSession/?testSessionId=" + testSessionId;
                    this.loadNewQuestion(url);
                });

            } else {
                $("#NextQuestionLink, #btnNext").click((e) => {
                    e.preventDefault();
                    var pager = $("#NextQuestionLink").attr("href").split("?")[1].split("=")[1];
                    var primaryDataUrl = "/AnswerQuestion/RenderAnswerBodyByNextQuestion/?pager=" + pager;
                    this.loadNewQuestion(primaryDataUrl);
                });

                $("#PreviousQuestionLink").click((e) => {
                    e.preventDefault();
                    var pager = $("#PreviousQuestionLink").attr("href").split("?")[1].split("=")[1];
                    var primaryDataUrl = "/AnswerQuestion/RenderAnswerBodyByPreviousQuestion/?pager=" + pager;
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
        var url = "/AnswerQuestion/RenderAnswerBodyForNewCategoryTestSession/?categoryId=" + $('#hddCategoryId').val();
        this.loadNewQuestion(url);
    }

    public loadNewLearningSession() {
        var url = "/AnswerQuestion/RenderAnswerBodyForNewCategoryLearningSession/?categoryId=" + $('#hddCategoryId').val();
        this.loadNewQuestion(url);
    }

    public loadNewQuestion(url: string) {
        $.ajax({
            url: url,
            type: 'POST',
            headers: { "cache-control": "no-cache" },
            success: result => {
                result = JSON.parse(result);
                if (!this._isInLearningTab) {
                    this.updateUrl(result.url);
                }
                this.sendGoogleAnalyticsPageView(result.offlineDevelopment);
                if (result.LearningSessionResult) {
                    this.showLearningSessionResult(result);
                    $(".ProgressBarSegment .ProgressBarLegend").hide();
                    return;
                }
                $("div#LicenseQuestion").remove();
                $("#AnswerBody").replaceWith(result.answerBodyAsHtml);

                if ($("#hddIsLearningSession").val() === "True" || this._answerBody.IsTestSession()) {
                    this.updateSessionHeader(result.sessionData);

                    if (result.sessionData.learningSessionId > -1)
                        $("#hddIsLearningSession").attr("data-learning-session-id", result.sessionData.learningSessionId);
                    if (result.sessionData.testSessionId > -1)
                        $("#hddIsTestSession").attr("data-test-session-id", result.sessionData.testSessionId);
                }
                else 
                    this.updateNavigationBar(result.navBarData);

                this.updateMenu(result.menuHtml);
                document.title = $(".QuestionText").html();
                $("div#answerQuestionDetails").replaceWith(result.questionDetailsAsHtml);
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
            }
        });
    }

    private updateNavigationBar(navBarData: any) {
        $("#AnswerQuestionPager .Current").replaceWith($(navBarData.currentHtml).find(".Current"));

        if (navBarData.nextUrl) {
            if($("#NextQuestionLink").length === 0)
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

    private showLearningSessionResult(result) {
        var container = this._isInLearningTab ? $('#AnswerBody') : $("#MasterMainWrapper");
        container.html(result.LearningSessionResult);
        new LearningSessionResult();
    }

    private updateSessionHeader(sessionStepData) {
        if ($("#hddIsTestSession").val() === "True") {
            $("#hddIsTestSession").attr("data-current-step-idx", sessionStepData.currentStepIdx);
            $("#hddIsTestSession").attr("data-is-last-step", sessionStepData.isLastStep);
            $(".SessionBar .QuestionCount").html(sessionStepData.currentSessionHeader);
        }
        else if ($("#hddIsLearningSession").val() === "True") {
            $("#hddIsLearningSession").attr("data-current-step-guid", sessionStepData.currentStepGuid);
            $("#hddIsLearningSession").attr("data-current-step-idx", sessionStepData.currentStepIdx);
            $("#hddIsLearningSession").attr("data-skip-step-index", sessionStepData.skipStepIdx);
            $("#hddIsLearningSession").attr("data-is-last-step", sessionStepData.isLastStep);
            $(".SessionBar .QuestionCount").html(sessionStepData.currentSessionHeader);
        }   
    }

    private updateUrl(url: string) {
        if(Modernizr.history)
            history.pushState({stateType: "BrowserNavigation"}, $(".QuestionText").html(), url);
    }

    private updateMenu(newMenuHtml: string) {
        if(newMenuHtml)
            $("#mainMenuThemeNavigation").replaceWith($(newMenuHtml));
    }

    private sendGoogleAnalyticsPageView(offlineDevelopment: boolean) {
        if (!offlineDevelopment)
            if (typeof ga !== 'undefined')
                ga('send', 'pageview');    
            
    }
}