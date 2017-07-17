using System.Collections.Generic;
using System.Linq;
using NHibernate;
using TrueOrFalse.Search;

public class UserRepo : RepositoryDbBase<User>
{
    private readonly SearchIndexUser _searchIndexUser;

    public UserRepo(ISession session, SearchIndexUser searchIndexUser): base(session){
        _searchIndexUser = searchIndexUser;
    }

    public User GetByEmail(string emailAddress) => 
        _session.QueryOver<User>()
            .Where(k => k.EmailAddress == emailAddress)
            .SingleOrDefault<User>();

    public User GetByName(string name) => 
        _session.QueryOver<User>()
            .Where(k => k.Name == name)
            .SingleOrDefault<User>();

    public User UserGetByFacebookId(string facebookId) => 
        _session
            .QueryOver<User>()
            .Where(u => u.FacebookId == facebookId)
            .SingleOrDefault();

    public User UserGetByGoogleId(string googleId) =>
        _session
            .QueryOver<User>()
            .Where(u => u.GoogleId == googleId)
            .SingleOrDefault();

    public bool FacebookUserExists(string facebookId) => 
        _session
            .QueryOver<User>()
            .Where(u => u.FacebookId == facebookId)
            .RowCount() == 1;

    public bool GoogleUserExists(string googleId) =>
        _session
            .QueryOver<User>()
            .Where(u => u.GoogleId == googleId)
            .RowCount() == 1;

    public void RemoveFollowerInfo(FollowerInfo followerInfo)
    {
        _session
            .CreateSQLQuery("DELETE FROM user_to_follower WHERE Id =" + followerInfo.Id)
            .ExecuteUpdate();
        _session.Flush();
        ReputationUpdate.ForUser(followerInfo.User);
    }

    public override void Update(User user)
    {
        this.Update(user, false);
    }

    public void Update(User user, bool runSolrUpdateAsync = false)
    {
        _searchIndexUser.Update(user, runSolrUpdateAsync);
        base.Update(user);
    }

    public override void Create(User user)
    {
        base.Create(user);
        _searchIndexUser.Update(user);
    }

    public override void Delete(int id)
    {
        var user = GetById(id);

        if (Sl.R<SessionUser>().IsLoggedInUserOrAdmin(user.Id))
            throw new InvalidAccessException();

        _searchIndexUser.Delete(user);
        base.Delete(id);
    }

    public IList<User> GetByIds(List<int> userIds) => GetByIds(userIds.ToArray());

    public override IList<User> GetByIds(params int[] userIds)
    {
        var questions = base.GetByIds(userIds);
        return userIds.Select(t => questions.First(q => q.Id == t)).ToList();
    }

    public IList<User> GetWhereReputationIsBetween(int newReputation, int oldRepution)
    {
        var query = _session.QueryOver<User>();

        if(newReputation < oldRepution)
            query.Where(u => u.Reputation > newReputation && u.Reputation < oldRepution);
        else
            query.Where(u => u.Reputation < newReputation && u.Reputation > oldRepution);
       
        return query.List();
    }

    public void UpdateActivityPointsData()
    {
        var totalPointCount = 0;
        foreach (var activityPoints in Sl.ActivityPointsRepo.GetActivtyPointsByUser(Sl.CurrentUserId))
        {
            totalPointCount += activityPoints.Amount;
        }

        var userLevel = UserLevelCalculator.GetLevel(totalPointCount);

        var user = GetByIds(Sl.SessionUser.UserId).First();
        user.ActivityPoints = totalPointCount;
        user.ActivityLevel = userLevel;
        Update(user);
    }
}