using Seedworks.Web.State;
using System;
using System.CodeDom;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class UserCache
{
    public const int ExpirationSpanInMinutes = 600;

    private static string GetCacheKey(int userId) => "UserCacheItem_" + userId;

    public static List<UserCacheItem> GetAllCacheItems()
    {
        var allUserIds = Sl.UserRepo.GetAllIds();
        var allUserValuations = new List<UserCacheItem>();
        foreach (var userId in allUserIds)                                  
        {
            allUserValuations.Add(GetItem(userId));
        }

        return allUserValuations;
    }

    public static UserCacheItem GetItem(int userId)
    {
        var cacheItem = Cache.Get<UserCacheItem>(GetCacheKey(userId));
        return cacheItem ?? CreateItemFromDatabase(userId);
    }

    public static bool IsInWishknowledge(int userId, int categoryId)
    {
        var cacheItem = GetItem(userId);
        var hasCategoryValuation = cacheItem.CategoryValuations.ContainsKey(categoryId);

        if (!hasCategoryValuation)
            return false;

        return cacheItem.CategoryValuations[categoryId].IsInWishKnowledge();
    }

    public static bool IsQuestionInWishknowledge(int userId, int questionId)
    {
        var cacheItem = GetItem(userId);
        var hasQuestionValuation = cacheItem.QuestionValuations.ContainsKey(questionId);

        if (!hasQuestionValuation)
            return false;

        return cacheItem.QuestionValuations[questionId].IsInWishKnowledge; 
    }

    public static UserCacheItem CreateItemFromDatabase(int userId)
    {
        var user = Sl.UserRepo.GetById(userId);

        var cacheItem = new UserCacheItem
        {
            User = user,
            CategoryValuations = new ConcurrentDictionary<int, CategoryValuation>(
                Sl.CategoryValuationRepo.GetByUser(userId, onlyActiveKnowledge: false)
                    .Select(v => new KeyValuePair<int, CategoryValuation>(v.CategoryId, v))),
            QuestionValuations = new ConcurrentDictionary<int, QuestionValuationCacheItem>(
                Sl.QuestionValuationRepo.GetByUserWithQuestion(userId)
                    .Select(v => new KeyValuePair<int, QuestionValuationCacheItem>(v.Question.Id, v.ToCacheItem())))
        };
            
        Add_valuationCacheItem_to_cache(cacheItem, userId);

        return cacheItem;
    }

    private static void Add_valuationCacheItem_to_cache(UserCacheItem cacheItem, int userId)
    {
        Cache.Add(GetCacheKey(userId), cacheItem, TimeSpan.FromMinutes(ExpirationSpanInMinutes), slidingExpiration: true);
    }

    public static IList<QuestionValuationCacheItem> GetQuestionValuations(int userId) => GetItem(userId).QuestionValuations.Values.ToList();
    public static IList<CategoryValuation> GetCategoryValuations(int userId) => GetItem(userId).CategoryValuations.Values.ToList();

    public static void AddOrUpdate(QuestionValuationCacheItem questionValuation)
    {
        var cacheItem = GetItem(questionValuation.User.Id);

        lock ("7187a2c9-a3a2-42ca-8202-f9cb8cb54137")
        {
            cacheItem.QuestionValuations.AddOrUpdate(questionValuation.Question.Id, questionValuation, (k, v) => questionValuation);
        }
    }

    public static void AddOrUpdate(CategoryValuation categoryValuation)
    {
        var cacheItem = GetItem(categoryValuation.UserId);

        lock ("82f573db-40a7-43d9-9e68-6cd78b626e8d")
        {
            cacheItem.CategoryValuations.AddOrUpdate(categoryValuation.CategoryId, categoryValuation, (k, v) => categoryValuation);
        }
    }

    public static void AddOrUpdate(User user)
    {
        var cacheItem = GetItem(user.Id);
        cacheItem.User = user;
    }

    /// <summary> Used for question delete </summary>
    public static void RemoveAllForQuestion(int questionId)
    {
        foreach (var userId in Sl.UserRepo.GetAllIds())
        {
            var cacheItem = GetItem(userId);
            cacheItem.QuestionValuations.TryRemove(questionId, out var questValOut);
        }
    }

    public static List<UserCacheItem> GetAllActiveCaches()
    {
        var enumerator = System.Web.HttpRuntime.Cache.GetEnumerator();
        List<string> keys = new List<string>();
        List<UserCacheItem> userCacheItems = new List<UserCacheItem>();

        while (enumerator.MoveNext())
        {
            keys.Add(enumerator.Key.ToString());
        }

        foreach (var userCacheKey in keys.Where(k => k.Contains("UserCacheItem_")))
        {
            var startIndex = userCacheKey.IndexOf("_") + 1;
            var endIndex = userCacheKey.Length - startIndex;
            var userId = Int32.Parse(userCacheKey.Substring(startIndex, endIndex));
            userCacheItems.Add(GetItem(userId));
        }

        return userCacheItems; 
    }

    /// <summary> Used for category delete </summary>
    public static void RemoveAllForCategory(int categoryId)
    {
        Sl.CategoryValuationRepo.DeleteCategoryValuation(categoryId);
        foreach (var userCache in GetAllActiveCaches())
        {
            userCache.CategoryValuations.TryRemove(categoryId, out var result); 
        }
    }

    public static void Remove(User user) => Remove(user.Id );

    public static void Remove(int userId)
    {
        var cacheKey = GetCacheKey(userId);
        var cacheItem = Cache.Get<UserCacheItem>(cacheKey);

        if (cacheItem != null)
        {
            Cache.Remove(cacheKey);
        }
    }

    public static void Clear()
    {
        var allUserIds = Sl.UserRepo.GetAllIds();

        foreach (var userId in allUserIds)
            Remove(userId);
    }
}
