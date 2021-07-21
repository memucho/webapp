enum QuestionRowDeleteSourcePage {
    QuestionRow,
    QuestionDetail
} 

class QuestionRowDelete {

    private _isInLearningTab: boolean;

    constructor(sourcePage: QuestionRowDeleteSourcePage) {

        this._isInLearningTab = $('#LearningTab').length > 0;

        var questionIdToDelete;
        $(() => {
            $('a[href*=#modalDeleteQuestion]').click(function (e) {
                questionIdToDelete = $(this).attr("data-questionId");
                populateDeleteQuestionId(questionIdToDelete);
                e.preventDefault();
            });

            $('#btnCloseQuestionDelete').click(function (e) {
                $('#modalDeleteQuestion').modal('hide');
                e.preventDefault();
            });

            $('#confirmQuestionDelete').click(function(e) {
                e.preventDefault();
                $("#questionDeleteResult").html("Die Frage wird gelöscht... Bitte habe einen Moment Geduld.");
                $("#questionDeleteResult").show();
                window.setTimeout(function() {
                    deleteQuestion(questionIdToDelete, sourcePage);
                }, 10);
            });
        });

        function populateDeleteQuestionId(questionId) {
            $("#questionDeleteResult").hide();
            $("#questionDeleteResult").html("");
            $("#questionDeleteResult").removeClass("alert-danger");
            $.ajax({
                type: 'POST',
                url: "/Question/DeleteDetails/" + questionId,
                cache: false,
                success: function (result) {
                    if (result.canNotBeDeleted) {
                        $("#questionDeleteCanDelete").hide();
                        $("#questionDeleteCanNotDelete").show();
                        $("#confirmQuestionDelete").hide();
                        $("#questionDeleteCanNotDelete").html(result.canNotBeDeletedReason);
                        $("#btnCloseQuestionDelete").html("Schließen");
                    } else {
                        $("#questionDeleteCanDelete").show();
                        $("#questionDeleteCanNotDelete").hide();
                        $("#confirmQuestionDelete").show();
                        $("#spanQuestionTitle").html(result.questionTitle.toString());
                        $("#btnCloseQuestionDelete").html("Abbrechen");
                    }
                    
                },
                error: function (e) {
                    console.log(e);
                    window.alert("Ein Fehler ist aufgetreten");
                    $("#confirmQuestionDelete").hide();
                    $("#btnCloseQuestionDelete").html("Schließen");
                }
            });
        }

        function deleteQuestion(questionId, sourcePage: QuestionRowDeleteSourcePage) {
            $.ajax({
                type: 'POST',
                async: false,
                url: "/Question/Delete/" + questionId,
                cache: false,
                success: function (e) {
                    $('#modalDeleteQuestion').modal('hide');
                    if (this._isInLearningTab)
                        window.location.reload();
                    else {
                        if (sourcePage == QuestionRowDeleteSourcePage.QuestionRow)
                            window.location.reload();
                        if (sourcePage == QuestionRowDeleteSourcePage.QuestionDetail)
                            window.location.href = "/Fragen/Meine";
                    }
                },
                error: function (e) {
                    $("#confirmQuestionDelete").hide();
                    $("#questionDeleteResult").removeClass("alert-info");
                    $("#questionDeleteResult").addClass("alert-danger");
                    $("#questionDeleteResult").html("Es ist ein Fehler aufgetreten! Möglicherweise sind Referenzen auf die Frage (Lernsitzungen, Termine, Wunschwissen-Einträge...) teilweise gelöscht.");

                }
            });
        }
        
    }
}

