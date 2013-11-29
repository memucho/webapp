﻿<%@ Page Title="Nutzer" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<UsersModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/Views/Users/Users.css") %>
    <%= Scripts.Render("~/bundles/Users") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-10">
        <% using (Html.BeginForm()) { %>
    
            <div style="float: right;">
                <a href="#" style="width: 140px" class="btn btn-default">
                    <i class="icon-plus-sign"></i> Benutzer einladen 
                </a>
            </div>
            <div class="box-with-tabs">
                <div class="green">
                    <ul class="nav nav-tabs">
                        <li class="active"><a href="#home" >Alle Nutzer (<%= Model.TotalSets %>)</a></li>
                        <li>
                            <a href="#profile">
                                Mein Netzwerk <span id="tabWishKnowledgeCount">(<%= Model.TotalMine %>)</span> <i class="icon-question-sign" id="tabInfoMyKnowledge"></i>
                            </a>
                        </li>
                    </ul>
                </div>
        
                <div class="box box-green">
                    <div class="form-horizontal">
                        <div class="control-group" style="margin-bottom: 15px; margin-top: -7px; ">
                            <label style="line-height: 18px; padding-top: 5px;"><nb>Suche</nb>:</label>
                            <%: Html.TextBoxFor(model => model.SearchTerm, new {style="width:297px;", id="txtSearch"}) %>
                            <a class="btn btn-default" style="height: 18px;" id="btnSearch"><img alt="" src="/Images/Buttons/tick.png" style="height: 18px;"/></a>
                        </div>
                        <div style="clear:both;"></div>
                    </div>

                    <div class="box-content">
                        
                        <div>
                            <% Html.Message(Model.Message); %>
                        </div>

                        <% foreach(var row in Model.Rows){
                            Html.RenderPartial("UserRow", row);
                        } %>
    
                    </div>
                    <% Html.RenderPartial("Pager", Model.Pager); %>
                </div>
            </div>
    <% } %>
    </div>
</asp:Content>
