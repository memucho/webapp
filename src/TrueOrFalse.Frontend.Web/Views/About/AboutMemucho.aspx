<%@ Page Title="Über memucho" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="System.Web.Mvc.ViewPage<AboutMemuchoModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/About/AboutMemucho.css" rel="stylesheet" />
    <%: Scripts.Render("~/bundles/AboutMemucho") %>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="pageTitleMemucho">
        <img src="/Images/Logo/LogoWordmark_w300.png"/>
        <h1>Schneller lernen, länger wissen</h1>
    </div>
            
    <div id="teamImgQuote">
        <img id="teamImg" src="/Images/Team/founderTeam_20161027_e1.jpg" alt="Hier sollte ien Bild sein"/>

        <div id="teamQuoteCircle" class="">
            <div class="circle">
                <div class="circleInner">
                    <div class="circleWrapper">
                        <div class="circleContent">
                            <i class="fa fa-quote-left quoteIcon">&nbsp;</i>
                            <div id="teamQuote">
                                Wir möchten dich dabei unterstützen, <strong>selbstbestimmt zu lernen</strong> - 
                                interaktiv und mit personalisierten Lern- und Analyse-Werkzeugen.
<%--                                        Wir möchten, dass du optimiert lernen und dein Wissen organisieren kannst - und mehr Spaß hast!
                                        Dabei fördern wir freie Bildungsmaterialien.--%>
                            </div>
                            <div id="teamQuoteNames">Christof, Robert und Jule</div>
                            <div id="teamQuoteNamesSub">Gründerteam memucho</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div id="advantages">
        <h2>Deine Vorteile</h2>
                
        <div class="advantage">
            <div class="advantageIcon" style="color: #B13A48">
                <i class="fa fa-heart-o">&nbsp;</i> <i class="fa fa-heart">&nbsp;</i>
            </div>
            <h3>Wunschwissen sammeln</h3>
            <p>
                Stelle dir dein <i class="fa fa-heart" style="color: #B13A48"></i> Wunschwissen zusammen und entscheide, was du dir merken möchtest.
                Damit hast du deinen Wissensstand zu allen Themen immer im Blick.
            </p>
            <p>
                Bei memucho findest du spannende Lerninhalte, die du interaktiv lernen kannst &#8211; 
                egal, was du parat haben möchtest: Geschichtswissen für die Schule, Methodenwissen für das Studium, 
                Hintergrundfakten zu Flucht & Migration oder Fußballwissen für die nächste WM.
            </p>
        </div>

        <div class="advantage">
            <div class="advantageIcon">
                <i class="fa fa-line-chart"></i>
            </div>
            <h3>Interaktiv & optimiert lernen</h3>
            <p>
                Unsere Algorithmen sagen dir immer, was du am dringendsten lernen musst und erinnern dich ans Wiederholen, bevor du vergisst. 
                So sparst du Zeit und gewinnst an Sicherheit. 
            </p>
            <p>
                Interaktiven Fragen helfen dir, Wissen im Gedächtnis besser zu verankern.
                Lernpunkte motivieren dich und führen dich zu immer neuen Levels.
            </p>
            <p>
                Für deine Prüfungen erstellen wir dir sogar deinen ganz persönlichen Lernplan. So erfährst du, wie viel Zeit du zum Lernen
                einplanen solltest und ob du deine Lernziele erreichst. So kannst du dich entspannt auf die Prüfungen vorbereiten.
            </p>
        </div>

        <div class="advantage">
            <div class="advantageIcon">
                <i class="fa fa-bar-chart"></i>
            </div>
            <h3>Wissensstand im Blick</h3>
            <p>
                Du möchtest dir gern 50, 500, 5.000 oder mehr Fragen merken? Mit memucho behältst du den Überblick und hast deinen Wissensstand immer im Blick.
            </p>
            <p>
                Wir zeigen dir für dein Wunschwissen, was du dringend lernen musst und was du schon sicher kannst. 
                Und wenn du für Prüfungen lernen musst, behältst du mit der Terminfunktion den Überblick über deinen Lernstand und die benötigte Lernzeit.
            </p>
        </div>

        <div class="advantage">
            <div class="advantageIcon">
                <i class="fa fa-share-alt"></i>
            </div>
            <h3>Wissen teilen und gemeinsam lernen</h3>
            <p>
                Nutze die vorhandenen Lerninhalte und teile dein eigenes Wissen. 
                Du kannst selbst eigene Fragen und Lernsets erstellen. Denn Wissen wird mehr, wenn man es teilt!
            </p>
            <p>
                Je mehr andere Nutzer mit deinen Fragen lernen, desto mehr Reputationspunkte erhältst du und wirst als Experte sichtbar.
                Lade dazu deine Freunde ein, folge ihnen und lerne mit ihnen gemeinsam. Wer wird am meisten Lernpunkte erreichen?
            </p>
        </div>
                
        <div id="advantagesFooter">
            <%  if (!Model.IsLoggedIn) { %>
                <div class="" style="text-align: center; display: inline-block;">
                    <a href="<%= Url.Action(Links.RegisterAction, Links.RegisterController) %>" class="btn btn-lg btn-primary" role="button"><i class="fa fa-chevron-circle-right">&nbsp;</i> Jetzt kostenlos registrieren</a>
                </div>
            <%  } %>
        </div>

    </div>
            

    <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>
            
            
    <div id="keyNumbers">
        <div class="row">
            <div class="col-xs-4 keyNumberCol">
                <div class="keyNumber">
                    <span class="CountUp" data-number="<%= Model.TotalActivityPoints %>" > 0 </span>
                </div>
                <div class="keyNumberExplanation">
                    Lernpunkte wurden bisher bei memucho errungen
                </div>
            </div>

            <div class="col-xs-4 keyNumberCol">
                <div class="keyNumber">
                    <span class="CountUp" data-number="<%= Model.TotalQuestionCount %>"> 0 </span>
                </div>
                <div class="keyNumberExplanation">
                    interaktive Fragen gibt es zu beantworten
                </div>
            </div>

            <div class="col-xs-4 keyNumberCol">
                <div class="keyNumber">
                    <span class="CountUp" data-number="<%= Model.PercentageQuestionsAnsweredMostlyWrong %>"> 0 </span> %
                </div>
                <div class="keyNumberExplanation">
                    der Fragen werden öfter falsch als richtig beantwortet
                </div>
            </div>
        </div>
    </div>
            
            
    <div id="principles">
        <h2>Unsere Prinzipien</h2>
        <div class="advantage">
            <div class="advantageIcon">
                <i class="fa fa-bullhorn"></i>
            </div>
            <h3>Freie Bildungsinhalte</h3>
            <div class="floatingImg">
                <img src="/Images/LogosPartners/oer_logo_EN_2_w400.png"/><br/>
                <p>
                    Jonathas Mello, <a target="_blank" href="https://creativecommons.org/licenses/by/3.0/">CC BY 3.0 Unported</a>
                </p>
            </div>
            <p>
                Alle Lerninhalte bei uns sind frei und rechtssicher lizenziert. 
                Du kannst sie nutzen, weiterverwenden und ergänzen, wie es für dich am besten passst.
                Wir nutzen dafür die offene Lizenz "Creative Commons Namensnennung" 
                (kurz: <a href="https://creativecommons.org/licenses/by/4.0/deed.de" target="_blank">CC BY 4.0</a>).
            </p>
            <p>
                Offene Bildungsinhalte ("Open Educational Resources", oder kurz "OER") demokratisieren Wissen und ermöglichen es Lehrenden und Lernenden, 
                die Materialien an ihre spezifischen Bedürfnisse anzupassen. 
                <a href="http://www.unesco.de/fileadmin/medien/Dokumente/Bildung/Was_sind_OER__cc.pdf" target="_blank">
                    Hier erfährst du mehr zu OER. <i class="fa fa-external-link"></i>
                </a>
            </p>
        </div>
        <div class="Clearfix"></div>
        <div class="advantage">
            <div class="advantageIcon">
                <i class="fa fa-leaf"></i>
            </div>
            <h3>Sozialunternehmen und Gemeinwohlorientierung</h3>
            <p>
                Wir möchten jedem Menschen den Zugang zu freien und hochwertigen Bildungsmaterialien ermöglichen. 
                Und wir sind überzeugt, dass Unternehmen eine ethische, soziale und ökologische Verantwortung haben.
                Deshalb möchten wir unser Unternehmen auf 
                <a href="http://www.gemeinwohl-oekonomie.org/de" target="_blank;">gemeinwohlfördernden Werten <i class="fa fa-external-link"></i></a> 
                aufbauen und werden dazu in Zukunft eine Gemeinwohlbilanz veröffentlichen.
            </p>
        </div>

        <div class="advantage">
            <div class="advantageIcon">
                <i class="fa fa-search-plus"></i>
            </div>
            <h3>Datenschutz, Transparenz und Open-Source</h3>
            <p>
                Wir nutzen deine Daten, damit du besser lernen kannst und um memucho besser zu machen. 
                Aber wir werden deine Daten niemals verkaufen. 
                (<a class="helpLink" href="<%= Links.FAQItem("DataPrivacy") %>">Mehr zum <i class="fa fa-lock">&nbsp;</i>Datenschutzerklärung</a>)
            </p>
            <p>
                Damit du überprüfen kannst, was wir versprechen und weil wir Open Source eine tolle Idee finden, 
                sind auch die Quelltexte von memucho frei verfügbar. Du findest sie 
                auf <a href="https://github.com/TrueOrFalse/TrueOrFalse" target="_blank"><i class="fa fa-github">&nbsp;</i>Github <i class="fa fa-external-link"></i></a>. 
                In Zukunft möchten wir neben der Gemeinwohlbilanz auch wichtige Unternehmenszahlen regelmäßig veröffentlichen.
            </p>
        </div>
                
    </div>


    <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>
     
    <div id="awards">
        <h2>Auszeichnungen</h2>
        <div class="row">
            <div class="col-xs-6 col-md-3 xxs-stack">
                <div class="logo-box">
                    <div class="img-logo">
                        <a href="https://www.land-der-ideen.de/ausgezeichnete-orte/preistraeger/memucho-online-plattform-zum-faktenlernen" target="_blank">
                            <img src="/Images/LogosPartners/landderideen_ausgezeichnet-2017_w190c.jpg" alt="memucho ist ein ausgezeichneter Ort im Land der Ideen 2017"/>
                        </a>
                    </div>
                    <%--                        <p>
                        memucho ist ein Ausgezeichneter Ort im Land der Ideen 2017.
                    </p>
                    <p class="logo-box-link">
                        <a href="https://www.land-der-ideen.de/ausgezeichnete-orte/preistraeger/memucho-online-plattform-zum-faktenlernen" target="_blank">
                            <span style="white-space: nowrap">Zum Wettbewerb <i class="fa fa-external-link"></i></span>
                        </a>
                    </p>--%>
                </div>        
            </div>
            <div class="col-xs-6 col-md-3 xxs-stack">
                <div class="logo-box">
                    <div class="img-logo">
                        <a href="http://www.innovationspreis.de/news/aktuelles/zehn-nominierungen-f%C3%BCr-den-innovationspreis-berlin-brandenburg-2016.html" target="_blank">
                            <img src="/Images/LogosPartners/innovationspreis-nominiertButton2016.png" alt="Nominiert 2016 für den Innovationspreis Berlin Brandenburg" width="170" height="110"/>
                        </a>
                    </div>
                    <%--                        <p>
                        memucho wurde für den Innovationspreis Berlin Brandenburg nominiert.
                    </p>
                    <p class="logo-box-link">
                        <a href="http://www.innovationspreis.de/news/aktuelles/zehn-nominierungen-f%C3%BCr-den-innovationspreis-berlin-brandenburg-2016.html" target="_blank">
                            <span style="white-space: nowrap">Zur Jury-Entscheidung <i class="fa fa-external-link"></i></span>
                        </a>
                    </p>--%>
                </div>    
            </div>
            <div class="clearfix visible-xs visible-sm"></div>
            <div class="col-xs-6 col-md-3 xxs-stack">
                <div class="logo-box">
                    <div class="img-logo">
                        <a href="https://www.deutscher-engagementpreis.de/wettbewerb/publikumspreis/voting-detail/?tx_epawards_voting%5BawardWinner%5D=963&tx_epawards_voting%5Baction%5D=show&tx_epawards_voting%5Bcontroller%5D=Vote&cHash=a36d2dd613b235a05d95e8ebc949a3e1" target="_blank">
                            <img style="margin-top: 45px;" src="/Images/LogosPartners/Logo_DEP_rgb_300x98.jpg" alt="" width="230" height="75"/>
                        </a>
                    </div>
                    <%--                        <p>
                        memucho ist Nominierter für den Deutschen Engagementpreis 2017.
                    </p>
                    <p class="logo-box-link">
                        <a href="https://www.deutscher-engagementpreis.de/wettbewerb/publikumspreis/voting-detail/?tx_epawards_voting%5BawardWinner%5D=963&tx_epawards_voting%5Baction%5D=show&tx_epawards_voting%5Bcontroller%5D=Vote&cHash=a36d2dd613b235a05d95e8ebc949a3e1" target="_blank">
                            <span style="white-space: nowrap">Zum Portrait <i class="fa fa-external-link"></i></span>
                        </a>
                    </p>--%>
                </div>    
            </div>
            <div class="col-xs-6 col-md-3 xxs-stack">
                <div class="logo-box">
                    <div class="img-logo">
                        <a href="https://www.netzsieger.de/p/memucho" target="_blank">
                            <img src="/Images/LogosPartners/Logo_netzsieger_170905-memucho-small.png" alt="" width="165" height="126"/>
                        </a>
                    </div>
                    <%--                        <p>
                        memucho erhält beim Einzeltest 4,7/5 Punkten.
                    </p>
                    <p class="logo-box-link">
                        <a href="https://www.netzsieger.de/p/memucho" target="_blank">
                            <span style="white-space: nowrap">Zum Testbericht <i class="fa fa-external-link"></i></span>
                        </a>
                    </p>--%>
                </div>    
            </div>
        </div>
    </div>
            

    <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>
            


    <div id="partner">
        <h2>Partner</h2>
        <div class="row">
            <div class="col-sm-4">
                <div class="logo-box">
                    <div class="img-logo">
                            <a href="http://lernox.de/" target="_blank">
                                <img style="margin-top: -35px;" src="/Images/LogosPartners/Logo_lernox.png" alt="Logo lernox.de"/>
                            </a>
                        </div>
                        <p>
                            Ankommen durch Sprache. DaF-/DaZ-Material finden und sammeln.
                        </p>
                        <p class="logo-box-link">
                            <a href="http://lernox.de/" target="_blank">
                                <span style="white-space: nowrap">lernox.de <i class="fa fa-external-link"></i></span>
                            </a>
                        </p>
                    </div>        
                </div>
                <div class="col-sm-4">
                    <div class="logo-box">
                        <div class="img-logo">
                            <a href="/Kategorien/Learning-Level-Up/722">
                                <img style="margin-top: 24px;" src="/Images/LogosPartners/Logo_LearningLevelUp.png" alt="Learning Level Up und memucho kooperieren!" />
                            </a>
                        </div>
                        <p>
                            Learning Level Up bietet Animationen, Grafiken und Videos zum Lernen. Wir freuen uns über die Kooperation!
                        </p>
                        <p class="logo-box-link">
                            <a href="/Kategorien/Learning-Level-Up/722">Zur Themenseite</a> 
                        </p>
                    </div>        
                </div>
                <div class="col-sm-4">
                    <div class="logo-box">
                        <div class="img-logo">
                            <a href="https://www.tutory.de/" target="_blank">
                                <img src="/Images/LogosPartners/Logo_tutory_250px.png" alt="tutory.de"/>
                            </a>
                        </div>
                        <p>
                            Mit tutory.de lassen sich im Handumdrehen tolle Arbeitsblätter direkt online erstellen.
                        </p>
                        <p class="logo-box-link">
                            <a href="https://www.tutory.de/" target="_blank">
                                <span style="white-space: nowrap">tutory.de <i class="fa fa-external-link"></i></span>
                            </a>
                        </p>
                    </div>        
                </div>
                <%--<div class="col-sm-4">
                    <div class="logo-box">
                        <div class="img-logo">
                            <a href="http://www.unesco.org/new/en/communication-and-information/access-to-knowledge/open-educational-resources/" target="_blank">
                                <img style="margin-top: -35px;" src="/Images/LogosPartners/oer_logo_EN_2_w400.png" alt="Logo Open Educational Resources"/>
                            </a>
                        </div>
                        <p>
                            Freie Bildungsmaterialien demokratisieren Bildung! Wir machen mit.
                        </p>
                        <p class="logo-box-link">
                            <a href="http://www.unesco.org/new/en/communication-and-information/access-to-knowledge/open-educational-resources/" target="_blank">
                                <span style="white-space: nowrap">Zur UNESCO-Seite <i class="fa fa-external-link"></i></span>
                            </a>
                        </p>
                    </div>        
                </div>--%>

        </div>
    </div>


    <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>

</asp:Content>
