<%@ Page Title="memucho: Schneller lernen, länger wissen" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
	Inherits="ViewPage<WelcomeModel>"%>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <link rel="canonical" href="<%= Settings.CanonicalHost %>">
    <meta name="description" content="memucho hilft dir beim Lernen. Du sparst Zeit, bist effizienter und es macht mehr Spaß! Entdecke neues Wissen oder füge dein eigenes hinzu.">
</asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <link href="/Views/Welcome/Welcome.css" rel="stylesheet" />
    
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

    <%= Scripts.Render("~/bundles/guidedTourScript") %>
    <%= Styles.Render("~/bundles/guidedTourStyle") %>
    <%= Scripts.Render("~/bundles/Welcome") %>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    
<div class="row">
       
    <div class="col-md-9">
        <div id="memuchoInfo">
            <h1 id="memuchoInfoHeader">
                Dein Wissens-Assistent
            </h1>            
            <div id="memuchoInfoMain">
                <p>
                    <span class="fa-stack fa-2x numberCircleWrapper">
                        <i class="fa fa-circle fa-stack-2x numberCircle numberCircleOne"></i>
                        <strong class="fa-stack-1x numberCircleText">1</strong>
                    </span>
                    <span class="memuchoInfoBenefit">Sammeln</span> <span class="memuchoInfoBenefitSub"><i class="fa fa-heart-o">&nbsp;</i>Entscheide, was du wissen möchtest.</span>
                </p>
                <p>
                    <span class="fa-stack fa-2x numberCircleWrapper">
                        <i class="fa fa-circle fa-stack-2x numberCircle numberCircleTwo"></i>
                        <strong class="fa-stack-1x numberCircleText">2</strong>
                    </span>
                    <span class="memuchoInfoBenefit">Lernen</span> <span class="memuchoInfoBenefitSub">Algorithmen helfen dir, zum idealen Zeitpunkt zu lernen.</span>
                </p>
                <p>
                    <span class="fa-stack fa-2x numberCircleWrapper">
                        <i class="fa fa-circle fa-stack-2x numberCircle numberCircleThree"></i>
                        <strong class="fa-stack-1x numberCircleText">3</strong>
                    </span>
                    <span class="memuchoInfoBenefit">Nicht vergessen</span> <span class="memuchoInfoBenefitSub">Wir erinnern dich, bevor du vergisst.</span>
                </p>
            </div>
            <div id="memuchoInfoFooter">
                <a href="#" class="btn btn-link btn-sm ButtonOnHover" id="btnStartWelcomeTour" data-click-log="WelcomeTour,Click,Start" style="line-height: normal;">
                    <i class="fa fa-map-signs">&nbsp;</i>Lerne memucho<br/>kennen in 6 Schritten
                </a>
                <a id="btnMoreAboutMemucho" href="<%= Links.AboutMemucho() %>" class="btn btn-primary">Erfahre mehr...</a><br />
            </div>
        </div>

