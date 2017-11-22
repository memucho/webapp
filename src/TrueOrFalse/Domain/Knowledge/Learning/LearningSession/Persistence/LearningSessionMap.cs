using FluentNHibernate.Mapping;

public class LearningSessionMap : ClassMap<LearningSession>
{
    public LearningSessionMap()
    {
        Id(x => x.Id);

        Map(x => x.StepsJson).CustomSqlType("varchar(8000)");

        References(x => x.User);

        References(x => x.SetToLearn);
        Map(x => x.SetsToLearnIdsString);
        Map(x => x.SetListTitle);
        References(x => x.CategoryToLearn);
        References(x => x.DateToLearn);

        Map(x => x.IsWishSession);

        Component(x => x.Settings, settings =>
        {
            settings.Map(x => x.LearningSessionType).Column("SettingLearningSessionType");
        });

        Map(x => x.IsCompleted);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}