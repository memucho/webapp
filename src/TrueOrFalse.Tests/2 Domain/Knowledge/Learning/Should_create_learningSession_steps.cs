using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TrueOrFalse.Tests;

public class Should_create_learningSession_steps : BaseTest
{
    [Test]
    public void Should_select_and_order_questions_correctly()
    {
        var context = ContextQuestion.New();
        var learner = context.Learner;

        Sl.R<SessionUser>().Login(learner);

        context.PersistImmediately()
            .AddQuestion(questionText: "question not answered 1")
            .AddQuestion(questionText: "question not answered 2")
            .AddQuestion(questionText: "question not answered 3")
            .AddQuestion(questionText: "question answered not today prob 20")
                .AddAnswers(countCorrect: 7, countWrong: 3, dateCreated: DateTime.Now.AddDays(-1))
                .SetProbability(20, learner)
            .AddQuestion(questionText: "question answered not today prob 40")//1
                .AddAnswers(countCorrect: 7, countWrong: 3, dateCreated: DateTime.Now.AddDays(-1))
                .SetProbability(40, learner)
            .AddQuestion(questionText: "question answered not today prob 30")//1
                .AddAnswers(countCorrect: 7, countWrong: 3, dateCreated: DateTime.Now.AddDays(-1))
                .SetProbability(30, learner)
            .AddQuestion(questionText: "question answered today prob 40")
                .AddAnswers(countCorrect: 7, countWrong: 3, dateCreated: DateTime.Now)
                .AddAnswers(countCorrect: 7, countWrong: 3, dateCreated: DateTime.Now.AddDays(-1))
                .SetProbability(40, learner)
            .AddQuestion(questionText: "question answered today prob 30")
                .AddAnswers(countCorrect: 7, countWrong: 3, dateCreated: DateTime.Now)
                .AddAnswers(countCorrect: 7, countWrong: 3, dateCreated: DateTime.Now.AddDays(-1))
                .SetProbability(30, learner)
            .AddQuestion(questionText: "question answered today prob 20")
                .AddAnswers(countCorrect: 7, countWrong: 3, dateCreated: DateTime.Now)
                .AddAnswers(countCorrect: 7, countWrong: 3, dateCreated: DateTime.Now.AddDays(-1))
                .SetProbability(20, learner);

        RecycleContainer();

        const int numberOfSteps = 10;
        var rnd = new Random();
        Console.WriteLine("Original order:" + Environment.NewLine
            + context.All.Select(s => s.Text).Aggregate((current, next) => current + Environment.NewLine + next));

        context.All = context.All.OrderBy(x => rnd.Next()).ToList();
        Console.WriteLine(Environment.NewLine + "Shuffled:" + Environment.NewLine
            + context.All.Select(s => s.Text).Aggregate((current, next) => current + Environment.NewLine + next));
        
        var steps = GetLearningSessionSteps.Run(context.All);

        Console.WriteLine(Environment.NewLine + "Selected steps:" + Environment.NewLine
            + steps.Select(s => s.Question.Text).Aggregate((current, next) => current + Environment.NewLine + next));

        Assert.That(steps[0].Question.Text.StartsWith("question not answered"));
        Assert.That(steps[1].Question.Text.StartsWith("question not answered"));
        Assert.That(steps[2].Question.Text.StartsWith("question not answered"));
        Assert.That(steps[3].Question.Text == "question answered not today prob 20");
        Assert.That(steps[4].Question.Text == "question answered not today prob 30");
        Assert.That(steps[5].Question.Text == "question answered not today prob 40");
        Assert.That(steps[6].Question.Text == "question answered today prob 20");
        Assert.That(steps[7].Question.Text == "question answered today prob 30");
        Assert.That(steps[8].Question.Text == "question answered today prob 40");

        var learningSession = new LearningSession();
        R<LearningSessionRepo>().Create(learningSession);
        learningSession.Steps = steps;
        R<LearningSessionRepo>().Update(learningSession);

        RecycleContainer();

        R<AnswerQuestion>().Run(steps[0].Question.Id, "answer1", -1, Guid.NewGuid(), 1, -1, learningSession.Id, steps[0].Guid);
        R<AnswerQuestion>().Run(steps[1].Question.Id, "answer2", -1, Guid.NewGuid(), 1, -1, learningSession.Id, steps[1].Guid);

        RecycleContainer();

        var learningSessionFromDb = R<LearningSessionRepo>().GetById(learningSession.Id);

        if (IsMysqlInMemoryEngine())
            learningSessionFromDb.Steps = learningSessionFromDb.Steps.OrderBy(s => s.Idx).ToList();

        Assert.That(learningSessionFromDb.Steps[0].AnswerWithInput.AnswerText, Is.EqualTo("answer1"));
        Assert.That(learningSessionFromDb.Steps[1].AnswerWithInput.AnswerText == "answer2");
        Assert.That(learningSessionFromDb.Steps[2].AnswerWithInput == null);
    }
}