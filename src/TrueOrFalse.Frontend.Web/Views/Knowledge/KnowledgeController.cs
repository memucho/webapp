﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

[SetUserMenu(UserMenuEntry.Knowledge)]
public class KnowledgeController : BaseController
{
    [SetMainMenu(MainMenuEntry.Knowledge)]
    public ActionResult Knowledge()
    {
        return View(new KnowledgeModel());
    }

    [SetMainMenu(MainMenuEntry.Knowledge)]
    public ActionResult EmailConfirmation(string emailKey)
    {
        return View("Knowledge", new KnowledgeModel(emailKey: emailKey));
    }

    public int GetNumberOfWishknowledgeQuestions()
    {
        if (_sessionUser.User != null)
        {
            return Resolve<GetWishQuestionCountCached>().Run(_sessionUser.User.Id, true);
        }

        return -1;
    }

    [RedirectToErrorPage_IfNotLoggedIn]
    public ActionResult StartLearningSession()
    {
        var user = _sessionUser.User;
        if (user.WishCountQuestions == 0)
            throw new Exception("Cannot start LearningSession from Wishknowledge with no questions.");

        var valuations = Sl.QuestionValuationRepo
            .GetByUserFromCache(user.Id)
            .QuestionIds().ToList();
        var wishQuestions = Resolve<QuestionRepo>().GetByIds(valuations);

        // if User has uncompleted WishSession that is less than 3 hours old, then continue this one. Else: Start new one
        var openWishSession = Sl.R<LearningSessionRepo>().GetLastWishSessionIfUncompleted(user);

        if (openWishSession != null)
        {
            if (DateTime.Now - openWishSession.DateModified < new TimeSpan(0, 5, 0))
                return Redirect(Links.LearningSession(openWishSession));
            openWishSession.CompleteSession();
        }

        var learningSession = new LearningSession
        {
            IsWishSession = true,
            Steps = GetLearningSessionSteps.Run(wishQuestions),
            User = user
        };

        R<LearningSessionRepo>().Create(learningSession);

        return Redirect(Links.LearningSession(learningSession));
    }

    public String GetKnowledgeContent(string content)
    {
        switch (content)
        {
            case "dashboard":
                return ViewRenderer.RenderPartialView("~/Views/Knowledge/Partials/_Dashboard.ascx",
                    new KnowledgeModel(), ControllerContext);
            case "topics":
                return ViewRenderer.RenderPartialView("~/Views/Knowledge/Partials/KnowledgeTopics.ascx",
                    new KnowledgeModel(), ControllerContext);
            case "questions":
                return ViewRenderer.RenderPartialView("~/Views/Knowledge/Partials/KnowledgeQuestions.ascx",
                    new KnowledgeModel(), ControllerContext);
            case "events":
                return ViewRenderer.RenderPartialView("~/Views/Knowledge/Dates/Dates.ascx", new DatesModel(),
                    ControllerContext);
            default: throw new ArgumentException("Argument false or null");
        }
    }

    public int GetDatesCount(string userId)
    {
        var Dates = R<DateRepo>().GetBy(Int32.Parse(userId), true);
        return Dates.Count - 1; // if last date is deleted counter is still 1  
        //after deleting, however, there is no longer an appointment
    }

    [HttpGet]
    public JsonResult GetCatsAndSetsWish(int page, int per_page, string sort = "", bool isAuthor = false)
    {
        var categoryAndSetDataWishKnowledge = new CategoryAndSetDataWishKnowledge();
        var unsort = categoryAndSetDataWishKnowledge.filteredCategoryWishKnowledge(ControllerContext);
        var sortList = categoryAndSetDataWishKnowledge.SortList(unsort, sort, isAuthor).ToList();
        var data = GetSiteForPagination(sortList, page, per_page);

        var total = sortList.Count;
        var last_page = getLastPage(sortList.Count, per_page);

        return Json(new { total, per_page, current_page = page, last_page, data }, JsonRequestBehavior.AllowGet);
    }


    // Was hast Du Depp Dir bei der Methode gedacht ? Biste Doof ? 
    //[HttpPost]
    //public string CountedWUWItoCategoryAndSet(bool isAuthor = false)
    //{
    //    var count = 0;
    //    var unsortList = categoryAndSetDataWishKnowledge.filteredCategoryWishKnowledge(ControllerContext);
    //    if (isAuthor)
    //        count = (categoryAndSetDataWishKnowledge.SortList(unsortList, "name|asc", isAuthor).Count);
    //    else
    //        count = (unsortList.Count);
                        
    //    if (count == 1)
    //        return "Du hast " + count + " Topic oder Set in deinem Wunschwissen";
    //    if (count == 0)
    //        return "Du hast noch keine Topics oder Sets in deinem Wunschwissen";

    //    return "Du hast " + count + " Topics und/oder Sets in deinem Wunschwissen";
    //}

    [HttpGet]
    public JsonResult GetQuestionsWish(int page, int per_page, string sort = "", bool isAuthor = false)
    {
        var knowledgeQuestions = new KnowledgeQuestions();
        var totalWishKnowledgeValuations = isAuthor
            ? UserValuationCache.GetQuestionValuations(UserId)
                .Where(v => v.Question.Creator.Id == UserId)
                .Distinct()
                .ToList()
           : UserValuationCache.GetQuestionValuations(UserId)
                .Distinct()
                .ToList();

        totalWishKnowledgeValuations = GetSiteForPagination(totalWishKnowledgeValuations, page, per_page).ToList();
        var unsortedListWithIsAuthor = knowledgeQuestions.GetQuestionsWishFromDatabase(UserId, isAuthor, totalWishKnowledgeValuations);

        var data = knowledgeQuestions.GetSortList(unsortedListWithIsAuthor, sort);
        var total = totalWishKnowledgeValuations.Count;
        var last_page = getLastPage(totalWishKnowledgeValuations.Count, per_page);

        return Json(new { total, per_page, current_page = page, last_page, data }, JsonRequestBehavior.AllowGet);
    }

    private int getLastPage(int listCount, int perPage)
    {
        var pages = listCount / perPage;
        var rest = listCount % perPage;
        var lastPage = 0;

        if (rest > 0)
        {
            lastPage = pages + 1;
            return lastPage;
        }

        lastPage = pages;
        return lastPage;
    }

    public static IList<T> GetSiteForPagination<T>(IList<T> sortList, int page, int per_page)
    {
        return sortList.Skip((page - 1) * per_page).Take(per_page).ToList();
    }
}