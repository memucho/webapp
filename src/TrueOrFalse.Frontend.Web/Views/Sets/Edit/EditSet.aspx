<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<EditSetModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <% Title = Model.PageTitle; %>
    <% if (Model.IsEditing) { %>
        <link rel="canonical" href="<%= Settings.CanonicalHost + Links.QuestionSetEdit(Model.Set.Name, Model.Set.Id) %>">
    <% } else {  %>
        <link rel="canonical" href="<%= Settings.CanonicalHost + Links.SetCreate() %>">
    <% } %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Sets/Edit/EditSet.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/fileUploader") %>
    <%= Scripts.Render("~/bundles/SetEdit") %>
    <script type="text/javascript">
        var isEditMode = <%= Model.IsEditing ? "true" : "false" %>;
        var questionSetId = "<%= Model.Id %>";
        var userName = "<%= Model.Username %>";

        $(function () {
            $('.control-label .show-tooltip').append($("<i class='fa fa-info-circle'></i>"));
        });
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div id="questionSetId" data-id="<%: Model.Id %>"></div>
    
<% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create",
       "EditSet", null, FormMethod.Post,
       new {id = "EditSetForm", enctype = "multipart/form-data", style = "margin:0px;"}))
   { %>
        
    <%: Html.HiddenFor(m => m.ImageIsNew) %>
    <%: Html.HiddenFor(m => m.ImageSource) %>
    <%: Html.HiddenFor(m => m.ImageWikiFileName) %>
    <%: Html.HiddenFor(m => m.ImageGuid) %>
    <%: Html.HiddenFor(m => m.ImageLicenseOwner) %>

    <div class="row">
        <div class="PageHeader col-xs-12">
            <h2 class="pull-left">
                <span class="ColoredUnderline Set">
                    <% if (Model.IsEditing) { %>
                        Lernset bearbeiten
                    <% } else { %>
                        Lernset erstellen
                    <% } %>
                </span>
            </h2>
            <div class="headerControls pull-right">
                <div>
                    <a href="<%= Links.SetsAll() %>" style="font-size: 12px; margin: 0;">
                        <i class="fa fa-list"></i>&nbsp;zur Übersicht
                    </a><br/>
                    <% if (Model.Set != null) { %>
                        <a href="<%= Links.SetDetail(Url, Model.Set) %>" style="font-size: 12px;">
                            <i class="fa fa-eye"></i>&nbsp;Detailansicht
                        </a> 
                    <% } %>            
                </div>
            </div>
            <div class="PageHeader col-xs-12">
                <% if (!Model.IsLoggedIn) { %>
                    <div class="bs-callout bs-callout-danger" style="margin-top: 0;">
                        <h4>Einloggen oder registrieren</h4>
                        <p>
                            Um Lernsets zu erstellen, <br/>
                            musst du dich <a href="#" data-btn-login="true">einloggen</a> oder <a href="/Registrieren">registrieren</a>.
                        </p>
                    </div>
                <% } %>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-9 xxs-stack">
            <% Html.Message(Model.Message); %>
        </div>
    </div>
    

    <div class="form-horizontal rowBase">
        <div class="FormSection">
            <div class="row">
                <div class="col-md-9">
                    <div class="row">
                        <div class="col-md-12 form-group ">
                            <%= LabelExtensions.LabelFor(Html, m => m.Title, new {@class = "RequiredField control-label columnLabel"}) %>
                            <div class="columnControlsFull">
                                <%= Html.TextBoxFor(m => m.Title, new {@class = "form-control", @maxlength = "255"}) %>
                            </div>                                    
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 ">
                            <div class="form-group">
                                <%= LabelExtensions.LabelFor(Html, m => m.Text, new {@class = "control-label columnLabel"}) %>
                                <div class="columnControlsFull">
                                    <%= Html.TextAreaFor(m => m.Text, new {@class = "form-control"}) %>
                                </div>
                            </div>                                    
                        </div>      
                    </div>
                </div>

                <div class="aside col-md-3 ">
                    <img id="questionSetImg" src="<%= Model.ImageUrl_206px %>" class="img-responsive" style="border-radius:5px;" />
                    <div style="margin-top: 10px;">
                        <a href="#" style="position: relative; top: -6px; font-size: 90%;" id="aImageUpload">[Verwende ein anderes Bild]</a>
                    </div>
                </div>
            </div>
                
            <div class="form-group">
                <label class="columnLabel control-label">
                    <span class="show-tooltip" title="Die Url aus der Adresszeile des Browser, zum Beispiel: https://www.youtube.com/watch?v=iQ6NxvQRfq4">
                        Youtube Url (optional)
                    </span>
                </label>
                <div class="columnControlsFull">
                    <%= Html.TextBoxFor(m => m.VideoUrl, new {@class = "form-control"}) %>
                    <div class="greyed" style="display: inline-block">
                        <a class="greyed" href="<%=Links.WidgetExamples() %>#widgetVideoQuiz" target="_blank">Beispiel für ein Video-Lernset <i class="fa fa-external-link"></i></a>
                    </div>
                </div>
            </div>
              
                        
            <div id="player" >
            </div>
                  

               
                    
                
             
            <div class="form-group">
                <label class="columnLabel control-label">
                    <span class="show-tooltip" title="Themen helfen bei der Einordnung des Lernsets und ermöglichen dir und anderen, Lernsets wiederzufinden.">
                        Themen
                    </span>
                </label>        
                                
                <div class="JS-RelatedCategories columnControlsFull">
                    <script type="text/javascript">
                        $(function () {
                            <% foreach (var category in Model.Categories)
                                    { %>
                            $("#txtNewRelatedCategory")
                                .val('<%= category.Name %>')
                                .data('category-id', '<%= category.Id %>')
                                .trigger("initCategoryFromTxt");
                            <% } %>
                        });
                    </script>
                    <div class="JS-CatInputContainer ControlInline">
                        <input id="txtNewRelatedCategory" class="form-control .JS-ValidationIgnore" type="text" placeholder="Wähle ein Thema"  />
                    </div>
                </div>                                
            </div>
            <div class="form-group" style="margin-top: -15px;">
                <div class="noLabel columnControlsFull">
                    <p class="form-control-static"><span class="RequiredField"></span> Pflichtfeld</p>
                </div>
            </div>

            <div class="buttons">
                <% if (Model.IsEditing) { %>
                    <input type="submit"  value="Speichern" class="btn btn-primary" name="btnSave" id="btnSave"  />
                <% } else { %>
                    <input type="submit" value="Lernset erstellen"  class="btn btn-primary" id="btn-save" 
                            name="btnSave" <% if (!Model.IsLoggedIn) { %> disabled="disabled" <% } %>/>
                <% } %>
            </div>                        
                    
            <div class="FormSection separationBorderTop"></div>

            <% if (Model.IsEditing) { %>
                <div class="form-group">
                    <div class="noLabel noControls">
                            
                        <h3 style="margin-top: 0px;">Enthaltene Fragen</h3>
                                
                        <p>
                            Du kannst die Reihenfolge der Fragen verändern, indem du sie an die richtige Stelle schiebst (Drag'n'Drop).
                            <span id="revertAction" class="pull-right hide2" style="font-size: 11px; font-weight: normal; position: relative; top: 7px; right: 7px; cursor: pointer">
                                [Rückgängig]
                            </span> Durch Klick auf den <span style="white-space: nowrap"><i class="fa fa-trash-o"></i> Mülleimer</span> 
                            entfernst du eine Frage aus dem Lernset, die Frage selbst wird dabei nicht gelöscht.
                        </p>
                        <p class="videoSetAnnotation">
                            <i class="fa fa-lg fa-youtube-play greyed"></i> <b>Video-Lernset:</b> Du hast ein youtube-Video zum Lernset angegeben. 
                            Jetzt kannst zu jeder Frage festlegen, zu welchem Zeitpunkt des Videos die Frage erscheinen soll. 
                            Lasse das Video laufen und klicke im richtigen Moment auf die <span style="white-space: nowrap"><i class="fa fa-clock-o"></i> Uhr</span>. 
                            Die aktuelle Zeit des Videos wird dann automatisch übernommen. Du kannst die Zeit auch direkt in das Feld eintragen.
                        </p>

                        <% if (Model.QuestionsInSet.Count == 0) { %>

                            <div class="alert alert-info" style="margin-top: 15px; margin-bottom: 5px;">
                                <p id="emptyLearnSet">
                                    <b>Dein Lernset enthält noch keine Fragen.</b> Weiter unten kannst du Fragen hinzufügen.
                                </p>
                            </div>
                                    
                        <% } %>
                                    
                        <ul id="ulQuestions" >
                            <% foreach (var questionInSet in Model.QuestionsInSet){ %>
                                        
                                <%Html.RenderPartial("~/Views/Sets/Edit/QuestionInSet.ascx", new QuestionInSetModel(questionInSet)); %>
                                          
                            <% } %>  
                        </ul>

                    </div>
                </div>
                      
                <div class="FormSection separationBorderTop"></div>
                        
                <div class="row">
                    <div class="col-md-12">
                        <h3>Fragen zum Lernset hinzufügen</h3>
                    </div>
                </div>
                    
                <div id="questionSearch">
                    <div class="row">
                        <div class="col-md-12">
                            <h4>Vorhandene Fragen finden und hinzufügen</h4>
                            <input id="addQuestionSearchField" type="text" class="form-control" placeholder="Tippe um Fragen zu finden"/ style="display: inline-block; margin-right: 15px;">
                            <a class="greyed" style="display: inline-block; margin-top: 8px;" data-toggle="collapse" href="#questionSearchAnnotation" aria-expanded="false" aria-controls="questionSearchAnnotation">
                                Hinweise ein-/ausblenden <i class="fa fa-caret-down"></i>
                            </a>
                            <div id="questionSearchAnnotation" class="collapse alert alert-info">
                                <p>
                                    Du kannst alle öffentlichen Fragen bei memucho in dein eigenes Lernset einbinden. Tippe in das Feld einen Suchbegriff, 
                                    markiere die gewünschten Fragen und füge sie dann hinzu.
                                </p>
                                <p>
                                    <b>Du findest nicht die richtigen Fragen, obwohl es sie gibt?</b> Dann suche die Fragen auf der 
                                    <a href="<%= Links.QuestionsAll() %>" target="_blank">Übersichtsseite aller vorhandenen Fragen <i class="fa fa-external-link"></i></a>.
                                    Dort hast du auch weitere Filtermöglichkeiten (Themenfilter, Wunschwissen, Eigene Fragen) 
                                    und kannst die Fragen ebenfalls direkt deinem Lernset hinzufügen.
                                </p>
                                        
                            </div>
                        </div>
                    </div>
                                                   
                    <div class="row" id="questionSearchResults">
                        <div class="col-md-12">
                            <h4>Ergebnis der Suche</h4>
                        </div>                            
                    </div>
                    
                    <div id="resultQuestions" class=""></div>
                        
                    <div class="row">
                        <div class="col-md-12">
                            <button id="addMarkedQuestionsToSetAndSave" class="btn btn-primary" type="submit"><i class="fa fa-plus"></i> Markierte Fragen zum Lernset hinzufügen</button>
                        </div>
                      
                        <div class="col-md-12 alert alert-success" role="alert" id="addQuestionsOutputInfo">
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-xs-12" style="padding-bottom: 10px" id="createNewQuestionInSet">
                        <h4>Neue Frage für dieses Lernset erstellen</h4>
                        <p>Du kannst eine neue Frage erstellen, die gleich diesem Lernset hinzugefügt wird.</p>
                        <a class="btn btn-default" href="<%= Links.CreateQuestion(setId: Model.Id) %>" target="_blank" id="btnCreateNewQuestionInSet">
                            <i class="fa fa-plus">&nbsp;</i> Neue Frage für dieses Lernset erstellen
                        </a> (öffnet im neuen Tab)

                    </div>
                </div>
            <% } %>                      

        </div>     
                                        
        <% if (!Model.IsEditing) { %>
        <div class="FormSection" style="margin-top: 30px;">
            <h3>Fragen hinzufügen</h3>
            <p>
                Nach dem Erstellen des Lernsets kannst du an dieser Stelle vorhandene Fragen oder neue Fragen deinem Lernset hinzufügen.
            </p>
                    
        </div>
        <% } %>

    </div>
   
<% } %>

<% Html.RenderPartial("~/Views/Images/ImageUpload/ImageUpload.ascx"); %>
    
<div id="modalRevertAction" class="modal">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h3>Letztes Löschen rückgängig machen</h3>
            </div>
            <div class="modal-body">
                <p>NOCH NICHT UMGESETZT</p>
            </div>
            <div class="modal-footer" id="tqsNoSetsFooter">
                <a href="#" class="btn btn-default" data-dismiss="modal">Schließen</a>
                <a href="#" class="btn btn-primary">Jetzt rückgängig machen</a>
            </div>
        </div>
    </div>
</div>

</asp:Content>