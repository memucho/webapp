using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Utils;
using Newtonsoft.Json;
using NHibernate.Util;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Web.Uris;

public class LearningSession : DomainEntity, IRegisterAsInstancePerLifetime
{
    public const int DefaultNumberOfSteps = 10;
    public virtual User User { get; set; }
    public virtual LearningSessionSettings Settings { get; set; } = new LearningSessionSettings();
    public virtual IList<LearningSessionStep> Steps{ get; set; }

    public virtual string StepsJson
    {
        get => JsonConvert.SerializeObject(Steps);
        set
        {
            if (value == null)
            {
                Steps = new List<LearningSessionStep>();
                return;
            }

            Steps = JsonConvert.DeserializeObject<IList<LearningSessionStep>>(value).Where(s => s.Question != null).OrderBy(s => s.Idx).ToList();
        }
    }

    public virtual Set SetToLearn { get; set; }
    public virtual string SetsToLearnIdsString { get; set; }
    public virtual string SetListTitle { get; set; }
    public virtual Date DateToLearn { get; set; }
    public virtual Category CategoryToLearn { get; set; }

    public virtual bool IsCompleted { get; set; }

    public virtual string UrlName
    {
        get
        {
            if (IsSetSession)
                return "Lernset-" + UriSegmentFriendlyUser.Run(SetToLearn.Name);

            if (IsSetsSession)
                return "Lernsets-" + UriSegmentFriendlyUser.Run(SetListTitle);

            if (IsCategorySession)
                return "Thema-" + UriSegmentFriendlyUser.Run(CategoryToLearn.Name);

            if (IsDateSession)
                return "Termin-" + DateToLearn.DateTime.ToString("D").Replace(",", "").Replace(" ", "_").Replace(".", "");

            if (IsWishSession)
                return "Wunschwissen";

            throw new Exception("unknown session type");
        }
    }

    public virtual bool IsSetSession { get { return SetToLearn != null; } }
    public virtual bool IsSetsSession { get { return !string.IsNullOrEmpty(SetsToLearnIdsString); } }
    public virtual bool IsDateSession{ get { return DateToLearn != null; }}
    public virtual bool IsCategorySession{ get { return CategoryToLearn != null; }}
    public virtual bool IsWishSession { get; set; }

    public virtual int TotalPossibleQuestions
    {
        get
        {
            if (IsSetSession)
                return SetToLearn.Questions().Count;

            if (IsSetsSession)
                return SetsToLearn().Sum(s => s.Questions().Count); //DB is accessed

            if (IsDateSession)
                return DateToLearn.AllQuestions().Count;

            if (IsCategorySession)
                return GetQuestionsForCategory.AllIncludingQuestionsInSet(CategoryToLearn.Id).Count;

            if (IsWishSession)
                return User.WishCountQuestions;

            throw new Exception("unknown session type");
        }
    }

    public virtual IList<Question> Questions()
    {
        return Steps.Select(s => s.Question).Distinct().ToList();
    }

    public virtual IList<Set> SetsToLearn()
    {
        if (string.IsNullOrEmpty(SetsToLearnIdsString))
            return new List<Set>();

        var setIds = SetsToLearnIdsString
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Convert.ToInt32(x));

        var setRepo = Sl.R<SetRepo>();

        return setIds
            .Select(setId => setRepo.GetById(setId))
            .Where(set => set != null)
            .ToList();
    }

    public virtual int CurrentLearningStepIdx()
    {
        return Steps.ToList()
            .FindIndex(s => s.AnswerState == StepAnswerState.Uncompleted);
    }


    public virtual void CompleteSession()
    {
        if(IsCompleted) return;

        Steps.Where(s => s.AnswerState == StepAnswerState.Uncompleted)
            .Each(s => s.AnswerState = StepAnswerState.NotViewedOrAborted);

        IsCompleted = true;

        Sl.R<LearningSessionRepo>().Update(this);
    }

    public static LearningSession InitDateSession(Date date, TrainingDate trainingDate)
    {
        var learningSession = new LearningSession
        {
            DateToLearn = date,
            User = date.User
        };

        if (trainingDate == null
            || (trainingDate.IsBoostingDate
                && !date.TrainingPlan.BoostingPhaseHasStarted()))
        {
            learningSession.Steps = GetLearningSessionSteps
                .Run(date.AllQuestions(), new LearningSessionSettings { AmountQuestions = date.TrainingPlanSettings.QuestionsPerDate_Minimum });
        }
        else if (trainingDate.LearningSession != null)
        {
            learningSession = trainingDate.LearningSession;
        }
        else
        {
            learningSession.Steps = GetLearningSessionSteps.Run(trainingDate);
            trainingDate.LearningSession = learningSession;
        }

        Sl.R<LearningSessionRepo>().Create(learningSession);

        if (trainingDate != null)
        {
            Sl.R<TrainingDateRepo>().Update(trainingDate);
        }

        return learningSession;
    }

    public virtual void SkipStep(int stepIdx)
    {
        var stepToSkip = Steps[stepIdx];
        if (stepToSkip != null && stepToSkip.AnswerState != StepAnswerState.Answered && stepToSkip.AnswerState != StepAnswerState.ShowedSolutionOnly)
        {
            stepToSkip.AnswerState = StepAnswerState.Skipped;
            Sl.R<LearningSessionRepo>().Update(this);
        }
    }

    public virtual void UpdateAfterWrongAnswerOrShowSolution(LearningSessionStep affectedStep)
    {
        if(LimitForThisQuestionHasBeenReached(affectedStep) 
            || LimitForNumberOfRepetitionsHasBeenReached())
            return;

        var newStepForRepetition = new LearningSessionStep
        {
            Guid = Guid.NewGuid(),
            Question = affectedStep.Question,
            IsRepetition = true,
        };

        var idxOfNewStep = Math.Min(affectedStep.Idx + 3, Steps.Count);

        Steps = Steps.OrderBy(s => s.Idx).ToList();

        Steps.Insert(idxOfNewStep, newStepForRepetition);

        ReindexSteps();

        Sl.R<LearningSessionRepo>().Update(this);
    }

    public virtual void ReindexSteps()
    {
        var idx = 0;

        foreach (var step in Steps)
        {
            step.Idx = idx;
            idx++;
        }
    }

    public virtual bool LimitForThisQuestionHasBeenReached(LearningSessionStep step)
    {
        return Steps.Count(s => s.Question == step.Question) >= 3;
    }

    public virtual bool LimitForNumberOfRepetitionsHasBeenReached()
    {
        return Steps.Count >= Steps.Select(s => s.Question).Distinct().Count()*2;
    }

    public virtual LearningSessionStep GetStep(Guid learningSessionStepGuid)
    {
        var steps = Steps.Where(s => s.Guid == learningSessionStepGuid).ToList();
        if (steps.Count > 1)
            throw new Exception("duplicate Guid");

        return steps.FirstOrDefault();
    }

    public static LearningSessionStep GetStep(int learningSessionId, Guid learningSessionStepGuid) => 
        Sl.LearningSessionRepo.GetById(learningSessionId).GetStep(learningSessionStepGuid);

    public virtual void FillAllAnswers()
    {
        var stepGuids = this.Steps.Where(s => s.AnswerState != StepAnswerState.NotViewedOrAborted).Select(x => x.Guid).ToList();
        var answers = Sl.AnswerRepo.GetByLearningSessionStepGuids(stepGuids);
        this.Steps.ForEach(s => s.Answers = answers.Where(a => a.LearningSessionStepGuid == s.Guid).ToList());
    }

}
