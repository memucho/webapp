using System;
using NHibernate;
using Seedworks.Lib.Persistence;

public class JobHistoryRepo : RepositoryDb<JobHistory>
{
    public JobHistoryRepo(ISession session) : base(session)
    {
    }

    public void AddUpdateAnswerAggregates(DateTime startedAt)
    {
        var jobHistoryEntry = new JobHistory
        {
            StartedAt = startedAt,
            FinishedAt = DateTime.Now,
            JobHistoryType = JobHistoryType.UpdateAnswerAggregates
        };

        Create(jobHistoryEntry);
    }
}