using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NHibernate.Util;
using TrueOrFalse.Frontend.Web.Code;

public class SetController : BaseController
{
    [SetMainMenu(MainMenuEntry.CategoryDetail)]
    [SetThemeMenu(true)]
    public void QuestionSet(string text, int id)
    {
        var category = Sl.CategoryRepo.GetBySetId(id);
        var categoryChanges = Sl.CategoryChangeRepo.GetForCategory(category.Id);
        var isDeleted = categoryChanges.Any(c => c.Category == category && c.Type == CategoryChangeType.Delete);
        if (isDeleted)
        {
            var baseSetId = Sl.SetRepo.GetById(id).CopiedFrom.Id;
            category = Sl.CategoryRepo.GetBySetId(baseSetId);
        }
        Response.Redirect(Links.CategoryDetailLearningTab(category));
    }

    public void QuestionSetById(int id)
    {
        QuestionSet("", id);
    }

    private void QuestionSet(Set set)
    {
        Response.Redirect(Links.CategoryDetail(Sl.CategoryRepo.GetBySetId(set.Id)));
    }

    [RedirectToErrorPage_IfNotLoggedIn]
    public ActionResult StartLearningSession(int setId)
    {
        return Redirect(Links.CategoryDetailLearningTab(Sl.CategoryRepo.GetBySetId(setId)));
    }

    public ActionResult StartTestSession(int setId)
    {
        return Redirect(Links.CategoryDetailLearningTab(Sl.CategoryRepo.GetBySetId(setId)));
    }

}