<%--        <div class="well" style="padding: 13px; padding-bottom: 10px;">
            <h1 style="margin-top: 0; margin-bottom: 15px; font-size: 24px; text-align: center;"><span style="white-space: nowrap;">Schneller lernen,</span> <span style="white-space: nowrap;">länger wissen</span></h1>
            <div class="row">
                <div class="col-xs-4 xxs-stack" style="text-align: center; font-size: 100%; padding: 5px;">
                    <p>
                        <i class="fa fa-2x fa-lightbulb-o" style="color: #2C5FB2;"></i>
                    </p>
                    <p>
                        Wir optimieren dein Lernen mit einem persönlichen Lernplan!<br/>
                    </p>
                </div>
                <div class="col-xs-4 xxs-stack" style="text-align: center; font-size: 100%; padding: 5px;">
                    <p>
                        <i class="fa fa-2x fa fa-clock-o" style="color: #2C5FB2;"></i>
                    </p>
                    <p>
                        Du sparst Zeit und siehst, wie viel Zeit du noch zum Lernen brauchst.
                    </p>
                </div>
                <div class="col-xs-4 xxs-stack" style="text-align: center; font-size: 100%; padding: 5px;">
                    <p>
                        <i class="fa fa-2x fa-line-chart" style="color: #2C5FB2;"></i>
                    </p>
                    <p>
                        Optimierte Algorithmen erinnern dich ans Lernen, bevor du vergisst.
                    </p>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12" style="text-align: left; margin-top: 15px;">
                    <a href="#" class="btn btn-link btn-sm ButtonOnHover" id="btnStartWelcomeTour" data-click-log="WelcomeTour,Click,Start" style="line-height: normal;">
                        <i class="fa fa-map-signs">&nbsp;</i>Lerne memucho<br/>kennen in 6 Schritten
                    </a>
                    <a id="btnMoreAboutMemucho" href="<%= Links.AboutMemucho() %>" class="btn btn-primary pull-right">ERFAHRE MEHR...</a><br />
                </div>
            </div>
        </div>--%>

        <h3 style="margin-top: 40px;">Finde deine Lerninhalte</h3>
        
        <div class="EduCategoryRow row">
            <div class="xxs-stack col-xs-6 col-sm-3">
                <a href="/Kategorien/Unterrichtsfaecher/682" class="EduCategory">
                    <span class="EduCategoryIcon">
                        <span class="fa-stack fa-lg">
                          <i class="fa fa-circle fa-stack-2x"></i>
                          <i class="fa fa-child fa-stack-1x fa-inverse IconForeground"></i>
                        </span>    
                    </span>
                    <span class="EduCategoryText">
                        Schule
                    </span>
                </a>
            </div>
            
            <div class="xxs-stack col-xs-6 col-sm-3">
                <a href="/Kategorien/Studienfaecher/687" class="EduCategory">
                    <span class="EduCategoryIcon">
                        <span class="fa-stack fa-lg">
                          <i class="fa fa-circle fa-stack-2x"></i>
                          <i class="fa fa-graduation-cap fa-stack-1x fa-inverse IconForeground"></i>
                        </span>    
                    </span>
                    <span class="EduCategoryText">
                        Studium
                    </span>
                </a>  
            </div>
            <div class="xxs-stack col-xs-6 col-sm-3">
                <a href="/Kategorien/Zertifikate/689" class="EduCategory show-tooltip" title="Sportbootführerscheine, Einbürgerungstest, Pferdebasispass etc.">
                    <span class="EduCategoryIcon">
                        <span class="fa-stack fa-lg">
                          <i class="fa fa-circle fa-stack-2x"></i>
                          <i class="fa fa-file-text-o fa-stack-1x fa-inverse IconForeground"></i>
                        </span>    
                    </span>
                    <span class="EduCategoryText">
                        Zertifikate
                    </span>
                </a>
            </div>
            <div class="xxs-stack col-xs-6 col-sm-3">
                <a href="/Kategorien/Allgemeinwissen/709" class="EduCategory">
                    <span class="EduCategoryIcon">
                        <span class="fa-stack fa-lg">
                          <i class="fa fa-circle fa-stack-2x"></i>
                          <i class="fa fa-lightbulb-o fa-stack-1x fa-inverse IconForeground"></i>
                        </span>    
                    </span>
                    <span class="EduCategoryText">
                        Allgemeinwissen
                    </span>
                </a>    
            </div>
        </div>
        
        <h3 class="welcomeContentSectionHeader">Schwerpunkt Europäische Union</h3>
        <p class="welcomeContentSectionTarget">Allgemeinwissen, Abitur Politik, Politikwissenschaft</p>
        <p class="welcomeContentSectionTeaser">Teste dein Wissen zu den Ländern der Europäischen Union und zu den wichtigen Aspekten der Europäischen Union.</p>
        <div class="row CardsPortrait" style="padding-top: 0;">
            <% //Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(45)); //EU-Integration %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(20, "Kennst du die Hauptstädte aller 28 Länder der Europäischen Union? Finde es heraus!")); %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(44)); //EU-Organe %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(55)); //EU GASP %>
        </div>
        <% Html.RenderPartial("WelcomeBoxSetImgQ", WelcomeBoxSetImgQModel.GetWelcomeBoxSetImgQModel(19, new[] { 468, 464, 460 })); // EU-Länder erkennen %>
        <% Html.RenderPartial("WelcomeBoxSetTxtQ", WelcomeBoxSetTxtQModel.GetWelcomeBoxSetTxtQModel(45, new[] { 805, 237, 1065})); // EU-Integration %>


        <h3 class="welcomeContentSectionHeader">Schwerpunkt Geschichte: Wichtige Epochen</h3>
        <p class="welcomeContentSectionTarget">Allgemeinwissen, Geschichte Abitur, Geschichte Sekundarstufe I</p>
        <p class="welcomeContentSectionTeaser">Teste dein Wissen zu wichtigen historischen Epochen - und lerne sie mit memucho, um sie immer abrufbereit zu haben!</p>
        <div class="row CardsPortrait" style="padding-top: 0;">
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(49)); //Abitur Franz. Rev %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(42)); //Abitur Weimarer Republik %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(52)); //Sek I Weimarer Republik %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(65)); //Abitur Absolutismus Preußen %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(62)); //Abitur Absolutismus Frankreich %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(58)); //Berliner Mauer %>
        </div>


        <h3 class="welcomeContentSectionHeader">Schwerpunkt Politik & Wirtschaft</h3>
        <p class="welcomeContentSectionTarget">Allgemeinwissen, Politik, Wirtschaft, Globalisierung, Migration, Geographie</p>
        <p class="welcomeContentSectionTeaser">Teste dein Wissen zum politischen Zeitgeschehen und lerne mit memucho die wichtigsten Grundlagen.</p>
        <div class="row CardsPortrait" style="padding-top: 0;">
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(64)); //Sozialstaat Deutschland %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(57)); //UN %>
            <% //Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(59)); //Wirtsch. Globalisierung %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(25)); //Hauptstädte Flächenbundesländer %>
            <% //Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(7)); //Bundeskanzler erkennen %>
        </div>
        <% Html.RenderPartial("WelcomeBoxSetTxtQ", WelcomeBoxSetTxtQModel.GetWelcomeBoxSetTxtQModel(59, new int[] { 1742, 1821, 1807 })); //Int'l Wirtschaftsbeziehungen %>
        <% Html.RenderPartial("WelcomeBoxSetTxtQ", WelcomeBoxSetTxtQModel.GetWelcomeBoxSetTxtQModel(66, new int[] { 2044, 2046, 2053 })); //Wirtschaftspolitik %>
        <% Html.RenderPartial("WelcomeBoxCategoryTxtQ", WelcomeBoxCategoryTxtQModel.GetWelcomeBoxCategoryTxtQModel(14, new int[] { 404, 405, 406 }, "Du verstehst den Wirtschafts-Teil der Zeitung nicht? Teste und erweitere dein Grundlagen-Wissen zu Wirtschaftsthemen!")); %>
        <% Html.RenderPartial("WelcomeBoxSetTxtQ", WelcomeBoxSetTxtQModel.GetWelcomeBoxSetTxtQModel(27, new int[] { 749, 635, 630 })); //Einbürgerungstest %>
        <% Html.RenderPartial("WelcomeBoxCategoryTxtQ", WelcomeBoxCategoryTxtQModel.GetWelcomeBoxCategoryTxtQModel(205, new int[] { 381, 379, 384 }, "Du möchtest dir eine fundierte Meinung zur Flüchtlingspolitik bilden? Erweitere dein Hintergrundwissen mit Fakten!")); %>


        <h3 class="welcomeContentSectionHeader">Der Basispass Pferdekunde: Die wichtigsten Grundlagen</h3>
        <p class="welcomeContentSectionTarget">Pferdesportler, Reiter, Pferdeinteressierte</p>
        <p class="welcomeContentSectionTeaser">Lerne die Grundlagen der Pferdehaltung und teste dein Wissen zu deinen Fertigkeiten im Umgang mit dem Pferd.</p>
        <div class="row CardsPortrait" style="padding-top: 0;">
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(46)); //Pferdehaltung und Fütterung %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(61)); //Körperteile Pferde %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(60)); //Abzeichen %>
        </div>
        <% Html.RenderPartial("WelcomeBoxSetImgQ", WelcomeBoxSetImgQModel.GetWelcomeBoxSetImgQModel(53, new[] { 1678, 1711, 1651 })); // Pferdefarben Basispass %>


        <h3 class="welcomeContentSectionHeader">Sportbootführerschein</h3>
        <p class="welcomeContentSectionTarget">Segler, Segelinteressierte, Sportbootführer</p>
        <p class="welcomeContentSectionTeaser">Bereite dich auf die Prüfung zum Sportbootführerschein Binnen oder See vor.</p>
        <div class="row CardsPortrait" style="padding-top: 0;">
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(47)); //Binnenschein-Basisfragen %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(48)); //Binnenschein-Binnen %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(51)); //Seeschein-See %>
        </div>
        <% Html.RenderPartial("WelcomeBoxSetTxtQ", WelcomeBoxSetTxtQModel.GetWelcomeBoxSetTxtQModel(50, new[] { 1391, 1370, 1359 })); //Binnenschein-Segeln%>


        <h3 class="welcomeContentSectionHeader">Literatur: Vertreter und Zitate aus wichtigen Epochen</h3>
        <p class="welcomeContentSectionTarget">Allgemeinwissen, Deutsch Abitur, Literaturwissenschaft</p>
        <p class="welcomeContentSectionTeaser">Kennst du die wichtigen Vertreter und Werke der verschiedenen Literaturepochen? Teste hier dein Wissen und lerne die wichtigsten Fakten.</p>
        <div class="row CardsPortrait" style="padding-top: 0;">
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(30)); //Literatur Klassik %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(35)); //Literatur Barock %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(29)); //Literatur Zitate %>
        </div>

        <h3 class="welcomeContentSectionHeader">Allgemeinwissen und Unterhaltung</h3>
        <p class="welcomeContentSectionTarget">Allgemeinwissen, Fußball, Sehenswürdigkeiten</p>
        <p class="welcomeContentSectionTeaser">Teste und erweitere dein Allgemeinwissen. Entscheide, was du lernen und behalten möchtest.</p>
        <div class="row CardsPortrait" style="padding-top: 0;">
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(14)); //Laubbäume %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(22, "Farfalle, Penne oder Rigatoni? Weißt du, wie diese Nudelsorten heißen?")); %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(18)); //Sieger Fußbal-WM Herren %>
<%--            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(17)); //Sehenswürdigkeiten %>--%>
        </div>
        <% Html.RenderPartial("WelcomeBoxSetImgQ", WelcomeBoxSetImgQModel.GetWelcomeBoxSetImgQModel(17, new[] { 373, 360, 367 })); //Sehenswürdigkeiten %>  
        <% Html.RenderPartial("WelcomeBoxCategoryTxtQ", WelcomeBoxCategoryTxtQModel.GetWelcomeBoxCategoryTxtQModel(336, new int[] { 973, 965, 962 }, "Knifflige Fragen! Wer erfand den Champagner? Der Mönch Dom Pérignon 1670?")); %>
        <div class="row CardsPortrait" style="padding-top: 0;">
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(13)); //Scherzfragen %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(12)); //James Bond%>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(37)); //ARD-Intendanten %>
        </div>
        <%--<% Html.RenderPartial("WelcomeBoxSetImgQ", WelcomeBoxSetImgQModel.GetWelcomeBoxSetImgQModel(14, new[] { 348, 341, 344 })); // Laubbäume %>--%>
        <%--<% Html.RenderPartial("WelcomeBoxSetTxtQ", WelcomeBoxSetTxtQModel.GetWelcomeBoxSetTxtQModel(12, new[] { 303, 288, 289 }, "Der berühmteste Agent im Dienste Ihrer Majestät: Kennst du die wichtigsten Fakten zu den James Bond-Filmen?")); %>--%>


        
        <h3 class="welcomeContentSectionHeader">Schwangerschaft und Elternzeit</h3>
        <p class="welcomeContentSectionTarget">(Zukünftige) Mütter & Väter</p>
        <p class="welcomeContentSectionTeaser">Weißt du Bescheid beim Thema Schwangerschaft? Kennst du die rechtlichen Grundlagen bei Mutterschutz und Elternzeit? Teste dein Wissen!</p>
        <% Html.RenderPartial("WelcomeBoxSetTxtQ", WelcomeBoxSetTxtQModel.GetWelcomeBoxSetTxtQModel(39, new int[] { 991, 993, 1007})); //Schwangerschaft %>
        <% Html.RenderPartial("WelcomeBoxSetTxtQ", WelcomeBoxSetTxtQModel.GetWelcomeBoxSetTxtQModel(40, new int[] { 1011, 1021, 1015 })); //Mutterschutz Rechtliche Grundlagen %>

       
        


        <div class="well">
            <h3>
                <a name="teaserWhatIsMemucho"></a>
                Was ist memucho?
            </h3>
            <p>
                memucho ist eine vernetzte Lern- und Wissensplattform. Damit kannst du:
            </p>
            
            <div class="row">
                <div class="col-xs-6 xxs-stack" style="text-align: center; font-size: 100%; padding: 5px 5px 10px;">
                  <i class="fa fa-clock-o fa-2x show-tooltip" style="color: #2C5FB2"></i><br/>
                    <b>Schneller lernen</b>
                    <p>
                        memucho analysiert dein Lernverhalten und wiederholt schwierige Fragen zum optimalen Zeitpunkt. 
                        So brauchst du weniger Zeit zum Lernen.
                    </p>
                </div>
                <div class="col-xs-6 xxs-stack" style="text-align: center; font-size: 100%; padding: 5px 5px 10px;">
                  <i class="fa fa-book fa-2x show-tooltip" style="color: #2C5FB2"></i><br/>
                    <b>Wissen erweitern</b>
                    <p>
                        Du möchtest gerne mehr über Politik, die Griechenland-Krise oder über James Bond-Filme wissen? 
                        Finde die passenden Lernsets und stelle dir dein Wunschwissen zusammen!
                    </p>
                </div>
                <div class="clearfix"></div>
                <div class="col-xs-6 xxs-stack" style="text-align: center; font-size: 100%; padding: 5px 5px 10px;">
                  <i class="fa fa-calendar-o fa-2x show-tooltip" style="color: #2C5FB2"></i><br/>
                    <b>Zu einem bestimmten Termin lernen</b>
                    <p>
                        Eine Klassenarbeit oder eine Prüfung steht an? Mit Terminen in memucho weißt du immer, 
                        was du schon sicher kannst und wo du weiter lernen musst.
                    </p>
                </div>
                <div class="col-xs-6 xxs-stack" style="text-align: center; font-size: 100%; padding: 5px 5px 10px;">
                  <i class="fa fa-pie-chart fa-2x show-tooltip" style="color: #2C5FB2"></i><br/>
                    <b>Überblick behalten</b>
                    <p>
                        Du möchtest dir gerne 50, 500, 5000 (oder mehr) Fakten merken? Kein Problem, mit memucho behältst du den Überblick.
                    </p>
                </div>
                <div class="clearfix"></div>
                <div class="col-xs-6 xxs-stack" style="text-align: center; font-size: 100%; padding: 5px 5px 10px;">
                  <i class="fa fa-share-alt fa-2x show-tooltip" style="color: #2C5FB2"></i><br/>
                    <b>Wissen teilen</b>
                    <p>
                        memucho ist ein offenes Netzwerk, wo du dein Wissen teilen und das Wissen anderer nutzen kannst. 
                        Denn Wissen wird mehr, wenn man es teilt!
                    </p>
                </div>
                <div class="col-xs-6 xxs-stack" style="text-align: center; font-size: 100%; padding: 5px 3px 20px;">
                  <i class="fa fa-users fa-2x show-tooltip" style="color: #2C5FB2"></i><br/>
                    <b>Gemeinsam lernen</b>
                    <p>
                        Lerne gemeinsam mit Freunden und verabrede dich zum Quizduell, um dich auf die Klassenarbeit vorzubereiten.
                    </p>
                </div>
                <div class="clearfix visible-xs"></div>
                <% if (!Model.IsLoggedIn) { %>
                    <div class="col-xs-12" style="margin-top: 10px; text-align: center">
                        <a  id="btnRegisterMoreFeatures" href="<%= Url.Action("Register", "Welcome") %>" class="btn btn-primary" role="button"><i class="fa fa-chevron-circle-right">&nbsp;</i> Jetzt Registrieren</a> <br/>
                        <div class="" style="margin-top: 3px; font-style: italic">*memucho ist kostenlos.</div>
                    </div>
                <% } %>
                <div class="col-xs-12" style="margin-top: 10px; text-align: right;">
                    <a id="btnMoreFeatures" href="<%= Links.AboutMemucho() %>" class="btn btn-link ButtonOnHover">ERFAHRE MEHR...</a><br />
                </div>
            </div>
        </div>

        <div class="well">
            <h3>
                <a name="teaserPrinciples"></a>
                Unsere Prinzipien
            </h3>
            <ul class="fa-ul">
                <li><i class="fa fa-li fa-book"></i>
                    <b>Freie Bildungsinhalte ("Open Educational Resources")</b>
                    <p>
                        Wir sind Teil der Bewegung zur Förderung frei zugänglicher Bildungsmaterialien.
                        In memucho unterliegen öffentliche Inhalte einer Creative Commons-Lizenz, 
                        genau wie fast alle Einträge auf Wikipedia. Öffentliche memucho-Inhalte 
                        können also von jedem kostenfrei und ohne Einschränkungen verwendet werden 
                        (<a rel="license" href="http://creativecommons.org/licenses/by/4.0/deed.de">Hier 
                        erfährst du genaueres zur Lizenz CC BY 4.0.</a>). Private Inhalte sind aber privat.
                    </p>
                </li>
                <li><i class="fa fa-li fa-tree"></i>
                    <b>Gemeinwohlorientierung</b><br/>
                    <p>
                        Wir möchten unser Unternehmen auf gemeinwohlfördernden Werten aufbauen. 
                        Als Teil der <a href="http://www.gemeinwohl-oekonomie.org/de">Gemeinwohlökonomie</a> 
                        sind wir davon überzeugt, dass Unternehmen der Gemeinschaft dienen müssen und deshalb 
                        eine ethische, soziale und ökologische Verantwortung haben. Daher werden wir in Zukunft 
                        eine Gemeinwohlbilanz veröffentlichen.
                    </p>
                </li>
                <li><i class="fa fa-li fa-exchange"></i>
                    <b>Mitwirkung und Vernetzung</b>
                    <p>
                        Wir glauben, dass Wissen vernetzt sein muss. Der Vernetzungsgedanke spielt bei uns eine 
                        große Rolle. In der aktuellen Beta-Phase ist uns euer Feedback ganz besonders wichtig, 
                        aber auch später werden Mitglieder mitentscheiden, welche Funktionen wir als nächstes umsetzen 
                        und aktive Mitglieder können Inhalte mitmoderieren.
                    </p>
                </li>
                <li><i class="fa fa-li fa-lock"></i>
                    <b>Datenschutz ist uns sehr sehr wichtig</b>
                    <p>
                        Wir nutzen deine Daten, damit du besser lernen kannst und um memucho besser zu machen. 
                        Aber wir werden deine Daten niemals verkaufen. (<a class="helpLink" href="<%= Links.FAQItem("DataPrivacy") %>">Erfahre mehr</a> über unseren Datenschutz.)
                    </p>
                </li>
                <li><i class="fa fa-li fa-github"></i>
                    <b>Open-Source und Transparenz</b>
                    <p>
                        Die Software, mit der memucho läuft, steht unter einer Open-Source-Lizenz. Die Quelltexte 
                        sind frei verfügbar und können von allen frei verwendet werden. Du findest sie 
                        auf <a href="https://github.com/TrueOrFalse/TrueOrFalse" target="_blank"><i class="fa fa-github">&nbsp;</i>Github</a>. 
                        In Zukunft möchten wir neben der Gemeinwohlbilanz auch unsere Unternehmenszahlen veröffentlichen.
                    </p> 
                </li>        
            </ul>
            <div class="row">
                <% if (!Model.IsLoggedIn) { %>
                    <div class="col-xs-12" style="margin-top: 10px; text-align: center">
                        <a id="btnRegisterMorePrinciples" href="<%= Url.Action("Register", "Welcome") %>" class="btn btn-success" role="button"><i class="fa fa-chevron-circle-right">&nbsp;</i> Jetzt Registrieren</a> <br/>
                        <div class="" style="margin-top: 3px; font-style: italic">*memucho ist kostenlos.</div>
                    </div>
                <% } %>
                <div class="col-xs-12" style="margin-top: 10px; text-align: right;">
                    <a id="btnMorePrinciples" href="<%= Links.AboutMemucho() %>" class="btn btn-link ButtonOnHover">ERFAHRE MEHR...</a><br />
                </div>
            </div>
        </div>
        <div class="well Team">
            <h3>
                <a name="teaserWhoWeAre"></a>
                Team
            </h3>
            <div class="row">
                
                <div class="col-xs-4 TeamPic">
                    <img src="/Images/Team/team_robert201509_155.jpg"/>
                        <br/> <b>Robert</b> (Gründer) <br/>
                </div>
                
                <div class="col-xs-4 TeamPic">
                    <img src="/Images/Team/team_jule201509-2_155.jpg"/>  
                    <br/> <b>Jule</b> (Gründerin) <br/> 
                </div>

                <div class="col-xs-4 TeamPic">
                    <img src="/Images/Team/team_christof_20170404_P3312344_155.jpg"/>  
                    <br/> <b>Christof</b> (Gründer) <br/> 
                </div>
                <div class="col-xs-4 TeamPic">
                    <img src="/Images/Team/team_lisa_sq_155.jpg"/>  
                    <br/> <b>Lisa</b> (Kommunikation) <br/> 
                </div>
                <div class="col-xs-4 TeamPic">
                    <img src="/Images/Team/team_julian20170404_P3312327_155.jpg"/>  
                    <br/> <b>Julian</b> (Entwicklung) <br/> 
                </div>
                <div class="col-xs-12" style="margin-top: 10px;">
                    <p>
                        Wir möchten den Zugang zu freien Bildungsinhalten verbessern und dass Faktenlernen einfacher wird und mehr Spaß macht. 
                        Und wir möchten dabei ein stabiles <a href="#teaserPrinciples">gemeinwohlorientiertes Unternehmen</a> aufbauen. 
                        Wir konzipieren, gestalten und programmieren memucho gemeinsam.
                    </p>
                    <p>
                        Wenn du Fragen oder Anregungen hast, schreibe uns eine E-Mail an <span class="mailme">team at memucho dot de</span> oder rufe Christof an: +49-1577-6825707.
                    </p>
                </div>
            </div>
        </div>  
    </div>
            
    <div class="col-md-3">
        <%
            if (!Model.IsLoggedIn){
        %>
            <div class="well" id="boxLoginOrRegister" style="padding: 20px; ">
                <a id="btnRegisterSidebar" href="<%= Url.Action("Register", "Register") %>" class="btn btn-primary" style="width: 100%;" role="button"><i class="fa fa-chevron-circle-right">&nbsp;</i> Jetzt Registrieren</a>
                <div style="margin-top: 3px; font-style: italic">*memucho ist kostenlos.</div>
            </div>
        <% } %>
        <% if (Model.IsLoggedIn) { %>
            <div class="well" style="padding: 20px; ">
                <div style="text-align: center; margin-bottom: 15px;">
                    <a href="<%= Links.StartWishLearningSession() %>" class="btn btn-primary show-tooltip <%= Model.UserHasWishknowledge ? "" : "disabled" %>" title="Startet eine persönliche Lernsitzung. Du wiederholst die Fragen aus deinem Wunschwissen, die am dringendsten zu lernen sind.">
                        <i class="fa fa-heart">&nbsp;&nbsp;</i>LERNEN
                    </a>
                </div>
                <% if (!Model.UserHasWishknowledge) { %>
                    <p style="font-size: 11px; margin-top: 5px;">
                        Du hast noch kein Wunschwissen. Füge Lernsets oder Fragen zu 
                        deinem <span style="white-space: nowrap"><i class="fa fa-heart show-tooltip" style="color:#b13a48;">&nbsp;</i>Wunschwissen</span> hinzu, 
                        um eine personalisierte Lernsitzung zu starten. 
                    </p>
                <% } else { %>
                    <p style="font-size: 11px; margin-top: 5px;">
                        *Starte eine personalisierte Lernsitzung mit 10 Fragen aus deinem Wunschwissen (enthält <a href="<%= Links.QuestionsWish() %>"><%= Model.WishCount %> Fragen</a>). 
                    </p>
                <% } %>
            </div>
        <% } %>

        <div class="well" id="" style="padding: 10px; ">
            
            <div style="text-align: center;">
                <a href="/Kategorien/Learning-Level-Up/722">
                    <img src="/Images/LogosPartners/Logo_LearningLevelUp.png" alt="Learning Level Up und memucho kooperieren!" style="margin-bottom: 10px;"/>
                </a>
            </div>
            <p style="text-align: center; margin-bottom: 0;">
                Wir freuen uns über die Kooperation mit Learning Level Up!
                <a href="/Kategorien/Learning-Level-Up/722" >
                    <span style="white-space: nowrap">Zur Themenseite.</span>
                </a> 
            </p>
        </div>        

        <div class="well" id="awardLandDerIdeen" style="padding: 10px; ">
            <div style="text-align: center;">
                <a href="https://www.land-der-ideen.de/ausgezeichnete-orte/preistraeger/memucho-online-plattform-zum-faktenlernen" target="_blank">
                    <img src="/Images/LogosPartners/landderideen_ausgezeichnet-2017_w190c.jpg" alt="memucho ist ein ausgezeichneter Ort im Land der Ideen 2017" style="margin-bottom: 10px; margin-left: -10px; margin-right: -10px;"/>
                </a>
            </div>
            <p style="text-align: center; margin-bottom: 5px;">
                memucho: Ausgezeichneter Ort im Land der Ideen 2017.
            </p>
            <p style="text-align: center; margin-bottom: 0;">
                <a href="https://www.land-der-ideen.de/ausgezeichnete-orte/preistraeger/memucho-online-plattform-zum-faktenlernen" target="_blank">
                    <span style="white-space: nowrap">Zum Wettbewerb <i class="fa fa-external-link"></i></span>
                </a>
            </p>
        </div>        

        <div class="well" id="nominationInnopreis" style="padding: 10px; ">
            <div style="text-align: center;">
                <img src="/Images/LogosPartners/innovationspreis-nominiertButton2016.png" alt="Nominiert 2016 für den Innovationspreis Berlin Brandenburg" width="170" height="110" style="margin-bottom: 10px;"/>
            </div>
            <p style="text-align: center; margin-bottom: 0;">
                <a href="http://www.innovationspreis.de/news/aktuelles/zehn-nominierungen-f%C3%BCr-den-innovationspreis-berlin-brandenburg-2016.html" target="_blank">
                    <span style="white-space: nowrap">Zur Jury-Entscheidung <i class="fa fa-external-link"></i></span>
                </a>
            </p>
        </div>        

