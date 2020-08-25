<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="ContentModuleApp">
    <% if (Model.Category.IsHistoric)
       { %>
        <div class="alert alert-info" role="alert">
            <b>Revision vom <%= Model.CategoryChange.DateCreated %></b>
            <br/>
            <% if (Model.NextRevExists)
               { %>
                Diese Seite zeigt einen <b>früheren Stand</b> des Themas.
            <% }
               else
               { %>
                Dies ist die <b>aktuelle Revision</b> des Themas.
            <% } %>
            
            <br />
            <br />
    
            In dieser Revisionsansicht gibt es nur <b>eingeschränkte Möglichkeiten, mit dem Thema 
            zu interagieren</b>, bspw. eine Lernsitzung zu starten. Bitte gehe dazu am besten zur 
            Liveansicht des Themas:
            <a href="<%= Links.CategoryDetail(Model.Category.Name, Model.Category.Id) %>">
                <%= Model.Name %>
            </a>
    
            <div class="dropdown pull-right" style="margin-top: 1em">
                <a class="btn btn-primary" href="<%= Links.CategoryHistoryDetail(Model.Id, Model.CategoryChange.Id) %>">
                    <i class="fa fa-code-fork"></i> &nbsp; Änderungen anzeigen
                </a>
                <a class="btn btn-default" href="<%= Links.CategoryHistory(Model.Id) %>">
                    <i class="fa fa-list-ul"></i> &nbsp; Bearbeitungshistorie
                </a>
                <% var buttonSetId = Guid.NewGuid(); %>
                <a href="#" id="<%= buttonSetId %>" class="dropdown-toggle btn btn-link btn-sm ButtonEllipsis" 
                   type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                    <i class="fa fa-ellipsis-v" style="font-size: 18px; margin-top: 2px;"></i>
                </a>
                <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%= buttonSetId %>">
                    <li>
                        <% if (new SessionUser().IsLoggedIn)
                           {
                               if (Model.NextRevExists)
                               { %>
                                <a id="restoreButton" data-allowed="logged-in" onclick="$('#alertConfirmRestore').show();">
                                    <i class="fa fa-undo"></i> &nbsp; Wiederherstellen
                                </a>
                            <% }
                               else
                               { %>
                                <a id="editButton" data-allowed="logged-in" href="<%= Links.CategoryEdit(Model.Category) %>">
                                    <i class="fa fa-edit"></i> &nbsp; Thema bearbeiten
                                </a>
                            <% } %>
                        <% } %>
                    </li>
                    <li>
                        <a href="<%= Links.CategoryChangesOverview(1) %>">
                            <i class="fa fa-list"></i> &nbsp; Bearbeitungshistorie aller Themen
                        </a>
                    </li>s
                </ul>
            </div>
            
            <br/>
            <br/>
            &nbsp;
    
        </div>
        <% if (Model.CategoryIsDeleted)
           {
               Html.RenderPartial("~/Views/Shared/Delete.ascx");
           }
        %>
        <% if (new SessionUser().IsLoggedIn && Model.NextRevExists)
           { %>
            <div id="alertConfirmRestore" class="row" style="display: none">
                <br/>
                <div class="alert alert-warning" role="alert">
                    <div class="col-12">
                        Der aktuelle Stand wird durch diese Version ersetzt. Wollen Sie das wirklich?
                    </div>
                    <br/>
                    <div class="col-12">
                        <nav>
                            <a class="btn btn-default navbar-btn" href="<%= Links.CategoryRestore(Model.Category.Id, Model.CategoryChange.Id) %>">
                                <i class="fa fa-undo"></i> Ja, Wiederherstellen
                            </a>
                            <a class="btn btn-primary navbar-btn" onclick="$('#alertConfirmRestore').hide();">
                                <i class="fa fa-remove"></i> Nein, Abbrechen
                            </a>
                        </nav>
                    </div>
                </div>
            </div>
        <% } %>
        <br/>
    <% } %>
    
    <div id="MarkdownContent" class="module" v-sortable="options">
        
        <%= Model.CustomPageHtml %>

        <div id="ContentModulePlaceholder" v-if="editMode" v-cloak>
            <content-module inline-template content-module-type="AddModuleButton">
                <div class="placeholderBorder" :class="{ hover : hoverState }" @click="addModule('before')" @mouseenter="updateHoverState(true)" @mouseleave="updateHoverState(false)">
                    <i class="fa fa-plus-square fa-5x" :class="{ hover : hoverState }"></i>
                </div>
            </content-module>
        </div>
    </div>
    <% if (!Model.CategoryIsDeleted) {%>
        <%: Html.Partial("~/Views/Categories/Detail/Partials/InlineEditFloatingActionButton.ascx") %>
        <%: Html.Partial("~/Views/Categories/Detail/Partials/ModalComponentCollection.ascx") %>
    <% } %>
    
</div>
<% if (!Model.CategoryIsDeleted && Model.ShowContent(Model.Category))
   { %>
<%: Html.Partial("~/Views/Categories/Detail/Partials/CategoryFooter/CategoryFooter.ascx") %>
    <% } %>
<%= Scripts.Render("~/bundles/js/CategoryEditMode") %>

