<%@ Page Title="memucho-Quiz in Blackboard einbetten" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<BaseModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Help/Widget.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/mailto") %>
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Widgets", Url = Links.HelpWidget(), ToolTipText  = "Widget"});
       Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "In Blackboard einbinden", Url = Links.HelpWidgetBlackboard(), ToolTipText  = "In Blackboard einbinden"});
       Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% Html.RenderPartial("~/Views/Help/WidgetMenu.ascx", new WidgetMenuModel());  %>

    <div class="row">
        <div class="col-xs-12">

            <div class="well">

                <h1 class="PageHeader"><span class="ColoredUnderline GeneralMemucho">In Blackboard einbinden</span></h1>
                <p class="teaserText">
                    Die Lerntechnologie und die Lerninhalte von memucho können als Widget leicht in Blackboard integriert werden. 
                    Sie erscheinen dann zum Beispiel direkt bei den Kursunterlagen für die Schüler oder Studenten.
                    Nötig ist eine Zeile HTML-Code, die du einfach von memucho kopieren kannst. memucho hat verschiedene Widgets zur Auswahl 
                    (<a href="<%= Links.WidgetExamples() %>">zur Widget-Übersicht</a>).
                </p>
                <p class="teaserText">
                    So sieht das aus:
                </p>
                <p class="screenshot" style="text-align: center;">
                    <img src="/Images/Screenshots/widget-blackboard_f04_done.png" />
                </p>
                   
            </div>
        </div>
    </div>



    <div class="row">
        <div class="col-xs-12">
            <div class="well explanationBox">

                <h2 class="PageHeader">
                    <span class="ColoredUnderline GeneralMemucho">Einbindung memucho-Widget in Blackboard: Anleitung</span>
                </h2>
                <p>
                    Auf der Seite des Lernsets, Video-Lernsets oder der einzelnen Frage findest du einen Link <code><i class="fa fa-code">&nbsp;</i>Einbetten</code>. 
                    Dort kannst das Widget, wenn du möchtest, konfigurieren und erhältst die Code-Zeile zur Einbettung des Widgets (<a href="<%= Links.HelpWidget() %>">mehr dazu</a>).
                    Beispiele für die Widgets zeigen wir in der <a href="<%= Links.WidgetExamples() %>">Widget-Übersicht</a>.
                </p>
                <p>
                    Diese Code-Zeile kannst du in Blackboard einfach an die Stelle einfügen, wo der Quiz erscheinen soll.
                    Dafür musst du dich aber im HTML bzw. Text-Modus befinden. Das geht ganz leicht.
                </p>
                
                <p>
                    Zunächst die <strong>Kurzfassung</strong>:
                </p>
                <ul>
                    <li class="screenshotExplanation">
                        An der gewünschten Stelle ein neues "Element" erstellen (unter "Inhalt erstellen") oder ein vorhandenes Element bearbeiten.
                    </li>
                    <li>
                        Im Textfeld ggf. in der Symbolleiste ganz rechts "Mehr anzeigen" <img src="/Images/Screenshots/widget-blackboard_button-more.png"/>, dann auf das Symbol 
                        <img src="/Images/Screenshots/widget-blackboard_button-html.png"/> "HTML-Codeansicht" (unten rechts) klicken.
                    </li>
                    <li class="screenshotExplanation">
                        Jetzt kannst du die Code-Zeile einfügen, 2x speichern, fertig
                    </li>
                </ul>
                
                <p style="margin-top: 20px;">
                    Und nun die <strong>ausführliche Schritt-für-Schritt-Anleitung</strong>:
                </p>
                <h4>1. Element in Blackboard bearbeiten oder ein neues erstellen</h4>
                <p class="screenshotExplanation">
                    Stelle sicher, dass du dich im Bearbeitungsmodus befindest, der Umschalter ist oben rechts.
                    Erstelle ein neues Element oder öffne ein vorhandenes, wo das Widget erscheinen soll. Du kannst zum Beispiel ein Element in einem Kursordner erstellen.
                </p>
                <p class="screenshot">
                    <img src="/Images/Screenshots/widget-blackboard_f01_add.png" />
                </p>

                <h4>2. In die HTML-Codeansicht wechseln</h4>
                <p class="screenshotExplanation">
                    Das neue Element muss nun einen Titel ("Name") bekommen. In dem großen Textfeld kannst du noch einen Text hinzufügen, wenn du magst.<br />
                    Falls du im Textfeld nur eine Leiste mit Symbolen siehst, musst du ganz rechts auf den Schalter "Mehr anzeigen" <img src="/Images/Screenshots/widget-blackboard_button-more.png"/> klicken. 
                    Dann kannst du in der dritten Zeile der Symbolleiste ganz rechts auf den Button "HTML-Codeansicht" 
                    <img src="/Images/Screenshots/widget-blackboard_button-html.png"/> klicken.
                </p>
                <p class="screenshot">
                    <img src="/Images/Screenshots/widget-blackboard_f02_edit-element_short.png" />
                </p>
                
                <h4>3. Einbettungs-Code einfügen</h4>
                <p class="screenshotExplanation">
                    Es öffnet sich ein neues Fenster, wo du den Inhalt des Textfeldes mit den HTML-Tags siehst (hier das &lt;p&gt; zur Markierung eines Textabsatzes). 
                    Gehe mit dem Cursor zu der Stelle, wo das Widget erscheinen soll und kopiere die Code-Zeile von memucho über die Zwischenablage in das Textfeld. 
                    Dann auf "Aktualisieren" klicken, um zu speichern.
                </p>
                <p class="screenshot">
                    <img src="/Images/Screenshots/widget-blackboard_f03_edit-html.png" />
                </p>

                <h4>4. Fertig!</h4>
                <p class="screenshotExplanation">
                    Jetzt musst du nur noch dein Element speichern, indem du auf "Senden" klickst und 
                    schon kannst du deinen Kursinhalt anschauen und sehen, dass alles geklappt hat. 
                    Die Kursteilnehmer können sich so direkt in Blackboard mit den Inhalten auseinandersetzen. Viel Spaß!
                </p>
                <p class="screenshot">
                    <img src="/Images/Screenshots/widget-blackboard_f04_done_short.png" />
                </p>

            </div>
            <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>
        </div>
    </div>
    
   
    <div class="row">
        <div class="col-xs-12">
            <div class="well" style="margin-top: 25px;">

                <h2 class="PageHeader">
                    <span class="ColoredUnderline GeneralMemucho">Fragen oder Probleme?</span>
                </h2>
                <div class="row">
                    <div class="col-xs-4 col-md-3 TeamPic">
                        <img src="/Images/Team/team_christof_20170404_P3312344_155.jpg" alt="Foto Christof"/>
                    </div>
                    <div class="col-xs-8 col-md-9">
                        <p>
                            Sind noch Fragen offen? Gibt es technische Probleme? Kein Problem, melde dich einfach bei uns, wir helfen dir gerne weiter.
                            Dein Ansprechpartner für alle Fragen zum Widget ist:<br/>
                        </p>
                        <p>
                            <strong>Robert Mischke</strong><br/>
                            E-Mail: <span class="mailme">team at memucho dot de</span><br/>
                            Telefon: +49-178 186 68 48<br/>
                        </p>
                        
                    </div>
                </div>

            </div>
            <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>
        </div>
    </div>

</asp:Content>