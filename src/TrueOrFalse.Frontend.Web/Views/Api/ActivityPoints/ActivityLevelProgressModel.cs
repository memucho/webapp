public class ActivityLevelProgressModel
{
    public User User;
    public int NextLevelBound;
    public int PointsToNextLevel;

    public ActivityLevelProgressModel(User user)
    {
        User = user;
        NextLevelBound = UserLevelCalculator.GetUpperLevelBound(user.ActivityLevel);
        PointsToNextLevel = UserLevelCalculator.GetPointsToNextLevel(user.ActivityLevel, user.ActivityPoints);
    }
}