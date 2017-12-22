using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class AddValuationEntries_ForQuestionsInSetsAndDates : IRegisterAsInstancePerLifetime
{
    public void RunForAllUsers()
    {
        var stopwatch = new Stopwatch();

        var users = Sl.R<UserRepo>().GetAll();
        foreach (var user in users)
            Run(user);

        Logg.r().Information("Users {0}", users.Count);

        Logg.r().Information(
            "AddProbabilitiesEntries_ForSetsAndDates all users: duration {0}", 
            stopwatch.Elapsed);
    }

    public void Run(User user)
    {
        var dates = Sl.R<DateRepo>().GetBy(user.Id);
        var setsFromDates = dates.SelectMany(d => d.Sets);

        var setsValuatedIds = Sl.R<SetValuationRepo>().GetByUser(user.Id).Select(v => v.SetId);
        var setsValuated = Sl.R<SetRepo>().GetByIds(setsValuatedIds.ToArray());
        
        var distinctSets = setsValuated.Union(setsFromDates, new SetComparerIdEquals());
        
        var allQuestionIds = distinctSets.SelectMany(d => d.QuestionIds()).Distinct().ToList();

        var valuations = Sl.QuestionValuationRepo.GetByQuestionsAndUserFromCache(allQuestionIds, user.Id);
        var notValuatedIds = allQuestionIds.Except(valuations.QuestionIds());
        AddValuationEntries(user, notValuatedIds);
    }

    public void Run(Set set, User user)
    {
        Run(new List<Set>{set}, user);
    }

    public void Run(IEnumerable<Set> sets, User user)
    {
        var questionIds = sets.SelectMany(set => set.QuestionIds()).Distinct().ToList();
        var valuations = Sl.R<QuestionValuationRepo>().GetByQuestionsAndUserFromCache(questionIds, user.Id);

        var notValuatedIds = questionIds.Except(valuations.QuestionIds());
        AddValuationEntries(user, notValuatedIds);
    }

    private static void AddValuationEntries(User user, IEnumerable<int> notValuatedQuestionIds)
    {
        var notValuatedQuestions = Sl.R<QuestionRepo>()
            .GetByIds(notValuatedQuestionIds.ToArray());

        var questionValuationRepo = Sl.R<QuestionValuationRepo>();

        foreach (var question in notValuatedQuestions)
        {
            questionValuationRepo.Create(new QuestionValuation
            {
                RelevancePersonal = -1,
                User = user,
                Question = question
            });
        }
    }
}
