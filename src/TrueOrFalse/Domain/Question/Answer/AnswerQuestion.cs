using System;
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
        int millisecondsSinceQuestionView,
        int playerId,
        int roundId)
    {
        var player = Sl.R<PlayerRepo>().GetById(playerId);
        var round = Sl.R<RoundRepo>().GetById(roundId);

        return Run(questionId, answer, userId, (question, answerQuestionResult) => {
            _answerLog.Run(question, 
                answerQuestionResult,
                userId, 
                questionViewGuid, 
                interactionNumber,
                millisecondsSinceQuestionView,
                player,
                round);
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
        /*for testing*/ DateTime dateCreated = default(DateTime))
    {
        var learningSessionRepo = Sl.R<LearningSessionRepo>();
        var learningSession = learningSessionRepo.GetById(learningSessionId);
        var learningSessionStep = LearningSession.GetStep(learningSessionId, stepGuid);

        var numberOfStepsBeforeAnswer = learningSession.Steps.Count;

        var result = Run(questionId, answer, userId, (question, answerQuestionResult) =>
        {
            _answerLog.Run(
                question, 
                answerQuestionResult, 
                userId,
                questionViewGuid,
                interactionNumber,
                millisecondsSinceQuestionView,
                learningSession: learningSession, 
                learningSessionStepGuid: learningSessionStep.Guid, 
                dateCreated: dateCreated);

            learningSessionStep.AnswerState = StepAnswerState.Answered;
            learningSessionRepo.Update(learningSession);

            if (learningSession.Settings.LearningSessionType == LearningSessionType.Learning && !answerQuestionResult.IsCorrect)
            {
                learningSession.UpdateAfterWrongAnswerOrShowSolution(learningSessionStep);
                answerQuestionResult.NewStepAdded = learningSession.Steps.Count > numberOfStepsBeforeAnswer;
            }

            answerQuestionResult.NumberSteps = learningSession.Steps.Count;
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

        if (testSessionId.HasValue && (countLastAnswerAsCorrect || countUnansweredAsCorrect))
        {
            var currentStep = Sl.SessionUser.GetPreviousTestSessionStep(testSessionId.Value);
            currentStep.AnswerState = TestSessionStepAnswerState.AnsweredCorrect;
        }

        if (learningSessionId.HasValue && (countLastAnswerAsCorrect || countUnansweredAsCorrect))
        {
            var learningSession = Sl.LearningSessionRepo.GetById(learningSessionId.Value);
            var learningSessionStep = learningSession.GetStep(new Guid(learningSessionStepGuid));
            learningSessionStep.AnswerState = StepAnswerState.Answered;

            var duplicateStep = learningSession.Steps.Where(x => x.Question == learningSessionStep.Question &&
                                                                        x.Idx > learningSessionStep.Idx).ToList();
            if (duplicateStep.Count > 1)
                throw new Exception(
                    "There shouldn't be more than one extra unanswered step of the same question in learning session");

            if (duplicateStep.Count > 0)
            {
                learningSession.Steps.Remove(duplicateStep.First());
                learningSession.ReindexSteps();
            }

            Sl.AnswerRepo.Update(learningSessionStep.Answer);
            Sl.LearningSessionRepo.Update(learningSession);
        }

        if (countLastAnswerAsCorrect)
            return Run(questionId, "", userId, (question, answerQuestionResult) =>
                _answerLog.CountLastAnswerAsCorrect(questionViewGuid), countLastAnswerAsCorrect: true
            );

        if (countUnansweredAsCorrect)
            return Run(questionId, "", userId, (question, answerQuestionResult) =>
                _answerLog.CountUnansweredAsCorrect(question, userId, questionViewGuid, interactionNumber, millisecondsSinceQuestionView), countUnansweredAsCorrect: true
            );

        throw new Exception("neither countLastAnswerAsCorrect nor countUnansweredAsCorrect true");
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
}