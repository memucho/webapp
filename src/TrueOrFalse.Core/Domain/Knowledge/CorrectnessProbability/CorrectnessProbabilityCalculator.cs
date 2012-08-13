﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public class CorrectnessProbabilityCalculator : IRegisterAsInstancePerLifetime
    {
        /// <returns>CorrectnessProbability as Percentage</returns>
        public int Run(IEnumerable<AnswerHistory> answerHistoryItems)
        {

            var totalWeightedItems = 0d;
            var totalWeightedValue = 0d;
            var index = 0;

            foreach(var historyItem in answerHistoryItems.OrderByDescending(d => d.DateCreated))
            {
                index++;
                var weight = 1d;
                if (index == 1) weight += 5;
                if (index == 2) weight += 2.5;
                if (index == 3) weight += 1.5;

                totalWeightedValue += (historyItem.AnswerredCorrectly ? 1 : 0) * weight;
                totalWeightedItems += weight;
            }

            return (int) ((totalWeightedValue / totalWeightedItems) * 100);
        }
    }
}
