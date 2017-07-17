public class TransferActivityPoints
{
    public static void FromSessionToUser()
    {
        if (Sl.SessionUser.ActivityPoints.Count > 0)
        {
            foreach (var activityPoints in Sl.SessionUser.ActivityPoints)
            {
                activityPoints.User = Sl.SessionUser.User;
                Sl.ActivityPointsRepo.Create(activityPoints);
            }
            Sl.SessionUser.ActivityPoints.Clear();
        }
    }
}