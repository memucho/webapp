﻿using TrueOrFalse;

public class SetQuestionRowModel
{
    public virtual Set Set { get; set; }
    public virtual Question Question { get; set; }
    public virtual int Sort { get; set; }

    public bool IsInWishknowledge;
    public bool UserIsInstallationAdmin;

    public HistoryAndProbabilityModel HistoryAndProbability;

    public SetQuestionRowModel(
        Question question,
        Set set,
        TotalPerUser totalForUser, 
        QuestionValuation questionValuation)
    {
        Question = question;
        Set = set;

        questionValuation = questionValuation ?? new QuestionValuation();

        IsInWishknowledge = questionValuation.IsInWishKnowledge();
        UserIsInstallationAdmin = Sl.R<SessionUser>().User.IsInstallationAdmin;

        HistoryAndProbability = new HistoryAndProbabilityModel
        {
            AnswerHistory = new AnswerHistoryModel(question, totalForUser),
            CorrectnessProbability = new CorrectnessProbabilityModel(question, questionValuation),
            QuestionValuation = questionValuation
        };
    }
}
