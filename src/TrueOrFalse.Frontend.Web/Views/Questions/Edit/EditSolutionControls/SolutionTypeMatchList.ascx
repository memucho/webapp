<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMatchList>" %>

<div class="col-sm-12" id="matchlist">
    <div id="pairs"></div>
    <div id="matchlist-button" class="row">
        <button type="button" class="btn col-sm-6" id="addPair">Paar hinzufügen</button>
        <button type="button" class="btn col-sm-4" id="responseModalButton" data-toggle="modal" data-target="#responseModal">Rechte Elemente hinzufügen</button>
    </div>
    <div id="responseModal" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Rechte Elemente</h4>
                </div>
                <div class="modal-body">
                    <div id="responseModalContent"></div>
                    <button type="button" class="btn" id="addRightPairElement">Weiteres Element hinzufügen</button>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Speichern</button>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="checkbox" id="matchlist-solutionOrderCheck">
            <label>
                <input name="isSolutionRandomlyOrdered" type="checkbox" value="" checked>Paare zufällig anordnen</label>
        </div>
    </div>
</div>

<script type="text/javascript">
    var addingElementRightId = $("#responseModal .form-control").length;
    var rightElementsExisting;
    var removeButton = $("");
    var defaultRightElement = $("");

    function getRemoveButton() {
        var removeButton = $("<a href='#' class='removeButton btn'><i class='fa fa-times'></i></a>");
        removeButton.click(function (event) {
            event.preventDefault();
            $(this).closest(".form-group").hide(500, function () { $(this).remove(); });
        });
        return removeButton;
    }

    function htmlEncodeSpaces(value) {
        return value.split(" ").join("&nbsp;");
    }

    function checkForHint() {
        if (rightElementsExisting === true) {
            $(".matchlist-rightpairelement")
                .each(function(selectElementIndex, selectElement) {
                    $(selectElement)
                        .children()
                        .each(function(optionElementIndex, optionElement) {
                            if ($(optionElement).val() === "Erst rechte Elemente erstellen") {
                                $(optionElement).html("Keine Zuordnung");
                            }
                        });
                    rightElementsExisting = false;
                });
        }
    }

    $("#addPair").click(function () {

        var addingPairElementId = $("#pairs .matchlist-leftelement").length;

        if (addingPairElementId !== 0)
            removeButton = $(getRemoveButton());

        $("#pairs").append($("<div class = 'form-inline form-group'>")
            .append($("<input type='text' name = 'LeftElement-" + addingPairElementId + "' class='matchlist-leftelement form-control'>"))
                .append($("<i class='matchlist-arrow fa fa-arrow-right fa-1x'></i>"))
                    .append($("<select class='matchlist-rightpairelement form-control' name='RightPairElement-" + addingPairElementId + "'>")
                        .append($(".matchlist-rightpairelement").last().children().clone()))
                            .append(removeButton));
        if (addingPairElementId === 0)
            $("[name='RightPairElement-0']").append($('<option selected="selected">').html("Keine Zuordnung"));
    });


    $("#addRightPairElement").click(function () {
        var matchlistRightElementInput = $("<input type='text' id='pairElementRight-" + addingElementRightId + "'name = 'RightElement-" + addingElementRightId + "' class='matchlist-rightelement form-control'>");
        var rightElementRemoveButton = getRemoveButton();
        $("#responseModalContent").append($("<div class='form-group form-inline matchlist-modalinput'>")
            .append(matchlistRightElementInput)
            .append(rightElementRemoveButton));

        matchlistRightElementInput.focus(function () { PreviousRightElementValue = this.value; }).
            change(function () {
                checkForHint();
                var rightElementValue = $(this).val();
                var rightElementId = $(this).attr('id');
                $(".matchlist-rightpairelement").each(function (selectElementIndex, selectElement) {
                    var hasValueChangedElement = ($(selectElement).val() === PreviousRightElementValue);
                    $(selectElement).children().each(function (optionElementIndex, optionElement) {
                        if ($(optionElement).attr('name') === rightElementId) {
                            $(optionElement).remove();
                        }
                    });
                    $(selectElement).append($('<option>').attr('name', rightElementId).html(htmlEncodeSpaces(rightElementValue)));
                    if (hasValueChangedElement)
                        $(selectElement).val(htmlEncodeSpaces(rightElementValue));
                });
                rightElementRemoveButton.click(function () {
                    $(".matchlist-rightpairelement").each(function (index, selectElement) {
                        $(selectElement).children().each(function (optionElementIndex, optionElement) {
                            if ($(optionElement).attr('name') === rightElementId)
                                $(optionElement).remove();
                        });
                    });
                });
            });
        addingElementRightId++;
    });

    <% if (Model != null)
    {
        Model.EscapeSolutionChars();
        Model.TrimElementTexts();

        for (int i = 0; i < Model.RightElements.Count; i++)
        { %>
            $("#addRightPairElement").click();
            $(".matchlist-rightelement").last().val('<%= Model.RightElements[i].Text %>');
        <% }
    for (int i = 0; i < Model.Pairs.Count; i++)
    { %>
        $("#addPair").click();
    <% if (i == 0)
    {
        foreach (var rightElement in Model.RightElements)
        { %>
    var elementLeftId = $('[id*="pairElementRight-"]').filter(function () { return this.value == '<%= rightElement.Text %>' }).attr('id');
            $(".matchlist-rightpairelement").last().append($('<option name ="' + elementLeftId + '">').html(htmlEncodeSpaces('<%= rightElement.Text %>')));
     <% }   
    } %>
        $(".matchlist-leftelement").last().val('<%= Model.Pairs[i].ElementLeft.Text %>');
        $(".matchlist-rightpairelement").last().val('<%= Model.Pairs[i].ElementRight.Text %>');
    <% }

    if (Model.IsSolutionOrdered)
    { %>
        $('[name="isSolutionRandomlyOrdered"]').prop("checked", false);
    <% }

    }
    else
    { %>
        $("#addRightPairElement").click();

        $("#addPair").click();
    $("[name='RightPairElement-0']").children().first().remove();
        $("[name='RightPairElement-0']").append($('<option selected="selected">').html("Erst rechte Elemente erstellen"));
        rightElementsExisting = true;
    <% } %>
</script>
