using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class SetController : BaseController
{
    private const string _viewLocation = "~/Views/Sets/Detail/Set.aspx";

    [SetMenu(MenuEntry.None)]
    [SetThemeMenu(isQuestionSetPage: true)]
    public ActionResult QuestionSet(string text, int id)
    {
        if (SeoUtils.HasUnderscores(text))
            return SeoUtils.RedirectToHyphendVersion_Set(RedirectPermanent, text, id);

        var set = Resolve<SetRepo>().GetByIdEager(id);
        return QuestionSet(set);
    }

    public void QuestionSetById(int id)
    {
        Response.Redirect(Links.SetDetail(Resolve<SetRepo>().GetById(id)));
    }

    private ActionResult QuestionSet(Set set)
    {
        SaveSetView.Run(set, User_());

        _sessionUiData.VisitedSets.Add(new QuestionSetHistoryItem(set));
        return View(_viewLocation, new SetModel(set));
    }

    public JsonResult GetRows(int id)
    {
        var set = Resolve<SetRepo>().GetById(id);

        var sbHtmlRows = new StringBuilder();
        foreach (var rowModel in set.QuestionsInSet)
            sbHtmlRows.Append(
                ViewRenderer.RenderPartialView(
                    "~/Views/Sets/Detail/SetQuestionRowResult.ascx",
                    rowModel,
                    ControllerContext)
            );

        return Json(new {Html = sbHtmlRows.ToString()});
    }

    [RedirectToErrorPage_IfNotLoggedIn]
    public ActionResult StartLearningSession(int setId)
    {
        var set = Resolve<SetRepo>().GetById(setId);
        if (set.Questions().Count == 0)
            throw new Exception("Cannot start LearningSession from set with no questions.");

        var learningSession = new LearningSession
        {
            SetToLearn = set,
            Steps = GetLearningSessionSteps.Run(set),
            User = _sessionUser.User
        };

        R<LearningSessionRepo>().Create(learningSession);

        return Redirect(Links.LearningSession(learningSession));
    }

    public ActionResult StartTestSession(int setId)
    {
        var set = Sl.SetRepo.GetByIdEager(setId);
        var testSession = new TestSession(set);

        Sl.SessionUser.AddTestSession(testSession);

        return Redirect(Links.TestSession(testSession.UriName, testSession.Id));
    }

    public ActionResult StartTestSessionForSets(List<int> setIds, string setListTitle)
    {
        var sets = Sl.SetRepo.GetByIdsEager(setIds.ToArray());
        var testSession = new TestSession(sets, setListTitle);

        Sl.SessionUser.AddTestSession(testSession);

        return Redirect(Links.TestSession(testSession.UriName, testSession.Id));
    }

    public string ShareSetModal(int setId) =>
        ViewRenderer.RenderPartialView("~/Views/Sets/Detail/Modals/ShareSetModal.ascx", new ShareSetModalModel(setId),
            ControllerContext);

    [HttpPost]
    public JsonResult Copy(int sourceSetId)
    {
        var copiedSetId = R<CopySet>().Run(sourceSetId, UserId);
        var copiedSet = R<SetRepo>().GetById(copiedSetId);
        return new JsonResult
        {
            Data = new
            {
                CopiedSetId = copiedSetId,
                CopiedSetName = copiedSet.Name,
                CopiedSetEditUrl = Links.QuestionSetEdit(copiedSet.Name, copiedSet.Id)
            }
        };
    }

    public string KnowledgeBar(int setId) =>
        ViewRenderer.RenderPartialView(
            "/Views/Sets/Detail/SetKnowledgeBar.ascx",
            new SetKnowledgeBarModel(Sl.SetRepo.GetById(setId)),
            ControllerContext
        );

}