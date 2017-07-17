using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TrueOrFalse.MultipleChoice;

public class QuestionSolutionMultipleChoice : QuestionSolution
{
    private const string AnswerListDelimiter = "</br>";
    public List<Choice> Choices = new List<Choice>();
    public bool isSolutionOrdered;

    public void FillFromPostData(NameValueCollection postData)
    {
        List<string> choices =
        (
                from key in postData.AllKeys
                where key.StartsWith("choice-")
                select postData.Get(key)
        )
        .ToList();

        List<string> choicesCorrect =
        (
            from key in postData.AllKeys
            where key.StartsWith("choice_correct-")
            select postData.Get(key)
        )
        .ToList();

        for (int i = 0; i < choices.Count; i++)
        {
            Choices.Add(new Choice
            {
                IsCorrect = choicesCorrect[i] == "Richtige Antwort",
                Text = choices[i]
            });
        }

        isSolutionOrdered = postData["isSolutionRandomlyOrdered"] != "";
    }

    public override bool IsCorrect(string answer)
    {
        string[] Answers = answer.Split(new string[] {"%seperate&xyz%"}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
        string[] Solutions = CorrectAnswer().Split(new[] { AnswerListDelimiter }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
        return Answers.OrderBy(t => t).SequenceEqual(Solutions.OrderBy(t => t));
    }

    public override string CorrectAnswer()
    {
        string CorrectAnswer = AnswerListDelimiter;
        foreach (var SingleChoice in Choices)
        {
            if (SingleChoice.IsCorrect)
            {
                CorrectAnswer += SingleChoice.Text;
                if (SingleChoice != Choices[Choices.Count - 1])
                    CorrectAnswer += AnswerListDelimiter;
            }
        }
        return CorrectAnswer;
    }

    public override string GetCorrectAnswerAsHtml()
    {
        string htmlListItems;

        var correctAnswer = CorrectAnswer();

        if (correctAnswer == AnswerListDelimiter)
            return "";

        if (!correctAnswer.Contains(AnswerListDelimiter))
        {
            htmlListItems = $"<li>{correctAnswer}</li>";
        }
        else
        {
            htmlListItems = correctAnswer
                .Split(new[] { AnswerListDelimiter }, StringSplitOptions.RemoveEmptyEntries)
                .Select(a => $"<li>{a}</li>")
                .Aggregate((a, b) => a + b);
        }

        return $"<ul>{htmlListItems}</ul>";
    }

    public override string GetAnswerForSEO()
    {
        return CorrectAnswer()
            .Split(new [] {AnswerListDelimiter}, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => $"{x}, ")
            .Aggregate((a, b) => a + b);
    }
}