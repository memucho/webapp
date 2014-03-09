﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MenuLeftModel>" %>
<%@ Import Namespace="Seedworks.Lib" %>
<%@ Import Namespace="TrueOrFalse" %>
<%@ Import Namespace="TrueOrFalse.Web.Context" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% int index; %>

<% if (Url.RequestContext.RouteData.Values["controller"].ToString() == Links.HelpController) { %>
    <div class="box" style="padding-left: 0px; padding-right: 0;">
        <div class="menu">
            <a href="<%= Url.Action(Links.HelpWillkommen, Links.HelpController) %>">
                <div class="main no-link"><i class="fa fa-caret-right"></i>  Hilfe</div>
            </a>
            
            <% var idx = 0; foreach (var helpPage in new SessionUiData().VisitedHelpPages) { idx++ ;%>
               <div class="sub">
                   <% var activeClass = ""; if (idx == 1) { activeClass = Model.Active(MenuEntry.Help); } %>
                   <a href="<%= Url.Action(helpPage.Text, Links.HelpController) %>" class="show-tooltip <%=activeClass %>" title="" data-placement="right">
                       <i class="fa fa-caret-right"></i> <%=helpPage.Text.Truncate(100)%>
                   </a>
               </div>
            <% } %>
        </div>
    </div>
<% } %>

<div class="mainMenuContainer">
    <nav id="mainMenu">
        <div class="list-group">
            <a class="list-group-item know <%: Model.Active(MenuEntry.Knowledge)%>" href="<%= Url.Action(Links.Knowledge, Links.KnowledgeController) %>">
                <i class="fa fa-caret-right"></i> Wunschwissen (<span id="menuWishKnowledgeCount"><%= Model.WishKnowledgeCount %></span>)
            </a>
            <a class="list-group-item quest <%= Model.Active(MenuEntry.Questions) %>" href="<%= Url.Action("Questions", "Questions") %>">
                <i class="fa fa-caret-right"></i> Fragen
            </a>

            <%  index = 0;
                var visitedQ = new SessionUiData().VisitedQuestions;
                foreach (var question in visitedQ) { 
               
                    index++ ;
                    string activeClass = (index == 1) ? Model.Active(MenuEntry.QuestionDetail) : "";

                    string url = "";
                    if(question.Set != null)
                        url = Links.AnswerQuestion(Url, question.Question, question.Set);
                    else
                        url = Links.AnswerQuestion(Url, question.SearchSpec);

                    string tooltip = "";
                    if (!String.IsNullOrEmpty(question.Text))
                    {
                        tooltip = "Frage: " + question.Text.Replace("\"", "'");
                        if (index != 1 || activeClass != "active")
                            tooltip += " <br><br> Antwort: " + question.Solution.Replace("\"", "'");                           
                    }
                %>
                <a href="<%= url %>" class="list-group-item quest show-tooltip sub <%=activeClass + " " + visitedQ.CssFirst(index) + visitedQ.CssLast(index)%>" title="<%= tooltip %>" data-placement="right" data-html="true">
                    <i class="fa fa-caret-right"></i> <%=question.Text.Truncate(100)%>
                </a>
                   
            <% } %>

            <a class="list-group-item set <%= Model.Active(MenuEntry.QuestionSet) %>" href="<%= Url.Action("Sets", "Sets")%>">
                <i class="fa fa-caret-right"></i> Fragesätze
            </a>    
            <%
                var visitedS = new SessionUiData().VisitedQuestionSets;
                index = 0; 
                foreach (var set in visitedS){ index++; %>
                    <% var activeClass = "";  if (index == 1) { activeClass = Model.Active(MenuEntry.QuestionSetDetail); } %>
                    <a href="<%= Links.SetDetail(Url, set.Name, set.Id) %>" class="show-tooltip list-group-item set sub <%= activeClass + " " + visitedS.CssFirst(index) + visitedS.CssLast(index) %>" title="Fragesatz: <%=set.Name%>" data-placement="right">
                        <i class="fa fa-caret-right"></i> <%=set.Name%>
                    </a>
            <% } %>
        
            <a class="list-group-item dues <%= Model.Active(MenuEntry.Dates) %>" href="<%= Links.Dates(Url) %>">
                <i class="fa fa-caret-right"></i> Termine
            </a>
                    
            <a class="list-group-item cat <%= Model.Active(MenuEntry.Categories) %>" style="margin-top: 10px;" href="<%= Url.Action(Links.Categories, Links.CategoriesController) %>">
                <i class="fa fa-caret-right"></i> Kategorien
            </a>
       
            <% var visitedC = new SessionUiData().VisitedCategories;
               index = 0; 
               foreach (var set in visitedC){ index++; %>
                <% var activeClass = "";  if (index == 1) { activeClass = Model.Active(MenuEntry.CategoryDetail); } %>
                <a href="<%= Links.CategoryDetail( Url, set.Name, set.Id) %>" class="show-tooltip cat sub <%= activeClass + visitedC.CssFirst(index) + visitedC.CssLast(index) %> list-group-item" title="Fragesatz: <%=set.Name%>" data-placement="right">
                    <i class="fa fa-caret-right"></i> <%=set.Name%>
                </a>
            <% } %>

            <a class="list-group-item users <%= Model.Active(MenuEntry.Users) %>" href="<%= Url.Action("Users", "Users")%>">
                <i class="fa fa-caret-right"></i> Nutzer<img src="/images/menu-icon-person.png" style="position: relative; top: -1px; left: 4px;" >
            </a>
            <%
                var visitedU = new SessionUiData().VisitedUserDetails;
                index = 0; 
                foreach (var user in visitedU){ index++;  %>
                <% var activeClass = ""; if (index == 1) { activeClass = Model.Active(MenuEntry.UserDetail); } %>
                <a href="<%= Links.UserDetail(Url, user.Name, user.Id) %>" class="list-group-item users sub <%= activeClass + visitedU.CssFirst(index) + visitedU.CssLast(index) %>">
                    <i class="fa fa-caret-right"></i> <%=user.Name%>
                </a>
            <% } %>
        
            <a class="list-group-item messages <%= Model.Active(MenuEntry.Messages) %>" href="<%= Links.Messages(Url) %>">
                Nachrichten
                <span id="badgeNewMessages" class="badge show-tooltip" title="Ungelesene Nachrichten" style="display:inline-block; position: relative; top: 1px;"><%= Model.UnreadMessageCount %></span>
            </a>
                            
            <% if (Model.IsInstallationAdmin){ %>
                <a class="list-group-item cat" style="margin-top: 10px;" href="<%= Url.Action("Maintenance", "Maintenance") %>">
                    <i class="fa fa-caret-right"></i> Administrativ
                </a>
            <% } %>

            <a class="<%= Model.Active(MenuEntry.Play) %> list-group-item play" href="#" style="margin-top: 10px;">
                <i class="fa fa-caret-right"></i> Spielen
            </a>
        </div>
    </nav>
</div>

<% if(Model.Categories.Any()){ %>
    <div class="box" style="padding-left: 0px; padding-right: 0;">
        <div class="menu">
            <div class="main no-link"><i class="fa fa-caret-right"></i> Kategorien</div>
            
            <% foreach(var catMenuItem in Model.Categories){ %>
                <div class="sub">
                    <a href="<%= Links.QuestionWithCategoryFilter(Url, catMenuItem) %>">(<span><%=catMenuItem.OnPageCount %>x) </span>  <i class="fa fa-caret-right"></i><span class="label label-category"> <%=catMenuItem.Category.Name %> </span></a>
                </div>
            <% } %>
        </div>
    </div>
<% } %>