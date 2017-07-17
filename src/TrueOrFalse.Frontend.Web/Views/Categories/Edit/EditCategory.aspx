<%@ Page Title="Thema bearbeiten" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<EditCategoryModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <% if (Model.IsEditing) { %>
        <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Links.CategoryEdit(Url, Model.Name, Model.Category.Id) %>">
    <% } else {  %>
        <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Links.CategoryCreate() %>">
    <% } %>
</asp:Content>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <%= Scripts.Render("~/bundles/fileUploader") %>
    <%= Styles.Render("~/bundles/CategoryEdit") %>
    <%= Scripts.Render("~/bundles/js/CategoryEdit") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<input type="hidden" id="hhdCategoryId" value="<%= Model.Id %>"/>
    
<% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", "EditCategory", null, 
    FormMethod.Post, new { enctype = "multipart/form-data", id="EditCategoryForm", data_is_editing=Model.IsEditing })){%>
    <div class="row">
        <div class="PageHeader col-xs-12">
            <h2 class="pull-left">
                <span class="ColoredUnderline Category">
                    <% if (Model.IsEditing) { %>
                        Thema bearbeiten
                    <% } else { %>
                        Thema erstellen
                    <% } %>
                </span>
            </h2>
            <div class="headerControls pull-right">
                <div>
                    <a href="<%= Url.Action(Links.CategoriesAction, Links.CategoriesController) %>" style="font-size: 12px; margin: 0;">
                        <i class="fa fa-list"></i>&nbsp;zur Übersicht
                    </a><br/>
                    <% if(Model.IsEditing){ %>
                        <a href="<%= Links.CategoryDetail(Model.Category) %>" style="font-size: 12px;">
                            <i class="fa fa-eye"></i>&nbsp;Detailansicht
                        </a> 
                    <% } %>            
                </div>
            </div>
        </div>
        <div class="PageHeader col-xs-12">
            <% if(!Model.IsLoggedIn){ %>
                <div class="bs-callout bs-callout-danger" style="margin-top: 0;">
                    <h4>Einloggen oder registrieren</h4>
                    <p>
                        Um Themen zu erstellen, <br/>
                        musst du dich <a href="#" data-btn-login="true">einloggen</a> oder <a href="/Registrieren">registrieren</a>.
                    </p>
                </div>
            <% }%>
        </div>
    </div>
    
    <div class="row">
        <div class="col-md-9 xxs-stack">
            <% Html.Message(Model.Message); %>
        </div>
    </div>
    <div class="row">
        <div class="aside col-md-3 col-md-push-9">
            <img id="categoryImg" src="<%= Model.ImageUrl %>" class="img-responsive" style="border-radius:5px;" />
            <div style="margin-top: 10px;">
                <a href="#" style="position: relative; top: -6px; font-size: 90%;" id="aImageUpload">[Verwende ein anderes Bild]</a>
            </div>
        </div>
        <div class="col-md-9 col-md-pull-3">
            <div class="form-horizontal">
                        
                <%: Html.HiddenFor(m => m.ImageIsNew) %>
                <%: Html.HiddenFor(m => m.ImageSource) %>
                <%: Html.HiddenFor(m => m.ImageWikiFileName) %>
                <%: Html.HiddenFor(m => m.ImageGuid) %>
                <%: Html.HiddenFor(m => m.ImageLicenseOwner) %>
                <input type="hidden" id="isCategoryEdit" value="true"/>
                <input type="hidden" id="isEditing" value="<%= Model.IsEditing ?  "true" : "false" %>"/>
                <input type="hidden" id="categoryId" value="<%= Model.IsEditing ?  Model.Category.Id.ToString() : "" %>"/>
                <input type="hidden" id="categoryType" value="<%= Model.IsEditing ? Model.Category.Type.ToString() : "" %>"/>

                <% if (!Model.IsEditing)
                   { %>
                <div id="CategoryTypeSelect" class="FormSection">
                    <div class="form-group">
                        <label class="columnLabel control-label">
                            Thementyp
                        </label>
                        <div class="columnControlsFull">
                            <div class="radio">
                                <label style="font-weight: normal">
                                    <input type="radio" name="rdoCategoryTypeGroup" value="standard" <%= Model.rdoCategoryTypeGroup == "standard" ? "checked" : "" %> />
                                    Thema (Standard)
                                    <i class="fa fa-question-circle show-tooltip" title="Für alle normalen Themen" data-placement="<%= CssJs.TooltipPlacementFormField %>"></i>
                                </label>
                            </div>
                            <div class="radio">
                                <label style="font-weight: normal">
                                    <input type="radio" name="rdoCategoryTypeGroup" value="media" <%= Model.rdoCategoryTypeGroup == "media" ? "checked" : "" %> />
                                    Medien
                                    <i class="fa fa-question-circle show-tooltip" title="Für Quellenangaben und für Fragen, die sich auf ein bestimmtes Buch, einen Zeitungsartikel usw. beziehen." data-placement="<%= CssJs.TooltipPlacementFormField %>"></i>
                                    <br/><span style="font-weight: normal;">(Bücher, Zeitungsartikel, Online-Beiträge, Videos etc.)</span>
                                    <select class="form-control" id="ddlCategoryTypeMedia" name="ddlCategoryTypeMedia" style="margin-top: 5px; display: none;" data-selectedValue="<%= Model.ddlCategoryTypeMedia %>" >
                                        <optgroup label="Druckmedien und eBooks">
                                            <option value="Book">Buch (auch eBooks)</option>
                                            <option value="VolumeChapter"><%= CategoryType.VolumeChapter.GetName() %></option>
                                            <option value="Daily"><%= CategoryType.Daily.GetName() %></option>
                                            <option value="DailyIssue">Tageszeitung: Ausgabe</option>
                                            <option value="DailyArticle">Tageszeitung: Artikel</option>
                                            <option value="Magazine"><%= CategoryType.Magazine.GetName() %></option>
                                            <option value="MagazineIssue">Zeitschrift: Ausgabe</option>
                                            <option value="MagazineArticle">Zeitschrift: Artikel</option>
                                        </optgroup>
                                        <optgroup label="Internet">
                                            <%--<option value="Website"><%= CategoryType.Website.GetName() %></option>--%>
                                            <option value="WebsiteArticle"><%= CategoryType.WebsiteArticle.GetName() %></option>
                                            <option value="WebsiteVideo" disabled><%= CategoryType.WebsiteVideo.GetName() %></option>
                                        </optgroup>
                                        <optgroup label="Film und Fernsehen" disabled>
                                            <option value="Movie" disabled><%= CategoryType.Movie.GetName() %></option>
                                            <option value="TvShow" disabled><%= CategoryType.TvShow.GetName() %></option>
                                            <option value="TvShowEpisode" disabled><%= CategoryType.TvShowEpisode.GetName() %></option>
                                        </optgroup>
                                    </select>
                                </label>
                            </div>
                            <div class="radio">
                                <label style="font-weight: normal">
                                    <input type="radio" name="rdoCategoryTypeGroup" value="education" <%= Model.rdoCategoryTypeGroup == "education" ? "checked" : "" %> />
                                    Aus- und Weiterbildung
                                    <br/>(Universitäten, Kurse, Professoren/Dozenten etc.)
                                    <select class="form-control" id="ddlCategoryTypeEducation" name="ddlCategoryTypeEducation" style="margin-top: 5px; display: none;" data-selectedValue="<%= Model.ddlCategoryTypeEducation %>">
                                        <option value="SchoolSubject"><%= CategoryType.SchoolSubject.GetName() %></option>
                                        <option value="FieldOfStudy"><%= CategoryType.FieldOfStudy.GetName() %></option>
                                        <option value="FieldOfTraining"><%= CategoryType.FieldOfTraining.GetName() %></option>
                                        <option value="Course"><%= CategoryType.Course.GetName() %></option>
                                        <option value="Certification"><%= CategoryType.Certification.GetName() %></option>
                                    </select>
                                </label>
                            </div>
                        </div>
                    </div>
                    
                </div>
                <% } %>
                <div class="FormSection">
                    <div id="CategoryDetailsBody">
                        <h4 class="CategoryTypeHeader">Formular wird geladen...</h4>                    
                    </div>
                </div>
                <div class="FormSection JS-ShowWithPartial" style="display: none;">
                    <div class="form-group">
                        <label class="columnLabel control-label">
                            Übergeordnete Themen
                            <i class="fa fa-question-circle show-tooltip" title="Hilft, Themen in Beziehung zueinander zu setzen. Beispiele: Thema Wirbeltiere - übergeordnet: Biologie. Thema Algebra - übergeordnet: Mathematik" data-placement="<%= CssJs.TooltipPlacementLabel %>" data-trigger="hover click"></i>
                        </label>
                        <div class="JS-RelatedCategories columnControlsFull">
                            <script type="text/javascript">
                                $(function () {
                                    <%foreach (var category in Model.ParentCategories) { %>
                                    $("#txtNewRelatedCategory")
                                        .val('<%=category.Name %>')
                                        .data('category-id', '<%=category.Id %>')
                                        .trigger("initCategoryFromTxt");
                                    <% } %>
                                });
                            </script>
                            <div class="JS-CatInputContainer ControlInline">
                                <input id="txtNewRelatedCategory" class="form-control .JS-ValidationIgnore" type="text" placeholder="Wähle ein Thema"  />
                            </div>
                        </div>
                    </div>

                    <% if (Model.IsInstallationAdmin){

                           if (Model.IsEditing)
                           { %>
                            <div style="margin-bottom: 20px;">
                                <a href="#EditAggregationModal" id="OpenEditAggregationModal" class="btn btn-default" data-toggle="modal">Unterthemen einschließen</a>
                            </div>

                            <% } %>
                        <div class="form-group">
                            <div class="noLabel columnControlsFull" style="font-size: 100%;">
                                 <div class="checkbox">
                                    <label>
                                        <%= Html.CheckBoxFor(m => Model.DisableLearningFunctions) %>Keine Lernoptionen anzeigen (nur für Admins) 
                                    </label>
                                </div>
                            </div>      
                        </div>
                        <div class="form-group">
                            <label class="columnLabel control-label" for="TopicMarkdown">
                                Freie Seitengestaltung für Themenseite
                                <i class="fa fa-question-circle show-tooltip" 
                                    title="Erfordert Markdown-Syntax. Zum Vergrößern des Eingabefelds bitte unten rechts größer ziehen." 
                                    data-placement="<%= CssJs.TooltipPlacementLabel %>" data-trigger="hover click"></i>
                            </label>
                            <div class="columnControlsFull">
                                <textarea class="form-control" name="TopicMarkdown" id="TopicMarkdown" 
                                    <% var x = Model.TopicMarkdown; %>
                                    rows="<%= string.IsNullOrEmpty(Model.TopicMarkdown) ? "4" : "16" %>" 
                                    style="width: 100%; max-width: 100%;"><%= Model.TopicMarkdown %></textarea>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="columnLabel control-label" for="FeaturedSetIdsString">
                                Offiziell präsentierte Lernsets
                                <i class="fa fa-question-circle show-tooltip" 
                                    title="Bitte Ids der Lernsets in der Form '1,2,3' angeben. Bitte darauf achten, dass diese Lernsets tatsächlich mit dem Thema versehen sind." 
                                    data-placement="<%= CssJs.TooltipPlacementLabel %>" data-trigger="hover click"></i>
                            </label>
                            <div class="columnControlsFull">
                                <input class="form-control" name="FeaturedSetIdsString" type="text" value="<%= Model.FeaturedSetIdsString %>">
                            </div>
                        </div>
                    <% } %>
                </div>
                <div class="FormSection JS-ShowWithPartial" style="display: none;">
                    <div class="form-group">
                        <div class="noLabel columnControlsFull">
                            <p class="form-control-static"><span class="RequiredField"></span> Pflichtfeld</p>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="noLabel columnControlsFull">
                            <% if (Model.IsEditing){ %>
                                <input type="submit" value="Speichern" class="btn btn-primary" name="btnSave" />
                                <a data-toggle="modal" href="#modalDeleteCategory" data-categoryId="<%= Model.Id %>" class="btn btn-danger"><i class="fa fa-trash-o"></i> Löschen</a>
                            <% } else { %>
                                <input type="submit" value="Thema erstellen" class="btn btn-primary" name="btnSave" <% if(!Model.IsLoggedIn){ %> disabled="disabled" <% } %>/>
                            <% } %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

<% } %>
    
