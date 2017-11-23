using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class GetRandomQuestions
{
    public static IList<Question> Run(IList<Question> questions, int amount, List<int> excludeQuestionIds = null, bool ignoreExclusionIfNotEnoughQuestions = true)
    {
        var result = questions;
        if ((excludeQuestionIds != null) && (excludeQuestionIds.Count > 0) && (result.Count > amount))
        {
            result = result.Where(q => !excludeQuestionIds.Contains(q.Id)).ToList();
            if (ignoreExclusionIfNotEnoughQuestions && (result.Count < amount))
            {
                //possible improvement: if questions are to be reasked, prioritize those that have been answered wrong by the user.
                var fillUpAmount = amount - result.Count;
                var fillUpQuestions = questions.Where(q => excludeQuestionIds.Contains(q.Id)).ToList();
                fillUpQuestions.Shuffle();
                ((List<Question>)result).AddRange(fillUpQuestions.Take(fillUpAmount).ToList());
            }
        }
        result.Shuffle();
        result = result.Take(amount).ToList();
        return result;
    }

    public static IList<Question> Run(Set set, int amount, List<int> excludeQuestionIds = null, bool ignoreExclusionIfNotEnoughQuestions = true)
    {
        return Run(set.Questions().ToList(), amount, excludeQuestionIds, ignoreExclusionIfNotEnoughQuestions);
    }

    public static IList<Question> Run(IList<Set> sets, int amount, List<int> excludeQuestionIds = null, bool ignoreExclusionIfNotEnoughQuestions = true)
    {
        return Run(sets.SelectMany(s => s.Questions()).ToList(), amount, excludeQuestionIds, ignoreExclusionIfNotEnoughQuestions);
    }

    public static IList<Question> Run(Category category, int amount, List<int> excludeQuestionIds = null, bool ignoreExclusionIfNotEnoughQuestions = true)
    {
        var featuredSets = category.FeaturedSets();
        var questions = featuredSets.Count > 0 
            ? featuredSets.SelectMany(s => s.Questions()).Distinct().ToList()
            : Sl.R<QuestionRepo>().GetForCategoryAggregated(category.Id, Sl.R<SessionUser>().UserId);

        return Run(questions.ToList(), amount, excludeQuestionIds, ignoreExclusionIfNotEnoughQuestions);
    }
}
