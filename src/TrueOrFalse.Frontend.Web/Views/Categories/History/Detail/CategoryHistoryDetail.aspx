<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<CategoryHistoryDetailModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="FluentNHibernate.Conventions" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/CategoryHistoryDetail") %>
    <%= Scripts.Render("~/bundles/js/CategoryHistoryDetail") %>
    <%= Scripts.Render("~/bundles/js/diff2html") %>
    <%= Styles.Render("~/Scripts/vendor/diff2html/diff2html.css") %>
    <%  Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem {Text = Model.CategoryName, Url = Model.CategoryUrl, ToolTipText = Model.CategoryName});
        Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="row">
        <div class="col-12">
            <h1><i class="fa fa-code-fork"></i> &nbsp; Änderungen für '<%= Model.CategoryName %>'</h1>
        </div>
    </div>
    
    <div class="Buttons">
        <div class="row">
            <div class="col-md-3 col-lg-3">            
                <a href="<%= Links.UserDetail(Model.Author) %>"><img src="<%= Model.AuthorImageUrl %>" height="20"/></a>
                <b><a href="<%= Links.UserDetail(Model.Author) %>"><%= Model.AuthorName %></a></b><br/>
                vom <%= Model.CurrentDateCreated %> 
            </div>
            
            <div class="col dropdown" style="float: right">
                <% if (ReleaseDate.IsAfterRelease(Model.CurrentDateCreated)) {%>
                    <a class="btn btn-primary" href="<%= Links.CategoryDetail(Model.CategoryName, Model.CategoryId, Model.CurrentId) %>">
                        <i class="fa fa-desktop"></i> &nbsp; Anzeige dieser Revision
                    </a>
                <%}%>
                <a class="btn btn-default" href="<%= Links.CategoryHistory(Model.CategoryId) %>">
                    <i class="fa fa-list-ul"></i> &nbsp; Bearbeitungshistorie
                </a>
                <% var buttonSetId = Guid.NewGuid(); %>
                <a href="#" id="<%= buttonSetId %>" class="dropdown-toggle btn btn-link btn-sm ButtonEllipsis" 
                   type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                    <i class="fa fa-ellipsis-v" style="font-size: 18px; margin-top: 2px;"></i>
                </a>
                <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%= buttonSetId %>">
                    <li>
                        <% if (new SessionUser().IsLoggedIn) {
                            if (Model.NextRevExists) { %>
                                <a id="restoreButton" data-allowed="logged-in" onclick="$('#alertConfirmRestore').show();">
                                    <div class="dropdown-icon"><i class="fa fa-undo"></i></div>
                                     Wiederherstellen
                                </a>
                            <% } else { %>
                                <a id="editButton" data-allowed="logged-in" href="<%= Links.CategoryEdit(Model.CategoryName, Model.CategoryId) %>">
                                    <div class="dropdown-icon"><i class="fa fa-edit"></i></div>Thema bearbeiten
                                </a>
                            <% } %>
                        <% } %>
                    </li>
                    <li>
                        <a href="<%= Links.CategoryChangesOverview(1) %>">
                            <div class="dropdown-icon"><i class="fa fa-list"></i></div>Bearbeitungshistorie aller Themen
                        </a>
                    </li>
                </ul>
            </div>
        </div>
        
        <% if (new SessionUser().IsLoggedIn && Model.NextRevExists) { %>
            <div id="alertConfirmRestore" class="row" style="display: none">
                <br/>
                <div class="alert alert-warning" role="alert">
                    <div class="col-12">
                        Der aktuelle Stand wird durch diese Version ersetzt. Wollen Sie das wirklich?
                    </div>
                    <br/>
                    <div class="col-12">
                        <nav>
                            <a class="btn btn-default navbar-btn" href="<%= Links.CategoryRestore(Model.CategoryId, Model.CurrentId) %>">
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
        
        <div class="row">
            <div class="col-12">
                <% var revisionMessage = "";
                   if (!Model.NextRevExists)
                       revisionMessage = "Dies ist die <b>aktuelle Revision</b> des Themas. ";
                   if (!Model.PrevRevExists)
                   {
                       revisionMessage += (revisionMessage.IsNotEmpty()) ?
                           "<br/><br/>Es ist auch die <b>initiale Revision</b> des Themas. " :
                           "Dies ist die <b>initiale Revision</b> des Themas. ";
                       revisionMessage += "Deswegen werden im folgenden als Änderungen die Werte dargestellt, mit denen das Thema erstellt wurde.";
                   }
                %>
                <% if (revisionMessage.IsNotEmpty()) { %>
                    <br />
                    <div class="alert alert-info" role="alert">
                        <%= revisionMessage %>
                    </div>
                <% } else { %>
                    <br />
                <% } %>
                <div id="noChangesAlert" class="alert alert-info" role="alert" style="display: none;">
                    Zwischen den beiden Revisionen (vom <%= Model.PrevDateCreated %> und 
                    vom <%= Model.CurrentDateCreated %>) gibt es <b>keine inhaltlichen Unterschiede</b>.
                </div>
                <input type="hidden" id="currentName" value="<%= Server.HtmlEncode(Model.CurrentName) %>"/>
                <input type="hidden" id="prevName" value="<%= Server.HtmlEncode(Model.PrevName) %>"/>

                <input type="hidden" id="currentMarkdown" value="<%= Server.HtmlEncode(Model.CurrentMarkdown) %>"/>
                <input type="hidden" id="prevMarkdown" value="<%= Server.HtmlEncode(Model.PrevMarkdown) %>"/>

                <input type="hidden" id="currentContent" value="<%= Server.HtmlEncode(Model.CurrentContent) %>"/>
                <input type="hidden" id="prevContent" value="<%= Server.HtmlEncode(Model.PrevContent) %>"/>
                
                <input type="hidden" id="currentSegments" value="<%= Server.HtmlEncode(Model.CurrentSegments) %>"/>
                <input type="hidden" id="prevSegments" value="<%= Server.HtmlEncode(Model.PrevSegments) %>"/>
                
                <input type="hidden" id="currentDescription" value="<%= Server.HtmlEncode(Model.CurrentDescription) %>"/>
                <input type="hidden" id="prevDescription" value="<%= Server.HtmlEncode(Model.PrevDescription) %>"/>
                
                <input type="hidden" id="currentWikipediaUrl" value="<%= Server.HtmlEncode(Model.CurrentWikipediaUrl) %>"/>
                <input type="hidden" id="prevWikipediaUrl" value="<%= Server.HtmlEncode(Model.PrevWikipediaUrl) %>"/>
                
                <input type="hidden" id="currentRelations" value="<%= Server.HtmlEncode(Model.CurrentRelations) %>"/>
                <input type="hidden" id="prevRelations" value="<%= Server.HtmlEncode(Model.PrevRelations) %>"/>

                <input type="hidden" id="imageWasUpdated" value="<%= Model.ImageWasUpdated %>"/>

                <div id="diffPanel">
                    <div id="diffName"></div>
                    <%if (Model.ImageWasUpdated) { %>
                        <div class="diffImage">
                            <div id="newImageAlert" class="panel panel-default">
                                <div class="panel-heading">Änderung des Bildes. Das aktuelle Bild ist:</div>
                                <div class="panel-body">
                                    <%= Model.ImageFrontendData.RenderHtmlImageBasis(350, false, ImageType.Category, "ImageContainer") %>
                                </div>
                            </div>
                        </div>
                    <% } %>  
                    <div id="diffDescription"></div>
                    <div id="diffWikipediaUrl"></div>
                    <div id="diffData"></div>
                    <div id="diffSegments"></div>
                    <div id="diffRelations"></div>
                    <div id="noRelationsAlert" class="alert alert-info" role="alert" style="display: none;">
                        Es können <b>keine Beziehungsdaten</b> angezeigt werden, da für die gewählten Revisionen keine enstsprechenden Daten vorliegen.
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
