<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<CategoryChangesOverviewModel>" %>

<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/CategoryHistory") %>
    <%
        Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem {Text = "Bearbeitungshistorie aller Themen", Url = Links.QuestionChangesOverview(1), ToolTipText = "Bearbeitungshistorie aller Themen"});
        Model.TopNavMenu.IsCategoryBreadCrumb = false;
    %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-12">
            <h1><i class="fa fa-list"></i>&nbsp; Bearbeitungshistorie aller Themen</h1>
        </div>
    </div>
    <% foreach (var day in Model.Days)
       {
           var afterRelease = ReleaseDate.IsAfterRelease(day.DateTime);
    %>

        <div class="row">
            <div class="col-md-12">
                <h3><%= day.Date %></h3>
            </div>
        </div>

        <% foreach (var item in day.Items)
           {
               if (item.IsVisibleToCurrentUser())
               {
                   { %>

                    <div class="row change-detail-model">                        
                        <div class="col-xs-4">
                            <a href="<%= Links.CategoryDetail(item.CategoryName, item.CategoryId) %>">
                                <b><%= item.CategoryName %></b>
                                <% if (item.Visibility == CategoryVisibility.Owner)
                                   { %>
                                    <i class="fas fa-lock"></i>
                                <% } %>
                            </a>
                        </div>
                        <div class="col-xs-4 show-tooltip" data-toggle="tooltip" data-placement="left" title="<%= item.DateTime %>">
                            <a href="<%= Links.UserDetail(item.Author) %>">
                                <img src="<%= item.AuthorImageUrl %>" height="20"/>
                            </a>
                            <b>
                                <a href="<%= Links.UserDetail(item.Author) %>"><%= item.AuthorName %></a>
                            </b>
                            um <%= item.Time %>
                            <div id="Typ"><%= item.Typ %></div>
                        </div>
                        <div class="col-xs-4">
                            <%if (afterRelease) {%>
                                <a class="btn btn-sm btn-default btn-primary" href="<%= Links.CategoryDetail(item.CategoryName, item.CategoryId, item.CategoryChangeId) %>">
                                    <i class="fa fa-desktop"></i>&nbsp; Revision anzeigen
                                </a>&nbsp;
                            <%} %>
                            <a class="btn btn-sm btn-default <%if (afterRelease) {%>editing-history<%} %> btn-primary c-changes-overview" href="<%= Links.CategoryHistoryDetail(item.CategoryId, item.CategoryChangeId) %>">
                                <i class="fa fa-code-fork"></i>&nbsp; Änderungen anzeigen
                            </a>
                            <a class="btn btn-sm btn-default editing-history" href="<%= Links.CategoryHistory(item.CategoryId) %>">
                                <i class="fa fa-list-ul"></i> &nbsp; Bearbeitungshistorie
                            </a>
                        </div>
                    </div>

                <% }
               }
           }
        } %>

    <br/>
    <br/>
    <div class="col-md-12 text-center">
        <div class="btn-group" role="group">
            <a type="button" class="btn btn-default" href="<%= Links.CategoryChangesOverview(Model.PageToShow - 1) %>" <%= Model.PageToShow == 1 ? "disabled" : "" %>>
                Neuere Revisionen

            </a>
            <a type="button" class="btn btn-default" href="<%= Links.CategoryChangesOverview(Model.PageToShow + 1) %>">
                Ältere Revisionen
            </a>
        </div>
    </div>
</asp:Content>