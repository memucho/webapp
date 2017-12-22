using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Seedworks.Lib.Persistence;

[JsonObject(MemberSerialization.OptIn)]
public class LearningSessionStep
{
    [JsonProperty]
    public Guid Guid;

    [JsonProperty]
    public int Idx;

    private Question _question;

    public Question Question
    {
        get
        {
            if (_question != null)
                return _question;

            _question = Sl.R<QuestionRepo>().GetById(QuestionId);
            QuestionId = _question?.Id ?? -1;
            return _question;
        }
        set
        {
            _question = value;
            QuestionId = _question.Id;
        }
    }

    [JsonProperty]
    public int QuestionId;

    public Answer AnswerWithInput
    {
        get
        {
            if (Answers == null)
                return null;

            if (Answers.Any(a => !a.IsView()))
                return Answers.OrderBy(a => a.InteractionNumber).Last(a => !a.IsView());

            return Answers.OrderBy(a => a.InteractionNumber).Last();
        }
    }

    private IList<Answer> _answers;

    public IList<Answer> Answers
    {
        get
        {
            if (_answers != null)
                return _answers;

            return Sl.AnswerRepo.GetByLearningSessionStepGuid(Guid);
        }
        set => _answers = value;
    }

    public AnswerCorrectness AnswerCorrectness => AnswerWithInput.AnswerredCorrectly;

    public bool AnsweredCorrectly =>
        AnswerCorrectness == AnswerCorrectness.True || AnswerCorrectness == AnswerCorrectness.MarkedAsTrue;

    [JsonProperty]
    public StepAnswerState AnswerState { get; set; }

    [JsonProperty]
    public bool IsRepetition { get; set; }


}