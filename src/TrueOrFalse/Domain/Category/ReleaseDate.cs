using System;

public class ReleaseDate
{
    public static DateTime Date = new DateTime(2021, 7, 18);
    public static bool IsAfterRelease(DateTime dateToCompareWith) => Date < dateToCompareWith;
}