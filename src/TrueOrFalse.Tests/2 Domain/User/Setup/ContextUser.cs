using System.Collections.Generic;
using NHibernate;
using NHibernate.Event;

public class ContextUser
{
    private readonly UserRepo _userRepo;

    public List<User> All = new List<User>();

    private ContextUser()
    {
        _userRepo = Sl.R<UserRepo>();
    }

    public static ContextUser New()
    {
        return new ContextUser();
    }

    public static User GetUser(string userName = "Firstname Lastname")
    {
        return New().Add(userName).Persist().All[0];
    }

    public ContextUser Add(string userName)
    {
        All.Add(new User { Name = userName });
        return this;
    }

    public ContextUser Add(User user)
    {
        All.Add(user);
        return this;
    }

    public ContextUser Persist()
    {
        foreach (var usr in All)
            _userRepo.Create(usr);

        _userRepo.Flush();

        return this;
    }
}