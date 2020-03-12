using System;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

public class TestSessionResultModel : BaseModel
{
    public TestSession TestSession;
    public int NumberQuestions;
    public int NumberCorrectAnswers;
    public int NumberWrongAnswers;
    public int NumberOnlySolutionView;
    public int NumberCorrectPercentage;
    public int NumberWrongAnswersPercentage;
    public int NumberOnlySolutionViewPercentage;
    public int PercentageAverageRightAnswers;
    public IList<TestSessionStep> Steps;

    public Category TestedCategory;
    public string LinkForRepeatTest;

    public ContentRecommendationResult ContentRecommendationResult;

    public bool IsInWidget;

    public virtual int TotalPossibleQuestions
    {
        get
        {
            if (TestSession.IsCategorySession)
                return GetQuestionsForCategory.AllIncludingQuestionsInSet(TestedCategory.Id).Count;

            throw new Exception("unknown session type");
        }
    }

    public TestSessionResultModel(TestSession testSession)
    {
        TestSession = testSession;

        if(testSession.SessionNotFound)
            return;

        if (TestSession.IsCategorySession)
        {
            TestedCategory = Sl.R<CategoryRepository>().GetById(TestSession.CategoryToTestId);
            LinkForRepeatTest = Links.TestSessionStartForCategory(TestedCategory.Name, TestedCategory.Id);
            ContentRecommendationResult = ContentRecommendation.GetForCategory(TestedCategory, 6);
        }
        else
        {
            throw new Exception("Type of TestSession is not defined.");
        }

        TestSession.FillUpStepProperties();
        Steps = TestSession.Steps.Where(s => s.AnswerState != TestSessionStepAnswerState.Uncompleted).ToList();

        NumberQuestions = Steps.Count(s => s.AnswerState != TestSessionStepAnswerState.Uncompleted);
        NumberCorrectAnswers = Steps.Count(s => s.AnswerState == TestSessionStepAnswerState.AnsweredCorrect);
        NumberWrongAnswers = Steps.Count(s => s.AnswerState == TestSessionStepAnswerState.AnsweredWrong);
        NumberOnlySolutionView = Steps.Count(s => s.AnswerState == TestSessionStepAnswerState.OnlyViewedSolution);

        PercentageShares.FromAbsoluteShares(new List<ValueWithResultAction>
        {
            new ValueWithResultAction{AbsoluteValue = NumberCorrectAnswers, ActionForPercentage = p => NumberCorrectPercentage = p},
            new ValueWithResultAction{AbsoluteValue = NumberWrongAnswers, ActionForPercentage = p => NumberWrongAnswersPercentage = p},
            new ValueWithResultAction{AbsoluteValue = NumberOnlySolutionView, ActionForPercentage = p => NumberOnlySolutionViewPercentage = p},
        });

        PercentageAverageRightAnswers = (int)Math.Round(Steps.Sum(s => s.Question.CorrectnessProbability) / (float)NumberQuestions);
    }

}
