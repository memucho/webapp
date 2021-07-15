using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Util;
using TrueOrFalse.Web;

public class StatisticsModel : BaseModel
{
    private readonly ISession _session;
    private readonly int _memuchoId = 26;

    public UIMessage Message;

    public DateTime Since = DateTime.Now.AddDays(-31);
    public DateTime SinceGoLive = new DateTime(2016,10,11);

    public IList<AmountPerDay> UsersTotalPerDay;
    public IList<AmountPerDay> UsersNewlyRegisteredPerDay;

    public IList<QuestionsCreatedPerDayResult> QuestionsCreatedPerDayResults;
    public IList<QuestionsCreatedPerDayResult> QuestionsExistingPerDayResults;

    public IList<UsageStatisticsOfLoggedInUsersResult> UsageStats;


    public StatisticsModel()
    {
        _session = Sl.R<ISession>();

        var users = _session
            .QueryOver<User>()
            .OrderBy(u => u.DateCreated).Asc
            .List();
        UsersNewlyRegisteredPerDay = users
            .Where(u => u.DateCreated.Date >= Since.Date)
            .GroupBy(u => u.DateCreated.Date)
            .Select(r => new AmountPerDay
            {
                DateTime = r.Key,
                Value = r.Count()
            })
            .ToList();
        UsersNewlyRegisteredPerDay = AmountPerDay.FillUpDatesWithZeros(UsersNewlyRegisteredPerDay.ToList(), Since, DateTime.Now);

        UsersTotalPerDay = AmountPerDay.FillUpDatesWithZeros(new List<AmountPerDay>(),SinceGoLive,DateTime.Now);
        UsersTotalPerDay.ForEach(d => d.Value = users.Count(u => u.DateCreated.Date <= d.DateTime.Date));


        var questionCountSoFarMemucho = _session
            .QueryOver<Question>()
            .Where(q => q.Creator.Id == _memuchoId)
            .And(q => q.DateCreated.Date < SinceGoLive)
            .List()
            .Count;
        var questionCountSoFarOthers = _session
            .QueryOver<Question>()
            .Where(q => q.Creator.Id != _memuchoId)
            .And(q => q.DateCreated.Date < SinceGoLive)
            .List()
            .Count;

        QuestionsCreatedPerDayResults = _session
            .QueryOver<Question>()
            .Where(q => q.DateCreated.Date >= SinceGoLive)
            .List()
            .GroupBy(q => q.DateCreated.Date)
            .Select(r => new QuestionsCreatedPerDayResult
            {
                DateTime = r.Key,
                CountByMemucho = r.Count(q => new UserTinyModel(q.Creator).Id == _memuchoId),
                CountByOthers = r.Count(q => new UserTinyModel(q.Creator).Id != _memuchoId)
            })
            .ToList();

        QuestionsExistingPerDayResults = new List<QuestionsCreatedPerDayResult>();
        QuestionsCreatedPerDayResults
            .OrderBy(q => q.DateTime)
            .ForEach(q =>
                {
                    questionCountSoFarMemucho += q.CountByMemucho;
                    questionCountSoFarOthers += q.CountByOthers;
                    QuestionsExistingPerDayResults.Add(new QuestionsCreatedPerDayResult
                    {
                        DateTime = q.DateTime,
                        CountByMemucho = questionCountSoFarMemucho,
                        CountByOthers = questionCountSoFarOthers
                    });
                });


        QuestionsCreatedPerDayResults = QuestionsCreatedPerDayResult.FillUpListWithZeros(QuestionsCreatedPerDayResults.Where(d => d.DateTime.Date >= Since.Date).ToList(), Since, DateTime.Now);
        if (QuestionsExistingPerDayResults.Last().DateTime.Date < DateTime.Now.Date) //add entry for current day at the end so that statistic doesn't stop at last day
            QuestionsExistingPerDayResults.Add(new QuestionsCreatedPerDayResult
            {
                DateTime = DateTime.Now.Date,
                CountByMemucho = QuestionsExistingPerDayResults.Last().CountByMemucho,
                CountByOthers = QuestionsExistingPerDayResults.Last().CountByOthers
            });

        /* Usage Stats */
        FillUsageStatistics();

        
    }

