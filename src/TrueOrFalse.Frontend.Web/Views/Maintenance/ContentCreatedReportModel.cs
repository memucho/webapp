using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Web;

public class ContentCreatedReportModel : BaseModel
{
    private ISession _session;

    public UIMessage Message;

    public DateTime Since = DateTime.Now.AddDays(-7);

    public IList<Question> RecentQuestionsAddedMemucho;
    public IList<Question> RecentQuestionsAddedNotMemucho;
    public IList<Category> CategoriesAdded;
    public IEnumerable<IGrouping<Category, CategoryChange>> CategoriesChanged;

    public ContentCreatedReportModel()
    {
        _session = Sl.R<ISession>();

        RecentQuestionsAddedMemucho = _session
            .QueryOver<Question>()
            .OrderBy(q => q.DateCreated).Desc
            .Where(q => q.Creator.Id == 26)
            .And(q => q.DateCreated > Since)
            .List();

        RecentQuestionsAddedNotMemucho = _session
            .QueryOver<Question>()
            .OrderBy(q => q.DateCreated).Desc
            .Where(q => q.Creator.Id != 26)
            .And(q => q.DateCreated > Since)
            .List();

        CategoriesAdded = _session
            .QueryOver<Category>()
            .OrderBy(c => c.DateCreated).Desc
            .Where(c => c.DateCreated > Since)
            .List();

        CategoriesChanged = _session
            .QueryOver<CategoryChange>()
            .OrderBy(c => c.DateCreated).Desc
            .Where(c => c.DateCreated > Since)
            .List()
            .GroupBy(c => c.Category);
    }

}