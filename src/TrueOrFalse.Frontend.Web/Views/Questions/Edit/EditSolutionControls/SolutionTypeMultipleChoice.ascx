<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMultipleChoice>" %>

<div id="choices">
</div>

<div class="form-group">
    <div class="noLabel columnControlsFull ButtonContainer">
        <button class="btn" id="addChoice">weitere mögliche Antwort hinzufügen</button>
    </div>
</div>
<div class="checkbox" id="multiplechoice-solutionOrderCheck">
    <label><input name="isSolutionRandomlyOrdered" type="checkbox" value="" checked>Antwortmöglichkeiten zufällig anordnen</label>
</div>


<script type="text/javascript">
    var addingChoiceId = $("#choices .form-group").length;
    $("#addChoice").click(function () {
        var actionButton = $("");
        if (addingChoiceId != 0) {
            actionButton = $("<span class='CloseButton input-group-btn'><a href='#' class='btn'><i class='fa fa-times'></i></a></span>");
            actionButton.click(function () {
                $(this).closest(".form-group").hide(500, function () { $(this).remove(); });
                return false;
            });
        }
        $("#choices")
            .append($("<div class='form-group row'>")
                .append($("<div class='noLabel'>")
                    .append($("<div class='col-xs-11 col-sm-4'>").append($("<select class='sequence-answertype form-control' name='choice_correct-" + addingChoiceId + "'><option>Richtige Antwort</option><option selected='selected'>Falsche Antwort</option></select>")),
                            $("<div class='col-xs-11 col-sm-7'>").append($("<input type='text' class='sequence-choice form-control' name='choice-" + addingChoiceId + "' />")),
                            $("<div class='col-xs-1'>").append(actionButton)
                    )
                )
            );

        addingChoiceId++;
        return false;

    });
    <% if (Model != null)
       {
           foreach (var choice in Model.Choices)
           { %>
                $("#addChoice").click();
                $(".sequence-choice").last().val('<%= choice.Text %>');
                $(".sequence-answertype").last().val('<%= choice.IsCorrect ? "Richtige Antwort" : "Falsche Antwort" %>');
            <% }

            if (Model.IsSolutionOrdered)
            { %>
                $('[name="isSolutionRandomlyOrdered"]').prop("checked", false);
            <% }
    }
    else
    { %>
            $("#addChoice").click();
            $("select[name = choice_correct-0]").val("Richtige Antwort");
            $("#addChoice").click();
        <% } %>
</script>
