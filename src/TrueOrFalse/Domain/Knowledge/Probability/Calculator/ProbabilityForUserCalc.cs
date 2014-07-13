﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse;

/// <summary>
/// Correctness probability for user
/// </summary>
public class ProbabilityForUserCalc : IRegisterAsInstancePerLifetime
{
    /// <returns>CorrectnessProbability as Percentage</returns>
    public int Run(IList<AnswerHistory> answerHistoryItems)
    {
        if (!answerHistoryItems.Any())
            return -1;

        var weightedFavorableOutcomes = 0d;
        var weightedTotalOutcomes = 0d;

        var index = 0;

        foreach(var historyItem in answerHistoryItems.OrderByDescending(d => d.DateCreated))
        {
            index++;
            var weight = 1d;
            if (index == 1) weight += 5;
            if (index == 2) weight += 2.5;
            if (index == 3) weight += 1.5;

            weightedFavorableOutcomes += (historyItem.AnswerredCorrectly ? 1 : 0) * weight;
            weightedTotalOutcomes += weight;
        }

        return (int) ((weightedFavorableOutcomes / weightedTotalOutcomes) * 100);
    }
}