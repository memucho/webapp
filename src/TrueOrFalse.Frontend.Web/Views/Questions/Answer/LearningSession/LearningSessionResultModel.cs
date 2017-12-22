using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class LearningSessionResultModel : BaseModel
{
    public LearningSession LearningSession;
    public int NumberSteps;
    public int NumberUniqueQuestions = 0;
    public int NumberCorrectAnswers; //answered correctly at first try
    public int NumberCorrectAfterRepetitionAnswers; 
    public int NumberWrongAnswers;
    public int NumberNotAnswered;
    public int NumberCorrectPercentage;
    public int NumberCorrectAfterRepetitionPercentage;
    public int NumberWrongAnswersPercentage;
    public int NumberNotAnsweredPercentage;

    public IEnumerable<IGrouping<int, LearningSessionStep>> AnsweredStepsGrouped;

    public Date DateToLearn;
    public bool DateIsInPast;
    public TimeSpanLabel DateRemainingTimeLabel;
    public TrainingPlan TrainingPlan;
    public int TrainingDateCount;
    public string RemainingTrainingTime;

    public IList<Set> SetsToLearn { get; set; }

    public int KnowledgeNotLearned;
    public int KnowledgeNeedsLearning;
    public int KnowledgeNeedsConsolidation;
    public int KnowledgeSolid;

    public int WishCountQuestions;
    public int WishCountSets;

    public LearningSessionResultModel(LearningSession learningSession)
    {
        LearningSession = learningSession;
        NumberSteps = LearningSession.Steps.Count();

        if (learningSession.IsDateSession)
        {
            DateToLearn = learningSession.DateToLearn;
            var remaining = DateToLearn.Remaining();
            DateIsInPast = remaining.TotalSeconds < 0;
            DateRemainingTimeLabel = new TimeSpanLabel(remaining, DateIsInPast);
            TrainingPlan = DateToLearn.TrainingPlan ?? new TrainingPlan();
            TrainingDateCount = TrainingPlan.OpenDates.Count;
            RemainingTrainingTime = new TimeSpanLabel(TrainingPlan.TimeRemaining).Full;
            var summary = KnowledgeSummaryLoader.Run(UserId, DateToLearn.AllQuestions().GetIds(), onlyValuated: false);
            KnowledgeNotLearned = summary.NotLearned;
            KnowledgeNeedsLearning = summary.NeedsLearning;
            KnowledgeNeedsConsolidation = summary.NeedsConsolidation;
            KnowledgeSolid = summary.Solid;
        }
        else if (learningSession.IsWishSession)
        {
            WishCountQuestions = learningSession.User.WishCountQuestions;
            WishCountSets = learningSession.User.WishCountSets;
            SetsToLearn = learningSession.SetsToLearn();
        }
        else if (learningSession.IsWishSession)
        {
            WishCountQuestions = learningSession.User.WishCountQuestions;
            WishCountSets = learningSession.User.WishCountSets;
        } else if (learningSession.IsSetsSession)
        {
            SetsToLearn = learningSession.SetsToLearn();
        }

        if (NumberSteps > 0)
        {
            AnsweredStepsGrouped = LearningSession.Steps.Where(s => s.AnswerState != StepAnswerState.NotViewedOrAborted).GroupBy(d => d.QuestionId);

            LearningSession.FillAllAnswers();

            NumberUniqueQuestions = AnsweredStepsGrouped.Count();

            NumberCorrectAnswers = AnsweredStepsGrouped.Count(
                g => g.First().AnswerState == StepAnswerState.Answered 
                && g.First().AnsweredCorrectly);

            NumberCorrectAfterRepetitionAnswers = AnsweredStepsGrouped.Count(
                g => g.Last().AnswerState == StepAnswerState.Answered 
                && g.Count() > 1 && g.Last().AnsweredCorrectly);

            NumberNotAnswered = AnsweredStepsGrouped.Count(g => g.All(a => a.AnswerState != StepAnswerState.Answered));

            NumberWrongAnswers = NumberUniqueQuestions - NumberNotAnswered - NumberCorrectAnswers - NumberCorrectAfterRepetitionAnswers;
            
            if (NumberWrongAnswers < 0)
                Logg.r().Error("Answered questions (wrong+skipped+...) don't add up at LearningSessionResult for LearningSession id=" + learningSession.Id);

            PercentageShares.FromAbsoluteShares(
                new List<ValueWithResultAction>
                {
                    new ValueWithResultAction{AbsoluteValue = NumberCorrectAnswers, ActionForPercentage = p => NumberCorrectPercentage = p},
                    new ValueWithResultAction{AbsoluteValue = NumberCorrectAfterRepetitionAnswers, ActionForPercentage = p => NumberCorrectAfterRepetitionPercentage = p},
                    new ValueWithResultAction{AbsoluteValue = NumberWrongAnswers, ActionForPercentage = p => NumberWrongAnswersPercentage = p},
                    new ValueWithResultAction{AbsoluteValue = NumberNotAnswered, ActionForPercentage = p => NumberNotAnsweredPercentage = p},
                });
        }
    }
}
