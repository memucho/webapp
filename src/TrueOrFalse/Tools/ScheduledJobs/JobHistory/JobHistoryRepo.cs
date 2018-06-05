using System;
using System.Linq;
using NHibernate;
using NHibernate.Transform;
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
            Type = JobHistoryType.UpdateAnswerAggregates
        };

        Create(jobHistoryEntry);
    }

    public JobHistory GetLastUpdateAnswerAggregates()
    {
        var query = 
            $@"SELECT * FROM answer 
               WHERE Type = {(int)JobHistoryType.UpdateAnswerAggregates}
               ORDER BY Id DESC 
               LIMIT 1";

        return _session.CreateSQLQuery(query)
            .SetResultTransformer(Transformers.AliasToBean(typeof(JobHistory)))
            .List<JobHistory>()
            .FirstOrDefault();
    }
}