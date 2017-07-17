using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Seedworks.Lib.Persistence;
using static System.String;

[Serializable]
[DebuggerDisplay("Id={Id} Name={Name}")]
public class User : DomainEntity
{
    public virtual string PasswordHashedAndSalted { get; set; }
    public virtual string Salt { get; set; }
    public virtual string EmailAddress { get; set; }
    public virtual string Name { get; set; }
    public virtual bool IsEmailConfirmed { get; set;  }
    public virtual bool IsInstallationAdmin { get; set; }
    public virtual DateTime? Birthday { get; set; }
    public virtual int Reputation { get; set; }
    public virtual int ReputationPos { get; set; }
    public virtual int WishCountQuestions { get; set; }
    public virtual int WishCountSets { get; set; }
    public virtual bool ShowWishKnowledge { get; set; }
    public virtual bool AllowsSupportiveLogin { get; set; }
    public virtual UserSettingNotificationInterval KnowledgeReportInterval { get; set; }
    public virtual IList<Membership> MembershipPeriods { get; set; }

    public virtual int CorrectnessProbability { get; set; }
    public virtual int CorrectnessProbabilityAnswerCount { get; set; }

    public virtual IList<FollowerInfo> Followers { get; set; }
    /// <summary>Users I follow</summary>
    public virtual IList<FollowerInfo> Following { get; set; }

    public virtual string FacebookId { get; set; }
    public virtual string GoogleId { get; set; }

    public virtual int ActivityPoints { get; set; }
    public virtual int ActivityLevel { get; set; }

    public virtual bool IsMemuchoUser => Settings.MemuchoUserId == Id;

    public virtual void AddFollower(User follower)
    {
        Followers.Add(new FollowerInfo {Follower = follower, User = this, DateCreated = DateTime.Now, DateModified = DateTime.Now});
        Sl.R<UserRepo>().Flush();
        UserActivityAdd.FollowedUser(follower, this);
        UserActivityUpdate.NewFollower(follower, this);
        ReputationUpdate.ForUser(this);
    }
    public virtual IList<int> FollowerIds(){
        return Followers.Select(f => f.Follower.Id).ToList();
    }

    public virtual IList<int> FollowingIds(){
        return Following.Select(f => f.User.Id).ToList();
    }

    /// <summary>Joined list of FollowerIds and FollowingIds</summary>
    public virtual IList<int> NetworkIds(){
        return FollowerIds().Union(FollowingIds()).ToList();
    }

    public User()
    {
        MembershipPeriods = new List<Membership>();
        Followers = new List<FollowerInfo>();
        Following = new List<FollowerInfo>();
    }

    public virtual bool IsMember()
    {
        if (MembershipPeriods.Count == 0)
            return false;

        return MembershipPeriods.Any(x => x.IsActive(DateTime.Now));
    }

    public virtual Membership CurrentMembership() => MembershipPeriods.FirstOrDefault(x => x.IsActive());

    public virtual bool IsFacebookUser() => !IsNullOrEmpty(FacebookId);

    public virtual bool IsGoogleUser() => !IsNullOrEmpty(GoogleId);
}

public class FacebookUserCreateParameter
{
    /// <summary>
    /// Facebook user id
    /// </summary>
    public string id { get; set; }

    public string name { get; set; }
    public string email { get; set; }
}

public class GoogleUserCreateParameter
{
    public string GoogleId { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string ProfileImage { get; set; }
}