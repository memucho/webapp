using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Search
{
    public class ToQuestionSolrMap
    {
        public static QuestionSolrMap Run(Question question, IList<QuestionValuationCacheItem> valuations)
        {

            var allCategories = EntityCache.GetCategoryCacheItems(question.Categories.Select(c => c.Id)).ToList();

            allCategories.AddRange(EntityCache.GetCategoryCacheItems(
                question.References
                    .Where(r => r.Category != null)
                    .Select(r => r.Category.Id)));

            var creator = new UserTinyModel(question.Creator);

            var result = new QuestionSolrMap
                {
                    Id = question.Id,
                    CreatorId = creator.Id,
                    Valuation = valuations.Count(v => v.IsInWishKnowledge),
                    IsPrivate = question.Visibility != QuestionVisibility.All,
                    Text = question.Text,
                    Description = question.Description,
                    Solution = question.Solution,
                    SolutionType = (int) question.SolutionType,
                    Categories = allCategories.Select(c => c.Name).ToArray(),
                    CategoryIds = allCategories.Select(c => c.Id).ToArray(),
                    AvgQuality = question.TotalQualityAvg,
                    Views = question.TotalViews,
                    DateCreated = question.DateCreated
                };

            return result;
        }
    }
}
