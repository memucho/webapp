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

        var contentHtml = string.IsNullOrEmpty(category.TopicMarkdown?.Trim())
            ? null
            : MarkdownToHtml.Run(category, ControllerContext);

        return View(_viewLocation,
            new CategoryModel(category)
            {
              CustomPageHtml = contentHtml
            });
    }

    public void CategoryById(int id)
    {
        Response.Redirect(Links.CategoryDetail(Resolve<CategoryRepository>().GetById(id)));
    }

    public ActionResult StartTestSession(int categoryId)
    {
        var category = Sl.R<CategoryRepository>().GetById(categoryId);
        var testSession = new TestSession(category);

        R<SessionUser>().AddTestSession(testSession);

        return Redirect(Links.TestSession(testSession.UriName, testSession.Id));
    }

    public ActionResult StartTestSessionForSetsInCategory(List<int> setIds, string setListTitle, int categoryId)
    {
        var sets = Sl.R<SetRepo>().GetByIds(setIds);
        var category = Sl.R<CategoryRepository>().GetById(categoryId);
        var testSession = new TestSession(sets, setListTitle, category);

        R<SessionUser>().AddTestSession(testSession);

        return Redirect(Links.TestSession(testSession.UriName, testSession.Id));
    }

    [RedirectToErrorPage_IfNotLoggedIn]
    public ActionResult StartLearningSession(int categoryId)
    {
        var category = Resolve<CategoryRepository>().GetById(categoryId);

        var questions = category.GetAggregatedQuestionsFromMemoryCache();

        if (questions.Count == 0)
            throw new Exception("Cannot start LearningSession with 0 questions.");

        var learningSession = new LearningSession
        {
            CategoryToLearn = category,
            Steps = GetLearningSessionSteps.Run(questions),
            User = _sessionUser.User
        };

        R<LearningSessionRepo>().Create(learningSession);

        return Redirect(Links.LearningSession(learningSession));
    }

    [RedirectToErrorPage_IfNotLoggedIn]
    public ActionResult StartLearningSessionForSets(List<int> setIds, string setListTitle)
    {
        var sets = R<SetRepo>().GetByIds(setIds);
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
}