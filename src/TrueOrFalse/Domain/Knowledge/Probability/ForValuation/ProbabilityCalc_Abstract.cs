using System.Collections.Generic;
using System.Linq;

public abstract class ProbabilityCalc_Abstract
{
    public ProbabilityCalcResult Run(Question question, User user)
    {
        var answers = Sl.R<AnswerRepo>().GetByQuestion(question.Id, user.Id);

        if(Sl.Session.Get<Question>(question.Id) == null || Sl.Session.Get<User>(user.Id) == null)
            return new ProbabilityCalcResult { Probability = 0, KnowledgeStatus = KnowledgeStatus.NotLearned };

        return Run(answers, question, user);
    }

    public ProbabilityCalcResult Run(Question question, User user, IList<Answer> answers)
    {
        if(question == null || user == null)
            return new ProbabilityCalcResult {Probability = 0, KnowledgeStatus = KnowledgeStatus.NotLearned};

        answers = answers
            .Where(x =>
                x.Question.Id == question.Id &&
                x.UserId == user.Id
            ).ToList();

        return Run(answers, question, user);
    }

	public abstract ProbabilityCalcResult Run(IList<Answer> previousHistoryItems, Question question, User user);
}