using System;
using System.Collections.Generic;
using System.Linq;

public class UpdateAnswerAggregates
{
    /// <summary>Updates answer aggregates for the given user</summary>
    public static void FullUpdate(User user) => _FullUpdate(false, user);

    /// <summary>Updates answer aggregates for users</summary>
    public static void FullUpdate(bool forUsersWhoLoggedInAfterPreviousFullUpdate = true) => _FullUpdate(forUsersWhoLoggedInAfterPreviousFullUpdate);

    private static void _FullUpdate(bool forUsersWhoLoggedInAfterPreviousFullUpdate = true, User userToUpdate = null)
    {
        Logg.r().Information("UpdateAnswerAggregates");

        var users = userToUpdate == null ? 
            GetUsers(forUsersWhoLoggedInAfterPreviousFullUpdate) :
            new List<User> { userToUpdate };

        var answerAggregatedRepo = Sl.AnswerAggregatedRepo;

        var allAggregatedEntries = answerAggregatedRepo.GetAll();

        foreach (var user in users)
        {
            Logg.r().Information("UpdateAnswerAggregates: Start user {0}", user.Id);

            var allAnsweredQuestionIds = Get_all_answered_question_ids(user);

            foreach (var questionId in allAnsweredQuestionIds)
            {
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

    private static IEnumerable<int> Get_all_answered_question_ids(User user)
    {
        //potential improvement: consider answer date

        return Sl.AnswerRepo
            .GetByUser(user.Id)
            .GroupBy(answer => answer.Question.Id)
            .Select(answersByQuestion => answersByQuestion.Key);
    }

    private static IList<User> GetUsers(bool forUsersWhoLoggedInAfterPreviousFullUpdate)
    {
        if (forUsersWhoLoggedInAfterPreviousFullUpdate)
            return Get_all_users_who_logged_in_after_the_last_full_update();

        return Sl.UserRepo.GetAll();
    }

    private static IList<User> Get_all_users_who_logged_in_after_the_last_full_update()
    {
        var users = Sl.UserRepo.GetAll();

        var historyEntry = Sl.JobHistoryRepo.GetLastUpdateAnswerAggregates();
        if (historyEntry != null)
        {
            users = users
                .Where(user => user.LastLogin != null && user.LastLogin > historyEntry.FinishedAt)
                .ToList();
        }

        return users;
    }
}