using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using NHibernate;

public class CreateLearningSession
{
    public static LearningSession ForCategory(int categoryId)
    {
        var category = Sl.CategoryRepo.GetByIdEager(categoryId);

        var questions = category.GetAggregatedQuestionsFromMemoryCache();

        if (questions.Count == 0)
            throw new Exception("Cannot start LearningSession with 0 questions.");

        var user = Sl.R<SessionUser>().User;

        var learningSession = new LearningSession
        {
            CategoryToLearn = category,
            Steps = GetLearningSessionSteps.Run(questions),
            User = user
        };

        Sl.LearningSessionRepo.Create(learningSession);

        return learningSession;
    }

    public static LearningSession ForCategory(int categoryId, bool testingMode)
    {
        if (testingMode == false)
            return ForCategory(categoryId); //todo Christof: Combine with above ForCategory


        var category = Sl.CategoryRepo.GetByIdEager(categoryId);

        var questions = category.GetAggregatedQuestionsFromMemoryCache();

        if (questions.Count == 0)
            throw new Exception("Cannot start LearningSession with 0 questions.");

        var user = Sl.R<SessionUser>().User;

        var learningSession = new LearningSession
        {
            CategoryToLearn = category,
            Steps = GetLearningSessionSteps.Run(questions),
            User = user,
            Settings = new LearningSessionSettings
            {
                AmountQuestions = 6,
                LearningSessionQuestionSelection = LearningSessionQuestionSelection.AllQuestions,
                LearningSessionType = LearningSessionType.Testing
            }
        };

        Sl.LearningSessionRepo.Create(learningSession);

        return learningSession;
    }

}
