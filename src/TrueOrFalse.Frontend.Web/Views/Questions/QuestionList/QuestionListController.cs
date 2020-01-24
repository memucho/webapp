﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using MarkdownSharp;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using RabbitMQ.Client.Framing.Impl;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

public class QuestionListController : BaseController
{
    [HttpPost]
    public JsonResult LoadQuestions(int categoryId, int itemCount, int pageNumber)
    {
        var newQuestionList = QuestionListModel.PopulateQuestionsOnPage(categoryId, pageNumber, itemCount, IsLoggedIn);
        return Json(newQuestionList);
    }

    [HttpPost]
    public JsonResult LoadQuestionBody(int questionId)
    {
        var question = EntityCache.GetQuestionById(questionId);
        var author = new UserTinyModel(question.Creator);
        var authorImage = new UserImageSettings(author.Id).GetUrl_128px_square(author);
        var valuationForUser = Resolve<TotalsPersUserLoader>().Run(_sessionUser.UserId, questionId);
        var solution = GetQuestionSolution.Run(question);

        var extendedQuestion = MarkdownToHtml.RepairImgTag(MarkdownInit.Run().Transform(question.TextExtended));
        var newExtendedQuestion = MarkdownToHtml.RepairImgTag(extendedQuestion);

        var extendedAnswer = MarkdownToHtml.RepairImgTag(MarkdownInit.Run().Transform(question.Description));
        var newExtendedAnswer = MarkdownToHtml.RepairImgTag(extendedAnswer);

        var json = Json(new
        {
            answer = solution.CorrectAnswer(),
            extendedAnswer = newExtendedAnswer,
            categories = question.Categories.Select(c => new
            {
                name = c.Name,
                categoryType = c.Type,
                linkToCategory = Links.CategoryDetail(c),
            }),
            references = question.References.Select(r => new
            {
                referenceType = r.ReferenceType.GetName(),
                additionalInfo = r.AdditionalInfo ?? "",
                referenceText = r.ReferenceText ?? ""
            }),
            author = author.Name,
            authorId = author.Id,
            authorImage = authorImage.Url,
            authorUrl = Links.UserDetail(author),
            extendedQuestion = newExtendedQuestion,
            commentCount = Resolve<CommentRepository>().GetForDisplay(question.Id)
                .Where(c => !c.IsSettled)
                .Select(c => new CommentModel(c))
                .ToList()
                .Count(),
            isCreator = author.Id == _sessionUser.UserId,
            editUrl = Links.EditQuestion(Url, question.Text, question.Id),
            historyUrl = Links.QuestionHistory(question.Id)
        });

        return json;
    }

    [HttpPost]
    public string RenderWishknowledgePinButton(bool isInWishknowledge)
    {
        return ViewRenderer.RenderPartialView("~/Views/Shared/AddToWishknowledgeButtonQuestionDetail.ascx", new AddToWishknowledge(isInWishknowledge, true), ControllerContext);
    }
}