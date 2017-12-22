using NHibernate;

public class QuestionFeatureRepo : RepositoryDbBase<QuestionFeature>
{
    public QuestionFeatureRepo(ISession session) : base(session)
    {
    }

    public void InsertRelation(int featureId, int questionId)
    {
        _session.CreateSQLQuery(
            $@"INSERT INTO `questionfeature_to_question` 
                (`QuestionFeature_id`, `Question_id`) 
              VALUES 
                ({featureId}, {questionId});").ExecuteUpdate();
    }

    public void TruncateTables()
    {
        _session.CreateSQLQuery("truncate table questionfeature").ExecuteUpdate();
        _session.CreateSQLQuery("truncate table questionfeature_to_question").ExecuteUpdate();
    }

    public int GetFeatureCount(int featureId)
    {
        return Session.QueryOver<QuestionFeature>()
            .Where(x => x.Id == featureId)
            .JoinQueryOver(x => x.Questions)
            .RowCount();
    }
}