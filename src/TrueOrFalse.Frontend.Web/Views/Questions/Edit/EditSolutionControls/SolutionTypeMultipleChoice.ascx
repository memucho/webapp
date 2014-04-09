﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMultipleChoice>" %>


<div id="choices"></div>
<div class="form-group">
    <div class="xxs-stack col-xs-offset-2 col-xs-10 ButtonContainer">
        <button class="btn" id="addChoice">Antwort hinzufügen</button>
    </div>
</div>

<script type="text/javascript">
    var addingChoiceId = 1;
    $("#addChoice").click(function () {
        $("#choices").append("<div class='form-group'><div class='xxs-stack col-xs-offset-2 col-xs-10'><input type='text' class='sequence-choice form-control' name='choice-" + addingChoiceId + "' /></div></div>");
        addingChoiceId++;
        return false;
    });
<% if(Model != null) foreach (var choice in Model.Choices) { %>
    $("#addChoice").click();
    $(".sequence-choice").last().val('<%:choice %>');
<% } %>
</script>
