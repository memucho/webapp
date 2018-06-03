using System;
using System.Linq;

public class UpdateAnswerAggregates
{
    public static void Run()
    {
        Logg.r().Information("UpdateAnswerAggregates");

        var users = Sl.UserRepo.GetAll();
        var answerAggregatedRepo = Sl.AnswerAggregatedRepo;

        var allAggregatedEntries = answerAggregatedRepo.GetAll();

        foreach (var user in users)
        {
            Logg.r().Information("UpdateAnswerAggregates: Start user {0}", user.Id);

            var allAnswersByQuestion = Sl.AnswerRepo
                .GetByUser(user.Id)
                .GroupBy(answer => answer.Question.Id);

            foreach (var answersByQuestion in allAnswersByQuestion)
            {
                var questionId = answersByQuestion.Key;

                var entryByQuestionAndUserId = 
                    allAggregatedEntries.FirstOrDefault(x => x.QuestionId == questionId && x.UserId == user.Id);

                var totalPerUserLoader = Sl.R<TotalsPersUserLoader>();
                var totalPerUser = totalPerUserLoader.Run(user.Id, questionId);

                if (entryByQuestionAndUserId == null)
                {
                    var answerAggregated = new AnswerAggregated();

                    answerAggregated.LastUpdated = DateTime.Now;
                    answerAggregated.UserId = user.Id;
                    answerAggregated.QuestionId = questionId;

                    answerAggregated.TotalFalse = totalPerUser.TotalFalse;
                    answerAggregated.TotalTrue = totalPerUser.TotalTrue;

                    answerAggregatedRepo.Create(answerAggregated);
                }
                else
                {
                    entryByQuestionAndUserId.LastUpdated = DateTime.Now;

                    entryByQuestionAndUserId.TotalFalse = totalPerUser.TotalFalse;
                    entryByQuestionAndUserId.TotalTrue = totalPerUser.TotalTrue;
                    answerAggregatedRepo.Update(entryByQuestionAndUserId);
                }
            }
        }
    }
}