using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class TopicToContinueLearningModel
{
    public bool IsLearningSession { get; }
    public bool IsTestSession { get; }
    public ContentRecommendationResult ContentRecommendationResult { get; }
    public IList<CategoryCacheItem> Categories; 
    public TopicToContinueLearningModel(AnswerQuestionModel answerQuestionModel, IList<CategoryCacheItem> categoryList)
    {
        IsLearningSession = answerQuestionModel.IsLearningSession;
        ContentRecommendationResult = answerQuestionModel.ContentRecommendationResult;
        Categories = categoryList;
    }

}
