<%@ Page Title="memucho Team" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<BaseModel>" %>

<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">    
    <% 
        Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem { Text = "Team", Url = Links.Team(), ToolTipText = "Team" });
        Model.TopNavMenu.IsCategoryBreadCrumb = false;
    %>
</asp:Content>
 
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%Html.RenderPartial("Partials/TeamPics"); %>

    <div class="TeamText">                                         
        <p class="ShortParagraph">
            memucho ist ein gemeinwohlorientiertes Unternehmen, das freie Bildungsinhalte fördert.<br />
            Unser Team möchte dich beim Lernen unterstützen. Dafür konzipieren, gestalten und<br />
            programmieren wir gemeinsam und laden dich ein, es auch auszuprobieren.
        </p>
        <p class="ShortParagraph" id="link-share">
            <a class="btn btn-primary" href="<%=Links.CategoryDetail("memucho-Tutorials",945) %>"><i class="fa fa-lg fa-play-circle">&nbsp;</i>Teile dein Wissen und mache es anderen zugänglich! </a>
        </p>
        <p class="ShortParagraph">
            Wenn du Fragen oder Anregungen hast, schreibe uns eine Email an <a href="mailto:team@memucho.de">team@memucho.de</a>   oder<br />
            rufe uns an: +49-0178-1866848
        </p>
    </div>
      
        <%= Styles.Render("~/bundles/Team") %>
</asp:Content>
