using FluentNHibernate.Mapping;

public class JobHistoryMap : ClassMap<JobHistory>
{
    public JobHistoryMap()
    {
        Id(x => x.Id);
        Map(x => x.JobHistoryType);
    }
}