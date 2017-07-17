<%@ Page Title="Über memucho" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/About/AboutMemucho.css" rel="stylesheet" />
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<div class="row">
    <div class="col-xs-12">
        <h1 class="PageHeader" style="margin-bottom: 20px;"><span class="ColoredUnderline Question">memucho: Die schnelle und sichere Art zu lernen</span></h1>
    </div>
</div>
<div class="row">
    <div class="col-xs-12">
        <div class="well">
            <p>
                Wir möchten, dass du mit memucho besser lernen kannst und mehr Spaß dabei hast, neues und altes Wissen zu entdecken. Das ist unser großes Ziel! 
                Noch sind wir in der <a href="<%= Links.BetaInfo() %>">Beta-Phase</a> und freuen uns, dass du von Anfang an dabei bist. 
                Hier erfährst du, <a href="#Vorteile">welche Vorteile</a> dir memucho bietet, <a href="#Prinzipien">welche Prinzipien</a> uns leiten und <a href="#Team">wer wir sind</a>.
            </p>
        </div>
        
        <div style="text-align: center; margin-bottom: 30px;">
            <iframe width="560" height="315" src="https://www.youtube.com/embed/qJW_V_q_3hs" frameborder="0" allowfullscreen></iframe>    
        </div>
    </div>
</div>
        
