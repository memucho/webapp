﻿<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<% if(Model.CountReferences > 0 || Model.TopQuestionsInSubCats.Count > 0) { %>
    <h4>Verwandte Inhalte</h4>
    <div id="Content" class="Box">
         <% if(Model.CountReferences > 0) { %>
            <h5 class="ContentSubheading Question">Fragen mit diesem Medium als Quellenangabe (<%=Model.CountReferences %>)</h5>
            <div class="LabelList">
                <% var index = 0; foreach(var question in Model.TopQuestionsWithReferences){ index++;%>
                    <div class="LabelItem LabelItem-Question">
                        <a href="<%= Links.AnswerQuestion(question, paramElementOnPage: index, categoryFilter:Model.Name) %>" rel="nofollow"><%= question.GetShortTitle(150) %></a>
                    </div>
                <% } %>
            </div>
        <% } %>
            
        <% if(Model.TopQuestionsInSubCats.Count > 0){ %>
            <h5 class="ContentSubheading Question">Fragen in untergeordneten Themen</h5>
            <div class="LabelList">
            <% var index = 0; foreach(var question in Model.TopQuestionsInSubCats){ index++;%>
                <div class="LabelItem LabelItem-Question">
                    <a href="<%= Links.AnswerQuestion(question) %>"><%= question.GetShortTitle(150) %></a>
                </div>
            <% } %>
            </div>
        <% } %>
    </div>
<% } %>
