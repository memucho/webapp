<%@ Page Title="memucho-Quiz in Moodle einbetten" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<BaseModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Help/Widget.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/mailto") %>
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Widgets", Url = Links.HelpWidget(), ToolTipText = "Widgets"});
       Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "In Moodle einbinden", Url = Links.HelpWidgetMoodle(), ToolTipText = "In Moodle einbinden"});
       Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("~/Views/Help/WidgetMenu.ascx", new WidgetMenuModel());  %>

    <div class="row">
        <div class="col-xs-12">

            <div class="well">

                <h1 class="PageHeader"><span class="ColoredUnderline GeneralMemucho">In Moodle einbinden</span></h1>
                <p class="teaserText">
                    Die Lerntechnologie und die Lerninhalte von memucho können als Widget leicht in Moodle integriert werden. 
                    Sie erscheinen dann zum Beispiel direkt bei den Kursunterlagen für die Schüler oder Studenten.
                    Nötig ist eine Zeile HTML-Code, die du einfach von memucho kopieren kannst. memucho hat verschiedene Widgets zur Auswahl 
                    (<a href="<%= Links.WidgetExamples() %>">zur Widget-Übersicht</a>).
                </p>
                <p class="teaserText">
                    So sieht das aus:
                </p>
                <p class="screenshot" style="text-align: center;">
                    <img src="/Images/Screenshots/widget-moodle_f05_doneview.png" />
                </p>
                   
            </div>
        </div>
    </div>



    <div class="row">
        <div class="col-xs-12">
            <div class="well explanationBox">

                <h2 class="PageHeader">
                    <span class="ColoredUnderline GeneralMemucho">Einbindung memucho-Widget in Moodle: Anleitung</span>
                </h2>
                <p>
                    Auf der Seite des Lernsets, Video-Lernsets oder der einzelnen Frage findest du einen Link <code><i class="fa fa-code">&nbsp;</i>Einbetten</code>. 
                    Dort kannst das Widget, wenn du möchtest, konfigurieren und erhältst die Code-Zeile zur Einbettung des Widgets (<a href="<%= Links.HelpWidget() %>">mehr dazu</a>).
                    Beispiele für die Widgets zeigen wir in der <a href="<%= Links.WidgetExamples() %>">Widget-Übersicht</a>.
                </p>
                <p>
                    Diese Code-Zeile kannst du in Moodle einfach an die Stelle einfügen, wo der Quiz erscheinen soll.
                    Dafür musst du dich aber im HTML-Modus befinden. Das geht ganz leicht. 
                </p>
                <p>
                    Zunächst die <strong>Kurzfassung</strong>:
                </p>
                <ul>
                    <li class="screenshotExplanation">
                        An die gewünschte Stelle "Material oder Aktivität hinzufügen", als Material "Textfeld" auswählen.
                    </li>
                    <li>
                        Im Textfeld auf "Mehr Symbole anzeigen" <img src="/Images/Screenshots/widget-moodle-1-mehrFelder-small.png" /> klicken und anschließend
                        den HTML-Modus einschalten <img src="/Images/Screenshots/widget-moodle-2-HTMLmode-small.png" />.
                    </li>
                    <li class="screenshotExplanation">
                        Jetzt kannst du die Code-Zeile einfügen, speichern, fertig
                    </li>
                </ul>
                
                <p style="margin-top: 20px;">
                    Und nun die <strong>ausführliche Schritt-für-Schritt-Anleitung</strong>:
                </p>
                
                <h4>1. Material hinzufügen</h4>
                <p class="screenshotExplanation">
                    Gehe an die gewünschte Stelle im Kurs, wo das Widget erscheinen soll und klicke dort auf "+ Material oder Aktivität hinzufügen".
                </p>
                <p class="screenshot">
                    <img src="/Images/Screenshots/widget-moodle_f01_add.png" />
                </p>

                <h4>2. Textfeld auswählen und hinzufügen</h4>
                <p class="screenshotExplanation">
                    Wähle in der Liste unten das Arbeitsmaterial "Textfeld" aus und klicke auf "Hinzufügen".
                </p>
                <p class="screenshot">
                    <img src="/Images/Screenshots/widget-moodle_f02_addTextField.png" />
                </p>

                <h4>3. HTML-Modus aktivieren</h4>
                <p class="screenshotExplanation">
                    Jetzt kannst du das Textfeld mit Inhalt füllen. Du kannst zum Beispiel einen Einführungssatz hinzufügen. 
                    Klicke dann bei den Symbolen zuerst auf "Mehr Symbole anzeigen" (ganz links) und dann in der sich öffnenden zweiten Zeile auf die Schaltfläche ganz rechts, 
                    um in den HTML-Modus zu wechseln.
                </p>
                <p class="screenshot">
                    <img src="/Images/Screenshots/widget-moodle_f03_texfield_options.png" />
                </p>

                <h4>4. Einbettungs-Code einfügen</h4>
                <p class="screenshotExplanation">
                    Im Textfeld siehst du jetzt auch die HTML-Tags (hier das &lt;p&gt; zur Markierung eines Textabsatzes). Gehe mit dem Cursor zu der Stelle, 
                    wo das Widget erscheinen soll und kopiere die Code-Zeile von memucho über die Zwischenablage in das Textfeld. Jetzt speichern.
                </p>
                <p class="screenshot">
                    <img src="/Images/Screenshots/widget-moodle_f04_textfield-with-embedcode.png" />
                </p>

                <h4>5. Fertig!</h4>
                <p class="screenshotExplanation">
                    Schon fertig! Auf der Kursseite siehst du nun, wie das Widget eingebettet ist. 
                    Deine Schüler oder Studenten können direkt loslernen oder ihr Wissen testen.
                </p>
                <p class="screenshot">
                    <img src="/Images/Screenshots/widget-moodle_f05_doneview.png" />
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