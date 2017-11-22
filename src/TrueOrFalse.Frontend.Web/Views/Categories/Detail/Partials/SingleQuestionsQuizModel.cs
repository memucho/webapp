using System;
using System.Collections.Generic;
using System.Linq;

public class SingleQuestionsQuizModel : BaseModel
{
    public string Title;
    public string Text;
    public IList<Question> Questions;

    public SingleQuestionsQuizModel(Category category, int maxQuestions, string title = "", string text = "", string questionIds = null, string order = null)
    {
        Title = String.IsNullOrEmpty(title) ? "Wie viel weißt du über das Thema " + category.Name + "?" : title;
        Text = text;

        if (string.IsNullOrEmpty(questionIds))
        {
            Questions = category.GetAggregatedQuestionsFromMemoryCache().Where(q => q.IsVisibleToCurrentUser()).ToList();
        }
        else
        {
            var questionIdsList = questionIds
                .Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToInt32(x));

            Questions = questionIdsList
                .Select(questionId => Sl.QuestionRepo.GetById(questionId))
                .Where(q => q != null)
                .Where(q => q.IsVisibleToCurrentUser())
                .ToList();
        }

        switch (order)
        {
            case "ViewsDescending":
                Questions = Questions.OrderByDescending(q => q.TotalViews).ToList();
                break;

            case "CorrectnessProbabilityAscending":
                Questions = Questions.OrderBy(q => q.CorrectnessProbability).ToList();
                break;

            case "CorrectnessProbabilityDescending":
                Questions = Questions.OrderByDescending(q => q.CorrectnessProbability).ToList();
                break;

            case null:
            case "":
            case "Random":
            default:
                Random rnd = new Random();
                Questions = Questions.OrderByDescending(q => rnd.Next()).ToList();
                break;
        }

        Questions = Questions.Take(maxQuestions).ToList();
    }

}
