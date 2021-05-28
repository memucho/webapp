using System;
using System.IO.Pipes;
using System.Linq;
using TrueOrFalse;

public class AnswerQuestion : IRegisterAsInstancePerLifetime
{
    private readonly QuestionRepo _questionRepo;
    private readonly AnswerLog _answerLog;

    public AnswerQuestion(QuestionRepo questionRepo, AnswerLog answerLog)
    {
        _questionRepo = questionRepo;
        _answerLog = answerLog;
    }

    public AnswerQuestionResult Run(
        int questionId, 
        string answer, 
        int userId,
        Guid questionViewGuid,
        int interactionNumber,
        int millisecondsSinceQuestionView)
    {

        return Run(questionId, answer, userId, (question, answerQuestionResult) => {
            _answerLog.Run(question, 
                answerQuestionResult,
                userId, 
                questionViewGuid, 
                interactionNumber,
                millisecondsSinceQuestionView);
        });
    }

    public AnswerQuestionResult Run(
       int questionId,
       string answer,
       int userId,
       Guid questionViewGuid,
       int interactionNumber,
       int millisecondsSinceQuestionView,
       int learningSessionId,
       Guid stepGuid,
       bool inTestMode = false,
        /*for testing*/ DateTime dateCreated = default(DateTime))
    {
        var learningSession = LearningSessionCache.GetLearningSession();

        var result = Run(questionId, answer, userId, (question, answerQuestionResult) =>
        {
            _answerLog.Run(
                question, 
                answerQuestionResult, 
                userId,
                questionViewGuid,
                interactionNumber,
                millisecondsSinceQuestionView,
                dateCreated: dateCreated);
            if (learningSession != null)
            {
                answerQuestionResult.NewStepAdded = learningSession.AddAnswer(answerQuestionResult);
                answerQuestionResult.NumberSteps = learningSession.Steps.Count;
            }
        });
        return result;
    }

    public AnswerQuestionResult Run(
        int questionId, 
        string answer, 
        int userId,
        Guid answerQuestionGuid,
        int interactionNumber,
        int millisecondsSinceQuestionView,
        /*for testing*/ DateTime dateCreated = default(DateTime))
    {
        return Run(questionId, answer, userId, (question, answerQuestionResult) => {
            _answerLog.Run(question, 
                answerQuestionResult, 
                userId, 
                answerQuestionGuid, 
                interactionNumber,
                millisecondsSinceQuestionView,
                dateCreated: dateCreated); 
        });
    }

    public AnswerQuestionResult Run(
        int questionId,
        int userId,
        Guid questionViewGuid,
        int interactionNumber,
        int? testSessionId,
        int? learningSessionId,
        string learningSessionStepGuid,
        int millisecondsSinceQuestionView = -1,
        bool countLastAnswerAsCorrect = false,
        bool countUnansweredAsCorrect = false,
        /*for testing*/ DateTime dateCreated = default(DateTime))
    {
        if(countLastAnswerAsCorrect && countUnansweredAsCorrect)
            throw new Exception("either countLastAnswerAsCorrect OR countUnansweredAsCorrect should be set to true, not both");

        if (countLastAnswerAsCorrect || countUnansweredAsCorrect)
        {
            var learningSession = LearningSessionCache.GetLearningSession();
            learningSession.SetCurrentStepAsCorrect();

            var answer =   Sl.AnswerRepo.GetByQuestionViewGuid(questionViewGuid).OrderByDescending(a => a.Id).First();
            answer.AnswerredCorrectly = AnswerCorrectness.MarkedAsTrue;
            return Run(questionId, "", userId, (question, answerQuestionResult) =>
                    _answerLog.CountLastAnswerAsCorrect(questionViewGuid), countLastAnswerAsCorrect: true);

            // Sl.AnswerRepo.Update(learningSessionStep.AnswerWithInput);
        }
        throw new Exception("neither countLastAnswerAsCorrect or countUnansweredAsCorrect true");
    }

    public AnswerQuestionResult Run(
        int questionId, 
        string answer, 
        int userId,
        Action<Question, AnswerQuestionResult> action,
        bool countLastAnswerAsCorrect = false,
        bool countUnansweredAsCorrect = false)
    {
        var question = _questionRepo.GetById(questionId);
        var solution = GetQuestionSolution.Run(question);

        var result = new AnswerQuestionResult
        {
            IsCorrect = solution.IsCorrect(answer),
            CorrectAnswer = solution.CorrectAnswer(),
            AnswerGiven = answer
            
        };

        action(question, result);

        ProbabilityUpdate_Question.Run(question);
        if (countLastAnswerAsCorrect)
            Sl.R<UpdateQuestionAnswerCount>().ChangeOneWrongAnswerToCorrect(questionId);
        else
            Sl.R<UpdateQuestionAnswerCount>().Run(questionId, countUnansweredAsCorrect || result.IsCorrect);

        ProbabilityUpdate_Valuation.Run(questionId, userId);

        return result;
    }


    public AnswerQuestionResult Run(
        int? learningSessionId,
        string learningSessionStepGuid,
        int questionId,
        string answer,
        int userId,
        Action<Question, AnswerQuestionResult> action,
        bool countLastAnswerAsCorrect = false,
        bool countUnansweredAsCorrect = false)
    {
        var question = _questionRepo.GetById(questionId);
        var solution = GetQuestionSolution.Run(question);

        var result = new AnswerQuestionResult
        {
            IsCorrect = solution.IsCorrect(answer),
            CorrectAnswer = solution.CorrectAnswer(),
            AnswerGiven = answer,
            LearningSessionId = learningSessionId,
            LearningSessionStepGuid = learningSessionStepGuid,
        };

        action(question, result);

        ProbabilityUpdate_Question.Run(question);
        if (countLastAnswerAsCorrect)
            Sl.R<UpdateQuestionAnswerCount>().ChangeOneWrongAnswerToCorrect(questionId);
        else
            Sl.R<UpdateQuestionAnswerCount>().Run(questionId, countUnansweredAsCorrect || result.IsCorrect);

        ProbabilityUpdate_Valuation.Run(questionId, userId);

        return result;
    }
}