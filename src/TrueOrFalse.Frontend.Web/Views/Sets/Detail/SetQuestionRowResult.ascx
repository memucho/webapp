﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SetQuestionRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="rowBase question-row" style="padding-top: 10px; padding-bottom: 10px;">
    <div class="col-lg-1 col-xs-2 col-0" style="padding-right: 0px;">
        <div class="ImageContainer ShortLicenseLinkText">
        <%= GetQuestionImageFrontendData.Run(Model.Question).RenderHtmlImageBasis(128, true, ImageType.Question, linkToItem: Links.AnswerQuestion(Url, Model.Question, Model.Set)) %>
        </div>                       
    </div>
    <div class="col-lg-9 col-sm-7 col-xs-10 col-1">
        <div class="Pin" data-question-id="<%= Model.Question.Id %>" style="float: right; display: inline-block">
            <a href="#" class="noTextdecoration" style="font-size: 16px; height: 10px; position: relative; top: 3px; padding-right: 7px; padding-left: 7px;">
                <i class="fa fa-heart show-tooltip iAdded <%= Model.IsInWishknowledge ? "" : "hide2" %>" style="color:#b13a48;" title="Aus deinem Wunschwissen entfernen"></i>
                <i class="fa fa-heart-o show-tooltip iAddedNot <%= Model.IsInWishknowledge ? "hide2" : "" %>" style="color:#b13a48;" title="Zu deinem Wunschwissen hinzuzufügen"></i>
                <i class="fa fa-spinner fa-spin hide2 iAddSpinner" style="color:#b13a48;"></i>
            </a>
        </div>                        
        <a href="<%= Links.AnswerQuestion(Url, Model.Question, Model.Set) %>" style="font-weight:normal; font-size:17px;">
            <%=Model.Question.Text %>
        </a>
    </div>
    <div class="Stats col-lg-2 col-sm-3 col-xs-10 col-2" style="padding-top: 5px; line-height: 15px;">
        <% Html.RenderPartial("HistoryAndProbability", Model.HistoryAndProbability); %>
    </div>
</div>