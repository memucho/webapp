﻿using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Search
{
    public class ToQuestionSolrMap
    {
        public static QuestionSolrMap Run(Question question, IEnumerable<QuestionValuation> valuations)
        {

            var allCategories = question.Categories.ToList();
            allCategories.AddRange(
                question.References
                    .Where(r => r.Category != null)
                    .Select(r => r.Category));

            var result = new QuestionSolrMap
                {
                    Id = question.Id,
                    CreatorId = question.Creator.Id,
                    ValuatorIds = valuations.Where(v => v.RelevancePersonal != -1).Select(x => x.User.Id).ToList(),
                    IsPrivate = question.Visibility != QuestionVisibility.All,
                    Text = question.Text,
                    Description = question.Description,
                    Solution = question.Solution,
                    SolutionType = (int) question.SolutionType,
                    Categories = allCategories.Select(c => c.Name).ToArray(),
                    CategoryIds = allCategories.Select(c => c.Id).ToArray(),
                    AvgQuality = question.TotalQualityAvg,
                    AvgValuation = question.TotalRelevancePersonalAvg,
                    Views = question.TotalViews,
                    DateCreated = question.DateCreated
                };

            return result;
        }
    }
}
