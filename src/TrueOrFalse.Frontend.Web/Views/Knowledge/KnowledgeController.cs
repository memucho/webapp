using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;


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
    public JsonResult GetCatsWish(int page, int per_page, string sort = "", bool isAuthor = false)
    {
        var itemCountPerPage = per_page; 
        var categoryDataWishKnowledge = new KnowledgeTopics(isAuthor);
        var unsortedItems = categoryDataWishKnowledge.FilteredCategoryWishKnowledge(ControllerContext);
        var sortedItems = categoryDataWishKnowledge.SortList(unsortedItems, sort).ToList();
        var data = GetPageForPagination(sortedItems, page, itemCountPerPage);

        var itemCount = sortedItems.Count;
        var last_page = GetLastPage(sortedItems.Count, itemCountPerPage);

        return Json(new { total = itemCount, per_page = itemCountPerPage, current_page = page, last_page, data }, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public JsonResult GetQuestionsWish(int page, int per_page, string sort = "", bool isAuthor = false)
    {
        var itemCountPerPage = per_page;
        var knowledgeQuestions = new KnowledgeQuestions(isAuthor, page, itemCountPerPage, sort);

        return Json(new {
            total = knowledgeQuestions.CountWishQuestions, per_page = itemCountPerPage,
            current_page = page, last_page =  knowledgeQuestions.LastPage,
            data = knowledgeQuestions.TotalWishKnowledgeQuestions
        }, JsonRequestBehavior.AllowGet);
    }

    public static int GetLastPage(int listCount, int itemCountPerPage)
    {
        var pages = listCount / itemCountPerPage;
        var remainingCount = listCount % itemCountPerPage;
        var lastPage = 0;

        if (remainingCount > 0)
        {
            lastPage = pages + 1;
            return lastPage;
        }

        lastPage = pages;
        return lastPage;
    }

    public static IList<T> GetPageForPagination<T>(IList<T> sortList, int page, int itemCountPerPage)
    {
        return sortList.Skip((page - 1) * itemCountPerPage).Take(itemCountPerPage).ToList();
    }
}