<%--        <div class="well" id="oerCamp" style="padding: 10px; ">
            <div style="text-align: center;">
                <img src="/Images/LogosPartners/OERCamp-Logo-Text_unten.jpg" alt="" width="170" height="183" style="margin-bottom: 10px;"/>
            </div>
            <p style="text-align: center; margin-bottom: 0;">
                memucho ist beim <a href="http://www.oercamp.de">OERcamp</a> in 
                <a href="http://www.oercamp.de/17/nord/workshops/#nordB4f" target="_blank">Hamburg</a> (23./24. Juni)
            </p>
        </div>--%>

        <div class="well">
            <h4>Neueste Lernsets</h4>
            <div class="LabelList">
                <% Html.RenderPartial("WelcomeBoxTopSets", WelcomeBoxTopSetsModel.CreateMostRecent(5)); %>
            </div>
        </div>
        
        <div class="well">
            <h4>Neueste Fragen</h4>
            <div class="LabelList">
                <% Html.RenderPartial("WelcomeBoxTopQuestions", WelcomeBoxTopQuestionsModel.CreateMostRecent(8)); %>
            </div>
        </div>
        
        <div class="well">
            <h4>Top-Themen nach Anzahl Fragen</h4>
                <div class="LabelList">
                    <% Html.RenderPartial("WelcomeBoxTopCategories", WelcomeBoxTopCategoriesModel.CreateTopCategories(5)); %>
                </div>
        </div>
        
        <div class="well">
            <h4>Neueste Themen:</h4>
            <div class="LabelList">
                <% Html.RenderPartial("WelcomeBoxTopCategories", WelcomeBoxTopCategoriesModel.CreateMostRecent(5)); %>
            </div>
        </div>

        <div class="well">
            <h4>Umfangreichste Lernsets</h4>
            <div class="LabelList">
                <% Html.RenderPartial("WelcomeBoxTopSets", WelcomeBoxTopSetsModel.CreateMostQuestions(5)); %>
            </div>
        </div>
        
        <% if (!Model.IsLoggedIn)
           { %>
            <div class="well" id="newsletterSignUp" style="padding: 20px; ">
                <h4>Newsletter</h4>
                <p>Du möchtest bei wichtigen Neuigkeiten benachrichtigt werden? Melde dich hier an:</p>
                <div class="alert alert-danger" role="alert" id="msgInvalidEmail" style="display:none">
                    Keine gültige E-Mail-Adresse.
                </div>
            
                <div class="alert alert-success" role="alert" id="msgEmailSend" style="display:none">
                    Deine Adresse wurde für den Newsletter angemeldet.
                </div>

                <form class="form-inline" style="color: white;">
                    <div class="">
                        <input type="email" class="form-control" id="txtNewsletterRequesterEmail" placeholder="deine@email.de" style="width: 100%;">
                    </div>
                    <div class="" style="text-align: center; margin-top: 10px;">
                        <a class="btn btn-primary" href="#" id="btnNewsletterRequest" style="">
                            <i class="fa fa-envelope-o">&nbsp;</i>Anmelden
                        </a>
                    </div>
                </form> 
            </div>
        <% } %>
        
        <%--<div class="row" style="padding-top: 10px;">
            <div class="col-md-12"><h3 class="media-heading">memucho-Netzwerk</h3></div>
        </div>
        
        <div class="panel panel-default" style="padding-top: 15px; opacity: 0.4;">
            <div class="panel-heading">Nutzer-Ranking nach Reputation</div>
            <div class="panel-body" style="padding-top: 12px;">
                <p style="padding-left: 5px;"><img src="/favicon-32x32.png" height="25" width="25" style="float: left; vertical-align: text-bottom"/>&nbsp;Pauli (130 Punkte)</p>
                <p style="padding-left: 5px;"><img src="/favicon-32x32.png" height="25" width="25" style="float: left; vertical-align: bottom"/>&nbsp;Robert (120 Punkte)</p>
                <p style="padding-left: 5px;"><img src="/favicon-32x32.png" height="25" width="25" style="float: left; vertical-align: middle"/>&nbsp;Christof (112 Punkte)</p>
            </div>
        </div>--%>

        <%--<div class="row" style="padding-top: 10px;">
            <div class="col-md-6">
                <h4 class="media-heading">Studienfächer</h4>
                <ul>
                    <li><a href="#">Psychologie</a></li>
                    <li><a href="#">Philosophie</a></li>
                    <li><a href="#">Informatik</a></li>
                    <li><a href="#">[mehr]</a></li>
                </ul>
            </div>

            <div class="col-md-6">
                <h4 class="media-heading">Schulfächer</h4>
                <ul>
                    <li><a href="#">Deutsch</a></li>
                    <li><a href="#">Mathe</a></li>
                    <li><a href="#">Geschichte</a></li>
                    <li><a href="#">[mehr]</a></li>
                </ul>
            </div>
        </div>--%>

    </div>
</div>

</asp:Content>