<div class="row">
    <div class="col-xs-12">        

        <div class="well">
            <h1 class="PageHeader" id="Vorteile">
                <span class="ColoredUnderline Question">Deine Vorteile bei memucho</span>
            </h1>
            
            <div class="row aboutRow">
                <div class="col-xs-3 xxs-stack aboutImg <%--col-xs-push-9--%>">
                    <div class="aboutImgInner">
                        <i class="fa fa-heart fa-3x" style="/*color:#b13a48*/"></i>
                    </div>
                </div>
                <div class="col-xs-9 xxs-stack aboutText <%--col-xs-pull-3--%>">
                    <h3>Wunschwissen sammeln und nie vergessen</h3>
                    <p>
                        Bei memucho kannst du dein <i class="fa fa-heart" style="color:#b13a48;"></i> Wunschwissen zusammenstellen. 
                        Entscheide, was du dir merken möchtest und memucho erinnert dich rechtzeitig daran, 
                        wenn du es wieder lernen musst, um es nicht zu vergessen.
                    </p>
                    <p>
                        Wo? Überall, wo du ein kleines <i class="fa fa-heart-o" style="color:#b13a48;"></i> findest, kannst du draufklicken, 
                        um ein Lernset, ein ganzes Thema oder eine einzelne Frage zu deinem Wunschwissen hinzuzufügen.
                        In deiner <a href="<%= Links.Knowledge() %>">Wissenszentrale</a> bekommst du jederzeit einen Überblick zu deinem Wunschwissen.
                    </p>
                </div>
            </div>

            <div class="row aboutRow">
                <div class="col-xs-3 xxs-stack aboutImg <%--col-xs-push-9--%>">
                    <div class="aboutImgInner">
                        <i class="fa fa-check-circle fa-3x"></i>
                    </div>
                </div>
                <div class="col-xs-9 xxs-stack aboutText <%--col-xs-pull-3--%>">
                    <h3>Sicherheit gewinnen</h3>
                    <p>
                        Mit memucho weißt du immer, wie viel Zeit du noch zum Lernen brauchst und ob du deine Lernziele erreichst. 
                        Du liegst im Plan? Dann kannst du mit gutem Gewissen ins Kino gehen.
                    </p>
                    <p>
                        Wo? Im Moment musst du dafür die einen Prüfungstermin anlegen (vergleiche den nächsten Punkt), aber ganz bald 
                        kannst du auch dein Wunschwissen optimiert lernen - also all das, was du dir immer merken möchtest.
                    </p>
                </div>
            </div>

            <div class="row aboutRow">
                <div class="col-xs-3 xxs-stack aboutImg">
                    <div class="aboutImgInner">
                        <i class="fa fa-line-chart fa-3x"></i>
                    </div>
                </div>
                <div class="col-xs-9 xxs-stack aboutText">
                    <h3>Optimiert für eine Prüfung lernen</h3>
                    <p>
                        memucho erstellt dir einen persönlichen Lernplan, wenn du dich auf eine Prüfung vorbereitest. 
                        Intelligente Algorithmen sagen dir, wann du was am besten lernst und ob du deine Lernziele erreichen wirst. 
                        memucho ist dein persönlicher Coach beim Auswendiglernen.
                    </p>
                    <p>
                        Wie das geht? Gehe zu <a href="<%= Links.Dates() %>">Termine</a>, lege einen neuen Termin an und wähle die
                        zu lernenden Lernsets aus. memucho erstellt dir dann automatisch einen persönlichen Lernplan.
                    </p>
                </div>
            </div>
            
            <div class="row aboutRow">
                <div class="col-xs-3 aboutImg xxs-stack">
                    <div class="aboutImgInner">
                        <i class="fa fa-clock-o fa-3x"></i>
                    </div>
                </div>
                <div class="col-xs-9 xxs-stack aboutText">
                    <h3>Zeit sparen</h3>
                    <p>
                        memucho weiß, welche Fragen du am dringendsten lernen musst und welche (noch) nicht.
                        So lernst du immer das Richtige und musst nie wieder umsonst lernen. Dadurch sparst du Zeit.
                    </p>
                    <p>
                        Wie wir das machen? Wir analysieren, wie und wofür du lernst und wie andere Nutzer die Inhalte gelernt haben. 
                        Dadurch können wir vorhersagen, wie gut du etwas (noch) weißt. Nutze dafür unsere Lernen-Funktion. 
                        Sie ist nur verfügbar, wenn du <a href="#" data-btn-login="true">angemeldet bist</a>.
                        <%--(<a href="<%= Links.AlgoInsightForecast() %>">Hier erfährst du mehr über unsere Technologie.</a>)--%>
                    </p>
                </div>
            </div>

            <div class="row aboutRow">
                <div class="col-xs-3 xxs-stack aboutImg">
                    <div class="aboutImgInner">
                        <i class="fa fa-cogs fa-3x"></i>
                    </div>
                </div>
                <div class="col-xs-9 xxs-stack aboutText">
                    <h3>Und vieles mehr...</h3>
                    <p>
                        Mit memucho behältst du immer den Überblick, egal wie viel Wissen du dir merken möchtest oder wieviele Prüfungen du hast. 
                        memucho zeigt dir ausführliche <a href="<%= Links.Knowledge() %>">Lernauswertungen</a> und Statistiken 
                        und du kannst <a href="<%= Links.Users() %>">deinen Freunden folgen</a>, 
                        mit ihnen lernen und im <a href="<%= Links.Games() %>">Quiz-Spiel</a> gegen sie antreten.
                    </p>
                </div>
            </div>
            <div class="row aboutRow">
                <%  var isLoggedIn = Sl.R<SessionUser>().IsLoggedIn;
                    if (!isLoggedIn) { %>
                    <div class="col-xs-12" style="margin-top: 10px; text-align: center">
                        <a id="btnRegisterAboutFeatures" href="<%= Url.Action("Register", "Welcome") %>" class="btn btn-success" role="button"><i class="fa fa-chevron-circle-right">&nbsp;</i> Jetzt Registrieren</a> <br/>
                        <div class="" style="margin-top: 3px; font-style: italic">*memucho ist kostenlos.</div>
                    </div>
                <%  } %>
            </div>

        </div>
    </div>
</div>
    
