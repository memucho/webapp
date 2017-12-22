using System.Collections.Generic;

public class ProbabilityCalcResult
{
    /// <summary>CorrectnessProbability</summary>
	public int Probability;
    public int AnswerCount;
    public KnowledgeStatus KnowledgeStatus;

    public static ProbabilityCalcResult GetResult(
        IList<Answer> answers, 
        int correctnessProbability)
    {
	    var result = new ProbabilityCalcResult
	    {
	        Probability = correctnessProbability,
            AnswerCount = answers.Count,
            KnowledgeStatus = GetKnowledgeStatus.Run(answers)
	    };

	    return result;
    }
}