﻿using System;
using System.Linq;

public class UpdateAnswerAggregates
{
    /// <summary>
    /// Considers all answers ever given
    /// </summary>
    public static void FullUpadte()
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

                var totalPerUser = Sl.R<TotalsPersUserLoader>().Run(user.Id, questionId);

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

    /// <summary>
    /// Considers all recent updates
    /// </summary>
    public static void Update()
    {
        //get answers aggregated

        var allAnswersAggregated = Sl.AnswerAggregatedRepo.GetAll();

        foreach (var answerAggregated in allAnswersAggregated)
        {
            
        }

        //get all answer of the last 

        //get all answer from logged in users in this period ... 
        //update records accordingly
    }
}