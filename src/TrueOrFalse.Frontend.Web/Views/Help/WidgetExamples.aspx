<%@ Page Title="Widget: Beispiele für Fragen und Lernsets (Quiz)" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<BaseModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Help/Widget.css" rel="stylesheet" />
    
    <%= Scripts.Render("~/bundles/mailto") %>
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Widgets", Url = Links.HelpWidget(), ToolTipText  = "Widgets"});
       Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Beispiele", Url = Links.WidgetExamples(), ToolTipText  = "Beispiele"});
       Model.TopNavMenu.IsCategoryBreadCrumb = false;
       Model.TopNavMenu.IsWidgetOrKnowledgeCentral = true;%>
    <%= Scripts.Render("~/bundles/js/Help") %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% Html.RenderPartial("~/Views/Help/WidgetMenu.ascx", new WidgetMenuModel{CurrentIsExample = true});  %>

    <div class="row">
        <div class="col-xs-12">
            <div class="well">

                <h1 class="PageHeader"><span class="ColoredUnderline GeneralMemucho">Widgets: Beispiele & Übersicht</span></h1>
                
                <p>
                    Die Inhalte und Lernfunktionen von memucho können als Widget nahtlos in bestehende Seiten eingebettet werden -  
                    einzelne interaktive Fragen, Quiz und Video-Quiz. Hier zeigen wir dir Beispiele.
                </p>

                <ul class="nav nav-pills nav-stacked">
                    <li><a href="#widgetsQuestion"><i class="fa fa-caret-right">&nbsp;</i>Interaktive Fragen</a></li>
                    <li><a href="#widgetQuiz"><i class="fa fa-caret-right">&nbsp;</i>Interaktiver Quiz</a></li>
                    <li><a href="#widgetVideoQuiz"><i class="fa fa-caret-right">&nbsp;</i>Interaktiver Video-Quiz</a></li>
                    <li><a href="#moreWidgets" style="margin-top: 25px;"><i class="fa fa-caret-right">&nbsp;</i>Wissensmanagement und personalisiertes Lernen</a></li>
                    <li><a href="#contact"><i class="fa fa-caret-right">&nbsp;</i>Kontakt</a></li>
                </ul>
            </div>
        </div>
    </div>
    
    <div class="row" id="widgetsQuestion">
        <div class="col-xs-12">
            <div class="well widgetExamples">
                
                <h1 class="PageHeader"><span class="ColoredUnderline GeneralMemucho">Interaktive Fragen</span></h1>

                <p>
                    Einzelne interaktive Fragen können eingebettet werden, um andere Inhalte wie längere Texte aufzulockern. Sie animieren die Leser zur Interaktion.
                    Das erhöht die Konzentration und den Spaß mit deinen Inhalten. Zur Auswahl stehen verschiedene Frage-Antwort-Typen, die für 
                    unterschiedliche Inhalte geeignet sein können.
                </p>
                    
                <div class="row widgetExample" id="singleChoice">
                    <div class="col-sm-12 col-md-2">
                        <h3 class="PageHeader">Single Choice</h3>
                    </div>
                    <div class="col-sm-12 col-md-10">
                        <script src="http://memucho.de/views/widgets/w.js" data-t="question" data-id="3629" data-width="100%" data-maxWidth="100%" data-hideKnowledgeBtn="true"></script>
                    </div>
                </div>

                <div class="row widgetExample" id="multipleChoice">
                    <div class="col-sm-12 col-md-2">
                        <h3 class="PageHeader">Multiple Choice</h3>
                    </div>
                    <div class="col-sm-12 col-md-10">
                        <script src="https://memucho.de/views/widgets/w.js" data-t="question" data-id="3485" data-width="100%" data-maxWidth="100%" data-hideKnowledgeBtn="true"></script>
                    </div>
                </div>

                <div class="row widgetExample" id="dragAndDrop">
                    <div class="col-sm-12 col-md-12">
                        <h3 class="PageHeader">Zuordnen (Drag'n'Drop)</h3>
                        <script src="https://memucho.de/views/widgets/w.js" data-t="question" data-id="3623" data-width="100%" data-maxWidth="100%" data-hideKnowledgeBtn="true"></script>
                    </div>
                </div>
                    
                <div class="row widgetExample" id="flip">
<%--                    <div class="col-sm-12 col-md-2">
                        <h3 class="PageHeader">Umdrehen (Flip-Card, Karteikarte)</h3>
                    </div>--%>
                    <div class="col-sm-12 col-md-12">
                        <h3 class="PageHeader">Umdrehen (Flip-Card, Karteikarte)</h3>
                        <script src="https://memucho.de/views/widgets/w.js" data-t="question" data-id="4261" data-width="100%" data-maxWidth="100%" data-hideKnowledgeBtn="true"></script>
                    </div>
                </div>
                    
                <div class="row widgetExample" id="text">
