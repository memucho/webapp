using System;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class KnowledgeController : BaseController
{
    [SetMenu(MenuEntry.Knowledge)]
    public ActionResult Knowledge()
    {
        return View(new KnowledgeModel());
    }

    [SetMenu(MenuEntry.Knowledge)]
    public ActionResult EmailConfirmation(string emailKey)
    {
        return View("Knowledge", new KnowledgeModel(emailKey:emailKey));
    }

    public int GetNumberOfWishknowledgeQuestions()
    {
        if (_sessionUser.User != null)
        {
            return Resolve<GetWishQuestionCountCached>().Run(_sessionUser.User.Id, true);
        }
        else
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
            if (DateTime.Now - openWishSession.DateModified < new TimeSpan(0,5,0))
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
}