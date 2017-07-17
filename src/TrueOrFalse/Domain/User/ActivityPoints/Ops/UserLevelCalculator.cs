using System;

public class UserLevelCalculator
{
    public static int GetLevel(int points)
    {
        for (int level = 0; level <= 100; level++)
        {
            if(points >= CalculateLowerLevelBound(level) && points < CalculateUpperLevelBound(level))
                return level;
        }

        return 100;
    }

    public static int GetPointsToNextLevel(int level, int actualPoints) => CalculateUpperLevelBound(level) - actualPoints;
    public static int GetUpperLevelBound(int level) => CalculateUpperLevelBound(level);

    private static int CalculateUpperLevelBound(int level)
    {
        return CalculateLowerLevelBound(level + 1);
    }

    private static int CalculateLowerLevelBound(int level)
    {
        switch (level)
        {
            case 1:
                return 65;

            case 2:
                return 200;

            case 3:
                return 500;

            case 4:
                return 1000;

            case 5:
                return 2000;

            case 6:
                return 4000;
        }
        return (int)Math.Round(Math.Pow(level - 5, 1.9) * 2000);
    }

    //public static int GetlevelProgressPercentage(int totalPoints)
    //{
    //    var level = GetLevel(totalPoints);
    //    var lowerLevelBound = CalculateLowerLevelBound(level);
    //    var pointsInLevel = totalPoints - (lowerLevelBound - 1);

    //    var totalInLevel = CalculateUpperLevelBound(level) - (lowerLevelBound - 1);

    //    return (pointsInLevel/totalInLevel) * 100;
    //}

}