using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class CategoryController : BaseController
{
    private const string _viewLocation = "~/Views/Categories/Detail/Category.aspx";

    [SetMenu(MenuEntry.CategoryDetail)]
    [SetThemeMenu(true)]
    public ActionResult Category(string text, int id)
    {
        if (SeoUtils.HasUnderscores(text))
            return SeoUtils.RedirectToHyphendVersion_Category(RedirectPermanent, text, id);

        var category = Resolve<CategoryRepository>().GetById(id);
        return Category(category);
    }

    private ActionResult Category(Category category)
    {
        SaveCategoryView.Run(category, User_());

        _sessionUiData.VisitedCategories.Add(new CategoryHistoryItem(category));

        return View(_viewLocation, GetModelWithContentHtml(category));
    }

    private CategoryModel GetModelWithContentHtml(Category category)
    {
        var contentHtml = string.IsNullOrEmpty(category.TopicMarkdown?.Trim())
            ? null
            : MarkdownToHtml.Run(category, ControllerContext);

        return new CategoryModel(category)
        {
            CustomPageHtml = contentHtml
        };
    }

    public void CategoryById(int id)
    {
        Response.Redirect(Links.CategoryDetail(Resolve<CategoryRepository>().GetById(id)));
    }

    public ActionResult StartTestSession(int categoryId)
    {
        var category = Sl.CategoryRepo.GetByIdEager(categoryId);
        var testSession = new TestSession(category);

        Sl.SessionUser.AddTestSession(testSession);

        return Redirect(Links.TestSession(testSession.UriName, testSession.Id));
    }

    public ActionResult StartTestSessionForSetsInCategory(List<int> setIds, string setListTitle, int categoryId)
    {
        var sets = Sl.SetRepo.GetByIds(setIds);
        var category = Sl.CategoryRepo.GetByIdEager(categoryId);
        var testSession = new TestSession(sets, setListTitle, category);

        Sl.SessionUser.AddTestSession(testSession);

        return Redirect(Links.TestSession(testSession.UriName, testSession.Id));
    }

    [RedirectToErrorPage_IfNotLoggedIn]
    public ActionResult StartLearningSession(int categoryId)
    {
        var learningSession = CreateLearningSession.ForCategory(categoryId);

        return Redirect(Links.LearningSession(learningSession));
    }

    [RedirectToErrorPage_IfNotLoggedIn]
    public ActionResult StartLearningSessionForSets(List<int> setIds, string setListTitle)
    {
        var sets = Sl.SetRepo.GetByIds(setIds);
        var questions = sets.SelectMany(s => s.Questions()).Distinct().ToList();

        if (questions.Count == 0)
            throw new Exception("Cannot start LearningSession with 0 questions.");

        var learningSession = new LearningSession
        {
            SetsToLearnIdsString = string.Join(",", sets.Select(x => x.Id.ToString())),
            SetListTitle = setListTitle,
            Steps = GetLearningSessionSteps.Run(questions),
            User = _sessionUser.User
        };

        R<LearningSessionRepo>().Create(learningSession);

        return Redirect(Links.LearningSession(learningSession));
    }

    public string Tab(string tabName, int categoryId)
    {
        return ViewRenderer.RenderPartialView(
            "/Views/Categories/Detail/Tabs/" + tabName + ".ascx",
            GetModelWithContentHtml(Sl.CategoryRepo.GetById(categoryId)),
            ControllerContext
        );
    }

    public string KnowledgeBar(int categoryId) => 
        ViewRenderer.RenderPartialView(
            "/Views/Categories/Detail/CategoryKnowledgeBar.ascx",
            new CategoryKnowledgeBarModel(Sl.CategoryRepo.GetById(categoryId)), 
            ControllerContext
        );
}