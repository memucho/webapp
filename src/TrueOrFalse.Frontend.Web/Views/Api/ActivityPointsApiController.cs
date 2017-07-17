using System;
using System.Web.Mvc;
             
public class ActivityPointsApiController : BaseController
{
    public JsonResult Add(string activityTypeString, int points)
    {
        var activityType = (ActivityPointsType)Enum.Parse(typeof(ActivityPointsType), activityTypeString);
        var isLoggedIn = Sl.SessionUser.IsLoggedInUser(Sl.SessionUser.UserId);
        var activityPoints = new ActivityPoints
        {
            Amount = points,
            ActionType = activityType,
            DateEarned = DateTime.Now
        };

        if (!isLoggedIn)
        {
            var oldUserLevel = UserLevelCalculator.GetLevel(Sl.SessionUser.getTotalActivityPoints());
            Sl.SessionUser.AddPointActivity(activityPoints);
            var newUserLevel = UserLevelCalculator.GetLevel(Sl.SessionUser.getTotalActivityPoints());

            var levelPopupAsHtml = "";
            if (oldUserLevel < newUserLevel)
                levelPopupAsHtml = ViewRenderer.RenderPartialView("~/Views/Api/ActivityPoints/LevelPopup.aspx", new LevelPopupModel(newUserLevel, Sl.SessionUser.getTotalActivityPoints(), false), ControllerContext);

            return new JsonResult { Data = new { totalPoints = Sl.SessionUser.getTotalActivityPoints(), levelPopup = levelPopupAsHtml} };
        }

        if (isLoggedIn)
        {
            var oldUserLevel = Sl.SessionUser.User.ActivityLevel;
            activityPoints.User = Sl.SessionUser.User;
            Sl.ActivityPointsRepo.Create(activityPoints);
            Sl.UserRepo.UpdateActivityPointsData();
            Sl.SessionUser.UpdateUser();

            var levelPopupAsHtml = "";
            if (oldUserLevel < Sl.SessionUser.User.ActivityLevel)
                levelPopupAsHtml = ViewRenderer.RenderPartialView("~/Views/Api/ActivityPoints/LevelPopup.aspx", new LevelPopupModel(Sl.SessionUser.User.ActivityLevel, Sl.SessionUser.User.ActivityPoints, true), ControllerContext);
            
            return new JsonResult { Data = new { totalPoints = Sl.SessionUser.User.ActivityPoints, userLevel = Sl.SessionUser.User.ActivityLevel, levelPopup = levelPopupAsHtml} };
        }

        return new JsonResult();
    }

}