<div class="row">
    <div class="col-xs-12">
        <div class="well">
            <h1 class="PageHeader" id="Prinzipien">
                <span class="ColoredUnderline Question">Unsere Prinzipien</span>
            </h1>
            <p>
                Wir wollen nicht nur dein Lernen vereinfachen, sondern damit freie Bildungsinhalte und Open-Source fördern und ein gemeinwohlorientiertes und 
                transparentes Unternehmen aufbauen. Diese Werte leiten unser Handeln. 
                <a href="<%= Links.FAQItem("Contact") %>">Melde dich bei uns</a>, wenn du Fragen oder Anregungen hast. Wir freuen uns über deine Nachricht!
            </p>

            <div class="row aboutRow" style="margin-top: 30px;">
                <div class="col-xs-3 aboutImg xxs-stack">
                    <div class="aboutImgInner">
                        <i class="fa fa-tree fa-3x"></i>
                    </div>
                </div>
                <div class="col-xs-9 xxs-stack aboutText">
                    <h3>Sozialunternehmen und Gemeinwohlorientierung</h3>
                    <p>
                        memucho fördert die freie Verfügbarkeit von Wissen, unterstützt Lernende beim Lernen und ist und bleibt kostenlos. 
                        Damit demokratisieren wir Wissen. Darüber hinaus möchten wir unser Unternehmen auf gemeinwohlfördernden Werten aufbauen. 
                        Als Teil der <a href="http://www.gemeinwohl-oekonomie.org/de"><i class="fa fa-external-link">&nbsp;</i>Gemeinwohlökonomie</a> sind wir davon überzeugt, 
                        dass Unternehmen der Gemeinschaft dienen müssen und deshalb eine ethische, soziale und ökologische Verantwortung haben. 
                        Dazu gehört in Zukunft die Veröffentlichung einer 
                        <a href="https://www.ecogood.org/de/gemeinwohl-bilanz/gemeinwohl-matrix/" target="_blank"><i class="fa fa-external-link">&nbsp;</i>Gemeinwohlbilanz</a>.
                    </p>
                </div>
            </div>

            <div class="row aboutRow">
                <div class="col-xs-3 aboutImg xxs-stack">
                    <div class="aboutImgInner">
                        <i class="fa fa-book fa-3x"></i>
                    </div>
                </div>
                <div class="col-xs-9 xxs-stack aboutText">
                    <h3>Offene Bildungsinhalte (OER)</h3>
                    <p>
                        Wir sind Teil der Bewegung zur Förderung frei zugänglicher Bildungsmaterialien, der "Open Educational Resources" (OER). 
                        In memucho unterliegen öffentliche Inhalte einer Creative Commons-Lizenz, genau wie fast alle Einträge auf Wikipedia. 
                        Unsere Fragen können also von jedem kostenfrei und ohne Einschränkungen verwendet werden. Private Inhalte sind aber privat. 
                        (<a href="http://creativecommons.org/licenses/by/4.0/deed.de" target="_blank"><i class="fa fa-external-link">&nbsp;</i>Hier erfährst du genaueres zur Creative-Commons-Lizenz CC BY 4.0</a> und 
                        <a href="http://www.unesco.de/fileadmin/medien/Dokumente/Bildung/Was_sind_OER__cc.pdf" target="_blank"><i class="fa fa-external-link">&nbsp;</i>hier gibt es Infos zu OER von der Unesco</a>).
