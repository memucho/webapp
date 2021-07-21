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
            </div>
            <br/>
            <br/>
        </div>
        <% if (Model.CategoryIsDeleted)
               Html.RenderPartial("~/Views/Shared/Delete.ascx");
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
    
    <div id="MarkdownContent" class="module">
        
        <%= Model.CustomPageHtml %>

    </div>
    <div id="TopicTabFABApp">
        <%: Html.Partial("~/Views/Categories/Detail/Partials/FloatingActionButton/FloatingActionButton.ascx", new FloatingActionButtonModel(Model.Category, true)) %>
    </div>
    <div>
        <%: Html.Partial("~/Views/Categories/Detail/Partials/Segmentation/SegmentationComponent.vue.ascx", new SegmentationModel(Model.Category)) %>
    </div>
</div>


<div id="TopicTabContentEnd"></div>
<%= Scripts.Render("~/bundles/js/FloatingActionButton") %>
<%= Scripts.Render("~/bundles/js/TopicTabFABLoader") %>


<% if (!Model.CategoryIsDeleted)
   { %>
<%: Html.Partial("~/Views/Categories/Detail/Partials/CategoryFooter/CategoryFooter.ascx") %>
    <% } %>
<%= Scripts.Render("~/bundles/js/CategoryEditMode") %>

