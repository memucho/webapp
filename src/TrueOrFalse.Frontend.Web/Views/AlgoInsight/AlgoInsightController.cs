using System.Web.Mvc;
using TrueOrFalse.Web;

[SetMenu(MenuEntry.None)]
[AccessOnlyLocal]
public class AlgoInsightController : BaseController
{
    private string _viewName = "AlgoInsight";

    public ActionResult Forecast()
    {
        return View(_viewName, new AlgoInsightModel{IsActiveTabForecast = true});
    }

    public ActionResult ForgettingCurve()
    {
        return View(_viewName, new AlgoInsightModel{IsActiveTabForgettingCurve = true});
    }

    [HttpPost]
    public JsonResult ForgettingCurvesJson(CurvesJsonCmd curvesJsonCmd)
    {
        return Json(ForgettingCurveJson.Load(curvesJsonCmd));
    }

    public ActionResult Repetition()
    {
        return View(_viewName, new AlgoInsightModel{IsActiveTabRepetition = true});
    }

    public ActionResult Various()
    {
        return View(_viewName, new AlgoInsightModel { IsActiveTabVarious = true });
    }

    [AccessOnlyAsAdmin]
	public ActionResult ReevaluateAlgos()
	{
        R<AnswerTestRepo>().TruncateTable();
        R<AnswerFeatureRepo>().TruncateTables();

        GenerateAnswerFeatures.Run();
        AssignAnswerFeatures.Run();
        AlgoTester.Run();

        return View("AlgoInsight", new AlgoInsightModel
        {
	        Message = new SuccessMessage("Eine Algorithmus-Bewertung wurde ausgeführt.")
        });
    }

    [AccessOnlyAsAdmin]
    public ActionResult ReevaluateQuestionFeatures()
    {
        R<QuestionFeatureRepo>().TruncateTables();
        GenerateQuestionFeatures.Run();
        AssignQuestionFeatures.Run();

        return View("AlgoInsight", new AlgoInsightModel
        {
            Message = new SuccessMessage("Eine Algorithmus-Bewertung wurde ausgeführt.")
        });
    }
}