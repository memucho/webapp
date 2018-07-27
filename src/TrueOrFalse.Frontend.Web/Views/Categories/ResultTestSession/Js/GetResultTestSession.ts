class GetResultTestSession {

    testSessionId: string ; 
    constructor() {

        var link = "/TestSessionResult/TestSessionResultAsync";
        $("#btnNext").on("click", function (e) {
            if (Utils.IsInWidget()) {
                return;
            }
            e.preventDefault();
            this.testSessionId = $("#hddIsTestSession").attr("data-test-session-id").valueOf();
            $.ajax({
                type: "POST",
                url: link,
                cache: false,
                data: { testSessionIdString: this.testSessionId },
                dataType: "html",
                success: function (data) {
                    $("#AnswerBody").html(data);
                    $("#QuestionCountCompletSideBar").remove();
                    new TestSessionResult();

                }

            });
        });
    }
}

