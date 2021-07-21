<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<BaseModel>" 
        Title="Fehler" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/Views/Error/Error.css") %>
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
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
        
    <img src="/Images/Error/memo-500_german_600.png" class="ErrorImage" />
    
    <p>
        Wir kümmern uns um das Problem. 
        <ul>
            <li>Wir wurden per E-Mail informiert.</li>
            <li>Bei dringenden Fragen kannst du Robert unter 0178-1866848 erreichen.</li>
            <li>Oder schicke eine E-Mail an <span class="mailme">team at memucho dot de</span>.</li>
        </ul>
    </p>

</asp:Content>
