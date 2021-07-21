<%@ Page Title="Hilfe & FAQ" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<HelpModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Links.HelpFAQ() %>">

    <link href="/Views/Help/FAQ.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "FAQ", Url = Links.HelpFAQ(), ToolTipText  = "FAQ"});
       Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
<script type="text/javascript" >

    /* Source http://www.html-advisor.com/javascript/hide-email-with-javascript-jquery/ */
    $(function () {
        var spt = $('span.mailme');
        var at = / at /;>
        var dot = / dot /g;
        var addr = $(spt).text().replace(at, "@").replace(dot, ".");
        $(spt).after('<a href="mailto:' + addr + '" title="Send an email">' + addr + '</a>').hover(function () { window.status = "Send a letter!"; }, function () { window.status = ""; });
        $(spt).remove();


        $(document).ready(function () { //http://stackoverflow.com/questions/12008389/linking-to-a-section-of-an-accordion-from-another-page#answer-12008992
            location.hash && $(location.hash.replace("#", "#FaqText")).collapse('show');
        });
    });
</script>    
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="PageHeader">FAQ - Häufig gestellte Fragen</h2>
    
    <div class="panel-group" id="FaqAccordion" role="tablist" aria-multiselectable="true">
        <% FAQAccordeonItem currentFaqItem;%>
        
        <div class="panel panel-default">
            <% currentFaqItem = new FAQAccordeonItem("HowMemuchoWorks"); %>
            <div class="panel-heading" role="tab" id="<%= currentFaqItem.ItemHtmlIdHeading %>">
                <h4 class="panel-title">
                    <a data-toggle="collapse" data-parent="#FaqAccordion" href="#<%= currentFaqItem.ItemHtmlIdText %>" aria-expanded="true" aria-controls="<%= currentFaqItem.ItemHtmlIdText %>">
                        Was ist memucho?
                    </a>
                </h4>
            </div>
            <div id="<%= currentFaqItem.ItemHtmlIdText %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="<%= currentFaqItem.ItemHtmlIdHeading %>">
                <div class="panel-body">
                    <p>
                        memucho ist dein Wissens-Assistent und intelligentes Lern-Tool. memucho erleichtert es dir, dir interessante und wissenswerte Dinge zu merken, 
                        dein Wissen zu organisieren und personalisiert zu lernen. Du kannst vorhandene Lerninhalte nutzen, sie neu zusammenstellen und/oder mit eigenen 
                        Lerninhalten ergänzen.
                        Die Lerninhalte sind in Frage-Antwort-Form (zum Beispiel mit Multiple Choice oder Zuordnen) oder als freie Karteikarten gespeichert. 
                        Einzelne Fragen werden dabei zu Lernsets gebündelt und sind auch über die zugeordneten Themen auffindbar. 
                    </p>
                    <p>
                        Was du dir (langfristig) merken möchtest kannst du einfach in dein persönliches Wunschwissen aufnehmen. Klicke dazu auf das Herz-Symbol 
                        bei einer Frage, einem Lernset oder einem ganzen Themengebiet. 
                        Du kannst auch einzelne Lernsets oder ganze Themen für eine Prüfung lernen und dir von memucho einen persönlichen Übungsplan erstellen lassen. 
                        Wir erinnern dich dann rechtzeitig ans Lernen.
                        Oder du testest einfach dein Wissen in einem Themengebiet. Oder du trittst im Echtzeit-Quiz gegen deine Freunde (oder memucho) an. 
                        Deinen Wissensstand und deine Lernerfolge hast du in deiner Wissenszentrale immer im Blick.
                    </p>
                </div>
            </div>
        </div>


        <div class="panel panel-default">
            <% currentFaqItem = new FAQAccordeonItem("WhatIsBeta"); %>
            <div class="panel-heading" role="tab" id="<%= currentFaqItem.ItemHtmlIdHeading %>">
                <h4 class="panel-title">
                    <a class="collapsed" data-toggle="collapse" data-parent="#FaqAccordion" href="#<%= currentFaqItem.ItemHtmlIdText %>" aria-expanded="false" aria-controls="<%= currentFaqItem.ItemHtmlIdText %>">
                        Was bedeutet BETA-Phase?
                    </a>
                </h4>
            </div>
            <div id="<%= currentFaqItem.ItemHtmlIdText %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="<%= currentFaqItem.ItemHtmlIdHeading %>">
                <div class="panel-body">
                    <p>
                        memucho befindet sich in der Beta-Phase. Das bedeutet, dass die Seite noch nicht ganz fertig ist. Einige Funktionen fehlen noch 
                        (zum Beispiel das Sammeln von Trophäen), andere Dinge müssen wir noch verbessern und benutzerfreundlicher machen 
                        (zum Beispiel den Lernalgorithmus). Außerdem haben wir noch sooo viele Ideen, die auf Verwirklichung warten. Wir arbeiten daran! 
                    </p>
                    <p>
                        Aber schon jetzt kannst du memucho nutzen - und uns als Beta-Nutzer wichtige Hinweise geben, wo etwas nicht funktioniert und 
                        welche Funktionen dir besonders dringend fehlen. Am besten wirst du schon jetzt <a href="<%= Links.Membership() %>">Fördermitglied</a>!
                    </p>
                    <p>
                        Mehr Infos zur Beta-Phase <a href="<%= Links.BetaInfo() %>">findest du hier</a>.
                    </p>
                </div>
            </div>
        </div>

        <div class="panel panel-default">
            <% currentFaqItem = new FAQAccordeonItem("WhyNameMemucho"); %>
          
            <div class="panel-heading" role="tab" id="<%= currentFaqItem.ItemHtmlIdHeading %>">
                <h4 class="panel-title">
                    <a data-toggle="collapse" data-parent="#FaqAccordion" href="#<%= currentFaqItem.ItemHtmlIdText %>" aria-expanded="true" aria-controls="<%= currentFaqItem.ItemHtmlIdText %>">
                        Woher kommt der Name memucho?
                    </a>
                </h4>
            </div>
            <div id="<%= currentFaqItem.ItemHtmlIdText %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="<%= currentFaqItem.ItemHtmlIdHeading %>">
                <div class="panel-body">
                    memucho ist ein Kunstwort setzt sich zusammen aus MEMO und MUCHO. "MEMO" kommt von "memorieren", 
                    stammt von dem Lateinischen Wort "memorare" ab und bedeutet "merken".<br/>
                    "Mucho" ist das spanisches Wort für "viel" und wird wie "mutscho" ausgesprochen. <br/>
                    memucho heißt also: "Viel merken" - denn darum geht es hier!
                </div>
            </div>
        </div>

        <div class="panel panel-default">
            <% currentFaqItem = new FAQAccordeonItem("QuestionLicense"); %>
            <div class="panel-heading" role="tab" id="<%= currentFaqItem.ItemHtmlIdHeading %>">
                <h4 class="panel-title">
                    <a data-toggle="collapse" data-parent="#FaqAccordion" href="#<%= currentFaqItem.ItemHtmlIdText %>" aria-expanded="true" aria-controls="<%= currentFaqItem.ItemHtmlIdText %>">
                        Unter welcher Lizenz stehen die Fragen bei memucho und (wie) können sie weiterverwendet werden?
                    </a>
                </h4>
            </div>
            <div id="<%= currentFaqItem.ItemHtmlIdText %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="<%= currentFaqItem.ItemHtmlIdHeading %>">
                <div class="panel-body">
                    In der Detailansicht steht bei jeder Frage die Lizenz, unter der sie veröffentlicht wurde.
                    Die nutzererstellten Fragen bei memucho stehen alle unter der Lizenz "Creative Commons - Namensnennung 4.0 International" 
                    (Kenner nennen sie kurz nur "<a rel="license" href="http://creativecommons.org/licenses/by/4.0/deed.de">CC BY 4.0.</a>").
                    Diese Fragen können bei angemessener Nennung des Urhebers (gib am besten die Url der Fragedetailseite und den Nutzernamen des Erstellers an) 
                    und der Lizenz frei weiterverwendet und dabei auch verändert werden.
                    Einige von uns eingestellte Fragen stehen unter anderen Lizenzen (zum Beispiel amtliche Fragesammlungen wie Führerscheinfragen). 
                    Ist bei diesen Fragen die Weiterverwendung eingeschränkt, findest du einen entsprechenden Hinweis darauf bei der Lizenzangabe.
                </div>
            </div>
        </div>

        <div class="panel panel-default">
            <% currentFaqItem = new FAQAccordeonItem("DataPrivacy"); %>
            <div class="panel-heading" role="tab" id="<%= currentFaqItem.ItemHtmlIdHeading %>">
                <h4 class="panel-title">
                    <a class="collapsed" data-toggle="collapse" data-parent="#FaqAccordion" href="#<%= currentFaqItem.ItemHtmlIdText %>" aria-expanded="false" aria-controls="<%= currentFaqItem.ItemHtmlIdText %>">
                        Wie sieht es bei euch genau mit dem Datenschutz aus?
                    </a>
                </h4>
            </div>
            <div id="<%= currentFaqItem.ItemHtmlIdText %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="<%= currentFaqItem.ItemHtmlIdHeading %>">
                <div class="panel-body">
                    <p>
                        Für uns sind Datensicherheit und Datenschutz wichtige Themen. Gerade weil wir viele Daten sammeln, sehen wir eine besondere 
                        Verantwortung darin, Daten zu schützen und transparent zu sein. Das kann jeder prüfen, denn der Programm-Quelltext für memucho ist
                        als Open Source-Software öffentlich und auf <a href="https://github.com/TrueOrFalse/TrueOrFalse"><i class="fa fa-github">&nbsp;</i>Github</a> einsehbar.
                    </p>
                    <p>
                        Der Datenschutz ist bei uns sehr streng. Wir fragen dich deshalb nicht nach persönlichen Daten, die wir nicht für die Seite
                        benötigen. Die wenigen persönlichen Daten, die wir von dir haben, sind sehr gut gesichert und wir werden sie auch niemals verkaufen.
                        Beispielsweise gibt für uns keine Möglichkeiten, dein Passwort zu ermitteln. Dein geheimes Passwort ist also bei uns sicher.
                        (Technisch: Passworte werden gehashed und mit einem SALT abgelegt gespeichert.)
                    </p>
                    <p>
                        Allerdings sind bei memucho das Wikipedia-Prinzip und der Netzwerk-Gedanke sehr wichtig. Deshalb stehen die öffentlichen 
                        Fragen unter einer Creative Commons-Lizenz (<a rel="license" href="http://creativecommons.org/licenses/by/4.0/deed.de">CC BY 4.0.</a>).
                        Jeder andere Nutzer kann die Fragen also für seine Zwecke nutzen, solange er die Quelle angibt. So ist das auch bei Wikipedia, 
                        deswegen können wir zum Beispiel viele Bilder von Wikipedia verwenden und sie auch weiterhin nutzen, wenn sie bei Wikipedia selbst 
                        nicht mehr zu sehen sein sollten. Die privaten Fragen von dir bleiben aber privat und sind nicht für andere sichtbar.
                    </p>
                </div>
            </div>
        </div>

        <div class="panel panel-default">
            <% currentFaqItem = new FAQAccordeonItem("WhichDataWhy"); %>
            <div class="panel-heading" role="tab" id="<%= currentFaqItem.ItemHtmlIdHeading %>">
                <h4 class="panel-title">
                    <a class="collapsed" data-toggle="collapse" data-parent="#FaqAccordion" href="#<%= currentFaqItem.ItemHtmlIdText %>" aria-expanded="false" aria-controls="<%= currentFaqItem.ItemHtmlIdText %>">
                        Welche Daten werden von euch erfasst? Warum?
                    </a>
                </h4>
            </div>
            <div id="<%= currentFaqItem.ItemHtmlIdText %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="<%= currentFaqItem.ItemHtmlIdHeading %>">
                <div class="panel-body">
                    <p>
                        Wir versuchen, möglichst alle deine Interaktionen mit Fragen und Antworten zu sammeln. Wie oft wurde eine Frage angesehen, wie oft und von wem
                        wurde sie zum Wunschwissen hinzugefügt, wie oft wurde die Frage beantwort und zu welchem Zeitpunkt, welche Antwort wurde gegeben.
                        Wir sammeln, wenn möglich, diese Daten personifziert, also nicht anonymisiert.
                    </p>
                    <p>
                        Wir sammeln diese Daten nur, weil wir dich so am besten beim Lernen unterstützen können. Wenn du zu einem bestimmten Termin lernst oder ein bestimmtes
                        Wissen niemals vergessen möchtest, dann möchte dir memucho die Fragen zum optimalen Zeitpunkt zum Lernen vorlegen. Damit du genug, aber auch nicht zu 
                        viel lernst. Dafür brauchen wir aber diese Daten, denn je mehr wir über das Lernverhalten von möglichst vielen Nutzern wissen, desto genauer können
                        unsere Algorithmen arbeiten. (Technisch: Wir setzen so genannte Machine Learning-Algorithmen ein, die selbstständig Lernmuster erkennen und Prognosen
                        abgeben können.)
                    </p>
                    <p>
                        Wir sammeln diese Daten nur, um memucho zu einem möglichst nützlichem Werkzeug zu machen und nicht, um Nutzerdaten zu verkaufen. Der 
                        Datenschutz ist bei uns sehr streng. Wir fragen dich deshalb nicht nach persönlichen Daten, die wir nicht für die 
                        Seite benötigen. Die wenigen persönlichen Daten, die wir von dir haben, sind sehr gut gesichert und wir werden sie auch niemals verkaufen.
                    </p>
                </div>
            </div>
        </div>

        <div class="panel panel-default">
            <% currentFaqItem = new FAQAccordeonItem("RevenueModel"); %>
            <div class="panel-heading" role="tab" id="<%= currentFaqItem.ItemHtmlIdHeading %>">
                <h4 class="panel-title">
                    <a data-toggle="collapse" data-parent="#FaqAccordion" href="#<%= currentFaqItem.ItemHtmlIdText %>" aria-expanded="true" aria-controls="<%= currentFaqItem.ItemHtmlIdText %>">
                        Wie finanziert sich memucho? 
                    </a>
                </h4>
            </div>
            <div id="<%= currentFaqItem.ItemHtmlIdText %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="<%= currentFaqItem.ItemHtmlIdHeading %>">
                <div class="panel-body">
                    <p>
                        Natürlich kostet die Entwicklung von memucho neben viel Liebe auch Zeit und Geld. Deswegen erzielen wir Einnahmen über bezahlte Mitgliedschaften 
                        mit Premiumfeatures ("Unterstützer"), die teilweise kostenpflichtige Bereitstellung von Widgets zur Einbindung auf anderen Webseiten sowie durch Werbung. 
                        Diese versuchen wir so zu gestalten, dass sie beim Lernen nicht ablenkt. 
                    </p>
                    <p>
                        Wenn du uns unterstützen möchtest, dann freuen wir uns sehr, dich als Unterstützer begrüßen zu dürfen!
                    </p>
                    <p>
                        <a href="<%= Links.Membership() %>" class="btn btn-primary">Jetzt Unterstützer werden</a>
                    </p>
                </div>
                    
            </div>
        </div>
        <div class="panel panel-default">
            <% currentFaqItem = new FAQAccordeonItem("ContentSponsoring"); %>
            <div class="panel-heading" role="tab" id="<%= currentFaqItem.ItemHtmlIdHeading %>">
                <h4 class="panel-title">
                    <a data-toggle="collapse" data-parent="#FaqAccordion" href="#<%= currentFaqItem.ItemHtmlIdText %>" aria-expanded="true" aria-controls="<%= currentFaqItem.ItemHtmlIdText %>">
                        Bei Lernmaterialien von memucho werden Sponsoren angezeigt. Was bedeutet das?
                    </a>
                </h4>
            </div>
            <div id="<%= currentFaqItem.ItemHtmlIdText %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="<%= currentFaqItem.ItemHtmlIdHeading %>">
                <div class="panel-body">
                    Wir bieten Werbeflächen und die Möglichkeit zur Präsentation von Inhalten vor allem Bildungsanbietern wie Schulbuchverlagen an. 
                    Daneben gibt es Sponsoren, die die Bereitsstellung von freien Bildungsinhalten unter der offenen Lizenz CC BY 4.0 durch memucho finanziell unterstützen.
                    Sie nehmen dabei aber keinen Einfluss auf die inhaltliche Gestaltung der Bildungsmaterialien.
                </div>
            </div>
        </div>
        <%--<div class="panel panel-default">
            <% currentFaqItem = new FAQAccordeonItem("Contact"); %>
          
            <div class="panel-heading" role="tab" id="<%= currentFaqItem.ItemHtmlIdHeading %>">
                <h4 class="panel-title">
                    <a class="collapsed" data-toggle="collapse" data-parent="#FaqAccordion" href="#<%= currentFaqItem.ItemHtmlIdText %>" aria-expanded="false" aria-controls="<%= currentFaqItem.ItemHtmlIdText %>">
                    Kontakt: Wie erreiche ich euch?
                    </a>
                </h4>
            </div>
            <div id="<%= currentFaqItem.ItemHtmlIdText %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="<%= currentFaqItem.ItemHtmlIdHeading %>">
                <div class="panel-body">
                    <p>
                        Per E-Mail: <span class="mailme">team at memucho dot de</span><br/> 
                        Robert erreichst du auch per Telefon (+49-178 186 68 48).
                    </p>
                    <p>
                        Oder du schreibst uns eine Postkarte oder kommst persönlich bei uns vorbei (am besten mit Anmeldung)!<br/>
                        Team memucho<br/>
                        c/o "Raecke Schreiber GbR"<br/>
                        Erkelenzdamm 59/61<br/>
                        D-10999 Berlin<br/>
                        (Aufgang: Portal 3A, 4.OG)
                    </p>
                </div>
            </div>
        </div>--%>
    </div>
    
</asp:Content>





