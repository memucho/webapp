﻿<%@ Page Title="Widget-Integration von memucho" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Help/Widget.css" rel="stylesheet" />
    <script type="text/javascript" >

        $(function () {
            $("span.mailme")
                .each(function() {
                    var spt = this.innerHTML;
                    var at = / at /;
                    var dot = / dot /g;
                    var addr = spt.replace(at, "@").replace(dot, ".");
                    $(this).after('<a href="mailto:' + addr + '" title="Schreibe eine E-Mail">' + addr + '</a>');
                    $(this).remove();
                });
        });
    </script>    

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="row">
        <div class="col-xs-12">

            <div class="well">

                <h1 class="PageHeader"><span class="ColoredUnderline GeneralMemucho">Lerninhalte in die eigene Webseite einbinden</span></h1>
                <p class="teaserText">
                    Die Lerntechnologie und die Lerninhalte von memucho können als Widget leicht in bestehende Webseiten integriert werden. 
                    Egal ob auf dem privaten Blog, der Vereins- oder Unternehmensseite, oder dem Schul- oder Lernmanagementsystem: Nötig ist eine Zeile HTML-Code, 
                    die du einfach von memucho kopieren kannst.
                </p>
                   
            </div>
        </div>
    </div>



    <div class="row">
        <div class="col-xs-12">
            <div class="well explanationBox">

                <h2 class="PageHeader">
                    <span class="ColoredUnderline GeneralMemucho">Quiz auf deiner Webseite: Wissenstest mit Auswertung einbinden</span>
                </h2>
                <p>
                    Du kannst einen Quiz zu einem ganzen Fragesatz auf deiner Webseite einbinden. So können deine Webseitenbesucher ihr Wissen testen - und das macht vielen Spaß.
                    Und du hast eine einfache Möglichkeit, schnell gute Inhalte auf deine Seite zu bringen.
                </p>
                <p>
                    Ein memucho-Widget hilft dir dabei, viele Nutzer auf deine Seite zu bekommen. Durch den Quiz ist die Verweildauer 
                    höher und das wertschätzen Suchmaschinen mit einer besseren Position.
                </p>
                <p>
                    Alle Inhalte bei memucho sind für das Widget nutzbar. <strong>Du kannst vorhandene Inhalte frei verwenden, neu zusammenstellen und bei Bedarf mit eigenen ergänzen.</strong>
                    Wenn du keinen perfekt passenden Fragesatz findest, erstelle einfach selbst einen und füge vorhandene oder neue Fragen von dir hinzu.
                </p>
                <p>
                    So geht's:
                </p>
                <h4>1. Fragesatz finden und Einbetten-Dialog öffnen</h4>
                <p class="screenshotExplanation">
                    Suche dir den passenden <a href="<%= Links.SetsAll() %>">Fragesatz</a> aus oder <a href="<%= Links.SetCreate() %>">erstelle dir selbst einen</a>.
                    Auf der Fragesatzseite findest du den Link <code><i class="fa fa-code fa-code">&nbsp;</i>Einbetten</code> (vgl. Bild).
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
                    Wo genau, das zeigen wir dir hier für die folgenden Systeme: 
                </p>
                <ul class="screenshotExplanation">
                    <li>Wordpress</li>
                    <li>Moodle</li>
                    <li>Blackboard</li>
                </ul>
                <h3>Fragen oder Probleme?</h3>
                <p>
                    Funktioniert es nicht? Sind noch Fragen offen? Kein Problem, melde dich einfach bei uns, wir helfen dir gerne weiter. Christof erreichst du unter 01577-6825707 
                    oder per E-Mail an <span class="mailme">christof at memucho dot de</span>.
                </p>
            </div>
        </div>
    </div>
    
    
    <div class="row">
        <div class="col-xs-12">
            <div class="well explanationBox">

                <h2 class="PageHeader">
                    <span class="ColoredUnderline GeneralMemucho">Video mit passendem Quiz einbinden</span>
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
                    Wenn du keinen perfekt passenden Fragesatz findest, suche dir das Video bei youtube und erstelle einfach selbst einen Fragesatz.
                </p>
                <p>
                    So geht's:
                </p>
                <h4>1. Video-Fragesatz finden und Einbetten-Dialog öffnen</h4>
                <p class="screenshotExplanation">
                    Suche dir den passenden <a href="<%= Links.SetsAll() %>">Fragesatz</a> mit einem Video aus oder <a href="<%= Links.SetCreate() %>">erstelle dir selbst einen</a>.
                    Auf der Fragesatzseite findest du den Link <code><i class="fa fa-code fa-code">&nbsp;</i>Einbetten</code> (vgl. Bild).
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
                    Wo genau, das zeigen wir dir hier für die folgenden Systeme: 
                </p>
                <ul class="screenshotExplanation">
                    <li>Wordpress</li>
                    <li>Moodle</li>
                    <li>Blackboard</li>
                </ul>
                <h3>Fragen oder Probleme?</h3>
                <p>
                    Funktioniert es nicht? Sind noch Fragen offen? Kein Problem, melde dich einfach bei uns, wir helfen dir gerne weiter. Christof erreichst du unter 01577-6825707 
                    oder per E-Mail an <span class="mailme">christof at memucho dot de</span>.
                </p>
                                
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-xs-12">
            <div class="well explanationBox">
                <h2 class="PageHeader">
                    <span class="ColoredUnderline GeneralMemucho">Einzelne Fragen per Widget einbinden</span>
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
                    Wo genau, das zeigen wir dir hier für die folgenden Systeme: 
                </p>
                <ul class="screenshotExplanation">
                    <li>Wordpress</li>
                    <li>Moodle</li>
                    <li>Blackboard</li>
                </ul>
                <h3>Fragen oder Probleme?</h3>
                <p>
                    Funktioniert es nicht? Sind noch Fragen offen? Kein Problem, melde dich einfach bei uns, wir helfen dir gerne weiter. Christof erreichst du unter 01577-6825707 
                    oder per E-Mail an <span class="mailme">christof at memucho dot de</span>.
                </p>
            </div>
        </div>
    </div>
    
    <div class="row">
        <div class="col-xs-12">
            <div class="well explanationBox">
                <h2 class="PageHeader">
                    <span class="ColoredUnderline GeneralMemucho">Nutzung für kommerzielle Anbieter</span>
                </h2>
                <p>
                    Die Nutzung der memucho-Widgets ist für nicht-kommerzielle Zwecke und Einzelpersonen kostenlos und werbefrei. 
                    Für Nutzer mit kommerziellen Interessen haben wir verschiedene Angebote (Preisliste).
                </p>

            </div>
        </div>
    </div>

</asp:Content>