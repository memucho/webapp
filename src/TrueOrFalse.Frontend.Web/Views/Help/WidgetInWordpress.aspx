<%@ Page Title="memucho-Quiz in Wordpress einbetten" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<BaseModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Help/Widget.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/mailto") %>
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Widgets", Url = Links.HelpWidget(), ToolTipText = "Widgets"});
       Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "In Wordpress einbinden", Url = Links.HelpWidgetWordpress(), ToolTipText = "In Wordpress einbinden"});
       Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% Html.RenderPartial("~/Views/Help/WidgetMenu.ascx", new WidgetMenuModel());  %>

    <div class="row">
        <div class="col-xs-12">

            <div class="well">

                <h1 class="PageHeader"><span class="ColoredUnderline GeneralMemucho">In Wordpress einbinden</span></h1>
                <p class="teaserText">
                    Die Lerntechnologie und die Lerninhalte von memucho können als Widget leicht in bestehende Wordpress-Webseiten und Blogs integriert werden. 
                    Nötig ist eine Zeile HTML-Code, die du einfach von memucho kopieren kannst. memucho hat verschiedene Widgets zur Auswahl 
                    (<a href="<%= Links.WidgetExamples() %>">zur Widget-Übersicht</a>).
                </p>
                   
            </div>
        </div>
    </div>



    <div class="row">
        <div class="col-xs-12">
            <div class="well explanationBox">

                <h2 class="PageHeader">
                    <span class="ColoredUnderline GeneralMemucho">Einbindung memucho-Widget in Wordpress: Anleitung</span>
                </h2>
                <p>
                    Auf der Seite des Lernsets, Video-Lernsets oder der einzelnen Frage findest du einen Link <code><i class="fa fa-code">&nbsp;</i>Einbetten</code>. 
                    Dort kannst das Widget, wenn du möchtest, konfigurieren und erhältst die Code-Zeile zur Einbettung des Widgets (<a href="<%= Links.HelpWidget() %>">mehr dazu</a>).
                    Beispiele für die Widgets zeigen wir in der <a href="<%= Links.WidgetExamples() %>">Widget-Übersicht</a>.
                </p>
                <p>
                    Diese Code-Zeile kannst du in Wordpress einfach an die Stelle deines Beitrags (Posts) oder deiner Seite (Page) einfügen, wo der Quiz erscheinen soll.
                    Dafür musst du dich aber im HTML bzw. Text-Modus befinden. Das geht ganz leicht:
                </p>
                <h4>1. Seite oder Beitrag zum Editieren in Wordpress öffnen</h4>
                <p class="screenshotExplanation">
                    Öffne die Seite oder den Beitrag in Wordpress, wo du das Widget einbinden möchtest und editiere ihn. 
                    Im Texteditor-Fenster gehst du mit dem Cursor an die Stelle, wo das Widget erscheinen soll.
                </p>
                <p class="screenshot">
                    <img src="/Images/Screenshots/widget-wordpress-01.png" />
                </p>

                <h4>2. In den Text-Modus gehen</h4>
                <p class="screenshotExplanation">
                    Am oberen Rand des Editor-Fensters findest du rechts einen Umschalter von "Visuell" zu "Text": <img src="/Images/Screenshots/widget-wordpress-textmode-small.png"/><br/>
                    Klicke auf "Text", um den Text- bzw. HTML-Modus zu öffnen. Nun siehst du den gleichen Text, aber zum Teil mit den Formatierungen in der HTML-Sprache, die du an den spitzen Klammern erkennst.
                </p>
                <p class="screenshotExplanation">
                    Genau dort, wo der Cursor noch ist, kannst du nun die Code-Zeile des Widgets einfügen.
                </p>
                <p class="screenshot">
                    <img src="/Images/Screenshots/widget-wordpress-02.png" />
                </p>
                
                <h4>3. Fertig!</h4>
                <p class="screenshotExplanation">
                    Jetzt kannst du deine Seite anschauen und sehen, dass alles geklappt hat. Viel Spaß!
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