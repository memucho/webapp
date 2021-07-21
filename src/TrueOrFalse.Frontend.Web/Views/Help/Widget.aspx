<%@ Page Title="Widget-Integration von memucho" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<BaseModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Help/Widget.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/mailto") %>
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Widgets", Url = Links.HelpWidget() , ToolTipText  = "Widgets"});
       Model.TopNavMenu.IsCategoryBreadCrumb = false;
       Model.TopNavMenu.SetBreadCrumb = false;
       Model.TopNavMenu.IsWidgetOrKnowledgeCentral = true;  %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("~/Views/Help/WidgetMenu.ascx", new WidgetMenuModel());  %>

    <div class="row">
        <div class="col-xs-12">

            <div class="well">

                <h1 class="PageHeader"><span class="ColoredUnderline GeneralMemucho">Inhalte in die eigene Webseite einbinden</span></h1>
                <p class="teaserText">
                    Die Inhalte und Technologie von memucho können als Widget leicht in bestehende Webseiten integriert werden. 
                    Egal ob auf dem privaten Blog, der Vereins- oder Unternehmensseite, oder dem Schul- oder Lernmanagementsystem: Nötig ist eine Zeile HTML-Code, 
                    die du einfach von memucho kopieren kannst.
                </p>
                   
                
                <ul class="nav nav-pills nav-stacked">
                    <li><a href="#quizOnPage"><i class="fa fa-caret-right">&nbsp;</i>Quiz auf deiner Webseite: Wissenstest mit Auswertung einbinden</a></li>
                    <li><a href="#video"><i class="fa fa-caret-right">&nbsp;</i>Video mit passendem Quiz einbinden</a></li>
                    <li><a href="#single"><i class="fa fa-caret-right">&nbsp;</i>Einzelne Fragen per Widget einbinden</a></li>
                    <li><a href="#terms"><i class="fa fa-caret-right">&nbsp;</i>Nutzung für Einzelpersonen und Organisationen</a></li>
                </ul>
            </div>
        </div>
    </div>

    <div class="row" id="quizOnPage">
        <div class="col-xs-12">
            <div class="well explanationBox">

                <h2 class="PageHeader">
                    <a></a><span class="ColoredUnderline GeneralMemucho">Quiz auf deiner Webseite: Wissenstest mit Auswertung einbinden</span>
                </h2>
                <p>
                    Du kannst einen Quiz zu einem ganzen Lernset auf deiner Webseite einbinden. So können deine Webseitenbesucher ihr Wissen testen - und das macht vielen Spaß.
                    Und du hast eine einfache Möglichkeit, schnell gute Inhalte auf deine Seite zu bringen.
                </p>
                <p>
                    Ein memucho-Widget hilft dir dabei, viele Nutzer auf deine Seite zu bekommen. Durch den Quiz ist die Verweildauer 
                    höher und das wertschätzen Suchmaschinen mit einer besseren Position.
                </p>
                <p>
                    Alle Inhalte bei memucho sind für das Widget nutzbar. <strong>Du kannst vorhandene Inhalte frei verwenden, neu zusammenstellen und bei Bedarf mit eigenen ergänzen.</strong>
                    Wenn du kein perfekt passendes Lernset findest, erstelle einfach selbst eines und füge vorhandene oder neue Fragen von dir hinzu.
                </p>
                <p>
                    So geht's:
                </p>
                <h4>1. Lernset finden und Einbetten-Dialog öffnen</h4>
                <p class="screenshotExplanation">
                    Suche dir das passende <a href="<%= Links.SetsAll() %>">Lernset</a> aus oder <a href="<%= Links.SetCreate() %>">erstelle dir selbst eines</a>.
                    Auf der Lernset-Seite findest du den Link <code><i class="fa fa-code fa-code">&nbsp;</i>Einbetten</code> (vgl. Bild).
                    Klicke darauf.
                </p>
                <p class="screenshot">
                    <img src="/Images/Screenshots/SetEmbedLink.png" />
                </p>

                <h4>2. HTML-Codezeile kopieren</h4>
                <p class="screenshotExplanation">
                    Es öffnet sich das Dialogfenster zur Einbettung. Oben findest du die HTML-Codezeile (vgl. Bild). 
                    Markiere sie und kopiere sie an die Stelle deiner Webseite, wo der Quiz bei dir erscheinen soll.<br/>
                </p>
                <p class="screenshotExplanation">
                    Wenn du magst, kannst du bei den "Einstellungen" noch ein paar Dinge verändern.
                    Unten im Dialogfenster siehst du gleich eine Vorschau, wie das Quiz-Widget bei dir aussehen wird.
                </p>
                <p class="screenshot">
                    <img src="/Images/Screenshots/SetEmbedModal.png" />
                </p>

                <h4>3. Code in die eigene Webseite einfügen</h4>                    
                <p class="screenshotExplanation">
                    Die Code-Zeile aus dem Dialog kannst du direkt an die Stelle deiner Webseite kopieren, wo der Quiz erscheinen soll.
                    Achte darauf, dass du dich in einem Modus befindest, wo du HTML-Code einfügen darfst (als Klartext, ohne Formatierungen). 
                    Oft gibt es dafür einen Umschalter vom Layout- in den HTML-/Text-Modus. 
                </p>
                <p class="screenshotExplanation">
                    Wo genau, das zeigen wir dir hier für die folgenden Systeme mit einer Schritt-für-Schritt-Anleitung: 
                </p>
                <ul class="screenshotExplanation">
                    <li><a href="<%= Links.HelpWidgetWordpress() %>">memuchos Quiz-Widget in <strong>Wordpress</strong> einbinden</a></li>
                    <li><a href="<%= Links.HelpWidgetMoodle() %>">memuchos Quiz-Widget in <strong>Moodle</strong> einbinden</a></li>
                    <li><a href="<%= Links.HelpWidgetBlackboard() %>">memuchos Quiz-Widget in <strong>Blackboard</strong> einbinden</a></li>

                </ul>
            </div>
            <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>
        </div>
    </div>
    
    
    <div class="row" id="video">
        <div class="col-xs-12">
            <div class="well explanationBox">

                <h2 class="PageHeader">
                    <a></a><span class="ColoredUnderline GeneralMemucho">Video mit passendem Quiz einbinden</span>
                </h2>
                <p>
                    memucho verbindet Videos direkt mit den passenden Fragen. So können Nutzer ein Video sehen, 
                    im richtigen Moment die richtigen Fragen beantworten und nach dem Video ihr Wissen testen.
                    Beides geht auch direkt auf deiner Webseite: Das Video und dadrunter die passenden Fragen werden nahtlos eingebunden.
                </p>
                <p>
                    Ein memucho-Widget hilft dir dabei, viele Nutzer auf deine Seite zu bekommen. Quizze machen vielen Nutzern Spaß, dadurch erhöht sich die Verweildauer 
                    auf deiner Seite und das wertschätzen Suchmaschinen mit einer besseren Position.
                </p>
                <p>
                    Alle Inhalte bei memucho sind für das Widget nutzbar. Videos kannst du von youtube direkt integrieren. 
                    <strong>Du kannst vorhandene Inhalte frei verwenden, neu zusammenstellen und bei Bedarf mit eigenen ergänzen.</strong>
                    Wenn du kein perfekt passendes Lernset findest, suche dir das Video bei youtube und erstelle einfach selbst ein Lernset.
                </p>
                <p>
                    So geht's:
                </p>
                <h4>1. Video-Lernset finden und Einbetten-Dialog öffnen</h4>
                <p class="screenshotExplanation">
                    Suche dir das passende <a href="<%= Links.SetsAll() %>">Lernset</a> mit einem Video aus oder <a href="<%= Links.SetCreate() %>">erstelle dir selbst eines</a>.
                    Auf der Lernset-Seite findest du den Link <code><i class="fa fa-code fa-code">&nbsp;</i>Einbetten</code> (vgl. Bild).
                    Klicke darauf.
                </p>
                <p class="screenshot">
                    <img src="/Images/Screenshots/SetVideoEmbedLink.png" />
                </p>

                <h4>2. HTML-Codezeile kopieren</h4>
                <p class="screenshotExplanation">
                    Es öffnet sich das Dialogfenster zur Einbettung. Oben findest du die HTML-Codezeile (vgl. Bild). 
                    Markiere sie und kopiere sie an die Stelle deiner Webseite, wo das Video mit dem Quiz bei dir erscheinen soll.<br/>
                </p>
                <p class="screenshotExplanation">
                    Wenn du magst, kannst du bei den "Einstellungen" noch ein paar Dinge verändern.
                    Unten im Dialogfenster siehst du gleich eine Vorschau, wie das Quiz-Widget bei dir aussehen wird.
                </p>
                <p class="screenshot">
                    <img src="/Images/Screenshots/SetVideoEmbedModal.png" />
                </p>

                <h4>3. Code in die eigene Webseite einfügen</h4>                    
                <p class="screenshotExplanation">
                    Die Code-Zeile aus dem Dialog kannst du direkt an die Stelle deiner Webseite kopieren, wo das Video mit dem Quiz erscheinen soll.
                    Achte darauf, dass du dich in einem Modus befindest, wo du HTML-Code einfügen darfst (als Klartext, ohne Formatierungen). 
                    Oft gibt es dafür einen Umschalter vom Layout- in den HTML-/Text-Modus. 
                </p>
                <p class="screenshotExplanation">
                    Wo genau, das zeigen wir dir hier für die folgenden Systeme mit einer Schritt-für-Schritt-Anleitung: 
                </p>
                <ul class="screenshotExplanation">
                    <li><a href="<%= Links.HelpWidgetWordpress() %>">memuchos Video-Quiz-Widget in <strong>Wordpress</strong> einbinden</a></li>
                    <li><a href="<%= Links.HelpWidgetMoodle() %>">memuchos Video-Quiz-Widget in <strong>Moodle</strong> einbinden</a></li>
                    <li><a href="<%= Links.HelpWidgetBlackboard() %>">memuchos Video-Quiz-Widget in <strong>Blackboard</strong> einbinden</a></li>
                </ul>
                                
            </div>
            <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>
        </div>
    </div>

    <div class="row" id="single">
        <div class="col-xs-12">
            <div class="well explanationBox">
                <h2 class="PageHeader">
                    <a></a><span class="ColoredUnderline GeneralMemucho">Einzelne Fragen per Widget einbinden</span>
                </h2>
                <p>
                    Du kannst auch eine einzelne Frage nahtlos auf deiner Webseite einbinden. So kannst du zum Beispiel bei einem Blog-Beitrag den Text auflockern und animierst 
                    die Leser dazu zu interagieren. Das erhöht die Konzentration und den Spaß mit deinen Inhalten, ohne zu stark von deinen Inhalten abzulenken.
                </p>
                <p>
                    Alle Fragen bei memucho sind für das Widget nutzbar. <strong>Du kannst vorhandene Fragen frei verwenden oder eigene erstellen.</strong>
                </p>
                <p>
                    So geht's:
                </p>
                <h4>1. Frage finden und Einbetten-Dialog öffnen</h4>
                <p class="screenshotExplanation">
                    Suche dir eine passende <a href="<%= Links.QuestionsAll() %>">Frage</a> aus oder <a href="<%= Links.CreateQuestion() %>">erstelle schnell eine eigene</a>.
                    Auf der Frageseite findest du den Link <code><i class="fa fa-code fa-code">&nbsp;</i>Einbetten</code> (vgl. Bild).
                    Klicke darauf.
                </p>
                <p class="screenshot">
                    <img src="/Images/Screenshots/QuestionEmbedLink.png" />
                </p>

                <h4>2. HTML-Codezeile kopieren</h4>
                <p class="screenshotExplanation">
                    Es öffnet sich das Dialogfenster zur Einbettung. Oben findest du die HTML-Codezeile (vgl. Bild). 
                    Markiere sie und kopiere sie an die Stelle deiner Webseite, wo die Frage bei dir erscheinen soll.<br/>
                </p>
                <p class="screenshotExplanation">
                    Wenn du magst, kannst du bei den "Einstellungen" noch ein paar Dinge verändern.
                    Unten im Dialogfenster siehst du gleich eine Vorschau, wie das Frage-Widget bei dir aussehen wird.
                </p>
                <p class="screenshot">
                    <img src="/Images/Screenshots/QuestionEmbedModal.png" />
                </p>

                <h4>3. Code in die eigene Webseite einfügen</h4>                    
                <p class="screenshotExplanation">
                    Die Code-Zeile aus dem Dialog kannst du direkt an die Stelle deiner Webseite kopieren, wo der Quiz erscheinen soll.
                    Achte darauf, dass du dich in einem Modus befindest, wo du HTML-Code einfügen darfst (als Klartext, ohne Formatierungen). 
                    Oft gibt es dafür einen Umschalter vom Layout- in den HTML-/Text-Modus. 
                </p>
                <p class="screenshotExplanation">
                    Wo genau, das zeigen wir dir hier für die folgenden Systeme mit einer Schritt-für-Schritt-Anleitung: 
                </p>
                <ul class="screenshotExplanation">
                    <li><a href="<%= Links.HelpWidgetWordpress() %>">memuchos Frage-Widget in <strong>Wordpress</strong> einbinden</a></li>
                    <li><a href="<%= Links.HelpWidgetMoodle() %>">memuchos Frage-Widget in <strong>Moodle</strong> einbinden</a></li>
                    <li><a href="<%= Links.HelpWidgetBlackboard() %>">memuchos Frage-Widget in <strong>Blackboard</strong> einbinden</a></li>
                </ul>
            </div>
            <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>
        </div>
    </div>
    
    <div class="row" id="terms">
        <div class="col-xs-12">
            <div class="well explanationBox">
                <h2 class="PageHeader">
                    <a></a><span class="ColoredUnderline GeneralMemucho">Für Einzelpersonen und Organisationen</span>
                </h2>
                <p>
                    Die Nutzung der memucho-Widgets ist <strong>für Einzelpersonen kostenlos und werbefrei</strong>. 
                    Für den Einsatz von mehr als fünf Widgets ist ein Pro-Account notwendig (<a href="<%= Links.Membership() %>">jetzt Pro-Mitglied werden</a>).
                </p>
                <p>
                    Für Organisationen haben wir verschiedene Angebote: &nbsp; <a href="<%= Links.WidgetPricing() %>" class="btn btn-default">Zu den Angeboten und Preisen</a>
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