    public void FillUsageStatistics()
    {
        var excludedUserIds = _session
            .QueryOver<User>()
            .Where(u => u.IsInstallationAdmin)
            .Select(u => u.Id)
            .List<int>();
        excludedUserIds.Add(-1);

        /* Part 1: How often did a logged in user (answer a questions | create a date | ...) on each day */

        var questionsAnsweredCount = _session
            .QueryOver<Answer>()
            //.Where(Restrictions.Not(Restrictions.In("UserId", excludedUserIds.ToList())))
            .WhereRestrictionOn(v => v.UserId).Not.IsIn((ICollection)excludedUserIds)
            .And(a => a.DateCreated.Date >= Since.Date)
            .And(a => a.AnswerredCorrectly != AnswerCorrectness.IsView)
            .List()
            .GroupBy(a => a.DateCreated.Date)
            .Select(r => new AmountPerDay
            {
                DateTime = r.Key,
                Value = r.Count()
            })
            .ToList();

        var questionsViewedCount = _session
            .QueryOver<QuestionView>()
            .WhereRestrictionOn(v => v.UserId).Not.IsIn((ICollection)excludedUserIds)
            .And(v => v.DateCreated.Date >= Since.Date)
            .List()
            .GroupBy(v => v.DateCreated.Date)
            .Select(r => new AmountPerDay
            {
                DateTime = r.Key,
                Value = r.Count()
            })
            .ToList();

        /* Part 2: How many different users have (answered Questions | created date | ...) on each day */
        // to check plausibility: "SELECT * from questionview Where UserId NOT IN (-1, 2, 25, 26, 33, 72, 75, 77) AND DateCreated > '2016-12-18';"

        var usersThatAnsweredQuestionCount = new List<AmountPerDay>();
        _session.QueryOver<Answer>()
                .WhereRestrictionOn(a => a.UserId).Not.IsIn((ICollection)excludedUserIds)
                .And(a => a.DateCreated.Date >= Since.Date)
                .List()
                .GroupBy(a => a.DateCreated.Date)
                .ForEach(u =>
                {
                    usersThatAnsweredQuestionCount.Add(new AmountPerDay
                    {
                        DateTime = u.Key,
                        Value = u.Select(s => s.UserId).Distinct().Count()
                    });
                });

        var usersThatViewedQuestionCount = new List<AmountPerDay>();
        _session.QueryOver<QuestionView>()
                .WhereRestrictionOn(v => v.UserId).Not.IsIn((ICollection)excludedUserIds)
                .And(v => v.DateCreated.Date >= Since.Date)
                .List()
                .GroupBy(v => v.DateCreated.Date)
                .ForEach(u =>
                {
                    usersThatViewedQuestionCount.Add(new AmountPerDay
                    {
                        DateTime = u.Key,
                        Value = u.Select(s => s.UserId).Distinct().Count()
                    });
                });


        /* merge single usage statistics into condensed list */

        UsageStats = new List<UsageStatisticsOfLoggedInUsersResult>();
        var curDay = Since.Date;
        while (curDay <= DateTime.Now.Date)
        {
            UsageStats.Add(new UsageStatisticsOfLoggedInUsersResult
            {
                DateTime = curDay,
                QuestionsAnsweredCount = (questionsAnsweredCount.Find(i => i.DateTime == curDay) == null) ? 0 : questionsAnsweredCount.Find(i => i.DateTime == curDay).Value,
                QuestionsViewedCount = (questionsViewedCount.Find(i => i.DateTime == curDay) == null) ? 0 : questionsViewedCount.Find(i => i.DateTime == curDay).Value,

                UsersThatAnsweredQuestionCount = (usersThatAnsweredQuestionCount.Find(i => i.DateTime == curDay) == null) ? 0 : usersThatAnsweredQuestionCount.Find(i => i.DateTime == curDay).Value,
                UsersThatViewedQuestionCount = (usersThatViewedQuestionCount.Find(i => i.DateTime == curDay) == null) ? 0 : usersThatViewedQuestionCount.Find(i => i.DateTime == curDay).Value,
            });
            curDay = curDay.AddDays(1);
        }
    }

}