<%  if (!Model.IsEditing) { %>
    <script type="text/javascript">
        <% if (Model.PreselectedType.GetCategoryTypeGroup() == CategoryTypeGroup.Standard)
        { %>
            $(function() {
                $("input:radio[name='rdoCategoryTypeGroup']").trigger('change', [false, 'Standard']);
            });
               
        <% } else if (Model.PreselectedType.GetCategoryTypeGroup() == CategoryTypeGroup.Media)
        {%>
            $(function () {
                $('[name="rdoCategoryTypeGroup"][value="media"]').prop('checked', true);
                $("input[name=rdoCategoryTypeGroup]:radio").trigger('change', [true]);
                $('#ddlCategoryTypeMedia').val('<%= Model.PreselectedType %>').trigger('change', ['<%= Model.PreselectedType%>']);
            });
        <% } %>
    </script>
<% }
    Html.RenderPartial("~/Views/Images/ImageUpload/ImageUpload.ascx");
    Html.RenderPartial("~/Views/Categories/Modals/ModalDeleteCategory.ascx");

    if (Model.IsEditing)
    { %>
    <div id="EditAggregationModal" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button class="close" data-dismiss="modal">×</button>
                    <h3>Unterthemen einschließen</h3>
                </div>
                <div class="modal-body clearfix">
                    <ul class="nav nav-tabs">
                        <li class="tab-unterthemen active"><a href="#">Unterthemen einschließen</a></li>
                        <li class="tab-categories-graph"><a href="#">Graphen Ansicht</a></li>
                    </ul>
                    <div class="tab-body">
                        <div style="text-align: center">
                            <i class="fa fa-spinner fa-spin"></i>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    
                    <a href="#" id="btnResetAggregation" class="btn btn-danger" style="float: left">Zurücksetzen</a>
                    <a href="#" class="btn btn-default" id="btnCloseAggregation">Schließen</a>
                    <a href="#" id="btnEditAggregation" class="btn btn-primary">Bearbeiten</a>
                </div>
            </div>
        </div>
    </div>
   <% } %>



</asp:Content>