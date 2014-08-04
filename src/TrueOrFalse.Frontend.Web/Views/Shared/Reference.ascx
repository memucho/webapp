﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrueOrFalse.Category>" %>
<%@ Import Namespace="System.Web.Razor.Parser.SyntaxTree" %>
<%@ Import Namespace="TrueOrFalse" %>

<%
    object type = Model.GetTypeModel();
    switch (Model.Type)
    {
       case CategoryType.Book:
           var book = (CategoryTypeBook)type;
%>
            <div class="Reference Book">
                <% if (!String.IsNullOrEmpty(book.Author)){
                    var htmlAuthors = book.Author
                        .Split(Environment.NewLine.ToCharArray(),StringSplitOptions.RemoveEmptyEntries)
                        .Aggregate((a, b) => (a + ";&nbsp" + b));%>
                        <div class="Author"><span><%= htmlAuthors %></span></div>
                <% }
                if (!String.IsNullOrEmpty(book.Title)){
                    %><div class="Title"><%
                        if (!String.IsNullOrEmpty(book.Subtitle)){
                        %><span><%= book.Title %> – <%= book.Subtitle %></span><%
                        } else {
                        %><span><%= book.Title %></span><% }
                    %></div><%
                }
                %>
                <%
                if (!String.IsNullOrEmpty(book.PublicationCity) || 
                    !String.IsNullOrEmpty(book.Publisher) || 
                    !String.IsNullOrEmpty(book.PublicationYear)) { %>
                    <div class="PublicationInfo">
                        <%
                        if (!String.IsNullOrEmpty(book.PublicationCity)) {
                            if (!String.IsNullOrEmpty(book.Publisher)) { %>
                                <span class="PublicationCity"><%= book.PublicationCity %>: </span><%
                            } else { %> 
                                <span class="PublicationCity"><%= book.PublicationCity %></span><%
                            }
                        } 
                        if (!String.IsNullOrEmpty(book.Publisher)) { %>
                            <span class="Publisher"><%= book.Publisher %></span><%
                        }
                        if (!String.IsNullOrEmpty(book.PublicationYear)){
                            if (!String.IsNullOrEmpty(book.PublicationCity) ||
                                !String.IsNullOrEmpty(book.Publisher)){
                                %><span class="PublicationYear">, <%= book.PublicationYear %></span><%
                            } else {
                                %><span class="PublicationYear"><%= book.PublicationYear %></span><%
                            }
                        } %>
                    </div>
                <% } %>
                    
                <% if (!String.IsNullOrEmpty(book.ISBN)){
                    %><div class="Isbn"><span>ISBN: <%= book.ISBN %></span></div>       
                <%}
                if (!String.IsNullOrEmpty(Model.WikipediaURL)){
                    %><div class="WikiUrl"><a href="<%= Model.WikipediaURL %>"><span><%= Model.WikipediaURL %></span></a></div><%
                }
                if (!String.IsNullOrEmpty(Model.Description)){
                    %><div class="Description"><span><%= Model.Description %></span></div><%
                } %>
            </div>
<%
           break; 
   
       case CategoryType.Daily:
           var daily = (CategoryTypeDaily)type;
            
%>
           <html></html>
<%
           break; 
%> 




<% } %>
