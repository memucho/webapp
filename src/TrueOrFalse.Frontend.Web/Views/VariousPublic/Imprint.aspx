<%@ Page Title="Impressum & Datenschutzerklärung" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="System.Web.Mvc.ViewPage<BaseModel>" %>

<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

    <script type="text/javascript">

        /* Source http://www.html-advisor.com/javascript/hide-email-with-javascript-jquery/ */
        $(function () {
            var spt = $('span.mailme');
            var at = / at /;
            var dot = / dot /g;
            var addr = $(spt).text().replace(at, "@").replace(dot, ".");
            $(spt).after('<a href="mailto:' + addr + '" title="Send an email">' + addr + '</a>').hover(function () { window.status = "Send a letter!"; }, function () { window.status = ""; });
            $(spt).remove();
        });
    </script>

    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem { Text = "Impressum & Datenschutz", Url = Links.Imprint, ToolTipText = "Impressum & Datenschutz" });
        Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="row" style="padding-top: 30px;">
        <div class="BackToHome col-md-3">
            <a href="/"><i class="fa fa-chevron-left">&nbsp;</i>Zur Startseite</a>
        </div>
        <div class="form-horizontal col-md-9">

            <h1 class="PageHeader">Impressum & Datenschutzerklärung</h1>

            <h2>1. Impressum</h2>

            <h3>Angaben gemäß § 5 TMG:</h3>
            memucho GmbH<br />
            Am Moorhof <br />
            Nettgendorfer Str. 7 <br />
            14947 Nuthe-Urstromtal<br />

            <h3>Registereintrag:</h3>
            Registergericht: Amtsgericht Cottbus, HRB 13499 CB<br />
            <h3>Vertreten durch:</h3>
            Geschäftsführer:<br />
            Robert Mischke<br />

            <h3>Kontakt:</h3>

            Telefon:
            <br />
            +49-178 186 68 48<br />
            <span class="mailme">team at memucho dot de</span>

            <h3>Verantwortlich für den Inhalt nach § 55 Abs. 2 RStV:</h3>
            <p>Christof Mauersberger, Robert Mischke<br />
            </p>

            <h3>Haftungsausschluss</h3>
            <p><strong>Haftung für Inhalte</strong></p>
            <p>
                Die Inhalte unserer Seiten wurden mit größter Sorgfalt erstellt. 
            Für die Richtigkeit, Vollständigkeit und Aktualität der Inhalte 
            können wir jedoch keine Gewähr übernehmen. Als Diensteanbieter sind wir gemäß § 7 Abs.1 TMG für 
            eigene Inhalte auf diesen Seiten nach den allgemeinen Gesetzen verantwortlich. 
            Nach §§ 8 bis 10 TMG sind wir als Diensteanbieter jedoch nicht 
            verpflichtet, übermittelte oder gespeicherte fremde Informationen zu 
            überwachen oder nach Umständen zu forschen, die auf eine rechtswidrige 
            Tätigkeit hinweisen. Verpflichtungen zur Entfernung oder Sperrung der 
            Nutzung von Informationen nach den allgemeinen Gesetzen bleiben hiervon 
            unberührt. Eine diesbezügliche Haftung ist jedoch erst ab dem 
            Zeitpunkt der Kenntnis einer konkreten Rechtsverletzung möglich. Bei 
            Bekanntwerden von entsprechenden Rechtsverletzungen werden wir diese Inhalte 
            umgehend entfernen.
            </p>
            <p><strong>Haftung für Links</strong></p>
            <p>
                Unser Angebot enthält Links zu externen Webseiten Dritter, auf deren 
            Inhalte wir keinen Einfluss haben. Deshalb können wir für diese 
            fremden Inhalte auch keine Gewähr übernehmen. Für die Inhalte 
            der verlinkten Seiten ist stets der jeweilige Anbieter oder Betreiber der 
            Seiten verantwortlich. Die verlinkten Seiten wurden zum Zeitpunkt der Verlinkung 
            auf mögliche Rechtsverstöße überprüft. Rechtswidrige 
            Inhalte waren zum Zeitpunkt der Verlinkung nicht erkennbar. Eine permanente 
            inhaltliche Kontrolle der verlinkten Seiten ist jedoch ohne konkrete Anhaltspunkte 
            einer Rechtsverletzung nicht zumutbar. Bei Bekanntwerden von Rechtsverletzungen 
            werden wir derartige Links umgehend entfernen.
            </p>
            <p><strong>Datenschutz</strong></p>
            <p>
                Die Nutzung unserer Webseite ist teils ohne Angabe personenbezogener Daten möglich. Soweit auf unseren Seiten personenbezogene Daten (beispielsweise Name, 
            Anschrift oder eMail-Adressen) erhoben werden, erfolgt dies, soweit möglich, stets auf freiwilliger Basis. Diese Daten werden ohne Ihre ausdrückliche Zustimmung nicht an Dritte weitergegeben.   
            </p>
            <p>
                Wir weisen darauf hin, dass die Datenübertragung im Internet (z.B. 
            bei der Kommunikation per E-Mail) Sicherheitslücken aufweisen kann. 
            Ein lückenloser Schutz der Daten vor dem Zugriff durch Dritte ist nicht 
            möglich.
            </p>
            <p>
                Der Nutzung von im Rahmen der Impressumspflicht veröffentlichten Kontaktdaten 
            durch Dritte zur Übersendung von nicht ausdrücklich angeforderter 
            Werbung und Informationsmaterialien wird hiermit ausdrücklich widersprochen. 
            Die Betreiber der Seiten behalten sich ausdrücklich rechtliche Schritte 
            im Falle der unverlangten Zusendung von Werbeinformationen, etwa durch Spam-Mails, 
            vor.
            </p>

            <p><strong>Nutzer unter 16 Jahren</strong><a name="under16">&nbsp;</a></p>
            <p>
                Wenn Du unter 16 Jahren alt bist, darfst Du dich bei memucho nur mit Zustimmung Deiner Eltern registieren.
            </p>
            <p>
                Die Zustimmung durch Deine Eltern kann per Email oder Telefon erfolgen. 
            Hier können Deine Eltern mit uns in Kontakt treten: <a href="/Kontakt">Kontakt</a>
            </p>
            <p>
                Du kannst Die Seite natürlich anonym Nutzen, also ohne Registrierung nutzen.
            </p>

            <p></p>
            <p>
                (Eine) Quelle: <i><a href="http://www.e-recht24.de/muster-disclaimer.htm" target="_blank">Disclaimer</a> von eRecht24, dem Portal zum Internetrecht von <a href="http://www.e-recht24.de/" target="_blank">Rechtsanwalt</a> Sören Siebert.</i>
            </p>

            <br />

        <h1>Datenschutzerkl&auml;rung</h1>

            <h2>Datenschutz</h2>

            <p>Wir haben diese Datenschutzerkl&auml;rung (Fassung 15.11.2019-311128432) verfasst, um Ihnen gem&auml;&szlig; der Vorgaben der&nbsp;&nbsp;<a href="https://eur-lex.europa.eu/legal-content/DE/ALL/?tid=1234&amp;uri=celex%3A32016R0679&amp;tid=311128432">Datenschutz-Grundverordnung (EU) 2016/679</a>&nbsp;zu erkl&auml;ren, welche Informationen wir sammeln, wie wir Daten verwenden und welche Entscheidungsm&ouml;glichkeiten Sie als Besucher dieser Webseite haben.</p>

            <p>Leider liegt es in der Natur der Sache, dass diese Erkl&auml;rungen sehr technisch klingen, wir haben uns bei der Erstellung jedoch bem&uuml;ht die wichtigsten Dinge so einfach und klar wie m&ouml;glich zu beschreiben.</p>

            <h2>Automatische Datenspeicherung</h2>

            <p>Wenn Sie heutzutage Webseiten besuchen, werden gewisse Informationen automatisch erstellt und gespeichert, so auch auf dieser Webseite.</p>

            <p>Wenn Sie unsere Webseite so wie jetzt gerade besuchen, speichert unser Webserver (Computer auf dem diese Webseite gespeichert ist) automatisch Daten wie</p>

            <ul>
                <li>
                    <p>die Adresse (URL) der aufgerufenen Webseite</p>
                </li>
                <li>
                    <p>Browser und Browserversion</p>
                </li>
                <li>
                    <p>das verwendete Betriebssystem</p>
                </li>
                <li>
                    <p>die Adresse (URL) der zuvor besuchten Seite (Referrer URL)</p>
                </li>
                <li>
                    <p>den Hostname und die IP-Adresse des Ger&auml;ts von welchem aus zugegriffen wird</p>
                </li>
                <li>
                    <p>Datum und Uhrzeit</p>
                </li>
            </ul>

            <p>in Dateien (Webserver-Logfiles).</p>

            <p>In der Regel werden Webserver-Logfiles zwei Wochen gespeichert und danach automatisch gel&ouml;scht. Wir geben diese Daten nicht weiter, k&ouml;nnen jedoch nicht ausschlie&szlig;en, dass diese Daten beim Vorliegen von rechtswidrigem Verhalten eingesehen werden.</p>

            <p>Die Rechtsgrundlage besteht nach&nbsp;&nbsp;<a href="https://eur-lex.europa.eu/legal-content/DE/TXT/HTML/?uri=CELEX:32016R0679&amp;from=DE&amp;tid=311128432">Artikel 6 &nbsp;Absatz 1 f DSGVO</a>&nbsp;(Rechtm&auml;&szlig;igkeit der Verarbeitung) darin, dass berechtigtes Interesse daran besteht, den fehlerfreien Betrieb dieser Webseite durch das Erfassen von Webserver-Logfiles zu erm&ouml;glichen.</p>

            <h2>Cookies</h2>

            <p>Unsere Website verwendet HTTP-Cookies um nutzerspezifische Daten zu speichern.</p>

            <p>Im Folgenden erkl&auml;ren wir, was Cookies sind und warum Sie genutzt werden, damit Sie die folgende Datenschutzerkl&auml;rung besser verstehen.</p>

            <h3>Was genau sind Cookies?</h3>

            <p>Immer wenn Sie durch das Internet surfen, verwenden Sie einen Browser. Bekannte Browser sind beispielsweise Chrome, Safari, Firefox, Internet Explorer und Microsoft Edge. Die meisten Webseiten speichern kleine Text-Dateien in Ihrem Browser. Diese Dateien nennt man Cookies.</p>

            <p>Eines ist nicht von der Hand zu weisen: Cookies sind echt n&uuml;tzliche Helferlein. Fast alle Webseiten verwenden Cookies. Genauer gesprochen sind es HTTP-Cookies, da es auch noch andere Cookies f&uuml;r andere Anwendungsbereiche gibt. HTTP-Cookies sind kleine Dateien, die von unserer Website auf Ihrem Computer gespeichert werden. Diese Cookie-Dateien werden automatisch im Cookie-Ordner, quasi dem &ldquo;Hirn&rdquo; Ihres Browsers, untergebracht. Ein Cookie besteht aus einem Namen und einem Wert. Bei der Definition eines Cookies m&uuml;ssen zus&auml;tzlich ein oder mehrere Attribute angegeben werden.</p>

            <p>Cookies speichern gewisse Nutzerdaten von Ihnen, wie beispielsweise Sprache oder pers&ouml;nliche Seiteneinstellungen. Wenn Sie unsere Seite wieder aufrufen, &uuml;bermittelt Ihr Browser die &bdquo;userbezogenen&ldquo; Informationen an unsere Seite zur&uuml;ck. Dank der Cookies wei&szlig; unsere Website, wer Sie sind und bietet Ihnen Ihre gewohnte Standardeinstellung. In einigen Browsern hat jedes Cookie eine eigene Datei, in anderen wie beispielsweise Firefox sind alle Cookies in einer einzigen Datei gespeichert.</p>

            <p>Es gibt sowohl Erstanbieter Cookies als auch Drittanbieter-Cookies. Erstanbieter-Cookies werden direkt von unserer Seite erstellt, Drittanbieter-Cookies werden von Partner-Webseiten (z.B. Google Analytics) erstellt. Jedes Cookie ist individuell zu bewerten, da jedes Cookie andere Daten speichert. Auch die Ablaufzeit eines Cookies variiert von ein paar Minuten bis hin zu ein paar Jahren. Cookies sind keine Software-Programme und enthalten keine Viren, Trojaner oder andere &bdquo;Sch&auml;dlinge&ldquo;. Cookies k&ouml;nnen auch nicht auf Informationen Ihres PCs zugreifen.</p>

            <p>So k&ouml;nnen zum Beispiel Cookie-Daten aussehen:</p>

            <ul>
                <li>
                    <p>Name: _ga</p>
                </li>
                <li>
                    <p>Ablaufzeit: 2 Jahre</p>
                </li>
                <li>
                    <p>Verwendung: Unterscheidung der Webseitenbesucher</p>
                </li>
                <li>
                    <p>Beispielhafter Wert: GA1.2.1326744211.152311128432</p>
                </li>
            </ul>

            <p>Ein Browser sollte folgende Mindestgr&ouml;&szlig;en unterst&uuml;tzen:</p>

            <ul>
                <li>
                    <p>Ein Cookie soll mindestens 4096 Bytes enthalten k&ouml;nnen</p>
                </li>
                <li>
                    <p>Pro Domain sollen mindestens 50 Cookies gespeichert werden k&ouml;nnen</p>
                </li>
                <li>
                    <p>Insgesamt sollen mindestens 3000 Cookies gespeichert werden k&ouml;nnen</p>
                </li>
            </ul>

            <h3>Welche Arten von Cookies gibt es?</h3>

            <p>Die Frage welche Cookies wir im Speziellen verwenden, h&auml;ngt von den verwendeten Diensten ab und wird in der folgenden Abschnitten der Datenschutzerkl&auml;rung gekl&auml;rt. An dieser Stelle m&ouml;chten wir kurz auf die verschiedenen Arten von HTTP-Cookies eingehen.</p>

            <p>Man kann 4 Arten von Cookies unterscheiden:</p>

            <p>Unbedingt notwendige Cookies</p>

            <p>Diese Cookies sind n&ouml;tig, um grundlegende Funktionen der Website sicherzustellen. Zum Beispiel braucht es diese Cookies, wenn ein User ein Produkt in den Warenkorb legt, dann auf anderen Seiten weitersurft und sp&auml;ter erst zur Kasse geht. Durch diese Cookies wird der Warenkorb nicht gel&ouml;scht, selbst wenn der User sein Browserfenster schlie&szlig;t.</p>

            <p>Funktionelle Cookies</p>

            <p>Diese Cookies sammeln Infos &uuml;ber das Userverhalten und ob der User etwaige Fehlermeldungen bekommt. Zudem werden mithilfe dieser Cookies auch die Ladezeit und das Verhalten der Website bei verschiedenen Browsern gemessen.</p>

            <p>Zielorientierte Cookies</p>

            <p>Diese Cookies sorgen f&uuml;r eine bessere Nutzerfreundlichkeit. Beispielsweise werden eingegebene Standorte, Schriftgr&ouml;&szlig;en oder Formulardaten gespeichert.</p>

            <p>Werbe-Cookies</p>

            <p>Diese Cookies werden auch Targeting-Cookies genannt. Sie dienen dazu dem User individuell angepasste Werbung zu liefern. Das kann sehr praktisch, aber auch sehr nervig sein.</p>

            <p>&Uuml;blicherweise werden Sie beim erstmaligen Besuch einer Webseite gefragt, welche dieser Cookiearten Sie zulassen m&ouml;chten. Und nat&uuml;rlich wird diese Entscheidung auch in einem Cookie gespeichert.</p>

            <h3>Wie kann ich Cookies l&ouml;schen?</h3>

            <p>Wie und ob Sie Cookies verwenden wollen, entscheiden Sie selbst. Unabh&auml;ngig von welchem Service oder welcher Website die Cookies stammen, haben Sie immer die M&ouml;glichkeit Cookies zu l&ouml;schen, nur teilweise zuzulassen oder zu deaktivieren. Zum Beispiel k&ouml;nnen Sie Cookies von Drittanbietern blockieren, aber alle anderen Cookies zulassen.</p>

            <p>Wenn Sie feststellen m&ouml;chten, welche Cookies in Ihrem Browser gespeichert wurden, wenn Sie Cookie-Einstellungen &auml;ndern oder l&ouml;schen wollen, k&ouml;nnen Sie dies in Ihren Browser-Einstellungen finden:</p>

            <p><a href="https://support.google.com/chrome/answer/95647?tid=311128432">Chrome: Cookies in Chrome l&ouml;schen, aktivieren und verwalten</a></p>

            <p><a href="https://support.apple.com/de-at/guide/safari/sfri11471/mac?tid=311128432">Safari: Verwalten von Cookies und Websitedaten mit Safari</a></p>

            <p><a href="https://support.mozilla.org/de/kb/cookies-und-website-daten-in-firefox-loschen?tid=311128432">Firefox: Cookies l&ouml;schen, um Daten zu entfernen, die Websites auf Ihrem Computer abgelegt haben</a></p>

            <p><a href="https://support.microsoft.com/de-at/help/17442/windows-internet-explorer-delete-manage-cookies?tid=311128432">Internet Explorer: L&ouml;schen und Verwalten von Cookies</a></p>

            <p><a href="https://support.microsoft.com/de-at/help/4027947/windows-delete-cookies?tid=311128432">Microsoft Edge: L&ouml;schen und Verwalten von Cookies</a></p>

            <p>Falls Sie grunds&auml;tzlich keine Cookies haben wollen, k&ouml;nnen Sie Ihren Browser so einrichten, dass er Sie immer informiert, wenn ein Cookie gesetzt werden soll. So k&ouml;nnen Sie bei jedem einzelnen Cookie entscheiden, ob Sie das Cookie erlauben oder nicht. Die Vorgangsweise ist je nach Browser verschieden. Am besten ist es Sie suchen die Anleitung in Google mit dem Suchbegriff &ldquo;Cookies l&ouml;schen Chrome&rdquo; oder &ldquo;Cookies deaktivieren Chrome&rdquo; im Falle eines Chrome Browsers oder tauschen das Wort &ldquo;Chrome&rdquo; gegen den Namen Ihres Browsers, z.B. Edge, Firefox, Safari aus.</p>

            <h3>Wie sieht es mit meinem Datenschutz aus?</h3>

            <p>Seit 2009 gibt es die sogenannten &bdquo;Cookie-Richtlinien&ldquo;. Darin ist festgehalten, dass das Speichern von Cookies eine Einwilligung des Website-Besuchers (also von Ihnen) verlangt. Innerhalb der EU-L&auml;nder gibt es allerdings noch sehr unterschiedliche Reaktionen auf diese Richtlinien. In Deutschland wurden die Cookie-Richtlinien nicht als nationales Recht umgesetzt. Stattdessen erfolgte die Umsetzung dieser Richtlinie weitgehend in &sect; 15 Abs.3 des Telemediengesetzes (TMG).</p>

            <p>Wenn Sie mehr &uuml;ber Cookies wissen m&ouml;chten und vor technischen Dokumentationen nicht zur&uuml;ckscheuen, empfehlen wir&nbsp;&nbsp;<a href="https://tools.ietf.org/html/rfc6265">https://tools.ietf.org/html/rfc6265</a>, dem Request for Comments der Internet Engineering Task Force (IETF) namens &ldquo;HTTP State Management Mechanism&rdquo;.</p>

            <h2>Speicherung pers&ouml;nlicher Daten</h2>

            <p>Pers&ouml;nliche Daten, die Sie uns auf dieser Website elektronisch &uuml;bermitteln, wie zum Beispiel Name, E-Mail-Adresse, Adresse oder andere pers&ouml;nlichen Angaben im Rahmen der &Uuml;bermittlung eines Formulars oder Kommentaren im Blog, werden von uns gemeinsam mit dem Zeitpunkt und der IP-Adresse nur zum jeweils angegebenen Zweck verwendet, sicher verwahrt und nicht an Dritte weitergegeben.</p>

            <p>Wir nutzen Ihre pers&ouml;nlichen Daten somit nur f&uuml;r die Kommunikation mit jenen Besuchern, die Kontakt ausdr&uuml;cklich w&uuml;nschen und f&uuml;r die Abwicklung der auf dieser Webseite angebotenen Dienstleistungen und Produkte. Wir geben Ihre pers&ouml;nlichen Daten ohne Zustimmung nicht weiter, k&ouml;nnen jedoch nicht ausschlie&szlig;en, dass diese Daten beim Vorliegen von rechtswidrigem Verhalten eingesehen werden.</p>

            <p>Wenn Sie uns pers&ouml;nliche Daten per E-Mail schicken &ndash; somit abseits dieser Webseite &ndash; k&ouml;nnen wir keine sichere &Uuml;bertragung und den Schutz Ihrer Daten garantieren. Wir empfehlen Ihnen, vertrauliche Daten niemals unverschl&uuml;sselt per E-Mail zu &uuml;bermitteln.</p>

            <p>Die Rechtsgrundlage besteht nach&nbsp;&nbsp;<a href="https://eur-lex.europa.eu/legal-content/DE/TXT/HTML/?uri=CELEX:32016R0679&amp;from=DE&amp;tid=311128432">Artikel 6 &nbsp;Absatz 1 a DSGVO</a>&nbsp;(Rechtm&auml;&szlig;igkeit der Verarbeitung) darin, dass Sie uns die Einwilligung zur Verarbeitung der von Ihnen eingegebenen Daten geben. Sie k&ouml;nnen diesen Einwilligung jederzeit widerrufen &ndash; eine formlose E-Mail reicht aus, Sie finden unsere Kontaktdaten im Impressum.</p>

            <h2>Rechte laut Datenschutzgrundverordnung</h2>

            <p>Ihnen stehen laut den Bestimmungen der DSGVO grunds&auml;tzlich die folgende Rechte zu:</p>

            <ul>
                <li>
                    <p>Recht auf Berichtigung (Artikel 16 DSGVO)</p>
                </li>
                <li>
                    <p>Recht auf L&ouml;schung (&bdquo;Recht auf Vergessenwerden&ldquo;) (Artikel 17 DSGVO)</p>
                </li>
                <li>
                    <p>Recht auf Einschr&auml;nkung der Verarbeitung (Artikel 18 DSGVO)</p>
                </li>
                <li>
                    <p>Recht auf Benachrichtigung &ndash; Mitteilungspflicht im Zusammenhang mit der Berichtigung oder L&ouml;schung personenbezogener Daten oder der Einschr&auml;nkung der Verarbeitung (Artikel 19 DSGVO)</p>
                </li>
                <li>
                    <p>Recht auf Daten&uuml;bertragbarkeit (Artikel 20 DSGVO)</p>
                </li>
                <li>
                    <p>Widerspruchsrecht (Artikel 21 DSGVO)</p>
                </li>
                <li>
                    <p>Recht, nicht einer ausschlie&szlig;lich auf einer automatisierten Verarbeitung &mdash; einschlie&szlig;lich Profiling &mdash; beruhenden Entscheidung unterworfen zu werden (Artikel 22 DSGVO)</p>
                </li>
            </ul>

            <p>Wenn Sie glauben, dass die Verarbeitung Ihrer Daten gegen das Datenschutzrecht verst&ouml;&szlig;t oder Ihre datenschutzrechtlichen Anspr&uuml;che sonst in einer Weise verletzt worden sind, k&ouml;nnen Sie sich an die&nbsp;&nbsp;<a href="https://www.bfdi.bund.de/DE/Datenschutz/Ueberblick/MeineRechte/Artikel/BeschwerdeBeiDatenschutzbehoereden.html?tid=311128432">Bundesbeauftragte f&uuml;r den Datenschutz und die Informationsfreiheit (BfDI)</a>&nbsp;wenden.</p>

            <h2>Auswertung des Besucherverhaltens</h2>

            <p>In der folgenden Datenschutzerkl&auml;rung informieren wir Sie dar&uuml;ber, ob und wie wir Daten Ihres Besuchs dieser Website auswerten. Die Auswertung der gesammelten Daten erfolgt in der Regel anonym und wir k&ouml;nnen von Ihrem Verhalten auf dieser Website nicht auf Ihre Person schlie&szlig;en.</p>

            <p>Mehr &uuml;ber M&ouml;glichkeiten dieser Auswertung der Besuchsdaten zu widersprechen erfahren Sie in der folgenden Datenschutzerkl&auml;rung.</p>

            <p>&nbsp;</p>

            <h3>5 Matomo (ehemals &bdquo;Piwik&ldquo;)</h3>

            <p>5.1 Umfang und Beschreibung der Verarbeitung personenbezogener Daten</p>

            <p>Unsere Webseite verwendet &quot;Matomo&quot; (ehemals &quot;Piwik&quot;), einen Webanalysedienst der InnoCraft Ltd., 150 Willis St, 6011 Wellington, New Zealand. Matomo speichert Cookies auf Ihrem Endger&auml;t, die eine Analyse der Benutzung unserer Webseite durch Sie erm&ouml;glichen. Die so erhobenen Informationen werden ausschlie&szlig;lich auf unserem Server gespeichert, und zwar folgende Daten:</p>

            <p>- zwei Bytes der IP-Adresse des aufrufenden Systems des Nutzers</p>

            <p>- die aufgerufene Webseite</p>

            <p>- die Website, von der der Nutzer auf die aufgerufene Webseite gelangt ist (Referrer)</p>

            <p>- die Unterseiten, die von der aufgerufenen Webseite aus aufgerufen werden</p>

            <p>- die Verweildauer auf der Webseite</p>

            <p>- die H&auml;ufigkeit des Aufrufs der Webseite</p>

            <p>Unsere Webseite verwendet Matomo mit der Einstellung &bdquo;Anonymize Visitors&rsquo; IP addresses&ldquo;. Dadurch werden IP-Adressen gek&uuml;rzt weiterverarbeitet, eine direkte Personenbeziehbarkeit wird damit ausgeschlossen. Die Software ist so eingestellt, dass die IP-Adressen nicht vollst&auml;ndig gespeichert werden, sondern 2 Bytes der IP-Adresse maskiert werden (z.B. 192.168.xxx.xxx). Auf diese Weise ist eine Zuordnung der gek&uuml;rzten IP-Adresse zum aufrufenden Rechner nicht mehr m&ouml;glich. Die mittels Matomo von Ihrem Browser &uuml;bermittelte IP-Adresse wird nicht mit anderen von uns erhobenen Daten zusammengef&uuml;hrt.</p>

            <p>5.2 Rechtsgrundlage f&uuml;r die Verarbeitung personenbezogener Daten</p>

            <p>Die Rechtsgrundlage f&uuml;r die Verarbeitung der Daten des Nutzers ist Art. 6 Abs. 1 lit. f DSGVO bzw. &sect; 15 Abs. 3 TMG.</p>

            <p>5.3 Zwecke der Verarbeitung</p>

            <p>Mit Matomo analysieren wir die Nutzung unserer Webseite und einzelner Funktionen und Angebote um das Nutzererlebnis fortlaufend verbessern zu k&ouml;nnen. Durch die statistische Auswertung des Nutzerverhaltens verbessern wir unser Angebot und gestalten es f&uuml;r Besucher interessanter.</p>

            <p>5.4 Dauer der Speicherung</p>

            <p>Die Daten der hier beschriebenen Verarbeitung werden nach einer Speicherdauer von 90Tagen gel&ouml;scht.</p>

            <p>5.5 Widerspruchs- und Beseitigungsm&ouml;glichkeit</p>

            <p>Die Auswertung k&ouml;nnen Sie verhindern, indem Sie vorhandene Cookies l&ouml;schen und eine Speicherung von Cookies in den Einstellungen Ihres Webbrowsers deaktivieren. Wir weisen darauf hin, dass Sie in diesem Fall m&ouml;glicherweise nicht alle Funktionen dieser Webseite vollumf&auml;nglich nutzen k&ouml;nnen.</p>

            <p>Matomo ist ein Open-Source-Projekt der InnoCraft Ltd., 150 Willis St, 6011 Wellington, New Zealand.</p>

            <p>Weitere Informationen zum Datenschutz finden Sie in der Datenschutzerkl&auml;rung unter:&nbsp;&nbsp;<a href="https://matomo.org/privacy-policy/">matomo.org/privacy-policy/</a></p>

            <p>&nbsp;</p>

            <h2>TLS-Verschl&uuml;sselung mit https</h2>

            <p>Wir verwenden https um Daten abh&ouml;rsicher im Internet zu &uuml;bertragen (Datenschutz durch Technikgestaltung&nbsp;&nbsp;<a href="https://eur-lex.europa.eu/legal-content/DE/TXT/HTML/?uri=CELEX:32016R0679&amp;from=DE&amp;tid=311128432">Artikel 25 Absatz 1 DSGVO</a>). Durch den Einsatz von TLS (Transport Layer Security), einem Verschl&uuml;sselungsprotokoll zur sicheren Daten&uuml;bertragung im Internet k&ouml;nnen wir den Schutz vertraulicher Daten sicherstellen. Sie erkennen die Benutzung dieser Absicherung der Daten&uuml;bertragung am kleinen Schlo&szlig;symbol links oben im Browser und der Verwendung des Schemas https (anstatt http) als Teil unserer Internetadresse.</p>

            <h2>Newsletter Datenschutzerkl&auml;rung</h2>

            <p>Wenn Sie sich f&uuml;r unseren Newsletter eintragen &uuml;bermitteln Sie die oben genannten pers&ouml;nlichen Daten und geben uns das Recht Sie per E-Mail zu kontaktieren. Die im Rahmen der Anmeldung zum Newsletter gespeicherten Daten nutzen wir ausschlie&szlig;lich f&uuml;r unseren Newsletter und geben diese nicht weiter.</p>

            <p>Sollten Sie sich vom Newsletter abmelden &ndash; Sie finden den Link daf&uuml;r in jedem Newsletter ganz unten &ndash; dann l&ouml;schen wir alle Daten die mit der Anmeldung zum Newsletter gespeichert wurden.</p>

            <h2>Google Fonts Lokal Datenschutzerkl&auml;rung</h2>

            <p>Wir verwenden Google Fonts der Firma Google Inc. (1600 Amphitheatre Parkway Mountain View, CA 94043, USA) auf unserer Webseite. Wir haben die Google-Schriftarten lokal, d.h. auf unserem Webserver &ndash; nicht auf den Servern von Google &ndash; eingebunden. Dadurch gibt es keine Verbindung zu Server von Google und somit auch keine Daten&uuml;bertragung bzw. Speicherung.</p>

            <h3>Was sind Google Fonts?</h3>

            <p>Google Fonts (fr&uuml;her Google Web Fonts) ist ein interaktives Verzeichnis mit mehr als 800 Schriftarten, die die&nbsp;&nbsp;<a href="https://de.wikipedia.org/wiki/Google_LLC?tid=311128432">Google LLC</a>&nbsp;zur freien Verwendung bereitstellt. Mit Google Fonts k&ouml;nnte man die Schriften nutzen, ohne sie auf den eigenen Server hochzuladen. Doch um diesbez&uuml;glich jede Informations&uuml;bertragung zum Google-Server zu unterbinden, haben wir die Schriftarten auf unseren Server heruntergeladen. Auf diese Weise handeln wir datenschutzkonform und senden keine Daten an Google Fonts weiter.</p>

            <p>Anders als andere Web-Schriften erlaubt uns Google uneingeschr&auml;nkten Zugriff auf alle Schriftarten. Wir k&ouml;nnen also unlimitiert auf ein Meer an Schriftarten zugreifen und so das Optimum f&uuml;r unsere Webseite rausholen. Mehr zu Google Fonts und weiteren Fragen finden Sie auf&nbsp;&nbsp;<a href="https://developers.google.com/fonts/faq?tid=311128432">https://developers.google.com/fonts/faq?tid=311128432</a>.</p>

            <h2>Google Fonts Datenschutzerkl&auml;rung</h2>

            <p>Wir verwenden Google Fonts der Firma Google Inc. (1600 Amphitheatre Parkway Mountain View, CA 94043, USA) auf unserer Webseite.</p>

            <p>F&uuml;r die Verwendung von Google-Schriftarten m&uuml;ssen Sie sich nicht anmelden bzw. ein Passwort hinterlegen. Weiters werden auch keine Cookies in Ihrem Browser gespeichert. Die Dateien (CSS, Schriftarten/Fonts) werden &uuml;ber die Google-Domains fonts.googleapis.com und fonts.gstatic.com angefordert. Laut Google sind die Anfragen nach CSS und Schriften vollkommen getrennt von allen anderen Google-Diensten. Wenn Sie ein Google-Konto haben, brauchen Sie keine Sorge haben, dass Ihre Google-Kontodaten, w&auml;hrend der Verwendung von Google Fonts, an Google &uuml;bermittelt werden. Google erfasst die Nutzung von CSS (Cascading Style Sheets) und der verwendeten Schriftarten und speichert diese Daten sicher. Wie die Datenspeicherung genau aussieht, werden wir uns noch im Detail ansehen.</p>

            <h3>Was sind Google Fonts?</h3>

            <p>Google Fonts (fr&uuml;her Google Web Fonts) ist ein interaktives Verzeichnis mit mehr als 800 Schriftarten, die die&nbsp;&nbsp;<a href="https://de.wikipedia.org/wiki/Google_LLC?tid=311128432">Google LLC</a>&nbsp;zur freien Verwendung bereitstellt.</p>

            <p>Viele dieser Schriftarten sind unter der SIL Open Font License ver&ouml;ffentlicht, w&auml;hrend andere unter der Apache-Lizenz ver&ouml;ffentlicht wurden. Beides sind freie Software-Lizenzen. Somit k&ouml;nnen wir sie frei verwenden, ohne daf&uuml;r Lizenzgeb&uuml;hren zu zahlen.</p>

            <h3>Warum verwenden wir Google Fonts auf unserer Webseite?</h3>

            <p>Mit Google Fonts k&ouml;nnen wir auf der eigenen Webseite Schriften nutzen, und m&uuml;ssen sie nicht auf unserem eigenen Server hochladen. Google Fonts ist ein wichtiger Baustein, um die Qualit&auml;t unserer Webseite hoch zu halten. Alle Google-Schriften sind automatisch f&uuml;r das Web optimiert und dies spart Datenvolumen und ist speziell f&uuml;r die Verwendung bei mobilen Endger&auml;ten ein gro&szlig;er Vorteil. Wenn Sie unsere Seite besuchen, sorgt die niedrige Dateigr&ouml;&szlig;e f&uuml;r eine schnelle Ladezeit. Des Weiteren sind Google Fonts sogenannte sichere Web Fonts. Unterschiedliche Bildsynthese-Systeme (Rendering) in verschiedenen Browsern, Betriebssystemen und mobilen Endger&auml;ten k&ouml;nnen zu Fehlern f&uuml;hren. Solche Fehler k&ouml;nnen teilweise Texte bzw. ganze Webseiten optisch verzerren. Dank des schnellen Content Delivery Network (CDN) gibt es mit Google Fonts keine plattform&uuml;bergreifenden Probleme. Google Fonts unterst&uuml;tzt alle g&auml;ngigen Browser ( Google Chrome, Mozilla Firefox, Apple Safari, Opera) &nbsp;und funktioniert zuverl&auml;ssig auf den meisten modernen mobilen Betriebssystemen, einschlie&szlig;lich Android 2.2+ und iOS 4.2+ (iPhone, iPad, iPod).</p>

            <p>Wir verwenden die Google Fonts also, damit wir unser gesamtes Online-Service so sch&ouml;n und einheitlich wie m&ouml;glich darstellen k&ouml;nnen. Nach dem Art. 6 Abs. 1 f lit. F DSGVO stellt das bereits ein &bdquo;berechtigtes Interesse&ldquo; an der Verarbeitung von personenbezogenen Daten dar. Unter &bdquo;berechtigtem Interesse&ldquo; versteht man in diesem Fall sowohl rechtliche als auch wirtschaftliche oder ideelle Interessen, die vom Rechtssystem anerkannt werden.</p>

            <h3>Welche Daten werden von Google gespeichert?</h3>

            <p>Wenn Sie unsere Webseite besuchen, werden die Schriften &uuml;ber einen Google-Server nachgeladen. Durch diesen externen Aufruf werden Daten an die Google-Server &uuml;bermittelt. So erkennt Google auch, dass Sie bzw. Ihre IP-Adresse unsere Webseite besucht. Die Google Fonts API wurde entwickelt, um die Erfassung, Speicherung und Verwendung von Endnutzerdaten auf das zu reduzieren, was f&uuml;r eine effiziente Bereitstellung von Schriften n&ouml;tig ist. API steht &uuml;brigens f&uuml;r &bdquo;Application Programming Interface&ldquo; und dient unter anderem als Daten&uuml;bermittler im Softwarebereich.</p>

            <p>Google Fonts speichert CSS- und Font-Anfragen sicher bei Google und ist somit gesch&uuml;tzt. Durch die gesammelten Nutzungszahlen kann Google die Beliebtheit der Schriften feststellen. Die Ergebnisse ver&ouml;ffentlicht Google auf internen Analyseseiten, wie beispielsweise Google Analytics. Zudem verwendet Google auch Daten des eigenen Web-Crawlers, um festzustellen, welche Webseiten Google-Schriften verwenden. Diese Daten werden in der BigQuery-Datenbank von Google Fonts ver&ouml;ffentlicht. BigQuery ist ein Webservice von Google f&uuml;r Unternehmen, die gro&szlig;e Datenmengen bewegen und analysieren wollen.</p>

            <p>Zu bedenken gilt allerdings noch, dass durch jede Google Font Anfrage auch Informationen wie IP-Adresse, Spracheinstellungen, Bildschirmaufl&ouml;sung des Browsers, Version des Browsers und Name des Browsers automatisch an die Google-Server &uuml;bertragen werden. Ob diese Daten auch gespeichert werden, ist nicht klar feststellbar bzw. wird von Google nicht eindeutig kommuniziert.</p>

            <h3>Wie lange und wo werden die Daten gespeichert?</h3>

            <p>Anfragen f&uuml;r CSS-Assets speichert Google einen Tag lang auf Ihren Servern, die haupts&auml;chlich au&szlig;erhalb der EU angesiedelt sind. Das erm&ouml;glicht uns, mithilfe eines Google-Stylesheets die Schriftarten zu nutzen. Ein Stylesheet ist eine Formatvorlage, &uuml;ber die man einfach und schnell z.B. das Design bzw. die Schriftart einer Webseite &auml;ndern kann.</p>

            <p>Die Font-Dateien werden bei Google ein Jahr gespeichert. Google verfolgt damit das Ziel, die Ladezeit von Webseiten grunds&auml;tzlich zu verbessern. Wenn Millionen von Webseiten auf die gleichen Schriften verweisen, werden sie nach dem ersten Besuch zwischengespeichert und erscheinen sofort auf allen anderen sp&auml;ter besuchten Webseiten wieder. Manchmal aktualisiert Google Schriftdateien, um die Dateigr&ouml;&szlig;e zu reduzieren, die Abdeckung von Sprache zu erh&ouml;hen und das Design zu verbessern.</p>

            <h3>Wie kann ich meine Daten l&ouml;schen bzw. die Datenspeicherung verhindern?</h3>

            <p>Jene Daten, die Google f&uuml;r einen Tag bzw. ein Jahr speichert k&ouml;nnen nicht einfach gel&ouml;scht werden. Die Daten werden beim Seitenaufruf automatisch an Google &uuml;bermittelt. Um diese Daten vorzeitig l&ouml;schen zu k&ouml;nnen, m&uuml;ssen Sie den Google-Support auf&nbsp;&nbsp;<a href="https://support.google.com/?hl=de&amp;tid=311128432">https://support.google.com/?hl=de&amp;tid=311128432</a>&nbsp;kontaktieren. Datenspeicherung verhindern Sie in diesem Fall nur, wenn Sie unsere Seite nicht besuchen.</p>

            <p>Anders als andere Web-Schriften erlaubt uns Google uneingeschr&auml;nkten Zugriff auf alle Schriftarten. Wir k&ouml;nnen also unlimitiert auf ein Meer an Schriftarten zugreifen und so das Optimum f&uuml;r unsere Webseite rausholen. Mehr zu Google Fonts und weiteren Fragen finden Sie auf&nbsp;&nbsp;<a href="https://developers.google.com/fonts/faq?tid=311128432">https://developers.google.com/fonts/faq?tid=311128432</a>. Dort geht zwar Google auf datenschutzrelevante Angelegenheiten ein, doch wirklich detaillierte Informationen &uuml;ber Datenspeicherung sind nicht enthalten. Es ist relativ schwierig (beinahe unm&ouml;glich), von Google wirklich pr&auml;zise Informationen &uuml;ber gespeicherten Daten zu bekommen.</p>

            <p>Welche Daten grunds&auml;tzlich von Google erfasst werden und wof&uuml;r diese Daten verwendet werden, k&ouml;nnen Sie auch auf&nbsp;&nbsp;<a href="https://policies.google.com/privacy?hl=de&amp;tid=311128432">https://www.google.com/intl/de/policies/privacy/</a>&nbsp;nachlesen.</p>

            <h2>YouTube Datenschutzerkl&auml;rung</h2>

            <p>Wir haben auf unserer Webseite YouTube-Videos eingebaut. So k&ouml;nnen wir Ihnen interessante Videos direkt auf unserer Seite pr&auml;sentieren. YouTube ist ein Videoportal, das seit 2006 eine Tochterfirma von Google LLC ist. Betrieben wird das Videoportal durch YouTube, LLC, 901 Cherry Ave., San Bruno, CA 94066, USA. Wenn Sie auf unserer Webseite eine Seite aufrufen, die ein YouTube-Video eingebettet hat, verbindet sich Ihr Browser automatisch mit den Servern von YouTube bzw. Google. Dabei werden (je nach Einstellungen) verschiedene Daten &uuml;bertragen. F&uuml;r die gesamte Datenverarbeitung ist Google verantwortlich und es gilt somit auch der Datenschutz von Google.</p>

            <p>Im Folgenden wollen wir Ihnen genauer erkl&auml;ren, welche Daten verarbeitet werden, warum wir YouTube-Videos eingebunden haben und wie Sie Ihre Daten verwalten oder l&ouml;schen k&ouml;nnen.</p>

            <h3>Was ist YouTube?</h3>

            <p>Auf YouTube k&ouml;nnen die User kostenlos Videos ansehen, bewerten, kommentieren und selbst hochladen. &Uuml;ber die letzten Jahre wurde YouTube zu einem der wichtigsten Social-Media-Kan&auml;le weltweit. Damit wir Videos auf unserer Webseite anzeigen k&ouml;nnen, stellt YouTube einen Codeausschnitt zur Verf&uuml;gung, den wir auf unserer Seite eingebaut haben.</p>

            <h3>Warum verwenden wir YouTube-Videos auf unserer Webseite?</h3>

            <p>YouTube ist die Videoplattform mit den meisten Besuchern und dem besten Content. Wir sind bem&uuml;ht, Ihnen die bestm&ouml;gliche User-Erfahrung auf unserer Webseite zu bieten. Und nat&uuml;rlich d&uuml;rfen interessante Videos dabei nicht fehlen. Mithilfe unserer eingebetteten Videos stellen wir Ihnen neben unseren Texten und Bildern weiteren hilfreichen Content zur Verf&uuml;gung. Zudem wird unsere Webseite auf der Google-Suchmaschine durch die eingebetteten Videos leichter gefunden. Auch wenn wir &uuml;ber Google Ads Werbeanzeigen schalten, kann Google &ndash; dank der gesammelten Daten &ndash; diese Anzeigen wirklich nur Menschen zeigen, die sich f&uuml;r unsere Angebote interessieren.</p>

            <h3>Welche Daten werden von YouTube gespeichert?</h3>

            <p>Sobald Sie eine unserer Seiten besuchen, die ein YouTube-Video eingebaut hat, setzt YouTube zumindest ein Cookie, das Ihre IP-Adresse und unsere URL speichert. Wenn Sie in Ihrem YouTube-Konto eingeloggt sind, kann YouTube Ihre Interaktionen auf unserer Webseite meist mithilfe von Cookies Ihrem Profil zuordnen. Dazu z&auml;hlen Daten wie Sitzungsdauer, Absprungrate, ungef&auml;hrer Standort, technische Informationen wie Browsertyp, Bildschirmaufl&ouml;sung oder Ihr Internetanbieter. Weitere Daten k&ouml;nnen Kontaktdaten, etwaige Bewertungen, das Teilen von Inhalten &uuml;ber Social Media oder das Hinzuf&uuml;gen zu Ihren Favoriten auf YouTube sein.</p>

            <p>Wenn Sie nicht in einem Google-Konto oder einem Youtube-Konto angemeldet sind, speichert Google Daten mit einer eindeutigen Kennung, die mit Ihrem Ger&auml;t, Browser oder App verkn&uuml;pft sind. So bleibt beispielsweise Ihre bevorzugte Spracheinstellung beibehalten. Aber viele Interaktionsdaten k&ouml;nnen nicht gespeichert werden, da weniger Cookies gesetzt werden.</p>

            <p>In der folgenden Liste zeigen wir Cookies, die in einem Test im Browser gesetzt wurden. Wir zeigen einerseits Cookies, die ohne angemeldeten YouTube-Konto gesetzt werden. Andererseits zeigen wir Cookies, die mit angemeldetem Account gesetzt werden. Die Liste kann keinen Vollst&auml;ndigkeitsanspruch erheben, weil die Userdaten immer von den Interaktionen auf YouTube abh&auml;ngen.</p>
            <ul>
                <li>Name: YSC</li>
                <li>Wert: b9-CV6ojI5Y</li>
                <li>Verwendungszweck: Dieses Cookie registriert eine eindeutige ID, um Statistiken des gesehenen Videos zu speichern.</li>
                <li>Ablaufdatum: nach Sitzungsende</li>
                <li>Name: PREF</li>
                <li>Wert: f1=50000000</li>
            </ul>
            <p>Verwendungszweck:&nbsp;Dieses Cookie registriert ebenfalls Ihre eindeutige ID. Google bekommt &uuml;ber PREF Statistiken, wie Sie YouTube-Videos auf unserer Webseite verwenden.</p>
            <ul>
                <li>
                    <p>Ablaufdatum:&nbsp;nach 8 Monate</p>
                </li>
                <li>
                    <p>Name:&nbsp;GPS</p>
                </li>
                <li>
                    <p>Wert:&nbsp;1</p>
                </li>
            </ul>

            <p>Verwendungszweck:&nbsp;Dieses Cookie registriert Ihre eindeutige ID auf mobilen Ger&auml;ten, um den GPS-Standort zu tracken.</p>
            <ul>
                <li>
                    <p>Ablaufdatum:&nbsp;nach 30 Minuten</p>
                </li>
                <li>
                    <p>Name:&nbsp;VISITOR_INFO1_LIVE</p>
                </li>
                <li>
                    <p>Wert:&nbsp;95Chz8bagyU</p>
                </li>
            </ul>

            <p>Verwendungszweck:&nbsp;Dieses Cookie versucht die Bandbreite des Users auf unseren Webseiten (mit eingebautem YouTube-Video) zu sch&auml;tzen.</p>
            <ul>
                <li>
                    <p>Ablaufdatum:&nbsp;nach 8 Monaten</p>
                </li>
            </ul>


            <p>Weitere Cookies, die gesetzt werden, wenn Sie mit Ihrem YouTube-Konto angemeldet sind:</p>
            <ul>
                <li>Name: APISID</li>
                <li>Wert: zILlvClZSkqGsSwI/AU1aZI6HY7311128432-</li>
            </ul>

            <p>Verwendungszweck:&nbsp;Dieses Cookie wird verwendet, um ein Profil &uuml;ber Ihre Interessen zu erstellen. Gen&uuml;tzt werden die Daten f&uuml;r personalisierte Werbeanzeigen.</p>
            <ul>

                <li>
                    <p>Ablaufdatum:&nbsp;nach 2 Jahre</p>
                </li>
                <li>
                    <p>Name:&nbsp;CONSENT</p>
                </li>
                <li>
                    <p>Wert:&nbsp;YES+AT.de+20150628-20-0</p>
                </li>
            </ul>

            <p>Verwendungszweck:&nbsp;Das Cookie speichert den Status der Zustimmung eines Users zur Nutzung unterschiedlicher Services von Google. CONSENT dient auch der Sicherheit, um User zu &uuml;berpr&uuml;fen und Userdaten vor unbefugten Angriffen zu sch&uuml;tzen.</p>

            <ul>
                <li>
                    <p>Ablaufdatum:&nbsp;nach 19 Jahren</p>
                </li>
                <li>
                    <p>Name:&nbsp;HSID</p>
                </li>
                <li>
                    <p>Wert:&nbsp;AcRwpgUik9Dveht0I</p>
                </li>
            </ul>


            <p>Verwendungszweck:&nbsp;Dieses Cookie wird verwendet, um ein Profil &uuml;ber Ihre Interessen zu erstellen. Diese Daten helfen personalisierte Werbung anzeigen zu k&ouml;nnen.</p>
            <ul>
                <li>
                    <p>Ablaufdatum:&nbsp;nach 2 Jahren</p>
                </li>
                <li>
                    <p>Name:&nbsp;LOGIN_INFO</p>
                </li>
                <li>
                    <p>Wert:&nbsp;AFmmF2swRQIhALl6aL&hellip;</p>
                </li>
            </ul>

            <p>Verwendungszweck:&nbsp;In diesem Cookie werden Informationen &uuml;ber Ihre Login-Daten gespeichert.</p>

            <ul>
                <li>
                    <p>Ablaufdatum:&nbsp;nach 2 Jahren</p>
                </li>
                <li>
                    <p>Name:&nbsp;SAPISID</p>
                </li>
                <li>
                    <p>Wert:&nbsp;7oaPxoG-pZsJuuF5/AnUdDUIsJ9iJz2vdM</p>
                </li>
            </ul>

            <p>Verwendungszweck:&nbsp;Dieses Cookie funktioniert, indem es Ihren Browser und Ihr Ger&auml;t eindeutig identifiziert. Es wird verwendet, um ein Profil &uuml;ber Ihre Interessen zu erstellen.</p>

            <ul>
                <li>
                    <p>Ablaufdatum:&nbsp;nach 2 Jahren</p>
                </li>
                <li>
                    <p>Name:&nbsp;SID</p>
                </li>
                <li>
                    <p>Wert:&nbsp;oQfNKjAsI311128432-</p>
                </li>
            </ul>

            <p>Verwendungszweck:&nbsp;Dieses Cookie speichert Ihre Google-Konto-ID und Ihren letzten Anmeldezeitpunkt in digital signierter und verschl&uuml;sselter Form.</p>
            <ul>
                <li>
                    <p>Ablaufdatum:&nbsp;nach 2 Jahren</p>
                </li>
                <li>
                    <p>Name:&nbsp;SIDCC</p>
                </li>
                <li>
                    <p>Wert:&nbsp;AN0-TYuqub2JOcDTyL</p>
                </li>
            </ul>

            <p>Verwendungszweck:&nbsp;Dieses Cookie speichert Informationen, wie Sie die Webseite nutzen und welche Werbung Sie vor dem Besuch auf unserer Seite m&ouml;glicherweise gesehen haben.</p>
            <ul>
                <li>
                    <p>Ablaufdatum:&nbsp;nach 3 Monaten</p>
                </li>
            </ul>

            <h3>Wie lange und wo werden die Daten gespeichert?</h3>

            <p>Die Daten, die YouTube von Ihnen erh&auml;lt und verarbeitet werden auf den Google-Servern gespeichert. Die meisten dieser Server befinden sich in Amerika. Unter&nbsp;&nbsp;<a href="https://www.google.com/about/datacenters/inside/locations/?hl=de">https://www.google.com/about/datacenters/inside/locations/?hl=de</a>&nbsp; sehen Sie genau wo sich die Google-Rechenzentren befinden. Ihre Daten sind auf den Servern verteilt. So sind die Daten schneller abrufbar und vor Manipulation besser gesch&uuml;tzt.</p>

            <p>Die erhobenen Daten speichert Google unterschiedlich lang. Manche Daten k&ouml;nnen Sie jederzeit l&ouml;schen, andere werden automatisch nach einer begrenzten Zeit gel&ouml;scht und wieder andere werden von Google &uuml;ber l&auml;ngere Zeit gespeichert. Einige Daten (wie Elemente aus &bdquo;Meine Aktivit&auml;t&ldquo;, Fotos oder Dokumente, Produkte), die in Ihrem Google-Konto gespeichert sind, bleiben so lange gespeichert, bis Sie sie l&ouml;schen. Auch wenn Sie nicht in einem Google-Konto angemeldet sind, k&ouml;nnen Sie einige Daten, die mit Ihrem Ger&auml;t, Browser oder App verkn&uuml;pft sind, l&ouml;schen.</p>

            <h3>Wie kann ich meine Daten l&ouml;schen bzw. die Datenspeicherung verhindern?</h3>

            <p>Grunds&auml;tzlich k&ouml;nnen Sie Daten im Google Konto manuell l&ouml;schen. Mit der 2019 eingef&uuml;hrten automatische L&ouml;schfunktion von Standort- und Aktivit&auml;tsdaten werden Informationen abh&auml;ngig von Ihrer Entscheidung &ndash; entweder 3 oder 18 Monate gespeichert und dann gel&ouml;scht.</p>

            <p>Unabh&auml;ngig, ob Sie ein Google-Konto haben oder nicht, k&ouml;nnen Sie Ihren Browser so konfigurieren, dass Cookies von Google gel&ouml;scht bzw. deaktiviert werden. Je nach dem welchen Browser Sie verwenden, funktioniert dies auf unterschiedliche Art und Weise. Die folgenden Anleitungen zeigen, wie Sie Cookies in Ihrem Browser verwalten:</p>

            <p><a href="https://support.google.com/chrome/answer/95647?tid=311128432">Chrome: Cookies in Chrome l&ouml;schen, aktivieren und verwalten</a></p>

            <p><a href="https://support.apple.com/de-at/guide/safari/sfri11471/mac?tid=311128432">Safari: Verwalten von Cookies und Websitedaten mit Safari</a></p>

            <p><a href="https://support.mozilla.org/de/kb/cookies-und-website-daten-in-firefox-loschen?tid=311128432">Firefox: Cookies l&ouml;schen, um Daten zu entfernen, die Websites auf Ihrem Computer abgelegt haben</a></p>

            <p><a href="https://support.microsoft.com/de-at/help/17442/windows-internet-explorer-delete-manage-cookies?tid=311128432">Internet Explorer: L&ouml;schen und Verwalten von Cookies</a></p>

            <p><a href="https://support.microsoft.com/de-at/help/4027947/windows-delete-cookies?tid=311128432">Microsoft Edge: L&ouml;schen und Verwalten von Cookies</a></p>

            <p>Falls Sie grunds&auml;tzlich keine Cookies haben wollen, k&ouml;nnen Sie Ihren Browser so einrichten, dass er Sie immer informiert, wenn ein Cookie gesetzt werden soll. So k&ouml;nnen Sie bei jedem einzelnen Cookie entscheiden, ob Sie es erlauben oder nicht. Da YouTube ein Tochterunternehmen von Google ist, gibt es eine gemeinsame Datenschutzerkl&auml;rung. Wenn Sie mehr &uuml;ber den Umgang mit Ihren Daten erfahren wollen, empfehlen wir Ihnen die Datenschutzerkl&auml;rung unter&nbsp;&nbsp;<a href="https://policies.google.com/privacy?hl=de">https://policies.google.com/privacy?hl=de.</a></p>

            <h2>YouTube Abonnieren Button Datenschutzerkl&auml;rung</h2>

            <p>Wir haben auf unserer Webseite den YouTube Abonnieren Button (engl. &bdquo;Subscribe-Button&ldquo;) eingebaut. Sie erkennen den Button meist am klassischen YouTube-Logo. Das Logo zeigt vor rotem Hintergrund in wei&szlig;er Schrift die W&ouml;rter &bdquo;Abonnieren&ldquo; oder &bdquo;YouTube&ldquo; und links davon das wei&szlig;e &bdquo;Play-Symbol&ldquo;. Der Button kann aber auch in einem anderen Design dargestellt sein.</p>

            <p>Unser YouTube-Kanal bietet Ihnen immer wieder lustige, interessante oder spannende Videos. Mit dem eingebauten &bdquo;Abonnieren-Button&ldquo; k&ouml;nnen Sie unseren Kanal direkt von unserer Webseite aus abonnieren und m&uuml;ssen nicht eigens die YouTube-Webseite aufrufen. Wir wollen Ihnen somit den Zugang zu unserem umfassenden Content so einfach wie m&ouml;glich machen. Bitte beachten Sie, dass YouTube dadurch Daten von Ihnen speichern und verarbeiten kann.</p>

            <p>Wenn Sie auf unserer Seite einen eingebauten Abo-Button sehen, setzt YouTube &ndash; laut Google &ndash; mindestens ein Cookie. Dieses Cookie speichert Ihre IP-Adresse und unsere URL. Auch Informationen &uuml;ber Ihren Browser, Ihren ungef&auml;hren Standort und Ihre voreingestellte Sprache kann YouTube so erfahren. Bei unserem Test wurden folgende vier Cookies gesetzt, ohne bei YouTube angemeldet zu sein:</p>
            <ul>
                <li>
                    <p>Name:&nbsp;YSC</p>
                </li>
                <li>
                    <p>Wert:&nbsp;b9-CV6ojI5311128432Y</p>
                </li>
                <li>
                    <p>Verwendungszweck:&nbsp;Dieses Cookie registriert eine eindeutige ID, um Statistiken des gesehenen Videos zu speichern.</p>
                </li>
                <li>
                    <p>Ablaufdatum:&nbsp;nach Sitzungsende</p>
                </li>
            </ul>

            <ul>
                <li>
                    <p>Name:&nbsp;PREF</p>
                </li>
                <li>
                    <p>Wert:&nbsp;f1=50000000</p>
                </li>
            </ul>

            <p>Verwendungszweck:&nbsp;Dieses Cookie registriert ebenfalls Ihre eindeutige ID. Google bekommt &uuml;ber PREF Statistiken, wie Sie YouTube-Videos auf unserer Webseite verwenden.</p>

            <ul>
                <li>
                    <p>Ablaufdatum:&nbsp;nach 8 Monate</p>
                </li>
                <li>
                    <p>Name:&nbsp;GPS</p>
                </li>
                <li>
                    <p>Wert:&nbsp;1</p>
                </li>
            </ul>

            <p>Verwendungszweck:&nbsp;Dieses Cookie registriert Ihre eindeutige ID auf mobilen Ger&auml;ten, um den GPS-Standort zu tracken.</p>
            <ul>
                <li>
                    <p>Ablaufdatum:&nbsp;nach 30 Minuten</p>
                </li>
                <li>
                    <p>Name:&nbsp;VISITOR_INFO1_LIVE</p>
                </li>
                <li>
                    <p>Wert:&nbsp;31112843295Chz8bagyU</p>
                </li>
            </ul>

            <p>Verwendungszweck:&nbsp;Dieses Cookie versucht die Bandbreite des Users auf unseren Webseiten (mit eingebautem YouTube-Video) zu sch&auml;tzen.</p>
            <ul>
                <li>
                    <p>Ablaufdatum:&nbsp;nach 8 Monaten</p>
                </li>
            </ul>
            <p>Anmerkung:&nbsp;Diese Cookies wurden nach einem Test gesetzt und k&ouml;nnen nicht den Anspruch auf Vollst&auml;ndigkeit erheben.</p>

            <p>Wenn Sie in Ihrem YouTube-Konto angemeldet sind, kann YouTube viele Ihrer Handlungen/Interaktionen auf unserer Webseite mit Hilfe von Cookies speichern und Ihrem YouTube-Konto zuordnen. YouTube bekommt dadurch zum Beispiel Informationen wie lange Sie auf unserer Seite surfen, welchen Browsertyp Sie verwenden, welche Bildschirmaufl&ouml;sung Sie bevorzugen oder welche Handlungen Sie ausf&uuml;hren.</p>

            <p>YouTube verwendet diese Daten zum einen um die eigenen Dienstleistungen und Angebote zu verbessern, zum anderen um Analysen und Statistiken f&uuml;r Werbetreibende (die Google Ads verwenden) bereitzustellen.</p>

            <h4>Online-Stellenbewerbungen / Veröffentlichung von Stellenanzeigen</h4>
            <p>Wir bieten Ihnen die Möglichkeit an, sich bei uns über unseren Internetauftritt bewerben zu können. Bei diesen digitalen Bewerbungen werden Ihre Bewerber- und Bewerbungsdaten von uns zur Abwicklung des Bewerbungsverfahrens elektronisch erhoben und verarbeitet.</p>
            <p>Rechtsgrundlage für diese Verarbeitung ist § 26 Abs. 1 S. 1 BDSG i.V.m. Art. 88 Abs. 1 DSGVO.</p>
            <p>Sofern nach dem Bewerbungsverfahren ein Arbeitsvertrag geschlossen wird, speichern wir Ihre bei der Bewerbung übermittelten Daten in Ihrer Personalakte zum Zwecke des üblichen Organisations- und Verwaltungsprozesses – dies natürlich unter Beachtung der weitergehenden rechtlichen Verpflichtungen.</p>
            <p>Rechtsgrundlage für diese Verarbeitung ist ebenfalls § 26 Abs. 1 S. 1 BDSG i.V.m. Art. 88 Abs. 1 DSGVO.</p>
            <p>Bei der Zurückweisung einer Bewerbung löschen wir die uns übermittelten Daten automatisch zwei Monate nach der Bekanntgabe der Zurückweisung. Die Löschung erfolgt jedoch nicht, wenn die Daten aufgrund gesetzlicher Bestimmungen, bspw. wegen der Beweispflichten nach dem AGG, eine längere Speicherung von bis zu vier Monaten oder bis zum Abschluss eines gerichtlichen Verfahrens erfordern.</p>
            <p>Rechtsgrundlage ist in diesem Fall Art. 6 Abs. 1 lit. f) DSGVO und § 24 Abs. 1 Nr. 2 BDSG. Unser berechtigtes Interesse liegt in der Rechtsverteidigung bzw. -durchsetzung.</p>
            <p>Sofern Sie ausdrücklich in eine längere Speicherung Ihrer Daten einwilligen, bspw. für Ihre Aufnahme in eine Bewerber- oder Interessentendatenbank, werden die Daten aufgrund Ihrer Einwilligung weiterverarbeitet. Rechtsgrundlage ist dann Art. 6 Abs. 1 lit. a) DSGVO. Ihre Einwilligung können Sie aber natürlich jederzeit nach Art. 7 Abs. 3 DSGVO durch Erklärung uns gegenüber mit Wirkung für die Zukunft widerrufen.</p>

            <p>
                <a target="_blank" href="https://www.adsimple.de/datenschutz-generator/">Muster-Datenschutzerklärung</a> der <a target="_blank" href="https://www.adsimple.de/">adsimple.de</a>
            </p>



            <hr />
            <p style="padding-top: 20px">
                memucho wird im Rahmen des EXIST-Programms durch das Bundesministerium für Wirtschaft und Energie und den Europäischen Sozialfonds gefördert.
            </p>

        </div>
    </div>

</asp:Content>
