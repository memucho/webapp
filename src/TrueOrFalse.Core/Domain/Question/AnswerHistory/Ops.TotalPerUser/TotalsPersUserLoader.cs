﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Transform;

namespace TrueOrFalse.Core
{
    public class TotalsPersUserLoader : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;

        public TotalsPersUserLoader(ISession session){
            _session = session;
        }

        public IList<TotalPerUser> Run(int userId, IList<Question> questions)
        {
            return Run(userId, questions.GetIds());
        }

        public IList<TotalPerUser> Run(int userId, IEnumerable<int> questionIds)
        {
            if (!questionIds.Any())
                return new TotalPerUser[]{};

            var sbQuestionIdRestriction = new StringBuilder();

            var firstHit = true;
            foreach (var questionId in questionIds)
                if(firstHit)
                {
                    sbQuestionIdRestriction.AppendLine("AND QuestionId = " + questionId);
                    firstHit = false;
                }
                else
                    sbQuestionIdRestriction.AppendLine("OR QuestionId = " + questionId);
            
            var query = String.Format(
                @"SELECT 
	                  QuestionId, 
	                  COUNT(QuestionId) -SUM(CAST(AnswerredCorrectly as Integer)) as TotalFalse, 
	                  SUM(CAST(AnswerredCorrectly as Integer)) as TotalTrue
                  FROM AnswerHistory
                  GROUP BY QuestionId, UserId
                  HAVING UserId = {0} 
                  {1}", userId, sbQuestionIdRestriction);

            return _session.CreateSQLQuery(query).
                            SetResultTransformer(Transformers.AliasToBean(typeof(TotalPerUser))).
                            List<TotalPerUser>();
        }
    }
}
