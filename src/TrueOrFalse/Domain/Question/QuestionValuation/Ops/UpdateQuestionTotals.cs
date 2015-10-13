﻿using System.Text;
using NHibernate;

public class UpdateQuestionTotals : IRegisterAsInstancePerLifetime
{
    private readonly QuestionValuationRepo _questionValuationRepo;
    private readonly CreateOrUpdateQuestionValue _createOrUpdateQuestionValue;
    private readonly ISession _session;
    private readonly ReputationUpdate _reputationUpdate;

    public UpdateQuestionTotals(QuestionValuationRepo questionValuationRepo,
                                CreateOrUpdateQuestionValue createOrUpdateQuestionValue,
                                ISession session,
                                ReputationUpdate reputationUpdate)
    {
        _questionValuationRepo = questionValuationRepo;
        _createOrUpdateQuestionValue = createOrUpdateQuestionValue;
        _session = session;
        _reputationUpdate = reputationUpdate;
    }

    public void Run(QuestionValuation questionValuation)
    {
        _questionValuationRepo.CreateOrUpdate(questionValuation);

        var sb = new StringBuilder();

        sb.Append(GenerateQualityQuery(questionValuation.Question.Id));
        sb.Append(GenerateRelevanceAllQuery(questionValuation.Question.Id));

        sb.Append(GenerateEntriesQuery("TotalRelevancePersonal", "RelevancePersonal", questionValuation.Question.Id));
        sb.Append(GenerateAvgQuery("TotalRelevancePersonal", "RelevancePersonal", questionValuation.Question.Id));    
            
        _session.CreateSQLQuery(sb.ToString()).ExecuteUpdate();
        _session.Flush();
    }
        
    public void UpdateQuality(int questionId, int userId, int quality)
    {
        _createOrUpdateQuestionValue.Run(questionId, userId, quality: quality);
        _session.CreateSQLQuery(GenerateQualityQuery(questionId)).ExecuteUpdate();
        _session.Flush();
    }

    public void UpdateRelevancePersonal(int questionId, User user, int relevance = 50)
    {
        _createOrUpdateQuestionValue.Run(questionId, user.Id, relevancePersonal: relevance);
        _session.CreateSQLQuery(GenerateRelevancePersonal(questionId)).ExecuteUpdate();
        _session.Flush();

        _reputationUpdate.ForQuestion(questionId);
    }

    public void UpdateRelevanceAll(int questionId, int userId, int relevance)
    {
        _createOrUpdateQuestionValue.Run(questionId, userId, relevanceForAll: relevance);
        _session.CreateSQLQuery(GenerateRelevanceAllQuery(questionId)).ExecuteUpdate();
        _session.Flush();            
    }

    private string GenerateQualityQuery(int questionId)
    {
        return
            GenerateEntriesQuery("TotalQuality", "Quality", questionId) + " " +
            GenerateAvgQuery("TotalQuality", "Quality", questionId) ;
    }

    private string GenerateRelevancePersonal(int questionId)
    {
        return
            GenerateEntriesQuery("TotalRelevancePersonal", "RelevancePersonal", questionId) + " " +
            GenerateAvgQuery("TotalRelevancePersonal", "RelevancePersonal", questionId);
    }

    private string GenerateRelevanceAllQuery(int questionId)
    {
        return
            GenerateEntriesQuery("TotalRelevanceForAll", "RelevanceForAll", questionId) + " " +
            GenerateAvgQuery("TotalRelevanceForAll", "RelevanceForAll", questionId);
    }

    private string GenerateAvgQuery(string fieldToSet, string fieldSource, int questionId)
    {
        return "UPDATE Question SET " + fieldToSet + "Avg = " +
                    "ROUND((SELECT SUM(" + fieldSource + ") FROM QuestionValuation " +
                    " WHERE QuestionId = " + questionId + " AND " + fieldSource + " != -1)/ " + fieldToSet + "Entries) " +
                "WHERE Id = " + questionId + ";";
    }

    private string GenerateEntriesQuery(string fieldToSet, string fieldSource, int questionId)
    {
        return "UPDATE Question SET " + fieldToSet + "Entries = " +
                    "(SELECT COUNT(Id) FROM QuestionValuation " +
                    "WHERE QuestionId = " + questionId + " AND " + fieldSource + " != -1) " +
                "WHERE Id = " + questionId + ";";
    }
}