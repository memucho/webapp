<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<ActivityLevelProgressModel>" %>

Du hast im Moment: <%= Model.User.ActivityPoints %> Punkte.<br/>
Das nächste Level erreichst du bei <%= Model.NextLevelBound %> Punkten. <br/>
Du brauchst also <%= Model.PointsToNextLevel %> Punkte um das nächste Level zu erreichen.