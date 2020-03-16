using System;
using System.Web.Mvc;

[Serializable]
public class QuestionHistory : HistoryBase<QuestionHistoryItem>
{
    public QuestionHistory()
    {
        _size = 5;
    }
}

[Serializable]
public class QuestionHistoryItem : HistoryItemBase
{
    public int Id { get; private set; }
    public int SetId { get; private set; }
    public string Text { get; private set; }
    public string Solution { get; private set; }
    public HistoryItemType Type { get; set; }
        
    public QuestionSearchSpec SearchSpec { get; set; }

    public Func<UrlHelper, string> Link;

    public Question Question;

    public QuestionHistoryItem(
        Question question, 
        QuestionSearchSpec seachSpec,
        HistoryItemType type = HistoryItemType.Any)
    {
        Type = type;
        FillQuestionFields(question);
            
        SearchSpec = QuestionSearchSpecSession.AddCloneToSession(seachSpec, this);
    }

    public QuestionHistoryItem(Question question, HistoryItemType type)
    {
        Type = type;
            
        FillQuestionFields(question);
    }

    private void FillQuestionFields(Question question)
    {
        Question = question;
        Id = question.Id;
        Text = question.Text;
        Solution = GetQuestionSolution.Run(question).GetCorrectAnswerAsHtml();
    }
}