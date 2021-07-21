<%@ Page Title="Widget: Angebote und Preise" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<BaseModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Help/Widget.css" rel="stylesheet" />
    
    <%= Scripts.Render("~/bundles/mailto") %>
    <script type="text/javascript" >
        $(function () {
            $("a.mailmeMore")
                .each(function () {
                    var hrefOrg = this.getAttribute('href');
                    var spt = hrefOrg.substr(0, hrefOrg.indexOf("?") - 1);
                    var at = / at /;
                    var dot = / dot /g;
                    var addr = spt.replace(at, "@").replace(dot, ".");
                    var hrefNew = "mailto:" + addr + hrefOrg.substr(hrefOrg.indexOf("?"));
                    this.setAttribute('href', hrefNew);
                });
        });
    </script>    
    
    <%= Scripts.Render("~/bundles/js/Help") %>
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Widgets", Url = Links.WidgetPricing(), ToolTipText = "Widgets"});
       Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Angebote und Preise", Url = Links.WidgetPricing(), ToolTipText = "Angebote und Preise"});
       Model.TopNavMenu.IsCategoryBreadCrumb = false; %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% Html.RenderPartial("~/Views/Help/WidgetMenu.ascx", new WidgetMenuModel{CurrentIsPricing = true});  %>

    <div class="row">
        <div class="col-xs-12">

            <div class="well">

                <h1 class="PageHeader"><span class="ColoredUnderline GeneralMemucho">memucho-Widget: Angebote und Preise</span></h1>
                <p class="teaserText">
                    Die Inhalte und Technologie von memucho können als Widget leicht in bestehende Webseiten integriert werden. 
                    Egal ob auf dem privaten Blog, der Vereins- oder Unternehmensseite, oder dem Schul- oder Lernmanagementsystem: Nötig ist eine Zeile HTML-Code, 
                    die du einfach von memucho kopieren kannst (<a href="<%= Links.HelpWidget() %>">eine genaue Anleitung findest du hier</a>).
                </p>
                <p class="teaserText">
                    memucho bietet die Einbettung von Lernsets als Quiz-Modul, von Video-Lernsets und von einzelnen Fragen an. 
                    Die Inhalte und Lernfunktion sind dann nahtlos in deine Seite integriert.
                    Beispiele für unsere Widgets kannst <a href="<%= Links.WidgetExamples() %>">du in dieser Übersicht</a> sehen.
                    Wir haben unterschiedliche Angebote für Einzelpersonen und für Organisationen.
                </p>
                
                <div id="switchesPriceType">
                    <a href="#" id="btnPricingIndividual" class="btn btn-primary btn-large">
                        <span class="buttonTitle">Einzelpersonen</span><br /><small>Individuen, inkl. Lehrer, Dozenten ...</small>
                    </a>
                    <a href="#" id="btnPricingOrgs" class="btn btn-notSelected btn-large">
                         <span class="buttonTitle">Organisationen</span><br /><small>Verlage, Bildungsanbieter</small>
                    </a>
                </div>
                   
                <div id="pricingIndividuals">
                    
                    <div class="content">
                        <div class="row">
                            <div class="col-md-10 col-md-offset-2" style="padding-bottom: 10px;">
                                Preise für Einzelpersonen:
                            </div>
                        </div>
	                    <div class="row">
		                    <!-- Pricing -->
		                    <div class="col-md-4 col-xs-6 xxs-stack col-md-offset-2">
			                    <div class="pricing hover-effect">
				                    <div class="pricing-head">
					                    <h3>Basis<%-- <span>Für alle </span>--%></h3>
					                    <h4>
					                        Kostenlos
                                            <span>für immer</span>
					                    </h4>
				                    </div>
				                    <ul class="pricing-content list-unstyled">
					                    <li>Unbegrenzte Aufrufe</li>
					                    <li>Werbefrei</li>
					                    <li>Maximal 5 Widgets</li>
				                    </ul>
				                    <div class="pricing-footer">
					                    <p>Die Nutzung von 5 Widgets ist im kostenlosen Basis-Account eingeschlossen.</p>
                                        <%  var isLoggedIn = Sl.R<SessionUser>().IsLoggedIn;
                                            if (!isLoggedIn)
                                            { %>
					                            <a href="#" data-btn-login="true" class="btn btn-primary">Jetzt einloggen</a>
                                        <% }
                                            else
                                            {%>
					                            <a href="<%= Links.HelpWidget() %>" class="btn btn-primary">Jetzt Widget einbinden</a>
                                            <%}%>
				                    </div>
			                    </div>
		                    </div>
		                    <div class="col-md-4 col-xs-6 xxs-stack">
			                    <div class="pricing hover-effect">
				                    <div class="pricing-head">
					                    <h3>Förderer<%-- <span>Für alle Pro-Nutzer inklusive</span>--%></h3>
					                    <h4 class="show-tooltip" data-placement="bottom" title="Entsprechend deiner finanziellen Möglichkeiten kannst du den Preis selbst festlegen."> 
					                        <i>ca. € </i>5
                                            <span>pro Monat *</span>
					                    </h4>
				                    </div>
				                    <ul class="pricing-content list-unstyled">
					                    <li>Unbegrenzte Aufrufe</li>
					                    <li>Werbefrei</li>
                                        <li>Unbegrenzte Anzahl an Widgets</li>
				                    </ul>
				                    <div class="pricing-footer">
					                    <p>
						                     Alle Förderer bei memucho können Widgets unbeschränkt und werbefrei nutzen.
					                    </p>
					                    <a href="<%= Links.Membership() %>" class="btn btn-primary">Förderer werden</a>
				                    </div>
			                    </div>
                                <p style="line-height: 12px;padding: 0 15px;margin-top: -8px;text-align: center;"><small>* Entsprechend deiner finanziellen Möglichkeiten kannst du den Preis selbst festlegen.</small></p>
		                    </div>
		                    <!--//End Pricing -->
	                    </div>
                        <div class="row">
                            <div class="col-xs-12 col-md-8 col-md-offset-2">
                                <p>
                                    Als Einzelpersonen zählen alle natürlichen Personen, insbesondere auch Lehrer oder Dozenten, 
                                    die die Widgets für ihre Lehrtätigkeit einsetzen, auch wenn sie Teil einer Bildungsinstitution sind.
                                </p>
                            </div>
                        </div>
                        
                        <h2>
                            <span class="ColoredUnderline GeneralMemucho">Vorteile der memucho-Widgets</span>
                        </h2>
                        
                        <p>
                            Als <strong>Lehrer oder Dozent</strong> kannst du die Widgets vielfältig einsetzen. Du kannst deinen Schülern oder Studenten selber Lerninhalte zusammenstellen 
                            und auf deinem Blog oder dem Schul-LMS anbieten. 
                            Dabei profitierst du von den vielen vorhandenen freien Inhalten, die du rechtssicher verwenden und bei Bedarf ergänzen kannst.
                            Du kannst die Inhalte auch als Teil des Lernprozesses von deinen Schülern oder Studenten erstellen lassen und dann selber bündeln. 
                            Das Ergebnis dieser Schüler-Projektarbeit kann als Quiz auf der Schulwebseite präsentiert werden. 
                            Weitere Ideen zur <a href="<%= Links.ForTeachers() %>">Nutzung von memucho in der Lehre haben wir hier gesammelt</a>.
                        </p>
                        <p>
                            Lernende schätzen die Wissenstests und profitieren direkt davon. Das Wiederholen von Wissen in kleinen Einheiten ist 
                            nachgewiesenermaßen eine effiziente Lernmethode, die in Quiz-Form noch dazu Spaß macht.
                        </p>
                        <p>
                            Wenn du selber eine <strong>Webseite zu Bildungs- oder Wissensthemen</strong> als Hobby betreibst, 
                            helfen dir die memucho-Widgets als <strong>wichtiger SEO-Faktor</strong>. 
                            Quizze und kleine Interaktionsmöglichkeiten wie das Frage-Widget können die 
                            Verweildauer der Nutzer auf deiner Seite deutlich erhöhen. Dadurch schätzen Suchmaschinen deine
                            Seite als hilfreicher ein und verbessern dein Ranking.
                        </p>
                        <p style="text-align: center; margin-top: 40px;">
                            <a href="<%= Links.WidgetExamples() %>" class="btn btn-default">Widgets: Beispiele & Übersicht</a>
                        </p>
                    </div>
                </div>
                

                <div id="pricingOrgs" style="display: none;">
                    
                    <div class="content">
                        
                        <div class="row">
                            <div class="col-md-12" style="padding-bottom: 10px;">
                                Preise für Organisationen:
                            </div>
                        </div>

	                    <div class="row">
		                    <!-- Pricing -->
		                    <div class="col-md-3 col-xs-6 xxs-stack">
			                    <div class="pricing hover-effect">
				                    <div class="pricing-head">
					                    <h3>Start <%--<span>Der Einstieg</span>--%></h3>
					                    <h4>
					                        Kostenlos
                                            <span>für immer</span>
					                    </h4>
				                    </div>
				                    <ul class="pricing-content list-unstyled">
					                    <li>Dezente Werbung<br/><small>(unaufdringliche Sponsor-Links)</small></li>
					                    <li>Mit memucho-Branding</li>
				                        <li>Unbegrenzt Widgets</li>
					                    <li>Unbegrenzte Aufrufe</li>
				                    </ul>
				                    <div class="pricing-footer">
					                    <%--<p>Geeignet als Start und für kleine Blogs und Projekte</p>--%>
					                    <a class="btn btn-primary mailmeMore" href="christof at memucho dot de ?subject=memucho-Widget Start&body=Hallo Christof, uns gefällt euer Angebot Start für die memucho-Widgets. Bitte schicke uns den Code, damit wir das Widget für unsere Organisation verwenden können.">Bestellen</a>
				                    </div>
			                    </div>
		                    </div>
		                    <div class="col-md-3 col-xs-6 xxs-stack">
			                    <div class="pricing hover-effect">
				                    <div class="pricing-head">
					                    <h3>Start Plus <%--<span>Für alle Pro-Nutzer</span>--%></h3>
					                    <h4> 
					                        <i>€ </i>39
                                            <span>pro Monat</span>
					                    </h4>
				                    </div>
				                    <ul class="pricing-content list-unstyled">
					                    <li>Werbefrei</li>
					                    <li>Mit memucho-Branding</li>
                                        <li>Mit Nutzungs-Statistik</li>
					                    <li>Inkl. 500.000 Aufrufe<br/><small>Je 100.000 weitere: € 3,50</small></li>
				                    </ul>
				                    <div class="pricing-footer">
					                    <%--<p>Geeignet für kleinere Content-Anbieter</p>--%>
					                    <a class="btn btn-primary mailmeMore" href="christof at memucho dot de ?subject=memucho-Widget Start Plus&body=Hallo Christof, uns gefällt euer Angebot Start Plus für die memucho-Widgets. Bitte schicke uns den Code, damit wir das Widget für unsere Organisation verwenden können.">Bestellen</a>
				                    </div>
			                    </div>
		                    </div>
		                    <div class="col-md-3 col-xs-6 xxs-stack">
			                    <div class="pricing hover-effect">
				                    <div class="pricing-head">
					                    <h3>White-Label<%-- <span>&nbsp;</span>--%></h3>
					                    <h4>
					                        <i>€ </i>150
                                            <span>pro Monat</span>
					                    </h4>
				                    </div>
				                    <ul class="pricing-content list-unstyled">
					                    <li>Werbefrei</li>
					                    <li>Ohne memucho-Branding</li>
				                        <li>Mit Nutzungs-Statistik</li>
					                    <li>Inkl. 100.000 Aufrufe<br/><small>Je 100.000 weitere: € 9,00</small></li>
				                    </ul>
				                    <div class="pricing-footer">
					                    <%--<p>Geeignet für größere Content-Anbieter und kommerzielle Bildungs-Anbieter</p>--%>
					                    <a class="btn btn-primary mailmeMore" href="christof at memucho dot de ?subject=memucho-Widget White-Label&body=Hallo Christof, uns gefällt euer Angebot White-Label für die memucho-Widgets. Bitte schicke uns den Code, damit wir das Widget für unsere Organisation verwenden können.">Bestellen</a>
				                    </div>
			                    </div>
		                    </div>
		                    <div class="col-md-3 col-xs-6 xxs-stack">
			                    <div class="pricing hover-effect">
				                    <div class="pricing-head">
					                    <h3>Strategisch<%-- <span>Flexibel und nachhaltig</span>--%></h3>
					                    <h4> 
					                        <i>ab € </i>800
                                            <span>pro Monat</span>
					                    </h4>
				                    </div>
				                    <ul class="pricing-content list-unstyled">
					                    <li>Werbefrei, kein Branding</li>
				                        <li>Mit Nutzungs-Statistik</li>
				                        <li>SLAs (Lifetime)</li>
					                    <li>Workflows/<br/><small>Individualisierung</small></li>
				                    </ul>
				                    <div class="pricing-footer">
					                    <%--<p>Geeignet für Bildungsverlage und große Content-Anbieter</p>--%>
					                    <a class="btn btn-primary mailmeMore" href="christof at memucho dot de ?subject=memucho-Widget Strategisch&body=Hallo Christof, uns gefällt euer Angebot Strategisch für die memucho-Widgets. Wir haben folgende Bedürfnisse und bitten um ein genaues Angebot: ...">Anfragen</a>
				                    </div>
			                    </div>
		                    </div>
		                    <!--//End Pricing -->
	                    </div>
                        
                        <p class="pricing" style="padding: 10px; color: #777; font-size: 14px;">
                            <b>Gemeinnützig? Gemeinwohlorientiert?</b><br />
                            Deine Organisation oder Unternehmen ist gemeinnützig oder gemeinwohlorientiert? Du förderst offene Bildungsinhalte und/oder Open-Source? 
                            Du kannst die Widgets vermutlich <b>kostenlos und uneingeschränkt</b> nutzen. <a href="#contact">Sprich uns an :-)</a>
                        </p>
                        
                        <h2>
                            <span class="ColoredUnderline GeneralMemucho">Vorteile der memucho-Widgets</span>
                        </h2>
                        
                        <p>
                            Die memucho-Widgets eignen sich <strong>für alle Content-Anbieter:</strong> Von der lokalen Zeitung, der kleinen Vereins- oder NGO-Seite, 
                            die ihre Webseite attraktiver gestalten und einen Zusatznutzen anbieten möchten - über die Schule oder Universität, die 
                            eine Integration personalisierter Lernfunktionen in ihr LMS wünscht - bis hin zum großen Nachhilfeanbieter oder Verlagshaus, 
                            die ihre eigenen Lerninhalte durch ein niederschwelliges digitales Angebot aufwerten möchten.
                        </p>
                        <p>
                            Wenn du ein <strong>Bildungsanbieter</strong> bist, profitierst du von den vielen bereits verfügbaren hochwertigen 
                            <strong>freien Inhalten</strong> bei memucho. Du kannst sie rechtssicher verwenden und durch eigene Inhalte leicht ergänzen. 
                            So erweiterst du schnell und günstig das Angebot an Lerninhalten auf deiner eigenen Seite.
                        </p>
                        <p>
                            Lernende schätzen die Wissenstests und profitieren direkt davon. Das Wiederholen von Wissen in kleinen Einheiten ist 
                            nachgewiesenermaßen eine effiziente Lernmethode, die in Quiz-Form noch dazu Spaß macht.
                        </p>
                        <p>
                            Als Betreiber einer Webseite helfen dir die memucho-Widgets als <strong>wichtiger SEO-Faktor</strong>. 
                            Quizze und kleine Interaktionsmöglichkeiten wie das Frage-Widget können die 
                            Verweildauer der Nutzer auf deiner Seite deutlich erhöhen. Dadurch schätzen Suchmaschinen deine
                            Seite als hilfreicher ein und verbessern dein Ranking.
                        </p>
                        
                        <p style="text-align: center; margin-top: 40px;">
                            <a href="<%= Links.HelpWidget() %>" class="btn btn-default">Zur Widget-Übersicht</a>
                        </p>

                    </div>                                        
                </div>

            </div>
            <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>
        </div>
    </div>

    <div class="row">
        <div class="col-xs-12" id="contact">
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
                            Telefon: +49-178 186 68 48<br/>
                        </p>
                        
                    </div>
                </div>

            </div>
            <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>
        </div>
    </div>

</asp:Content>