<%--                    <div class="col-sm-12 col-md-2">
                        <h3 class="PageHeader">Text, Zahl, Datum</h3>
                    </div>--%>
                    <div class="col-sm-12 col-md-12">
                        <h3 class="PageHeader">Text, Zahl, Datum</h3>
                        <script src="https://memucho.de/views/widgets/w.js" data-t="question" data-id="3638" data-width="100%" data-maxWidth="100%" data-hideKnowledgeBtn="true"></script>
                    </div>
                </div>
                    
            </div>
            <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>
        </div>
    </div>
    
    <div class="row" id="widgetQuiz">
        <div class="col-xs-12">
            <div class="well widgetExamples">
                
                <h1 class="PageHeader"><span class="ColoredUnderline GeneralMemucho">Interaktiver Quiz</span></h1>
                <script src="https://memucho.de/views/widgets/w.js" data-t="set" data-id="22" data-width="100%" data-hideKnowledgeBtn="true"></script>
                <p style="margin-top: 20px;">
                    Du kannst ein ganzes Lernset, also eine Sammlung von Fragen, als interaktiven Quiz in deine Seite einbinden.
                </p>
                <ul class="nav nav-pills nav-stacked feature-list">
                    <li><i class="fa fa-check" aria-hidden="true"></i> Alle vorhandenen und eigene Fragen nutzbar</li>
                    <li><i class="fa fa-check" aria-hidden="true"></i> Anzahl Fragen pro Durchlauf einstellbar</li>
                    <li><i class="fa fa-check" aria-hidden="true"></i> Titel und Untertitel für eigene Lernsets frei einstellbar</li>
                    <li><i class="fa fa-check" aria-hidden="true"></i> Alle interaktiven Frage-Typen frei kombinierbar</li>
                    <li><i class="fa fa-check" aria-hidden="true"></i> Rückmeldung für Nutzer bei jeder Frage</li>
                    <li><i class="fa fa-check" aria-hidden="true"></i> Auswertung mit Wissensstand</li>
                    <li><i class="fa fa-check" aria-hidden="true"></i> Ergebnis-Vergleich mit anderen Nutzern</li>
                </ul>
            </div>
            <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>
        </div>
    </div>
    
    <div class="row" id="widgetVideoQuiz">
        <div class="col-xs-12">
            <div class="well widgetExamples">

                <h1 class="PageHeader"><span class="ColoredUnderline GeneralMemucho">Interaktiver Video-Quiz</span></h1>
                <script src="https://memucho.de/views/widgets/w.js" data-t="setVideo" data-id="95" data-width="100%" data-maxWidth="100%" data-hideKnowledgeBtn="true"></script>
                <p style="margin-top: 20px;">
                    Du kannst genauso leicht ein Video mit Fragen verknüpfen und beides bei dir einbinden.
                </p>
                <ul class="nav nav-pills nav-stacked feature-list">
                    <li><i class="fa fa-check" aria-hidden="true"></i> Video hält an der entscheidenden Stelle an</li>
                    <li><i class="fa fa-check" aria-hidden="true"></i> Springe von einer Frage zur Stelle im Video</li>
                    <li><i class="fa fa-check" aria-hidden="true"></i> Rückmeldung für Nutzer bei jeder Frage</li>
                    <li><i class="fa fa-check" aria-hidden="true"></i> Übersicht über Fortschritt durch farbige Symbole</li>
                    <li><i class="fa fa-check" aria-hidden="true"></i> Alle youtube-Videos verwendbar <i class="fa fa-info-circle show-tooltip" style="margin-left: 10px;" data-original-title="Nutze ein vorhandenes Lernset oder erstelle ein eigenes und füge dort die youtube-URL hinzu.">&nbsp;</i></li>
                    <li><i class="fa fa-check" aria-hidden="true"></i> Alle interaktiven Frage-Typen frei kombinierbar</li>
                </ul>
            </div>
            <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>
        </div>
    </div>
    
    <div class="row" id="moreWidgets">
        <div class="col-xs-12">
            <div class="well widgetExamples">
                
                <h1 class="PageHeader"><span class="ColoredUnderline GeneralMemucho">Wissensmanagement und personalisiertes Lernen</span></h1>

                <p style="margin-bottom: 5px;">
                    Gerne stellen wir dir weitere Funktionen als Widget zur Verfügung. Dazu gehören insbesondere
                </p>
                <ul class="nav nav-pills nav-stacked feature-list">
                    <li><i class="fa fa-check" aria-hidden="true"></i> <b>Personalisiertes Lernen:</b> Deine Nutzer lernen individuell und nachhaltig</li>
                    <li><i class="fa fa-check" aria-hidden="true"></i> <b>Wissens- und Lernstandsanzeige</b> für ein Thema und ggf. dessen Teilbereiche</li>
                    <li><i class="fa fa-check" aria-hidden="true"></i> <b>Wissensmanagement:</b> Verwaltung von Wunschwissen aus verschiedenen Themen/Kapiteln</li>
                    <li><i class="fa fa-check" aria-hidden="true"></i> <b>Terminlernen:</b> Deine Nutzer erhalten für die Prüfungsvorbereitung einen individuellen Lernplan</li>
                </ul>
                <p>
                    Für personalisierte Angebote kann eine Nutzerschnittstelle erforderlich sein, die wir gerne nach Absprache einrichten. 
                    Dabei integrieren wir uns auch in vorhandene Single Sign On-Systeme.
                </p>
                <p>
                    Wenn du zum Beispiel zu einer Bildungseinrichtung, einem Bildungsanbieter oder einem Bildungsverlag gehörst, dann können wir
                    dich mit angepassten Lösungen unterstützen. So kannst du mit sehr geringem technischen Aufwand einen echten Zusatznutzen für Lernende schaffen.
                    Alle Widgets können flexibel in bestehende LMS, CMS oder andere Webseiten integriert werden.
                </p>

                <div class="row">
                    <div class="col-md-6 col-xs-12">
                        <h3>Eigenschaften & Vorteile aller Widgets</h3>
                        <ul class="nav nav-pills nav-stacked feature-list">
                            <li><i class="fa fa-check" aria-hidden="true"></i> Responsive Design: Ideal auf allen Bildschirmen</li>
                            <li><i class="fa fa-check" aria-hidden="true"></i> Kleiner Payload (Inhalt asynchron geladen)</li>
                            <li><i class="fa fa-check" aria-hidden="true"></i> Vielfältige Fragetypen und Video-Unterstützung</li>
                            <li><i class="fa fa-check" aria-hidden="true"></i> Alle vorhandenen Inhalte sowie eigene Inhalte frei nutz- und kombinierbar</li>
                            <li><i class="fa fa-check" aria-hidden="true"></i> Flexibel einbindbar bei allen Seiten/Systemen</li>
                            <li><i class="fa fa-check" aria-hidden="true"></i> Kein technischer Wartungsaufwand</li>
                        </ul>
                    </div>
                    <div class="col-md-6 col-xs-12">
                        <h3>Konfiguration</h3>
                        <ul class="nav nav-pills nav-stacked feature-list">
                            <li><i class="fa fa-check" aria-hidden="true"></i> Breite flexibel einstellbar</li>
                            <li><i class="fa fa-check" aria-hidden="true"></i> Optionaler Wunschwissen-Button <i class="fa fa-info-circle show-tooltip" style="margin-left: 10px;" data-original-title="Die Schaltfläche 'Zum Wunschwissen hinzufügen' kann ausgeblendet werden. Sie erleichtert Nutzern das Weiterlernen auf memucho."></i></li>
                            <li><i class="fa fa-check" aria-hidden="true"></i> Optionales Branding <i class="fa fa-info-circle show-tooltip" style="margin-left: 10px;" data-original-title="Für Organisationen: Das memucho-Logo kann entfernt und das Layout an das Corporate Design angepasst werden."></i></li>
                        </ul>
                    </div>
                </div>
                <p style="margin-bottom: 30px;">
                    Technische Hilfe zur Einbettung der Widgets findest du <a href="<%= Links.HelpWidget()%>">hier</a>, auch speziell für die Systeme 
                    <a href="<%= Links.HelpWidgetWordpress() %>">Wordpress</a>, <a href="<%= Links.HelpWidgetMoodle() %>">Moodle</a> und 
                    <a href="<%= Links.HelpWidgetBlackboard() %>">Blackboard</a>.
                </p>

            </div>
            <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>
        </div>
    </div>

    <div class="row" id="contact">
        <div class="col-xs-12">
            <div class="well" style="margin-top: 25px;">

                <h2 class="PageHeader">
                    <span class="ColoredUnderline GeneralMemucho">Kontakt</span>
                </h2>

                <div class="row">
                    <div class="col-xs-4 col-md-3 TeamPic">
                        <img src="/Images/Team/team_christof_20170404_P3312344_155.jpg" alt="Foto Christof"/>
                    </div>
                    <div class="col-xs-8 col-md-9">
                        <p>
                            Du hast Fragen? Du hättest gerne ein angepasstes Angebot? Sprich uns einfach an, wir freuen uns über deine Nachricht.
                            Dein Ansprechpartner für alle Fragen zum Widget ist:<br/>
                        </p>
                        <p>
                            <strong>Robert Mischke</strong><br/>
                            E-Mail: <span class="mailme">team at memucho dot de</span><br/>
                            Telefon:+49-178 186 68 48<br/>
                        </p>
                        
                    </div>
                </div>

            </div>
            <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>
        </div>
    </div>

</asp:Content>