using System.Collections.Concurrent;

public class UserValuationCacheItem
{
    public int UserId;
    
    public ConcurrentDictionary<int, CategoryValuation> CategoryValuations;
    public ConcurrentDictionary<int, QuestionValuation> QuestionValuations;
}