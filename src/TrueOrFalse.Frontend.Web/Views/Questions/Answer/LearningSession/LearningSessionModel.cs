using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class LearningSessionModel : BaseModel
{
    public LearningSession LearningSession;


    public LearningSessionModel(AnswerQuestionModel answerQuestionModel)
    {
        LearningSession = answerQuestionModel.LearningSession;
    }
}
