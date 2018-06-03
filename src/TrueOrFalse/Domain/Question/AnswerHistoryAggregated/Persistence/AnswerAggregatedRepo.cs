using NHibernate;
using Seedworks.Lib.Persistence;

public class AnswerAggregatedRepo : RepositoryDb<AnswerAggregated> 
{
    public AnswerAggregatedRepo(ISession session) : base(session){}

}