﻿using System;
using System.Text;
using NHibernate;

namespace TrueOrFalse.Core
{
    public class UpdateQuestionTotals : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionValuationRepository _questionValuationRepository;
        private readonly CreateOrUpdateQuestionValue _createOrUpdateQuestionValue;
        private readonly ISession _session;

        public UpdateQuestionTotals(QuestionValuationRepository questionValuationRepository,
                                    CreateOrUpdateQuestionValue createOrUpdateQuestionValue,
                                    ISession session)
        {
            _questionValuationRepository = questionValuationRepository;
            _createOrUpdateQuestionValue = createOrUpdateQuestionValue;
            _session = session;
        }

        public void Run(QuestionValuation questionValuation)
        {
            _questionValuationRepository.CreateOrUpdate(questionValuation);

            var sb = new StringBuilder();

            sb.Append(GenerateQualityQuery(questionValuation.QuestionId, questionValuation.UserId));
            sb.Append(GenerateRelevanceAllQuery(questionValuation.QuestionId, questionValuation.UserId));
            
            sb.Append(GenerateAvgQuery("TotalRelevancePersonalAvg", "RelevancePersonal", questionValuation.QuestionId, questionValuation.UserId));    
            sb.Append(GenerateEntriesQuery("TotalRelevancePersonalEntries", "RelevancePersonal", questionValuation.QuestionId, questionValuation.UserId));

            _session.CreateSQLQuery(sb.ToString()).ExecuteUpdate();
            _session.Flush();
        }
        
        public void UpdateQuality(int questionId, int userId, int quality)
        {
            _createOrUpdateQuestionValue.Run(questionId, userId, quality:quality);
            _session.CreateSQLQuery(GenerateQualityQuery(questionId, userId)).ExecuteUpdate();
            _session.Flush();
        }

        public void UpdateRelevancePersonal(int questionId, int userId, int relevance)
        {
            _createOrUpdateQuestionValue.Run(questionId, userId, relevancePeronal: relevance);
            _session.CreateSQLQuery(GenerateRelevancePersonal(questionId, userId)).ExecuteUpdate();
            _session.Flush();            
        }

        public void UpdateRelevanceAll(int questionId, int userId, int relevance)
        {
            _createOrUpdateQuestionValue.Run(questionId, userId, relevanceForAll: relevance);
            _session.CreateSQLQuery(GenerateRelevanceAllQuery(questionId, userId)).ExecuteUpdate();
            _session.Flush();            
        }

        private string GenerateQualityQuery(int questionId, int userId)
        {
            return 
             GenerateAvgQuery("TotalQualityAvg", "Quality", questionId, userId) + " " +
             GenerateEntriesQuery("TotalQualityEntries", "Quality", questionId, userId);
        }

        private string GenerateRelevancePersonal(int questionId, int userId)
        {
            return
                GenerateAvgQuery("TotalRelevancePersonalAvg", "RelevancePersonal", questionId, userId) + " " +
                GenerateEntriesQuery("TotalRelevancePersonalEntries", "RelevancePersonal", questionId, userId);
        }

        private string GenerateRelevanceAllQuery(int questionId, int userId)
        {
            return 
                GenerateAvgQuery("TotalRelevanceForAllAvg", "RelevanceForAll", questionId, userId) + " " +
                GenerateEntriesQuery("TotalRelevanceForAllEntries", "RelevanceForAll", questionId, userId);
        }

        private string GenerateAvgQuery(string fieldToSet, string fieldSource, int questionId, int userId)
        {
            return String.Format("UPDATE Question SET {0} = " +
                                   "(SELECT SUM({1}) FROM QuestionValuation " +
	                               "WHERE UserId = " + userId + " AND QuestionId = " + questionId + " AND {2} != -1) " +
                                 "WHERE Id = "+ questionId + ";", fieldToSet, fieldSource, fieldSource);
        }

        private string GenerateEntriesQuery(string fieldToSet, string fieldSource, int questionId, int userId)
        {
            return String.Format("UPDATE Question SET {0} = " +
                                   "(SELECT COUNT(Id) FROM QuestionValuation " +
                                   "WHERE UserId = " + userId + " AND QuestionId = " + questionId + " AND {2} != -1) " +
                                 "WHERE Id = "+ questionId + ";", fieldToSet, fieldSource, fieldSource);
        }

    }
}
