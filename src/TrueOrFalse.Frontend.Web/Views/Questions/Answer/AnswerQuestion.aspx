﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Core.Web" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <script src="<%= Url.Content("~/Views/Questions/Answer/AnswerQuestion.js") %>" type="text/javascript"></script>
    
    <style type="text/css">
        
        .selectorShowAnswer{/* marker class */}
        
        .btnRight{float: right; margin-right: 149px;}
        
        div.headerLinks {}
        div.headerLinks i { margin-top: 2px;}
        
        .questionBlockWidth { width: 400px;}
    </style>
    
    <script type="text/javascript">
        var ajaxUrl_SendAnswer = "<%= Model.AjaxUrl_SendAnswer(Url) %>";
        var ajaxUrl_GetAnswer = "<%= Model.AjaxUrl_GetAnswer(Url) %>";
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row" style="min-height: 400px;">
        <div class="span7 ">
            
            <div class="row" style="padding-bottom: 20px;">
                <div style="float:left"><h2>Frage beantworten</h2></div>
                <div class="pull-right headerLinks" style="padding-top:12px;">
                    <a href="<%= Url.Action(Links.Questions, Links.QuestionsController) %>"><i class="icon-th-list"></i> zur übersicht</a><br style="line-height: 10px;"/>
                    <a href="<%= Url.Action(Links.EditQuestion, Links.EditQuestionController, new {id = Model.QuestionId}, null) %>"><i class="icon-pencil"></i> bearbeiten</a>                                        
                </div>                    
            </div>
            

            <h3 class="questionBlockWidth row" style="padding-bottom:12px;"><%= Model.QuestionText %></h3>
            
            <div class="row alert alert-info" id="divCorrectAnswer" style="display: none; margin-top:5px; width: 360px;">
                <b>Richtige Antwort:</b>
                <span id="spanCorrectAnswer"></span>
            </div>
            
            <div class="row alert alert-error" id="divWrongAnswer" style="display: none; margin-top:5px; width: 360px;">
                <b>Falsche Antwort </b>
                <a href="#" id="errorTryCount" style="float: right; margin-right: -30px;">(zwei Versuche)</a><br/>
                
                <div style="margin-top:5px;" id="answerFeedback">Du könntest es wenigstens probieren!</div>
                
                <div style="margin-top:7px; display: none;" id="divAnswerHistory" >
                    Historie:
                    <ul style="padding-top:5px;" id="ulAnswerHistory">
                    </ul>
                </div>
            </div>
        
            <textarea id="txtAnswer" class="questionBlockWidth row" style="height: 30px;"></textarea>    
            
            <div class="row" >
                <%--<%= Buttons.Submit("Überspringen", inline:true)%>--%>
                <div id="buttons-first-try">
                    <a href="#" class="selectorShowAnswer">Antwort anzeigen</a>
                    <a href="#" id="btnCheck" class="btn btn-primary btnRight">Antworten</a>
                </div>
                <div id="buttons-next-answer" style="display: none;">
                    
                    <div class="" id="divAnsweredCorrect" style="display: none; float:left; margin-top:5px; width: 250px;">
                        <b style="color: green;">Richtig!</b> <span id="wellDoneMsg"></span>
                    </div>
                    <a href="#" id="btnNext" class="btn btn-success btnRight">N&auml;chste Frage</a>
                </div>
                <div id="buttons-edit-answer" style="display: none;">
                    <a href="#" class="selectorShowAnswer">Antwort anzeigen</a>
                    <a href="#" id="btnEditAnswer" class="btn btn-warning btnRight">Antwort &Uuml;berarbeiten</a>
                </div>
                <div id="buttons-answer-again" style="display: none">
                    <a href="#" class="selectorShowAnswer">Antwort anzeigen</a>
                    <a href="#" id="btnCheckAgain" class="btn btn-warning btnRight">Nochmal Antworten</a>
                </div>
            </div>

        </div>
        
        <div class="span2" style="padding-top: 90px; padding-left: 20px;">
            Erstellt von: <%= Model.CreatorName %><br />
            vor: <%= Model.CreationDate %> <br />
            <br />
            <%= Model.TimesAnswered %> beantwortet<br />
            <%= Model.TimesAnsweredCorrect %> richtig<br />
            <%= Model.TimesAnsweredWrong %>x falsche<br />
            <%= Model.TimesJumpedOver %> übersprungen<br />
            druchschn. Antwortzeit <%= Model.AverageAnswerTime %><br />            
        </div>
        
    </div>

</asp:Content>