<%--                        <img src="/Images/LogosPartners/Logo-OER_200px_by_Jonathasmello.png" />
                        <span class="greyed" style="font-size: 11px;">OER-Logo, von Jonathas Mello (CC BY 3.0)</span>--%>
                    </p>
                </div>
            </div>

            <div class="row aboutRow">
                <div class="col-xs-3 aboutImg xxs-stack">
                    <div class="aboutImgInner">
                        <i class="fa fa-search-plus fa-3x"></i>
                    </div>
                </div>
                <div class="col-xs-9 xxs-stack aboutText">
                    <h3>Open Source und Transparenz</h3>
                    <p>
                        Die Software, mit der memucho läuft, steht unter einer Open-Source-Lizenz. Die Quelltexte sind frei verfügbar und können von allen frei verwendet werden. 
                        Du findest sie auf <a href="https://github.com/TrueOrFalse/TrueOrFalse" target="_blank"><i class="fa fa-github">&nbsp;</i>Github</a>. Wenn wir etwas versprechen, kannst du es im Quelltext überprüfen. 
                        Weil uns Transparenz auch sonst wichtig ist, möchten wir in Zukunft neben der Gemeinwohlbilanz auch unsere Unternehmenszahlen veröffentlichen.
                    </p>
                </div>
            </div>

            <div class="row aboutRow">
                <%  if (!isLoggedIn) { %>
                    <div class="col-xs-12" style="margin-top: 10px; text-align: center">
                        <a id="btnRegisterAboutFeatures" href="<%= Url.Action("Register", "Welcome") %>" class="btn btn-success" role="button"><i class="fa fa-chevron-circle-right">&nbsp;</i> Jetzt Registrieren</a> <br/>
                        <div class="" style="margin-top: 3px; font-style: italic">*memucho ist kostenlos.</div>
                    </div>
                <%  } %>
            </div>

        </div>
    </div>
</div>
    
    

<div class="row">
    <div class="col-xs-12">
        <div class="well">
            <h1 class="PageHeader" id="Team">
                <span class="ColoredUnderline Question">Das Team</span>
            </h1>
            <p>
                Wir möchten den Zugang zu freien Bildungsinhalten verbessern und mit memucho ein soziales Wissensnetzwerk aufbauen, mit dem
                Faktenlernen effizienter wird und mehr Spaß macht. Wir konzipieren, gestalten und programmieren memucho gemeinsam.
            </p>

            <div class="row" style="margin-top: 30px;">
                <div class="col-xs-4 TeamPic">
                    <img src="/Images/Team/team_robert201509_155.jpg"/>
                        <br/> <b>Robert</b><br/>(Gründer) 
                </div>
                
                <div class="col-xs-4 TeamPic">
                    <img src="/Images/Team/team_jule201509-2_155.jpg"/>  
                    <br/> <b>Jule</b><br/>(Gründerin)
                </div>

                <div class="col-xs-4 TeamPic">
                    <img src="/Images/Team/team_christof_20170404_P3312344_155.jpg"/>  
                    <br/> <b>Christof</b><br/>(Gründer)
                </div>
                <div class="col-xs-4 TeamPic">
                    <img src="/Images/Team/team_lisa_sq_155.jpg"/>  
                    <br/> <b>Lisa</b><br/>(Kommunikation)
                </div>
                <div class="col-xs-4 TeamPic">
                    <img src="/Images/Team/team_julian20170404_P3312327_155.jpg"/>  
                    <br/> <b>Julian</b><br/>(Entwicklung)
                </div>
            </div>
        </div>
    </div>
</div>

   
<div class="row">
    <div class="col-xs-12">
        <div class="well">
            <h1 class="PageHeader" id="Partner">
                <span class="ColoredUnderline Question">Unsere Partner</span>
            </h1>
            <p class="partnerLogos">
                <a href="https://learning-levelup.de/" target="_blank"><img src="/Images/LogosPartners/Logo_LearningLevelUp.png"/></a>
                <a href="https://www.tutory.de/" target="_blank"><img src="/Images/LogosPartners/Logo_tutory_250px.png"/></a>
                <a href="http://www.lernen-mit-spass.ch/" target="_blank"><img src="/Images/LogosPartners/Logo_lernenmitspass_w200.gif"/></a>
                <%--<a href="http://www.geschichtsinfos.de/" target="_blank"><img src="/Images/LogosPartners/Logo_Geschichtsinfos_de_w200.jpg"/></a>--%>
            </p>
        </div>
    </div>
</div>

</asp:Content>
