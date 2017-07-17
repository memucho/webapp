<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMatchList>" %>
<div class="row">
    <div class="col-sm-12">
        <div class="row">
            <div class="col-sm-12">
                <div id="matchlist-pairs" class="row"></div>
            </div>
            <div class="col-sm-12">
                <div class="row">
                    <div id="matchlist-rightelements"></div>
                </div>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    <% Model.EscapeSolutionChars();
    Model.TrimElementTexts();

    var localPairs = Model.Pairs;
    var random = new Random();
    if (!Model.isSolutionOrdered)
        localPairs = Model.Pairs.OrderBy(x => random.Next()).ToList();

    foreach (var pair in localPairs)
    { %>
    var rightDropElement = $("<div class='col-sm-6'>").append($("<div class='matchlist-droppable'>").attr("name", "<%= pair.ElementLeft.Text %>").droppable({
        accept: '.matchlist-rightelement',
        hoverClass: 'matchlist-hovered',    
        drop: handleElementDrop
    }));
    $("div#matchlist-pairs").append($("<div class='col-sm-12'>").append($("<div class = 'row matchlist-pairrow'>")
        .append($("<div class='col-sm-5 matchlist-leftelementwrapper'>").append($("<span class= 'matchlist-leftelement'><%= pair.ElementLeft.Text %></span>")))
        .append($("<div class='col-sm-1'>").append($("<i class=' matchlist-arrow fa fa-arrow-right fa-1x'>")))
        .append(rightDropElement)));
    <% }

    foreach (var elementRight in Model.RightElements.OrderBy(x => random.Next()))
    { %>
    var rightDragElement = $("<span class='matchlist-rightelement'>").attr("name", "<%= elementRight.Text %>")
        .html("<%= elementRight.Text %>")
        .draggable({
            containment: '#AnswerBody',
            stack: '.matchlist-rightelement',
            cursor: 'move',
            helper: 'clone',
            start: function (event, ui) {
                $(ui.helper).css('z-index', 100);
            },
            revert: 'true'
        });
    var droppableWidth = $('.matchlist-droppable').first().width() - 4;
    rightDragElement.width(droppableWidth);
    $("#matchlist-rightelements").append(rightDragElement);
    <% } %>

    var answerCount = 0;
    function handleElementDrop(event, ui) {
        ui.helper.data('dropped', true);

        if ($(this).attr('id') !== undefined)
            if ($('#rightElementResponse-' + $(this).attr('id').split("-")[1]).attr("id") !== $(ui.draggable).attr("id"))
                $('#rightElementResponse-' + $(this).attr('id').split("-")[1]).remove();

        if (ui.draggable.hasClass('helper-clone')) {
            $(this).append(ui.draggable);
            $(ui.draggable).addClass("matchlist-rightelement-dropped");
            $(this).addClass('matchlist-droppable-dropped');

            $(ui.draggable).width('');

            var oldElementLeftId = 'leftElementResponse-' + ui.draggable.attr('id').split("-")[1];
            $('#' + oldElementLeftId).removeAttr('id');
            $(this).attr('id', oldElementLeftId);

        } else {
            var helperClone = ui.helper.clone();
            helperClone.draggable({
                containment: '#AnswerBody',
                stack: ".matchlist-rightelement",
                cursor: 'move',
                start: function (event, ui) {
                    ui.helper.data('dropped', false);
                    $(this).removeClass('matchlist-rightelement-dropped');

                    var oldElementLeftId = 'leftElementResponse-' + $(this).attr('id').split('-')[1];
                    var oldElementLeftWidth = $('#' + oldElementLeftId).width();
                    $(this).width(oldElementLeftWidth);

                    $('#' + oldElementLeftId).removeClass('matchlist-droppable-dropped');

                },
                stop: function (event, ui) {
                    if (!ui.helper.data('dropped')) {
                        $('#leftElementResponse-' + $(this).attr('id').split("-")[1]).removeAttr('id');
                        $(this).remove();
                    }
                }
            });
            helperClone.addClass('helper-clone');
            $(this).append(helperClone);
            helperClone.addClass('matchlist-rightelement-dropped');
            $(this).addClass('matchlist-droppable-dropped');
            $(helperClone).width('');

            if ($(this).attr('id') !== undefined) {
                helperClone.attr('id', 'rightElementResponse-' + $(this).attr('id').split("-")[1]);
            } else {
                helperClone.attr('id', 'rightElementResponse-' + answerCount);
                $(this).attr('id', 'leftElementResponse-' + answerCount);
            }

            answerCount++;
        }
    }
</script>
