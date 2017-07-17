using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Security;
using Seedworks.Web.State;
using TrueOrFalse.Utilities.ScheduledJobs;

public class SessionUser : SessionBase, IRegisterAsInstancePerLifetime
{
    public bool HasBetaAccess
    {
        get => Data.Get("isBetaLogin", false);
        set => Data["isBetaLogin"] = value;
    }

    public bool IsLoggedIn
    {
        get => Data.Get("isLoggedIn", false);
        private set => Data["isLoggedIn"] = value;
    }

    public bool IsInstallationAdmin
    {
        get => Data.Get("isAdministrativeLogin", false);
        set => Data["isAdministrativeLogin"] = value;
    } 

    public User User
    {
        get => Data.Get<User>("user");
        private set => Data["user"] = value;
    }

    public bool IsLoggedInUser(int userId)
    {
        if (!IsLoggedIn)
            return false;

        return userId == User.Id;
    }

    public bool IsLoggedInUserOrAdmin(int userId)
    {
        return IsLoggedInUser(userId) || IsInstallationAdmin;
    }

    public void Login(User user)
    {
        HasBetaAccess = true;
        IsLoggedIn = true;
        User = user;

        if (user.IsInstallationAdmin)
            IsInstallationAdmin = true;

        if(HttpContext.Current != null)
            FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);

        JobScheduler.StartImmediately_InitUserValuationCache(user.Id);
    }

    public void Logout()
    {
        IsLoggedIn = false;
        IsInstallationAdmin = false;
        User = null;
        if (HttpContext.Current != null)
            FormsAuthentication.SignOut();
    }

    public void UpdateUser()
    {
        User = Sl.Resolve<UserRepo>().GetById(Sl.SessionUser.UserId);
    }

    public int UserId
    {
        get
        {
            if (IsLoggedIn)
                return User.Id;

            return -1;
        }
    }

    public List<TestSession> TestSessions
    {
        get { return Data.Get<List<TestSession>>("testSessions"); }
        set { Data["testSessions"] = value; }
    }

    public TestSessionStep GetPreviousTestSessionStep(int testSessionId) =>
        GetCurrentTestSessionStep(testSessionId, offset: -1);

    public TestSessionStep GetCurrentTestSessionStep(int testSessionId, int offset = 0)
    {
        var currentStepIndex = TestSessions.Find(s => s.Id == testSessionId).CurrentStepIndex - 1 + offset;

        return TestSessions
            .Find(s => s.Id == testSessionId)
            .Steps.ElementAt(currentStepIndex);
    }

    private int _currentTestSessionId
    {
        get { return Data.Get("_currentTestSessionId", 0); }
        set { Data["_currentTestSessionId"] = value; }
    }

    public int GetNextTestSessionId()
    {
        lock ("6F33CE8C-F40E-4E7D-85D8-5C025AD98F87")
        {
            var currentSessionId = _currentTestSessionId;
            currentSessionId++;
            _currentTestSessionId = currentSessionId;
            return currentSessionId;
        }
    }

    public void AddTestSession(TestSession testSession)
    {
        if (testSession.NumberOfSteps == 0)
            throw new Exception("Cannot start TestSession from set with no questions.");

        TestSessions.Add(testSession);
    }

    public List<int> AnsweredQuestionIds
    {
        get { return Data.Get<List<int>>("answeredQuestionIds"); }
        set { Data["answeredQuestionIds"] = value; }
    }

    public SessionUser()
    {
        if (AnsweredQuestionIds == null)
            AnsweredQuestionIds = new List<int>();

        if (TestSessions == null)
            TestSessions = new List<TestSession>();
    }

    public List<ActivityPoints> ActivityPoints => Data.Get("pointActivitys", new List<ActivityPoints>());

    public void AddPointActivity(ActivityPoints activityPoints)
    {
        ActivityPoints.Add(activityPoints);
    }

    public int getTotalActivityPoints()
    {
        int totalPoints = 0;
        foreach (var activity in ActivityPoints)
        {
            totalPoints += activity.Amount;
        }

        return totalPoints;
    }
}