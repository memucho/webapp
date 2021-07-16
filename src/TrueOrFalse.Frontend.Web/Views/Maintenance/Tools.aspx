<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<ToolsModel>" %>

<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Scripts.Render("~/bundles/MaintenanceTools") %>
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem { Text = "Administrativ", Url = "/Maintenance", ToolTipText = "Administrativ" });
        Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem { Text = "Tools", Url = "/Maintenance/Tools", ToolTipText = "Tools" });
        Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("AntiForgeryToken"); %>

    <nav class="navbar navbar-default" style="" role="navigation">
        <div class="container">
            <a class="navbar-brand" href="#">Maintenance</a>
            <ul class="nav navbar-nav">
                <li><a href="/Maintenance">Allgemein</a></li>
                <li><a href="/MaintenanceImages/Images">Bilder</a></li>
                <li><a href="/Maintenance/Messages">Nachrichten</a></li>
                <li class="active"><a href="/Maintenance/Tools">Tools</a></li>
                <li><a href="/Maintenance/CMS">CMS</a></li>
                <li><a href="/Maintenance/ContentCreatedReport">Cnt-Created</a></li>
                <li><a href="/Maintenance/Statistics">Stats</a></li>
            </ul>
        </div>
    </nav>
    <% Html.Message(Model.Message); %>

    <h4>Tools</h4>
    <a href="<%= Url.Action("Throw500", "Maintenance") %>" data-url="toSecurePost">
        <i class="fa fa-gavel"></i>
        Exception werfen
    </a>
    <br />
    <a href="<%= Url.Action("ReloadListFromIgnoreCrawlers", "Maintenance") %>" data-url="toSecurePost">
        <i class="fa fa-gavel"></i>
        List von den igniorierten Crawlers neu laden
    </a>
    <br />
    <a href="<%= Url.Action("CleanUpWorkInProgressQuestions", "Maintenance") %>" data-url="toSecurePost">
        <i class="fa fa-gavel"></i>
        Clean up work in progress questions
    </a>
    <br />

    <a href="<%= Url.Action("Start100TestJobs", "Maintenance") %>" data-url="toSecurePost">
        <i class="fa fa-gavel"></i>
        Start 100 test jobs
    </a>
    <br />

    <h4 style="margin-top: 20px;">Delete User</h4>
    <div class="form-horizontal">

        <% using (Html.BeginForm("UserDelete", "Maintenance"))
            {%>
        <%= Html.AntiForgeryToken() %>

        <div class="form-group col-md-6">
            <label class="col-sm-4 control-label">UserId</label>
            <div class="col-xs-4">
                <%= Html.TextBoxFor(m => m.UserId, new {@class="form-control"} ) %>
            </div>
            <div class="col-sm-offset-4 col-sm-12" style="margin-top: 15px;">
                <input type="submit" value="User löschen" class="btn btn-primary" />
            </div>
        </div>
        <div class="col-md-6" style="margin-bottom: 50px;">
            1.Vor dem Löschen prüfen oder der Nutzer relevante Inhalte erstellt hat.<br />
            <br />
            2. Hat der Nutzer relevante Inhalte erstellt, muss er eine Email bekommen<br />
            X
                in der er darüber informiert wird das seine Inhalte unter Lizenz CC BY 2.0 DE<br />
            anonymisiert weiterverwendet werden.<br />
            <br />
            3. memucho muss nach dem Löschen neu gestartet werden da es sonst Probleme mit dem EntityCache gibt.
        </div>
        <% } %>
    </div>
    <h4 style="margin-top: 20px;">Ändere Zahlungsstatus</h4>
    <div class="form-horizontal col-md-12" style="padding-bottom: 50px; ">
        <% using (Html.BeginForm("ChangePayment", "Maintenance"))
            {%>
        <%= Html.AntiForgeryToken() %>

        <div class="form-group col-md-6">
            <label class="col-sm-4 control-label">UserId</label>
            <div class="col-xs-4">
                <%= Html.TextBoxFor(m => m.UserId, new {@class="form-control"} ) %>
                <%=  @Html.EnumDropDownListFor(m => m.PaymentStatus, "Select a Item", new { @class = "form-control" })%>
            </div>
            <div class="col-sm-offset-4 col-sm-12" style="margin-top: 15px;">
                <input type="submit" value="Zahlungsstatus ändern" class="btn btn-primary" />
            </div>
        </div>
        <% } %>
    </div>
    

</asp:Content>
