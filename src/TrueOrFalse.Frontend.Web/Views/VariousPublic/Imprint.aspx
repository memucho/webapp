<%@ Page Title="Impressum & Datenschutz" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

<script type="text/javascript" >

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
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<div class="row" style="padding-top:30px;">
    <div class="BackToHome col-md-3">
        <a href="/"><i class="fa fa-chevron-left">&nbsp;</i>Zur Startseite</a>
    </div>
    <div class="form-horizontal col-md-9">

        <h2 class="PageHeader">Impressum & Datenschutz</h2>

        <p style="padding-bottom: 5px;">Angaben gemäß § 5 TMG:<br/></p>
        Christof Mauersberger<br />
        Leopoldstraße 8b<br />
        10317 Berlin<br />

        <h3>Kontakt:</h3>
        <table><tr>
        <td><p>Telefon: </p></td>
        <td><p>+49-1577-6825707</p></td></tr>
        <tr><td><p>E-Mail:</p></td>
        <td><p><span class="mailme">team at memucho dot de</span></p></td>
        </tr></table>
        <h3>Verantwortlich für den Inhalt nach § 55 Abs. 2 RStV:</h3>
        <p>Christof Mauersberger<br />
        <br />
        Leopoldstraße 8b<br />
        10317 Berlin</p>
        <p> </p>
        <p>Quelle: <i>erstellt mit dem <a href="http://www.e-recht24.de/impressum-generator.html" target="_blank">Impressum-Generator Einzelunternehmer</a> von eRecht24 u. in Teilen angepasst.</i></p>

        <h3>Haftungsausschluss</h3>
        <p><strong>Haftung für Inhalte</strong></p>
        <p>Die Inhalte unserer Seiten wurden mit größter Sorgfalt erstellt. 
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
            umgehend entfernen.</p>
        <p><strong>Haftung für Links</strong></p>
        <p>Unser Angebot enthält Links zu externen Webseiten Dritter, auf deren 
            Inhalte wir keinen Einfluss haben. Deshalb können wir für diese 
            fremden Inhalte auch keine Gewähr übernehmen. Für die Inhalte 
            der verlinkten Seiten ist stets der jeweilige Anbieter oder Betreiber der 
            Seiten verantwortlich. Die verlinkten Seiten wurden zum Zeitpunkt der Verlinkung 
            auf mögliche Rechtsverstöße überprüft. Rechtswidrige 
            Inhalte waren zum Zeitpunkt der Verlinkung nicht erkennbar. Eine permanente 
            inhaltliche Kontrolle der verlinkten Seiten ist jedoch ohne konkrete Anhaltspunkte 
            einer Rechtsverletzung nicht zumutbar. Bei Bekanntwerden von Rechtsverletzungen 
            werden wir derartige Links umgehend entfernen.</p>
        <p><strong>Datenschutz</strong></p>
        <p>Die Nutzung unserer Webseite ist teils ohne Angabe personenbezogener Daten möglich. Soweit auf unseren Seiten personenbezogene Daten (beispielsweise Name, 
            Anschrift oder eMail-Adressen) erhoben werden, erfolgt dies, soweit möglich, stets auf freiwilliger Basis. Diese Daten werden ohne Ihre ausdrückliche Zustimmung nicht an Dritte weitergegeben.   
        </p>
        <p>Wir weisen darauf hin, dass die Datenübertragung im Internet (z.B. 
            bei der Kommunikation per E-Mail) Sicherheitslücken aufweisen kann. 
            Ein lückenloser Schutz der Daten vor dem Zugriff durch Dritte ist nicht 
            möglich. </p>
        <p>Der Nutzung von im Rahmen der Impressumspflicht veröffentlichten Kontaktdaten 
            durch Dritte zur Übersendung von nicht ausdrücklich angeforderter 
            Werbung und Informationsmaterialien wird hiermit ausdrücklich widersprochen. 
            Die Betreiber der Seiten behalten sich ausdrücklich rechtliche Schritte 
            im Falle der unverlangten Zusendung von Werbeinformationen, etwa durch Spam-Mails, 
            vor.</p><p> </p>
            <p>Quelle: <i><a href="http://www.e-recht24.de/muster-disclaimer.htm" target="_blank">Disclaimer</a> von eRecht24, dem Portal zum Internetrecht von <a href="http://www.e-recht24.de/" target="_blank">Rechtsanwalt</a> Sören Siebert.</i>
        </p>
        
        <h3>Datenschutz</h3>
        
        <p><strong>Erfassung von Daten</strong></p>
        <p>
            Während Sie auf unsere Webseiten zugreifen, erfassen wir automatisch Daten von allgemeiner 
            Natur. Diese Daten (Server-Logfiles) umfassen zum Beispiel die Art Ihres Browsers, Ihr 
            Betriebssystem, den Domainnamen Ihres Internetanbieters sowie weitere ähnliche allgemeine Daten. 
            Diese Daten fallen bei jeder Nutzung des Internets an, sind personenunabhängig und werden 
            unter anderem dazu genutzt, um Ihnen die Webseiten korrekt darzustellen. Diese anonymen Daten 
            werden statistisch ausgewertet, um unseren Service für Sie zu verbessern.    
        </p>
        
        <p><strong>Anmeldung auf unserer Webseite</strong></p>
        <p>
            Bei der Anmeldung für spezifische Angebote erfragen wir einige persönliche Daten wie Name, 
            Anschrift, Kontakt, Telefonnummer oder E-Mail-Adresse. Angemeldete Nutzer können auf bestimmte 
            Zusatzleistungen zugreifen. Angemeldete Nutzer haben die Möglichkeit, alle angegebenen persönlichen 
            Daten zu jedem Zeitpunkt zu ändern oder löschen zu lassen. Sie können auch jederzeit die von ihnen 
            gespeicherten Daten bei uns erfragen. Soweit gesetzlich keine Frist für die Aufbewahrung der Daten 
            besteht, können diese geändert oder gelöscht werden. Bitte kontaktieren Sie uns dazu über unsere Kontaktdaten.            
        </p>
        
        <p><strong>Kostenpflichtige Leistungen</strong></p>
        <p>
            Wenn Sie kostenpflichtige Leistungen in Anspruch nehmen, können zusätzliche Daten erforderlich werden, unter 
            anderen Angaben zur Art der Bezahlung.            
        </p>
        <p>
            Die Verwendung von aktuellster Technik und Verschlüsselungsverfahren sorgt für einen umfassenden Schutz ihrer Daten.            
        </p>
        
        <p><strong>Kontakt</strong></p>
        <p>
            Wenn Sie uns über unsere Kontaktseite kontaktieren oder uns eine E-Mail schicken, werden die entsprechenden 
            Daten zur Bearbeitung gespeichert.            
        </p>
        
        <p><strong>Löschung oder Sperrung von Daten</strong></p>
        <p>
            Ihre persönlichen Daten werden nur so lange gespeichert, wie dies vom Gesetz vorgeschrieben beziehungsweise 
            notwendig ist, um die angegebenen Dienste zu leisten. Nach Ablauf dieser Fristen werden die persönlichen Daten 
            der Nutzer regelmäßig gesperrt oder gelöscht.            
        </p>
        
        <p><strong>Cookies</strong></p>
        <p>
            Unsere Webseite verwendet „Cookies“. Cookies sind Textdateien, die vom Server einer Webseite auf 
            Ihren Rechner übertragen werden. Dazu gehören bestimmte Daten wie Ihre IP-Adresse, Browser, Betriebssystem 
            und Internet-Verbindung.
        </p>
        <p>
            Cookies starten keine Programme und übertragen keine Viren. Die durch Cookies gesammelten Informationen 
            dienen dazu, Ihnen die Navigation zu erleichtern und die Anzeige unserer Webseiten zu optimieren.            
        </p>
        <p>
            Daten, die von uns erfasst werden, werden niemals ohne Ihre Einwilligung an Dritte weitergegeben 
            oder mit personenbezogenen Daten verknüpft.            
        </p>
        <p>
            Die Verwendung von Cookies kann über Einstellungen in ihrem Browser verhindert werden. In den Erläuterungen 
            zu Ihrem Internetbrowser finden Sie Informationen darüber, wie man diese Einstellungen verändern kann. 
            Einzelne Funktionen unserer Website können unter Umständen nicht richtig funktionieren, wenn die Verwendung 
            von Cookies deaktiviert ist.            
        </p>

        <p><strong>Datenschutzerklärung für die Nutzung von Facebook-Plugins (Like-Button)</strong></p>
        <p>
            Auf unseren Seiten sind Plugins des sozialen Netzwerks Facebook, Anbieter Facebook Inc., 
            1 Hacker Way, Menlo Park, California 94025, USA, integriert. Die Facebook-Plugins erkennen 
            Sie an dem Facebook-Logo oder dem "Like-Button" ("Gefällt mir") auf unserer Seite. Eine 
            übersicht über die Facebook-Plugins finden Sie hier: 
            <a href="http://developers.facebook.com/docs/plugins/">http://developers.facebook.com/docs/plugins/</a>.
        </p>
        <p>
            Wenn Sie unsere Seiten besuchen, wird über das Plugin eine direkte Verbindung zwischen 
            Ihrem Browser und dem Facebook-Server hergestellt. Facebook erhält dadurch die Information, 
            dass Sie mit Ihrer IP-Adresse unsere Seite besucht haben. Wenn Sie den Facebook "Like-Button" 
            anklicken während Sie in Ihrem Facebook-Account eingeloggt sind, können Sie die Inhalte 
            unserer Seiten auf Ihrem Facebook-Profil verlinken. Dadurch kann Facebook den Besuch unserer 
            Seiten Ihrem Benutzerkonto zuordnen. Wir weisen darauf hin, dass wir als Anbieter der Seiten 
            keine Kenntnis vom Inhalt der übermittelten Daten sowie deren Nutzung durch Facebook erhalten. 
            Weitere Informationen hierzu finden Sie in der Datenschutzerklärung von Facebook 
            unter <a href="http://de-de.facebook.com/policy.php">http://de-de.facebook.com/policy.php</a>.
        </p>
        <p>
            Wenn Sie nicht wünschen, dass Facebook den Besuch unserer Seiten Ihrem Facebook-Nutzerkonto 
            zuordnen kann, loggen Sie sich bitte aus Ihrem Facebook-Benutzerkonto aus.
        </p>
        <p> </p>
        <p><strong>
            Datenschutzerklärung für die Nutzung von Google Analytics
        </strong></p>
        <p>
            Diese Website nutzt Funktionen des Webanalysedienstes Google Analytics. Anbieter ist die Google Inc., 
            1600 Amphitheatre Parkway Mountain View, CA 94043, USA.
        </p>
        <p>
            Google Analytics verwendet sog. "Cookies". Das sind Textdateien, die auf Ihrem Computer gespeichert 
            werden und die eine Analyse der Benutzung der Website durch Sie ermöglichen. Die durch den Cookie 
            erzeugten Informationen über Ihre Benutzung dieser Website werden in der Regel an einen Server von 
            Google in den USA übertragen und dort gespeichert.
        </p>
        <p>
            Im Falle der Aktivierung der IP-Anonymisierung auf dieser Webseite wird Ihre IP-Adresse von Google 
            jedoch innerhalb von Mitgliedstaaten der Europäischen Union oder in anderen Vertragsstaaten des 
            Abkommens über den Europäischen Wirtschaftsraum zuvor gekürzt. Nur in Ausnahmefällen wird die volle 
            IP-Adresse an einen Server von Google in den USA übertragen und dort gekürzt. Im Auftrag des Betreibers 
            dieser Website wird Google diese Informationen benutzen, um Ihre Nutzung der Website auszuwerten, um 
            Reports über die Websiteaktivitäten zusammenzustellen und um weitere mit der Websitenutzung und der 
            Internetnutzung verbundene Dienstleistungen gegenüber dem Websitebetreiber zu erbringen. Die im Rahmen 
            von Google Analytics von Ihrem Browser übermittelte IP-Adresse wird nicht mit anderen Daten von Google 
            zusammengeführt.
        </p>
        <p>
            Sie können die Speicherung der Cookies durch eine entsprechende Einstellung Ihrer Browser-Software 
            verhindern; wir weisen Sie jedoch darauf hin, dass Sie in diesem Fall gegebenenfalls nicht sämtliche 
            Funktionen dieser Website vollumfänglich werden nutzen können. Sie können darüber hinaus die Erfassung 
            der durch das Cookie erzeugten und auf Ihre Nutzung der Website bezogenen Daten (inkl. Ihrer IP-Adresse) 
            an Google sowie die Verarbeitung dieser Daten durch Google verhindern, indem sie das unter dem folgenden 
            Link verfügbare Browser-Plugin herunterladen und 
            installieren: <a href="http://tools.google.com/dlpage/gaoptout?hl=de" >http://tools.google.com/dlpage/gaoptout?hl=de</a>
        </p>
        <p>
            Sie können die Erfassung durch Google Analytics verhindern, indem Sie auf folgenden Link klicken. 
            Es wird ein Opt-Out-Cookie gesetzt, das das Erfassung Ihrer Daten bei zukünftigen Besuchen dieser 
            Website verhindert: <a href="javascript:gaOptout()">Google Analytics deaktivieren</a>
        </p>
        <p> </p>
        <hr/>
        <p style="padding-top: 20px">
            memucho wird im Rahmen des EXIST-Programms durch das Bundesministerium für Wirtschaft und Energie und den Europäischen Sozialfonds gefördert.
        </p>

    </div>
</div>

</asp:Content>
