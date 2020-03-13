using System;
using System.Web.Mvc;
using static System.String;

[SetMainMenu(MainMenuEntry.None)]
public class WidgetController : BaseController
{
    public ActionResult DiscontinuationInfo(string host)
    {
        return View(
            "~/Views/Widgets/WidgetDiscontinuationInfo.aspx",
            new WidgetBaseModel(host));
    }

    public ActionResult SetWithoutStartScreen(int setId, bool? hideAddToKnowledge, string host, string widgetKey, int questionCount = -1, string title = null, string text = null)
    {
        return View(
            "~/Views/Widgets/WidgetDiscontinuationInfo.aspx",
            new WidgetBaseModel(host));
    }

    public ActionResult Question(int questionId, bool? hideAddToKnowledge, string host, string widgetKey)
    {
        return View(
            "~/Views/Widgets/WidgetDiscontinuationInfo.aspx",
            new WidgetBaseModel(host));
    }

    public ActionResult Set(int setId, bool? hideAddToKnowledge, string host, string widgetKey, int questionCount = -1)
    {
        SaveWidgetView.Run(
            host, 
            !IsNullOrEmpty(widgetKey) ? widgetKey : setId.ToString(), 
            WidgetType.SetStartPage,
            setId
        );

        return View(
            "~/Views/Widgets/WidgetDiscontinuationInfo.aspx");
    }

    public ActionResult SetVideo(int setId, bool? hideAddToKnowledge, string host, string widgetKey)
    {
        return View(
            "~/Views/Widgets/WidgetDiscontinuationInfo.aspx",
            new WidgetBaseModel(host));
    }
}