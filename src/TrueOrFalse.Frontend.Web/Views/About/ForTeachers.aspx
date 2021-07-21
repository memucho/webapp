<%@ Page Title="memucho für Lehrende" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<ForTeachersModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/About/ForTeachers.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/mailto") %>
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "für Lehrer", Url = Links.ForTeachers(), ToolTipText = "für Lehrer"});
    Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="row">
        <div class="col-xs-12">

            <div class="well">

                <h1 class="PageHeader" style="margin-bottom: 15px; margin-top: 0;"><span class="ColoredUnderline GeneralMemucho">memucho als Lehrer und Dozent sinnvoll nutzen</span></h1>
                <p class="teaserText">
                    memucho unterstützt Lernende dabei, sich Grundlagenwissen einzuprägen und komplexe Zusammenhänge leichter rekonstruieren zu können.
                    Gleichzeitig erhöht memucho deutlich die Motivation der Schüler und Studenten.
                </p>
                <p class="teaserText">
                    Egal, wie du deinen Unterricht gestaltest, es gibt verschiedene Möglichkeiten, memucho sinnvoll einzubinden.
                    Das Tolle ist: memucho ist für dich und für die Schüler und Studenten kostenlos.
                </p>

                <div class="row">
                    <div class="col-xs-12 col-sm-6">
                        <div class="row overviewBlock">
                            <div class="col-xs-2 overviewIcon">
                                <i class="fa fa-puzzle-piece" style="color: #afd534">&nbsp;</i>
                            </div>
                            <div class="col-xs-10">
                                <p class="overviewHeader">Inhalte passend zum Lernstoff</p>
                                <p class="overviewSubtext">Vorhandene Inhalte übernehmen, neu zusammenstellen und mit eigenen Fragen ergänzen. So passen die Inhalte genau zum behandelten Stoff.</p>
                            </div>
                            <div class="col-xs-12">
                                <p class="overviewMore">
                                    <a href="#CompileContentMore" class="btn btn-primary">Mehr erfahren</a>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-xs-12 col-sm-6">
                        <div class="row overviewBlock">
                            <div class="col-xs-2 overviewIcon">
                                <i class="fa fa-lightbulb-o" style="color: #afd534">&nbsp;</i>
                            </div>
                            <div class="col-xs-10">
                                <p class="overviewHeader">Schüler erstellen Lerninhalte</p>
                                <p class="overviewSubtext">
                                    Schüler können als Gruppenarbeit oder Hausaufgabe den Lernstoff bei memucho selbst anzulegen. Davon profitieren alle.
                                </p>
                            </div>
                            <div class="col-xs-12">
                                <p class="overviewMore">
                                    <a href="#StudentContentMore" class="btn btn-primary">Mehr erfahren</a>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>




    <div class="row">
        <div class="col-xs-12">
            <div class="well">
                <h2 style="margin-bottom: 15px;" class="PageHeader" id="CompileContentMore">
                    <span class="ColoredUnderline GeneralMemucho"><i class="fa fa-puzzle-piece" style="color: #afd534">&nbsp;&nbsp;</i>Inhalte passend zum Lernstoff</span>
                </h2>
                <p class="subheader">
                    Vorhandene Inhalte übernehmen, neu zusammenstellen und mit eigenen Inhalten ergänzen
                </p>
                <p>
                    Bei memucho gibt es bereits <a href="<%= Links.QuestionsAll()%>"><%= Model.TotalPublicQuestionCount.ToString("N0") %> öffentliche Fragen</a>,
                    eingeordnet in <a href="<%= Links.CategoriesAll()%>"><%= Model.TotalCategoryCount.ToString("N0") %> Themen und Unterthemen</a>. 
                    Alle Inhalte bei memucho sind frei und rechtssicher verwendbar, sie stehen unter der Creative-Commons-Lizenz "CC-BY 4.0".
                </p>
                </p>
            </div>
        </div>
    </div>

    
    <div class="row">
        <div class="col-xs-12">
            <div class="well">
                <h2 style="margin-bottom: 15px;" class="PageHeader" id="StudentContentMore">
                    <span class="ColoredUnderline GeneralMemucho"><i class="fa fa-lightbulb-o" style="color: #afd534">&nbsp;&nbsp;</i>Schüler/Studenten erstellen Lerninhalte</span>
                </h2>
                <p class="subheader">
                    Schüler und Studenten können Lerninhalte selbst erstellen. Davon profitieren alle!
                </p>
                <p>
                    Den Lernstoff selbst aufzubereiten ist eine sehr effektive Art zu Lernen. 
                    Daher kannst du als Lehrer oder Dozent eine Gruppenarbeit oder eine Hausaufgabe vergeben, bei der deine Schüler bzw. Studenten 
                    die passenden Fragen zum behandelten Lernstoff selbst erstellen. 
                    Dabei kannst du Themen gut aufteilen, denn die einzelnen Fragen von unterschiedlichen Schülern können leicht zu 
                    einheitlichen Lernsets zusammengefasst werden, die für die ganze Klasse relevant sind.
                </p>
                <p>
                    Deine Schüler und Studenten lernen dadurch nicht nur selber viel, sie tun auch etwas Gutes: Sie erstellen offene Bildungsinhalte! 
                    Davon profitieren direkt die Mitschüler oder Kommilitonen, aber auch alle anderen, die damit lernen möchten. 
                    Angst, dass die Qualität nicht stimmt? Für gute Fragen, mit denen andere lernen wollen, erhalten die Ersteller Reputationspunkte. 
                    Das motiviert sie und gibt ein gutes Gefühl. Außerdem werden Fehler schnell erkannt, wenn die eigenen Mitschüler damit lernen. 
                    Über die Kommentarfunktion kann auf Fehler hingewiesen werden und Verbesserungsvorschläge gemacht werden. 
                    Wir achten darauf, dass die Kommentare fair und konstruktiv sind. Fehler sind kein Problem, denn sie können leicht korrigiert werden.
                </p>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-xs-12">
            <div class="well" style="margin-top: 25px;">
                <h2 class="PageHeader">
                    <span class="ColoredUnderline GeneralMemucho">Kontakt</span>
                </h2>

                <div class="row">
                    <div class="col-xs-4 col-md-3 TeamPic">
<%--                        <img src="/Images/Team/team_christof_20170404_P3312344_155.jpg" alt="Foto Christof"/>--%>
                    </div>
                    <div class="col-xs-8 col-md-9">
                        <p>
                            Du hast Fragen? Du hast selber tolle Ideen? Du kannst uns von deinen Erfahrungen beim Einsatz digitaler Medien im Unterricht bzw. in der Lehre erzählen?
                            Sprich uns einfach an, wir freuen uns über deine Nachricht! Dein Ansprechpartner ist:<br/>
                        </p>
                        <p>
                            <strong>Robert</strong><br/>
                            E-Mail: <span class="mailme">robert at memucho dot de</span><br/>
                            Telefon: +49-178 186 68 48<br/>
                        </p>
                        
                    </div>
                </div>

            </div>
        </div>
    </div>



</asp:Content>