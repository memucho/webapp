public class LevelPopupModel
{
    public int UserLevel;
    public int UserPoints;
    public bool IsLoggedIn;
    public int PointsToNextLevel;

    public LevelPopupModel(int level, int points, bool isLoggedIn)
    {
        UserLevel = level;
        UserPoints = points;
        IsLoggedIn = isLoggedIn;
        PointsToNextLevel = UserLevelCalculator.GetUpperLevelBound(level);

    }
}