using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TrueOrFalse;

public class TrainingPlanCreator
{
    public static int[] QuestionsToTrackIds = {};

    public static TrainingPlan Run(Date date, TrainingPlanSettings settings)
    {
        var stopWatch = Stopwatch.StartNew();

        var trainingPlan = Run_(date, settings);

        Logg.r().Information("Traingplan created (in memory): {duration} DateCount: {dateCount} QuestionCount: {questionCount}",
            stopWatch.Elapsed,
            trainingPlan.Dates.Count,
            trainingPlan.Questions.Count);

        return trainingPlan;
    }

    private static TrainingPlan Run_(Date date, TrainingPlanSettings settings)
    {
        var trainingPlan = new TrainingPlan();
        trainingPlan.Date = date;
        trainingPlan.Settings = settings;

        if (date.AllQuestions().Count == 0)
            return trainingPlan;

        if (date.AllQuestions().Count < settings.QuestionsPerDate_Minimum)
            settings.QuestionsPerDate_Minimum = date.AllQuestions().Count;

        if (settings.QuestionsPerDate_IdealAmount < settings.QuestionsPerDate_Minimum)
            settings.QuestionsPerDate_IdealAmount = settings.QuestionsPerDate_Minimum;

        settings.AnswerProbabilities = GetInitialAnswerProbabilities(date);

        trainingPlan.Dates = GetDates(date, settings);

        var probabilitiesAtTimeOfDate = ReCalcAllAnswerProbablities(date.DateTime, settings.AnswerProbabilities);

        trainingPlan.LearningGoalIsReached = probabilitiesAtTimeOfDate
            .All(p => p.CalculatedProbability >= settings.AnswerProbabilityThreshold 
                && GetKnowledgeStatus.Run(p.History.Select(h => h.Answer).ToList()) == KnowledgeStatus.Solid);

        return trainingPlan;
    }

    private static List<AnswerProbability> GetInitialAnswerProbabilities(Date date)
    {
        foreach (var question in date.AllQuestions())
            ProbabilityUpdate_Valuation.Run(question.Id, date.User.Id);

        var answerRepo = Sl.R<AnswerRepo>();

        return date
                .AllQuestions()
                .Select(q =>
                    new AnswerProbability
                    {
                        User = date.User,
                        Question = q,
                        CalculatedProbability = Sl.QuestionValuationRepo.GetByFromCache(q.Id, date.User.Id).KnowledgeStatus.GetProbability(q.Id),
                        CalculatedAt = DateTimeX.Now(),
                        History = answerRepo
                            .GetByQuestion(q.Id, date.User.Id).Select(x => new AnswerProbabilityHistory(x, null))
                            .ToList()
                    })
                .ToList();
    }

    private static IList<TrainingDate> GetDates(Date date, TrainingPlanSettings settings)
    {
        var learningDates = new List<TrainingDate>();

        var upperTimeBound = date.DateTime.Date;
            
        var boostParameters = new AddFinalBoostParameters(date, learningDates, settings);

        if (settings.AddFinalBoost)
            upperTimeBound = 
                SetBoostingDateTimesFromEndtimeBackwards(boostParameters, upperTimeBound);

        AddDatesFromNowOnForwards(
            date,
            settings,
            learningDates,
            upperTimeBound);

        if (settings.AddFinalBoost)
            boostParameters.AddBoostingDates();

        AddExpirationDate(date, learningDates);

        return learningDates;
    }

    private static DateTime SetBoostingDateTimesFromEndtimeBackwards(AddFinalBoostParameters boostParameters, DateTime upperTimeBound)
    {
        boostParameters.SetInitialBoostingDateProposal(upperTimeBound);
        boostParameters.SetBoostingDateTimes();

        return boostParameters.CurrentBoostingDateProposal;
    }

