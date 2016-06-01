﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NHibernate.Criterion;
using RazorEngine.Compilation.ImpromptuInterface.Build;
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

    public static TrainingPlan Run_(Date date, TrainingPlanSettings settings)
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

        settings.DebugAnswerProbabilities = GetInitialAnswerProbabilities(date);

        trainingPlan.Dates = GetDates(date, settings);

        var probabilitiesAtTimeOfDate = ReCalcAllAnswerProbablities(date.DateTime, settings.DebugAnswerProbabilities);
        trainingPlan.LearningGoalIsReached = probabilitiesAtTimeOfDate
            .All(p => p.CalculatedProbability >= settings.AnswerProbabilityThreshold 
                && GetKnowledgeStatus.Run(p.History.Select(h => h.Answer).ToList()) == KnowledgeStatus.Solid);

        return trainingPlan;
    }

    private static List<AnswerProbability> GetInitialAnswerProbabilities(Date date)
    {
        var probUpdateValRepo = Sl.R<ProbabilityUpdate_Valuation>();

        foreach (var question in date.AllQuestions())
        {
            probUpdateValRepo.Run(question.Id, date.User.Id);
        }

        var answerRepo = Sl.R<AnswerRepo>();
        var questionValuationRepo = Sl.R<QuestionValuationRepo>();

        return date
                .AllQuestions()
                .Select(q =>
                    new AnswerProbability
                    {
                        User = date.User,
                        Question = q,
                        CalculatedProbability = questionValuationRepo.GetBy(q.Id, date.User.Id).KnowledgeStatus.GetProbability(q.Id),
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

        var upperTimeBound = date.DateTime.Subtract(DateTimeX.Now()) < TimeSpan.FromHours(settings.NumberOfHoursLastTrainingShouldStartBeforeDate)
                                ? date.DateTime.AddHours(-1)
                                : date.DateTime.AddHours(-settings.NumberOfHoursLastTrainingShouldStartBeforeDate);

        var boostParameters = new AddFinalBoostParameters(date, learningDates, settings);

        if (settings.AddFinalBoost)
        {
            boostParameters.SetInitialBoostingDateProposal(upperTimeBound);
            boostParameters.SetBoostingDateTimes();

            upperTimeBound = boostParameters.CurrentBoostingDateProposal;
        }

        var nextDateProposal = RoundTime(DateTimeX.Now().AddMinutes(TrainingPlanSettings.TryAddDateIntervalInMinutes));

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
                        Math.Max(settings.SpacingBetweenSessionsInMinutes,
                            TrainingPlanSettings.TryAddDateIntervalInMinutes)));
                continue;
            }

            nextDateProposal = nextDateProposal.AddMinutes(TrainingPlanSettings.TryAddDateIntervalInMinutes);
        }

        if (settings.AddFinalBoost)
        {
            boostParameters.FillBoostingDates();
        }

        return learningDates;
    }

    private static bool TryAddDate(
        TrainingPlanSettings settings,
        DateTime proposedDateTime,
        List<TrainingDate> learningDates)
    {
        var newAnswerProbabilities = 
            ReCalcAllAnswerProbablities(proposedDateTime, settings.DebugAnswerProbabilities);

        var belowThresholdCount = newAnswerProbabilities.Count(x => x.CalculatedProbability < settings.AnswerProbabilityThreshold);

        if (settings.DebugAnswerProbabilities.Count(x => x.History.Count > 0 && x.CalculatedProbability < 15) > 0)
            Debugger.Break();

        //if((proposedDateTime.Hour % 4) == 0 && proposedDateTime.Minute < 15)
        //    answerProbabilities.Log();

        if (belowThresholdCount < settings.QuestionsPerDate_Minimum)
            return false;

        settings.DebugAnswerProbabilities = newAnswerProbabilities.ToList();

        learningDates.Add(
            SetUpTrainingDate(
                proposedDateTime,
                settings,
                settings.DebugAnswerProbabilities
                    .OrderBy(x => x.CalculatedProbability)
                    .ThenBy(x => x.History.Count)
                    .ToList(),
                maxApplicableNumberOfQuestions: belowThresholdCount,
                idealNumberOfQuestions: settings.QuestionsPerDate_IdealAmount
            ));

        var probs = settings.DebugAnswerProbabilities.OrderBy(x => x.Question.Id).ToList();

        var log = "XXX:" +
                  settings.DebugAnswerProbabilities.OrderBy(x => x.Question.Id)
                      .Select(x => x.Question.Id + ": " + x.CalculatedProbability)
                      .ToList()
                      .Aggregate((a, b) => a + " | " + b);

        Logg.r().Information(log);

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

            settings.DebugAnswerProbabilities
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

    private static List<AnswerProbability> ReCalcAllAnswerProbablities(DateTime dateTime, List<AnswerProbability> answerProbabilities)
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
        if(!nameof(TrainingPlanSettings.TryAddDateIntervalInMinutes).ToLower().Contains("minutes"))
            throw new Exception($"If {nameof(TrainingPlanSettings.TryAddDateIntervalInMinutes)} is based on time interval other than minutes rounding method has to be adjusted");

        return DateTimeUtils.RoundToNearestMinutes(dateTime, TrainingPlanSettings.TryAddDateIntervalInMinutes, toLower: toLower);
    }
}

