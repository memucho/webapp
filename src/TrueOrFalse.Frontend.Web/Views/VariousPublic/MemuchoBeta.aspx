<%@ Page Title="memucho Beta-Phase" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<BaseModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

<script type="text/javascript" >

    /* Source http://www.html-advisor.com/javascript/hide-email-with-javascript-jquery/ */
    $(function () {
        var spt = $('span.mailme');
        var at = / at /;
        var dot = / dot /g;
        var addr = $(spt).text().replace(at, "@").replace(dot, ".");
        $(spt).after('<a href="mailto:' + addr + '" title="Schreibe eine E-Mail">' + addr + '</a>').hover(function () { window.status = "Schreibe eine E-Mail!"; }, function () { window.status = ""; });
        $(spt).remove();
    });
</script>    
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "[beta]", Url = Links.BetaInfo()});
       Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<div class="row">
    <div class="col-xs-12">
        <h1 class="PageHeader" style="margin-bottom: 15px; margin-top: 0px;"><span class="ColoredUnderline Question">memucho ist in der Beta-Phase</span></h1>
    </div>
</div>
<div class="row">
    <div class="col-xs-12 col-sm-8">
        <p>
            Wir möchten, dass du mit memucho besser lernen kannst und mehr Spaß dabei hast, neues und altes Wissen zu entdecken. Das ist unser großes Ziel!
        </p>
        <p>
            Aber wir haben gerade erst angefangen. Einiges sieht noch nicht ganz so poliert aus, an anderen Stellen fehlen noch Funktionen, 
            hier und da gibt es noch echte Fehlerchen zu entdecken.
            Und dann haben wir auch noch soooo viele tolle Ideen!
        </p>
        <h3>Du möchtest uns unterstützen?</h3>
        <p>
            Das ist toll! Wir freuen uns vor allem, dass du von Anfang an dabei bist! Du kannst uns ganz leicht unterstützen:
            <ul>
                <li>Lerne mit memucho, entdecke interessantes Wissen und erstelle selbst tolle Fragen.</li>
                <li>Erzähle deinen Freunden von uns und teile interessantes Wissen auf Facebook.</li>
                <li>Erzähle uns, was dir an memucho gut gefällt, was du vermisst und welche Fehler dir aufgefallen sind.</li>
            </ul>
            Vorschläge und Fehler kannst du <a href="#" onclick="_urq.push(['Feedback_Open']);">direkt hier eintragen</a>, du kannst auch  
            Robert anrufen (+49-0178-1866848) oder ihm eine E-Mail schreiben (<span class="mailme">robert at memucho dot de</span>).
        </p>
    </div>
    <div class="col-xs-12 col-sm-4" style="text-align: center;">
        <img src="/Images/Logo/memucho_MEMO_happy_blau_mit Schild_DANKE.png" width="200" height="276"/>        
    </div>
</div>

</asp:Content>