    private static void AddDatesFromNowOnForwards(Date date, TrainingPlanSettings settings, List<TrainingDate> learningDates, DateTime upperTimeBound)
    {
        var earliestPossibleTimeAfterLastTraining = GetEarliestPossibleTimeAfterLastTraining(date, settings);

        var nextDateProposal = RoundTime((DateTime.Now > earliestPossibleTimeAfterLastTraining
                ? DateTime.Now
                : earliestPossibleTimeAfterLastTraining)
                .AddMinutes(TrainingPlanSettings.TryAddDateIntervalInMinutes),
                toLower: true);

        while (nextDateProposal < upperTimeBound)
        {
            if (settings.IsInSnoozePeriod(nextDateProposal))
            {
                nextDateProposal = nextDateProposal.AddMinutes(TrainingPlanSettings.TryAddDateIntervalInMinutes);
                continue;
            }

            if (TryAddDate(settings, nextDateProposal, learningDates))
            {
                nextDateProposal = RoundTime(
                    nextDateProposal
                    .AddMinutes(
                        Math.Max(settings.GetMinSpacingInMinutes((date.DateTime.Date - nextDateProposal.Date).Days),
                            TrainingPlanSettings.TryAddDateIntervalInMinutes)));
                continue;
            }

            nextDateProposal = nextDateProposal.AddMinutes(TrainingPlanSettings.TryAddDateIntervalInMinutes);
        }
    }

    private static DateTime GetEarliestPossibleTimeAfterLastTraining(Date date, TrainingPlanSettings settings)
    {
        var answersOfLastTrainingDone = date.LearningSessions.Any() ?
            date.LearningSessions.OrderBy(x => x.DateCreated)
                .Last().Steps.Where(s => s.AnswerWithInput != null)
                .Select(s => s.AnswerWithInput)
                .OrderBy(a => a.DateCreated).ToList() : new List<Answer>();

        if (!answersOfLastTrainingDone.Any())
        {
            return DateTime.MinValue; 
        }

        var endTimeOfLastTrainingDone = answersOfLastTrainingDone.Last().DateCreated;

        return endTimeOfLastTrainingDone.AddMinutes(settings.GetMinSpacingInMinutes((date.DateTime.Date - endTimeOfLastTrainingDone.Date).Days));
    }

    private static void AddExpirationDate(Date date, List<TrainingDate> learningDates)
    {
        if(!learningDates.Any())
            return;

        for (var i = 0; i < learningDates.Count - 1; i++)
        {
            var beginningOfNextDay = learningDates[i].DateTime.Date.AddDays(1);
            var timeOfNextDate = learningDates[i + 1].DateTime;

            learningDates[i].ExpiresAt = timeOfNextDate < beginningOfNextDay
                                        ? timeOfNextDate
                                        : beginningOfNextDay;
        }

        learningDates.Last().ExpiresAt = date.DateTime;
    }

    private static bool TryAddDate(
        TrainingPlanSettings settings,
        DateTime proposedDateTime,
        List<TrainingDate> learningDates)
    {
        var newAnswerProbabilities = 
            ReCalcAllAnswerProbablities(proposedDateTime, settings.AnswerProbabilities);

        var belowThresholdCount = newAnswerProbabilities.Count(x => x.CalculatedProbability < settings.AnswerProbabilityThreshold);

        if (settings.AnswerProbabilities.Count(x => x.History.Count > 0 && x.CalculatedProbability < 15) > 0)
            Debugger.Break();

        if (settings.DebugLog
            && proposedDateTime.Hour%4 == 0
            && proposedDateTime.Minute < 15)
        {
            settings.AnswerProbabilities.Log();
        }

        if (belowThresholdCount < settings.QuestionsPerDate_Minimum)
            return false;

        settings.AnswerProbabilities = newAnswerProbabilities.ToList();

        learningDates.Add(
            SetUpTrainingDate(
                proposedDateTime,
                settings,
                settings.AnswerProbabilities
                    .OrderBy(x => x.CalculatedProbability)
                    .ThenBy(x => x.History.Count)
                    .ToList(),
                maxApplicableNumberOfQuestions: belowThresholdCount,
                idealNumberOfQuestions: settings.QuestionsPerDate_IdealAmount
            ));

        if (settings.DebugLog)
        {
            var log = settings.AnswerProbabilities.OrderBy(x => x.Question.Id)
                         .Select(x => x.Question.Id + ": " + x.CalculatedProbability)
                         .ToList()
                         .Aggregate((a, b) => a + " | " + b);

            Logg.r().Information(log);
        }

        return true;
    }

