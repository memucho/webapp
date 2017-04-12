using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using TrueOrFalse.Domain.Question.SolutionType.MatchList;

public class QuestionSolutionMatchList : QuestionSolution
{
    private const string PairSeperator = "%pairseperator%";
    private const string ElementSeperator = "%elementseperator%";
    public List<Pair> Pairs = new List<Pair>();
    public List<ElementRight> RightElements = new List<ElementRight>();

    public void FillFromPostData(NameValueCollection postData)
    {
        List<string> LeftElementText =
        (
            from key in postData.AllKeys
            where key.StartsWith("LeftElement-")
            select postData.Get(key)
        )
        .ToList();

        List<string> RightPairElementText =
        (
            from key in postData.AllKeys
            where key.StartsWith("RightPairElement-")
            select postData.Get(key)
        )
        .ToList();

        List<string> RightElementText =
        (
            from key in postData.AllKeys
            where key.StartsWith("RightElement-")
            select postData.Get(key)
        )
        .ToList();

        for (int i = 0; i < LeftElementText.Count; i++)
        {
            Pairs.Add(new Pair()
            {
                ElementLeft = new ElementLeft() {Text = LeftElementText[i]},
                ElementRight = new ElementRight() {Text = RightPairElementText[i]}
            });
        }

        foreach (var singleRightElementText in RightElementText)
        {
            RightElements.Add(new ElementRight() {Text = singleRightElementText});
        }
    }

    public static MatchListAnswerPairs deserializeMatchListAnswer(string answerJSON)
    {
        var serilizer = new JavaScriptSerializer();
        return serilizer.Deserialize<MatchListAnswerPairs>(answerJSON);
    }

    public override bool IsCorrect(string answer)
    {
        if (answer == "")
            return false;
        var answerObject = deserializeMatchListAnswer(answer);
        var questionPairs = this.Pairs.OrderBy(t => t.ElementLeft.Text).ToList();
        for (int i = 0; i < questionPairs.Count; i++)
        {
            if (questionPairs[i].ElementRight.Text == "Keine Zuordnung")
            {
                questionPairs.RemoveAt(i);
                i--;
            }
        }
        var answerPairs = answerObject.Pairs.OrderBy(t => t.ElementLeft.Text).ToList();
        bool answerCorrect = true;
        if (questionPairs.Count != answerPairs.Count)
            answerCorrect = false;
        else
        {
            for (int i = 0; i < questionPairs.Count(); i++)
            {
                bool isSameElementLeft = questionPairs[i].ElementLeft.Text == answerPairs[i].ElementLeft.Text;
                bool isSameElementRight = questionPairs[i].ElementRight.Text == answerPairs[i].ElementRight.Text;
                if (isSameElementLeft && isSameElementRight)
                    continue;
                answerCorrect = false;
            }
        }
        return answerCorrect;
        
    }

    public override string CorrectAnswer()
    {
        string CorrectAnswerMessage = PairSeperator;
        foreach (var pair in this.Pairs)
            CorrectAnswerMessage += pair.ElementLeft.Text + ElementSeperator + pair.ElementRight.Text + PairSeperator;

        return CorrectAnswerMessage;
    }

    public override string GetCorrectAnswerAsHtml()
    {
        var htmlListItems = CorrectAnswer()
            .Split(new[] { PairSeperator }, StringSplitOptions.RemoveEmptyEntries)
            .Select(a => $"<li>{String.Join(" - ",a.Split(new [] { ElementSeperator }, StringSplitOptions.RemoveEmptyEntries))}</li>")
            .Aggregate((a, b) => a + b);

        return $"<ul>{htmlListItems}</ul>";
    }

    public override string GetAnswerForSEO()
    {
        return CorrectAnswer()
            .Split(new [] {PairSeperator}, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => $"{String.Join(" - ", x.Split(new []{ElementSeperator}, StringSplitOptions.RemoveEmptyEntries))}, ")
            .Aggregate((a, b) => a + b);
    }
}

public class MatchListAnswerPairs
{
    public List<Pair> Pairs { get; set; }
}