using FluentNHibernate.Mapping;

public class UserMap : ClassMap<User>
{
    public UserMap()
    {
        Id(x => x.Id);
        Map(x => x.PasswordHashedAndSalted);
        Map(x => x.Salt);
        Map(x => x.EmailAddress);
        Map(x => x.Name);
        Map(x => x.IsEmailConfirmed);
        Map(x => x.IsInstallationAdmin);
        Map(x => x.AllowsSupportiveLogin);
        Map(x => x.ShowWishKnowledge);
        Map(x => x.KnowledgeReportInterval);

        Map(x => x.CorrectnessProbability);
        Map(x => x.CorrectnessProbabilityAnswerCount);

        HasMany(x => x.MembershipPeriods)
            .Cascade.All().Not.LazyLoad();

        HasMany(x => x.Followers)
            .KeyColumn("User_id")
            .Cascade.All()
            .LazyLoad();

        HasMany(x => x.Following)
            .KeyColumn("Follower_id")
            .Cascade.All()
            .LazyLoad();

        Map(x => x.Reputation);
        Map(x => x.ReputationPos);

        Map(x => x.WishCountQuestions);
        Map(x => x.WishCountSets);

        Map(x => x.Birthday);

        Map(x => x.FacebookId);
        Map(x => x.GoogleId);

        Map(x => x.ActivityPoints);
        Map(x => x.ActivityLevel);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}