    public static TrainingDate SetUpTrainingDate(
        DateTime dateTime,
        TrainingPlanSettings settings,
        List<AnswerProbability> orderedAnswerProbabilities,
        bool isBoostingDate = false,
        int maxApplicableNumberOfQuestions = -1, 
        int idealNumberOfQuestions = -1,
        int exactNumberOfQuestionsToTake = -1)
    {
        if (maxApplicableNumberOfQuestions == -1)
            maxApplicableNumberOfQuestions = orderedAnswerProbabilities.Count;

        if (idealNumberOfQuestions == -1)
            idealNumberOfQuestions = orderedAnswerProbabilities.Count;

        if (exactNumberOfQuestionsToTake != -1)
            maxApplicableNumberOfQuestions = idealNumberOfQuestions = exactNumberOfQuestionsToTake;

        var trainingDate = new TrainingDate
        {
            DateTime = dateTime,
            IsBoostingDate = isBoostingDate
        };

        trainingDate.AllQuestions =
            orderedAnswerProbabilities
                .Select(x => new TrainingQuestion
                {
                    Question = x.Question,
                    ProbBefore = x.CalculatedProbability,
                    ProbAfter = x.CalculatedProbability
                })
                .ToList();

        for (int i = 0; i < maxApplicableNumberOfQuestions; i++)
        {
            var trainingQuestion = trainingDate.AllQuestions[i];
            trainingQuestion.IsInTraining = true;
            /*directly after training the probability is almost 100%!*/
            trainingQuestion.ProbAfter = 99;

            settings.AnswerProbabilities
                .By(trainingQuestion.Question.Id)
                .AddAnswerAndSetProbability(trainingQuestion.ProbAfter, trainingDate.DateTime, trainingDate);

            if (idealNumberOfQuestions < i + 2)
                break;
        }

        return trainingDate;
    }

    public static void RemoveDatesIncludingAnswers(List<TrainingDate> trainingDates, List<TrainingDate> collidingDates, List<AnswerProbability> answerProbabilities)
    {
        collidingDates.ForEach(d => trainingDates.Remove(d));

        foreach (var answerProb in answerProbabilities)
        {
            var historiesToRemove = answerProb.History.Where(a => collidingDates.Any(d => d == a.TrainingDate));
            answerProb.History = answerProb.History.Except(historiesToRemove).ToList();
        }
    }

    public static List<AnswerProbability> ReCalcAllAnswerProbablities(DateTime dateTime, List<AnswerProbability> answerProbabilities)
    {
        var forgettingCurve = new ProbabilityCalc_Curve_HalfLife_24h(); 

        foreach (var answerProbability in answerProbabilities)
        {
            if (QuestionsToTrackIds.Contains(answerProbability.Question.Id))
            {
                Logg.r().Information("TrainingPlanCreator: Question " + answerProbability.Question.Id + "DateTime: " + dateTime.ToString("s") + ", oldProb: " + answerProbability.CalculatedProbability);
            }

            var newProbability = forgettingCurve.Run(
                answerProbability.History.Select(x => x.Answer).ToList(),
                answerProbability.Question,
                answerProbability.User, 
                (int) (dateTime - answerProbability.CalculatedAt).TotalMinutes, 
                answerProbability.CalculatedProbability
            );

            answerProbability.CalculatedProbability = newProbability;
            answerProbability.CalculatedAt = dateTime;
            //answerProbability.ForgettingCurveDataPoints;


            if (QuestionsToTrackIds.Contains(answerProbability.Question.Id))
            {
                Logg.r().Information("TrainingPlanCreator: Question " + answerProbability.Question.Id + "DateTime: " + dateTime.ToString("s") + ", newProb: " + answerProbability.CalculatedProbability);
            }
        }

        return answerProbabilities;
    }

    public static DateTime RoundTime(DateTime dateTime, bool toLower = false)
    {
        return DateTimeUtils.RoundToNearestMinutes(dateTime, TrainingPlanSettings.TryAddDateIntervalInMinutes, toLower: toLower);
    }
}

