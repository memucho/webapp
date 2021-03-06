﻿<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>
<%
    var model = Model.Model == null ? 
            new CategoryTypeDailyIssue() : 
            (CategoryTypeDailyIssue)Model.Model;
%>

<h4 class="CategoryTypeHeader"><%= CategoryType.DailyIssue.GetName() %></h4>
<input class="form-control" name="Name" type="hidden" value="<%= Model.Name %>">
<div class="form-group">
    <label class="RequiredField columnLabel control-label" style="font-weight: bold;" for="xxx">
        Zeitung
    </label>
    <div class="JS-RelatedCategories columnControlsFull">
        
        <% if(Model.IsEditing){ %>
            <p class="form-control-static">
                <%= model.Daily.Name %>
                <span>
                    <i class="fa fa-question-circle show-tooltip" title="Dieses Feld kannst du leider nicht mehr bearbeiten. Für eine andere Zeitung lege bitte ein neues Thema an." data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
                </span>
                <input id="hddTxtDaily" class="form-control" name="hddTxtDaily" type="hidden" value="<%= model.Daily.Id %>">
            </p>

        <% }else{ %>
            <div class="JS-CatInputContainer ControlInline xxs-stack">
                <input id="TxtDaily" class="form-control" name="TxtDaily" type="text" value="" placeholder="Suche nach Titel oder ISSN">
            </div>
        <% } %>
    </div>
</div>
<div class="form-group FormGroupInline">
    <label class="RequiredField columnLabel control-label">
        Erscheinungsdatum
        <i class="fa fa-question-circle show-tooltip" title='Bitte als Zahl angeben.'  data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
    </label>
    <div class="columnControlsFull">
        <div class="JS-ValidationGroup">
            <label class="sr-only" for="PublicationDateDay">Tag</label>
            <div style="" class="ControlInline">
                <input class="form-control InputDayOrMonth JS-ValidationGroupMember" name="PublicationDateDay" type="text" value="<%= string.IsNullOrEmpty(model.PublicationDateDay) ? null : model.PublicationDateDay.PadLeft(2, '0') %>" placeholder="TT">
            </div>
            <label class="control-label LabelInline">.</label>
            <label class="sr-only" for="PublicationDateMonth">Monat</label>
            <div style="" class="ControlInline">
                <input class="form-control InputDayOrMonth JS-ValidationGroupMember" style="" name="PublicationDateMonth" type="text" value="<%= string.IsNullOrEmpty(model.PublicationDateMonth) ? null : model.PublicationDateMonth.PadLeft(2, '0') %>" placeholder="MM">
            </div>
            <label class="control-label LabelInline">.</label>
            <label class="sr-only" for="PublicationDateYear">Jahr</label>
            <div style="" class="ControlInline">
                <input class="form-control InputYear JS-ValidationGroupMember" name="PublicationDateYear" type="text" value="<%= model.PublicationDateYear %>" placeholder="JJJJ">
            </div>
        </div>
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="Volume">
        Jahrgang
        <i class="fa fa-question-circle show-tooltip" title='Keine Jahreszahl, sondern gibt an, im wievielten Jahr eine Zeitung oder Zeitschrift zum Zeitpunkt der jeweiligen Ausgabe erscheint (nur eintragen, falls angegeben).'  data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control InputVolume" name="Volume" type="text" value="<%= model.Volume %>">
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="No">
        Ausgabennummer
        <i class="fa fa-question-circle show-tooltip" title='Bitte als Zahl angeben (Führende Nullen sind möglich).'  data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control InputIssueNo" name="No" type="text" value="<%= model.No %>">
    </div>
</div>

 <%if (!Model.IsEditing) { %>
    <script type="text/javascript">
        $('[name="TxtDaily"]').rules("add", { required: true, });
        $(function () {
            new AutocompleteCategories("#TxtDaily", true, AutoCompleteFilterType.Daily);
        });
    </script>
<% } %>