using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;

namespace TrueOrFalse
{
    public class ProbabilityUpdate_Valuation
    {
        public static void Run(int userId)
        {
            Sl.QuestionValuationRepo
                .GetByUser(userId, onlyActiveKnowledge:false)
                .ForEach(Run);
        }

        private static void Run(QuestionValuation questionValuation)
        {
            UpdateValuationProbabilitys(questionValuation);

            Sl.QuestionValuationRepo.CreateOrUpdate(questionValuation);

            //Logg.r().Information("Calculated probability in {elapsed} for question {questionId} and user {userId}: ", sp.Elapsed, question.Id, user.Id);
        }

        public static void Run(IEnumerable<Set> sets, int userId)
        {
            sets
                .SelectMany(s => s.QuestionsInSet.Select(qis => qis.Question.Id))
                .ForEach(questionId => Run(questionId, userId));
        }

        public static void Run(int questionId, int userId)
        {
            if (userId == -1)
                return;

            Run(Sl.QuestionRepo.GetByIdFromMemoryCache(questionId), Sl.UserRepo.GetById(userId));
        }

        public static void Run(Question question, User user, SaveType saveType = SaveType.CacheAndDatabase)
        {
            var questionValuation =
                Sl.QuestionValuationRepo.GetBy(question.Id, user.Id) ??
                    new QuestionValuation
                    {
                        Question = question, 
                        User = user
                    };

            Run(questionValuation, saveType);
        }

        private static void Run(QuestionValuation questionValuation, SaveType saveType)
        {
            UpdateValuationProbabilitys(questionValuation);

            Sl.QuestionValuationRepo.CreateOrUpdateBySaveType(questionValuation, saveType);

            //Logg.r().Information("Calculated probability in {elapsed} for question {questionId} and user {userId}: ", sp.Elapsed, question.Id, user.Id);
        }

        private static void UpdateValuationProbabilitys(QuestionValuation questionValuation)
        {
            var question = questionValuation.Question;
            var user = questionValuation.User;

            var probabilityResult = Sl.R<ProbabilityCalc_Simple1>().Run(question, user);
            questionValuation.CorrectnessProbability = probabilityResult.Probability;
            questionValuation.CorrectnessProbabilityAnswerCount = probabilityResult.AnswerCount;
            questionValuation.KnowledgeStatus = probabilityResult.KnowledgeStatus;
        }
    }
}