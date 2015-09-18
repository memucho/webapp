﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
	Inherits="ViewPage<WelcomeModel>"%>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <title>MEMuchO</title>
    <link href="/Views/Welcome/Welcome.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    
<div class="row">
       
    <div class="col-md-8">
            
        <div class="well" style="background-color: white; padding: 13px;">
            <h1 style="margin-top: 0; margin-bottom: 7px; font-size: 24px;">MEMuchO ist eine Lern- und Wissensplattform</h1>
            <ul style="margin-top: 0; margin-bottom: 0; padding-top: 3px; font-size: 16px; list-style: circle ">                
                <li><a href="#teaserWhatIsMemucho">Wie hilft dir MEMuchO? &nbsp; <i class="fa fa-arrow-right" style="" ></i></a></li>
                <li><a href="#teaserPrinciples">Wikipedia-Prinzip, Vernetzung und Gemeinwohlorientierung &nbsp; <i class="fa fa-arrow-right" style="" ></i></a></li>
                <li><a href="#teaserWhoWeAre">Wer sind wir? &nbsp; <i class="fa fa-arrow-right" style="" ></i></a></li>
            </ul>
        </div>
            
        <div class="row ThumbnailRow" style="padding-top: 0px;">
            <% Html.RenderPartial("WelcomeBoxSingleQuestion", WelcomeBoxSingleQuestionModel.GetWelcomeBoxQuestionVModel(381, 205)); %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(14)); %>
            <% Html.RenderPartial("WelcomeBoxSingleQuestion", WelcomeBoxSingleQuestionModel.GetWelcomeBoxQuestionVModel(questionId: 404, contextCatId: 14)); %>
        </div>

        <div class="panel panel-default">
            <% Html.RenderPartial("WelcomeBoxSetImgQuestions", WelcomeBoxSetImgQuestionsModel.GetWelcomeBoxSetImgQuestionsModel(17, new int[] { 373, 360, 367 }, "Weißt du, wo diese weltweit bekannten Sehenswürdigkeiten stehen?")); %>
        </div>
        <div class="panel panel-default">
            <% Html.RenderPartial("WelcomeBoxSetTextQuestions", WelcomeBoxSetTextQuestionsModel.GetWelcomeBoxSetTextQuestionsModel(12, new int[] { 303, 288, 289 }, "Der berühmteste Agent im Dienste Ihrer Majestät: Kennst du die wichtigsten Fakten zu den James Bond-Filmen?")); %>
        </div>
        <div class="panel panel-default">
            <% Html.RenderPartial("WelcomeBoxSetTextQuestions", WelcomeBoxSetTextQuestionsModel.GetWelcomeBoxSetTextQuestionsModel(20, new int[] { 494, 485, 503 }, "Kennst du die Hauptstädte aller 28 Länder der Europäischen Union? Finde es heraus!")); %>
        </div>


        <div class="row ThumbnailRow" style="padding-top: 0px;">
            <% Html.RenderPartial("WelcomeBoxSingleQuestion", WelcomeBoxSingleQuestionModel.GetWelcomeBoxQuestionVModel(questionId: 385)); %>
            <% Html.RenderPartial("WelcomeBoxSingleQuestion", WelcomeBoxSingleQuestionModel.GetWelcomeBoxQuestionVModel(questionId: 337)); %>
            <% Html.RenderPartial("WelcomeBoxSingleQuestion", WelcomeBoxSingleQuestionModel.GetWelcomeBoxQuestionVModel(questionId: 233)); %>
        </div>

        <div class="panel panel-default">
            <% Html.RenderPartial("WelcomeBoxCategoryImgQ", WelcomeBoxCategoryImgQModel.GetWelcomeBoxCategoryImgQModel(211, new int[] { 394, 395, 390 }, "Farfalle, Penne oder Rigatoni? Weißt du wie diese Nudelsorten heißen?")); %>
        </div>

        <div class="well">
            <h3><a name="teaserWhatIsMemucho">Was ist MEMuchO?</a></h3>
            <p>
                MEMuchO ist eine vernetzte Lern- und Wissensplattform. Mit MEMuchO kannst du:
            </p>
            <div class="row">
                <div class="col-xs-6 col-md-4" style="text-align: center; font-size: 100%; padding: 5px 3px 20px;">
                  <i class="fa fa-clock-o fa-2x show-tooltip" style="color: #2C5FB2" title="MEMuchO analysiert dein Lernverhalten und wiederholt schwierige Fragen zum optimalen Zeitpunkt. So brauchst du weniger Zeit zum Lernen."></i><br/>
                    <b>Schneller lernen</b>
                </div>
                <div class="col-xs-6 col-md-4" style="text-align: center; font-size: 100%; padding: 5px 3px 20px;">
                  <i class="fa fa-book fa-2x show-tooltip" style="color: #2C5FB2" title="Du möchtest gerne mehr über Politik, die Griechenland-Krise oder über James Bond-Filme wissen? Finde die passenden Fragesätze und stelle dir dein Wunschwissen zusammen!"></i><br/>
                    <b>Allgemein- und Spezialwissen erweitern</b>
                </div>
                <div class="clearfix visible-xs"></div>
                <div class="col-xs-6 col-md-4" style="text-align: center; font-size: 100%; padding: 5px 3px 20px;">
                  <i class="fa fa-calendar-o fa-2x show-tooltip" style="color: #2C5FB2" title="Eine Klassenarbeit, eine Prüfung oder ein wichtiges Gespräch steht an? Lege einen Termin an und bestimme, was du bis dahin wissen musst. Mit MEMuchO weißt du immer, was du schon sicher kannst und wo du noch weiter üben musst."></i><br/>
                    <b>Zu einem bestimmten Termin lernen</b>
                </div>
                <div class="clearfix visible-md"></div>
                <div class="col-xs-6 col-md-4" style="text-align: center; font-size: 100%; padding: 5px 3px 20px;">
                  <i class="fa fa-pie-chart fa-2x show-tooltip" style="color: #2C5FB2" title="Du möchtest dir gerne 50, 500, 5000 (oder mehr) Fakten merken? Kein Problem, mit MEMuchO behältst du den Überblick."></i><br/>
                    <b>Überblick behalten</b>
                </div>
                <div class="clearfix visible-xs"></div>
                <div class="col-xs-6 col-md-4" style="text-align: center; font-size: 100%; padding: 5px 3px 20px;">
                  <i class="fa fa-share-alt fa-2x show-tooltip" style="color: #2C5FB2" title="MEMuchO ist ein offenes Netzwerk, wo du dein Wissen teilen und auf das Wissen anderer zurückgreifen kannst. Denn Wissen wird mehr, wenn man es teilt!"></i><br/>
                    <b>Wissen teilen</b>
                </div>
                <div class="col-xs-6 col-md-4" style="text-align: center; font-size: 100%; padding: 5px 3px 20px;">
                  <i class="fa fa-users fa-2x show-tooltip" style="color: #2C5FB2" title="Lerne gemeinsam mit Freunden und verabrede dich zum Quizduell, um dich auf die Klassenarbeit vorzubereiten."></i><br/>
                    <b>Gemeinsam lernen</b>
                </div>
                <div class="clearfix visible-xs"></div>
                <div class="col-xs-12">
                    <p>
                        Ausprobieren? <a href="<%= Url.Action("Register", "Welcome") %>">Registriere dich</a> und lege los! <br/>
                        Tolle Idee? Unterstütze uns und werde <a id="SupportUs" class="helpLink TextLinkWithIcon" href="<%= Url.Action(Links.Membership, Links.AccountController) %>">
                        <i class="fa fa-thumbs-up"></i>Fördermitglied</a> der ersten Stunde!
                    </p>                
                </div>
            </div>
            
        </div>
        <div class="well">

            <h3><a name="teaserPrinciples">Unsere Prinzipien</a></h3>
            <ul class="fa-ul">
                <li><i class="fa fa-li fa-book"></i>
                    <b>Freie Bildungsinhalte ("Open Educational Resources")</b>
                    <p>
                        Wir sind Teil der Bewegung zur Förderung frei zugänglicher Bildungsmaterialien.
                        In MEMuchO unterliegen öffentliche Inhalte einer Creative Commons-Lizenz, 
                        genau wie fast alle Einträge auf Wikipedia. Öffentliche MEMuchO-Inhalte 
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
                        Wir nutzen deine Daten, damit du besser lernen kannst und um MEMuchO besser zu machen. 
                        Aber wir werden deine Daten niemals verkaufen. (<a class="helpLink" href="<%= Url.Action(Links.HelpFAQ, Links.HelpController) %>">Erfahre mehr</a> über unseren Datenschutz.)
                    </p>
                </li>
                <li><i class="fa fa-li fa-github"></i>
                    <b>Open-Source und Transparenz</b>
                    <p>
                        Die Software, mit der MEMuchO läuft, steht unter einer Open-Source-Lizenz. Die Quelltexte 
                        sind frei verfügbar und können von allen frei verwendet werden. Du findest sie 
                        auf <a href="https://github.com/TrueOrFalse/TrueOrFalse"><i class="fa fa-github"></i>Github</a>. 
                        In Zukunft möchten wir neben der Gemeinwohlbilanz auch unsere Unternehmenszahlen veröffentlichen.
                    </p> 
                </li>        
            </ul>
            <p>
                Du willst es ausprobieren? <a href="<%= Url.Action("Register", "Welcome") %>">Registriere dich</a> und lege los! <br/>
                Du findest das eine tolle Idee, möchtest mitmachen und uns unterstützen?
                Werde <a id="SupportUs" class="helpLink TextLinkWithIcon" href="<%= Url.Action(Links.Membership, Links.AccountController) %>">
                <i class="fa fa-thumbs-up"></i>Fördermitglied</a> der ersten Stunde!
            </p>
        </div>
        <div class="well Founder">
            <h3><a name="teaserWhoWeAre">Team</a></h3>
            <div class="row">
                
                <div class="col-xs-4 ImageColumn">
                    <img src="http://www.gravatar.com/avatar/b937ba0e44b611a418f38cb24a8e18ea?s=128"/>
                        <br/> <b>Robert</b> (Gründer) <br/>
                </div>
                
                <div class="col-xs-4 ImageColumn">
                    <img src="/Images/no-profile-picture-128.png"/>  
                    <br/> <b>Jule</b> (Gründerin) <br/> 
                </div>

                <div class="col-xs-4 ImageColumn">
                    <img src="/Images/Team/team_christof2014_128.jpg"/>  
                    <br/> <b>Christof</b> (Gründer) <br/> 
                </div>


                <div class="col-xs-12" style="margin-top: 10px;">
                    <p>
                        Wir möchten, dass Faktenlernen einfacher wird und mehr Spaß macht. Wir möchten den Zugang zu freien Bildungsinhalten verbessern. 
                        Und wir möchten dabei ein stabiles <a href="#teaserPrinciples">gemeinwohlorientiertes Unternehmen</a> aufbauen. 
                        Als Gründungsteam konzipieren, gestalten und programmieren wir MEMuchO gemeinsam.
                    </p>
                    <p>
                        Wenn du Fragen oder Anregungen hast, trete einfach mit uns in <a class="helpLink" href="<%= Url.Action(Links.HelpFAQ, Links.HelpController) %>">Kontakt</a>.
                    </p>
                </div>
            </div>
        </div>  
    </div>
            
    <div class="col-md-4">
        <%
            var userSession = new SessionUser();
            if (!userSession.IsLoggedIn){
        %>
            <div class="box" style="padding: 20px; ">
                <a href="<%= Url.Action("Login", "Welcome") %>" class="btn btn-success btn-lg" style="width: 100%" role="button">Anmelden</a>
                <br/><br/>
                <a href="<%= Url.Action("Register", "Welcome") %>" class="btn btn-primary btn-lg" style="width: 100%;" role="button">Registrieren</a>
            </div>
        <% } %>
            
            
        <div class="row">
            <div class="col-md-12"><h3 class="media-heading">Entdecke neues Wissen!</h3></div>
        </div>

        <div class="row" style="padding-top: 15px;">
            <div class="col-md-12">
                <h4 class="media-heading">Top-Kategorien nach Fragen:</h4>
                <% Html.RenderPartial("WelcomeBoxTopCategories", WelcomeBoxTopCategoriesModel.CreateTopCategories(5)); %>
            </div>
        </div>
        
        <div class="row" style="padding-top: 15px;">
            <div class="col-md-12">
                <h4 class="media-heading">Neueste Fragesätze:</h4>
                <% Html.RenderPartial("WelcomeBoxTopSets", WelcomeBoxTopSetsModel.CreateMostRecent(5)); %>
            </div>
        </div>
        
        <div class="row" style="padding-top: 15px;">
            <div class="col-md-12">
                <h4 class="media-heading">Umfangreichste Fragesätze:</h4>
                <% Html.RenderPartial("WelcomeBoxTopSets", WelcomeBoxTopSetsModel.CreateMostQuestions(5)); %>
            </div>
        </div>
        
        <div class="row" style="padding-top: 15px;">
            <div class="col-md-12">
                <h4 class="media-heading">Neueste Fragen:</h4>
                <% Html.RenderPartial("WelcomeBoxTopQuestions", WelcomeBoxTopQuestionsModel.CreateMostRecent(8)); %>
            </div>
        </div>
        
        <div class="row" style="padding-top: 15px;">
            <div class="col-md-12">
                <h4 class="media-heading">Neueste Kategorien:</h4>
                <% Html.RenderPartial("WelcomeBoxTopCategories", WelcomeBoxTopCategoriesModel.CreateMostRecent(5)); %>
            </div>
        </div>

        <div class="row">
            <hr/>
            <div class="col-md-12"><h3 class="media-heading">MEMuchO-Netzwerk</h3></div>
        </div>
        
        <div class="row" style="padding-top: 15px;">
            <div class="col-md-12">
                <h4 class="media-heading">Nutzer-Ranking nach Reputation</h4>
                <p style="padding-left: 15px;"><img src="http://placebear.com/25/25" style="float: left; vertical-align: middle"/>&nbsp;Pauli (130 Punkte)</p>
                <p style="padding-left: 15px;"><img src="http://placecage.com/25/25" style="float: left; vertical-align: middle"/>&nbsp;Robert (120 Punkte)</p>
                <p style="padding-left: 15px;"><img src="http://placebear.com/25/25" style="float: left; vertical-align: middle"/>&nbsp;Christof (112 Punkte)</p>
            </div>
        </div>

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