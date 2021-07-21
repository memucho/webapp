using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

public class CategoryChangesOverviewModel : BaseModel
{
    public IList<CategoryChangeDayModel> Days;
    public int PageToShow;
    public CategoryChangesOverviewModel(int pageToShow)
    {
        PageToShow = pageToShow;
        const int revisionsToShow = 100;
        var revisionsToSkip = (PageToShow - 1) * revisionsToShow;
        var query = $@"SELECT * FROM CategoryChange cc ORDER BY cc.DateCreated DESC LIMIT {revisionsToSkip},{revisionsToShow}";
        var revisions = Sl.R<ISession>().CreateSQLQuery(query).AddEntity(typeof(CategoryChange)).List<CategoryChange>();

        
        Days = revisions
            .GroupBy(change => change.DateCreated.Date)
            .OrderByDescending(group => group.Key)
            .Select(group => new CategoryChangeDayModel(
                group.Key,
                (IList<CategoryChange>)group.OrderByDescending(g => g.DateCreated).ToList()
            ))
            .ToList();
    }
}