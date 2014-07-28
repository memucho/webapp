﻿<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>
<%
    var model = Model.Model == null ? 
            new CategoryTypeMagazineArticle() : 
            (CategoryTypeMagazineArticle)Model.Model;
%>

<h4 class="CategoryTypeHeader"><%= CategoryType.MagazineArticle.GetName() %></h4>
<input class="form-control" name="Name" type="hidden" value="<%= Model.Name %>">
<div id="JS-MagazineSelect" class="form-group">
    <label class="RequiredField columnLabel control-label" for="">
        Zeitschrift
    </label>
    <div class="JS-RelatedCategories columnControlsFull">
        <% if(Model.IsEditing){ %>
        <p class="form-control-static">
            <%= model.Magazine.Name %>
            <span>
                <i class="fa fa-question-circle show-tooltip" title="Dieses Feld kannst du leider nicht mehr bearbeiten. Für eine andere Zeitschrift lege bitte eine neue Kategorie an." data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
            </span>
            <input id="hddTxtMagazine" class="form-control" name="hddTxtMagazine" type="hidden" value="<%= model.Magazine.Name %>">
        </p>
        <% }else{ %>
            <div class="JS-CatInputContainer ControlInline">
                <input id="TxtMagazine" class="form-control" name="TxtMagazine" type="" value="" placeholder="Suche nach Titel oder ISSN">    
            </div>
        <% } %>
            
    </div>
</div>
<% if(Model.IsEditing){ %>
    <div class="form-group">
        <label class="RequiredField columnLabel control-label" for="">
            Ausgabe
        </label>
        <div class="columnControlsFull">
            <p class="form-control-static">
                <%= model.MagazineIssue.Name %>
                <span>
                    <i class="fa fa-question-circle show-tooltip" title="Dieses Feld kannst du leider nicht mehr bearbeiten. Für eine andere Zeitschrift/Ausgabe lege bitte eine neue Kategorie an." data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
                </span>
                <input id="hddTxtMagazineIssue" class="form-control" name="hddTxtMagazineIssue" type="hidden" value="<%= model.MagazineIssue.Name %>">
            </p>
        </div>
    </div>
<% } %>

<div class="form-group">
    <label class="RequiredField columnLabel control-label" for="Title">
        Titel des Artikels
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="Title" type="text" value="<%= model.Title %>">
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="Subtitle">Untertitel</label>
    <div class="columnControlsFull">
        <input class="form-control" name="Subtitle" type="text" value="<%= model.Subtitle %>">
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="Author">
        Autor(en)
        <i class="fa fa-question-circle show-tooltip" title='Bitte gib einen Autor je Zeile im Format "Nachname, Vorname" an.'  data-placement="<%= CssJs.TooltipPlacementLabel %>"></i>
    </label>
    <div class="columnControlsFull">
            <textarea class="form-control" name="Author" type="text" placeholder="Name, Vorname"><%= model.Author %></textarea>
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label">Seiten Artikel</label>
    
    <div class="columnControlsFull">
        <div class="JS-ValidationGroup">
            <label class="control-label LabelInline" for="PagesChapterFrom">von</label>
            <div class="ControlInline">
                <input class="form-control InputPageNo JS-ValidationGroupMember" name="PagesChapterFrom" type="text" value="<%= model.PagesChapterFrom%>">
            </div>
            <label class="control-label LabelInline" for="PagesChapterTo">bis</label>
            <div style="" class="ControlInline">
                <input class="form-control InputPageNo JS-ValidationGroupMember" style="" name="PagesChapterTo" type="text" value="<%= model.PagesChapterTo %>">
            </div>
        </div>
    </div>    
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="Description">
        Beschreibung
        <i class="fa fa-question-circle show-tooltip" 
            title="<%= EditCategoryTypeModel.DescriptionInfo %>" data-placement="<%= CssJs.TooltipPlacementLabel %>">
        </i>
    </label>
    <div class="columnControlsFull">
        <textarea class="form-control" name="Description" type="text"><%= Model.Description %></textarea>
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="Url">
        Online-Version
        <i class="fa fa-question-circle show-tooltip" 
            title="Falls der Artikel zusätzlich online zugänglich ist, gib bitte hier die URL (vorzugsweise einen Perma-Link) an." data-placement="<%= CssJs.TooltipPlacementLabel %>">
        </i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="Url" type="text" value="<%= model.Url %>">
    </div>
</div>
<% if (!Model.IsEditing)
   { %>
    <script type="text/javascript">
        $(function () {
            $('[name="TxtMagazine"]').rules("add", { required: true, });
            var autoComplete = new AutocompleteCategories("#TxtMagazine", true, AutoCompleteFilterType.Magazine);
            autoComplete.OnAdd = function () {
                $("#IssueSelect").remove();
                $("#JS-MagazineSelect").after(
                    "<div id='IssueSelect' class='form-group'>" +
                        "<label class='RequiredField columnLabel control-label' for=''>" +
                        "Ausgabe" +
                        "</label>" +
                        "<div class='JS-RelatedCategories columnControlsFull'>" +
                            "<div class='JS-CatInputContainer ControlInline'>" +
                                "<input id='TxtMagazineIssue' class='form-control' name='TxtMagazineIssue' type='' value='' placeholder='Suche nach Ausgabe'>" +
                            "</div>" +
                        "</div>" +
                    "</div>"
                    );
                new AutocompleteCategories("#TxtMagazineIssue", true, AutoCompleteFilterType.MagazineIssue, "#TxtMagazine");
                fnEditCatValidation("MagazineArticle");
                $('[name="TxtMagazineIssue"]').rules("add", { required: true, });
            };
            autoComplete.OnRemove = function () {
                $("#IssueSelect").remove();
            };
        });
    </script>
<% } %>
