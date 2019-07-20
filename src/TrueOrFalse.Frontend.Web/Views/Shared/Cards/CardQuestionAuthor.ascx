<%--<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewUserControl<CardQuestionAuthorModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% if (Model.Authors.Count == 1)
    {
        var author = Model.Authors.First();

%>
<input id="isFollow" type="hidden" value="<%= Model.DoIFollow %>" />
<input id="author" type="hidden" value="<%= author.User.Id %>" name="<%= author.User.Name %>" data-question-created="<%= Model.Reputation.ForQuestionsCreated %>" data-sets-created="<%= author.User.Name %>" data-question-created="<%= Model.Reputation.ForSetsCreated %>" />

<div id="AutorCard" style="width: 267px">
    <div class="column-left">
        <div class="ImageContainer">
            <div class="card-image-large" style="background: url(<%= author.ImageUrl %>) center;"></div>
        </div>
    </div>
    <div class="column-right">
        <div class="card-title">
            <span>Erstellt von:</span>
        </div>
        <div id="card-link" class="card-link">
            <a href="<%= Links.UserDetail(author.User) %>">
                <%= author.Name %> 
            </a>
            <i id="followIcon" class="fas follower"></i>
        </div>
        <div class="author-reputation">
            <span>Reputation:</span>
            <br />
            <span><%= author.Reputation %> Punkte (Rang <%= author.ReputationPos %>)</span>
        </div>
    </div>
    <div class="autor-card-footer-bar">
        <div class="show-tooltip" title='<%= Model.Reputation.User.Name %> hat <%= Model.Reputation.ForQuestionsCreated %> Fragen / <%= Model.Reputation.ForSetsCreated %> Lernsets erstellt.'>
            <i class="fa fa fa-question-circle"></i>
            <span class="footer-bar-text"><%= Model.Reputation.ForQuestionsCreated %></span>
        </div>
        <div class="show-tooltip" title="<% if (Model.Reputation.User.ShowWishKnowledge)
            { %> <%= author.Name %> hat sein/ihr Wunschwissen veröffentlicht und <%= Model.AmountWishCountQuestions %> Fragen gesammelt <% }
            else
            { %>  <%= author.Name %> hat sein Wunschwissen leider nicht veröffentlicht. <% } %> ">
            <span class="fa fa-heart"></span>
            <span class="footer-bar-text"><%= Model.AmountWishCountQuestions %></span>
        </div>
        <div id="follow-tooltip" data-allowed="logged-in" class="show-tooltip " <% if (!Model.IsCurrentUser)
            { %>title="<% if (Model.DoIFollow)
            { %>Du folgst <%= author.Name %> und nimmst an ihren/seinen Aktivitäten teil.<% }
            else
            { %>Folge <%= author.Name %>, um an ihren/seinen Aktivitäten teilzuhaben.<% } %>"
            <% } %>>
            <div id="follower" class="fas follower"></div>
            <span class="footer-bar-text"><%= Model.Reputation.ForUsersFollowingMe %></span>
        </div>
    </div>
</div>
<% }
   if (Model.Authors.Count != 1)
   {%>
    <div id="MultipleAutorCard">
        <div class="card-title">
            <%= Model.Authors.Count %>
            <span>Beitragende</span>
        </div>
        <div class="autor-container">
            <% 
                foreach (var author in Model.Authors.Take(3))
                { %>
                <div class="single-autor-container">
                    <div class="multiple-autor-card-image">
                        <img class="ItemImage JS-InitImage" alt="" src="<%= author.ImageUrl %>" data-append-image-link-to="ImageContainer" />
                    </div>
                    <a href="<%= Links.UserDetail(author.User)%>" class="card-link"><%= author.Name %></a>
                </div>
            <%  } %>
        </div>

        <%if (Model.Authors.Count > 3)
          { %>
            <div id="AllAutorsContainer" class="card-link cursor-hand">
                <span style="align-self: center;">weitere Beitragende <i id="ExtendAngle" style="color: #979797;" class="fa fa-angle-right"></i></span>
            </div>
            <div id="AllAutorsList" style="display: none; margin-bottom: 19px" class="card-link">
                <% foreach (var author in Model.Authors.Skip(3))
                   {%>
                    <a href="<%= Links.UserDetail(author.User)%>" style="display: unset; font-size: 14px;" class="card-link"><%= author.Name %></a>
                <%} %>
            </div>
        <% } %>
    </div>
<%} %>--%>
