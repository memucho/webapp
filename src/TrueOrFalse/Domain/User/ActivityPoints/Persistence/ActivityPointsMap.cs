using FluentNHibernate.Mapping;

public class ActivityPointsMap : ClassMap<ActivityPoints>
{
    public ActivityPointsMap()
    {
        Id(x => x.Id);
        Map(x => x.Amount);
        Map(x => x.DateEarned);
        References(x => x.User);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}