﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMatchList>" %>
<div class="col-sm-12 row">
<div id="matchlist-pairs"></div>
<div id="matchlist-rightElements"></div>
</div>

<script type="text/javascript">
    <% var random = new Random();
   foreach (var pair in Model.Pairs.OrderBy(x => random.Next()))
   { %>
    var rightDropElement = $("<div class='matchlist-dropable col-sm-5' name = '<%= pair.ElementLeft.Text %>'>").droppable( {
        accept: '#matchlist-rightElements span',
        hoverClass: 'matchlist-hovered',
        drop: handleElementDrop
    } );
    $("div#matchlist-pairs").append($("<div class = 'col-sm-12 row pair-row'>")
        .append($("<span class= 'col-sm-5'><%= pair.ElementLeft.Text %></span>"))
        .append($("<i class=' matchlist-arrow fa fa-arrow-right fa-1x col-sm-2'>"))
        .append(rightDropElement));
    <% }

    foreach (var elementRight in Model.RightElements.OrderBy(x => random.Next()))
    { %>
    var rightDragElement = $("<span class='matchlist-rightelement' name='<%= elementRight.Text %>'>").html("<%= elementRight.Text %>").draggable({
        containment: '#AnswerInputSection',
        stack: '#matchlist-rightElements span',
        cursor: 'move',
        helper: 'clone',
        revert: 'invalid'
    });
    $("#matchlist-rightElements").append(rightDragElement);
    <% } %>

    var answerCount = 0;
    function handleElementDrop(event, ui) {
        $(this).droppable('disable');
        ui.draggable.draggable('disable');
        ui.draggable.position({ of: $(this), my: 'center', at: 'center' });
        $(this).attr('id', 'leftElementResponse-' + answerCount);
        ui.draggable.attr('id', 'rightElementResponse-' + answerCount);
        answerCount++;
    }
</script>