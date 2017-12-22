using System.Diagnostics;
using NHibernate.Util;

public class ProbabilityUpdate_Question : IRegisterAsInstancePerLifetime
{
    public static void Run()
    {
        var sp = Stopwatch.StartNew();

        foreach (var question in Sl.R<QuestionRepo>().GetAll())
            Run(question);

        Logg.r().Information("Calculated all question probabilities in {elapsed} ", sp.Elapsed);
    }

    public static void Run(Question question)
    {
        var answers = Sl.AnswerRepo.GetByQuestion(question.Id);

        question.CorrectnessProbability = ProbabilityCalc_Question.Run(answers);
        question.CorrectnessProbabilityAnswerCount = answers.Count;

        Sl.QuestionRepo.UpdateFieldsOnly(question);

        question.Categories.ForEach(c => KnowledgeSummaryUpdate.ScheduleForCategory(c.Id));
    }
}