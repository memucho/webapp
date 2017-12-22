using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Seedworks.Lib.Persistence;

public class EntityCache
{
    private const string _cacheKeyQuestions = "allQuestions _EntityCache";
    private const string _cacheKeyCategories = "allCategories_EntityCache";
    private const string _cacheKeySets = "allSets_EntityCache";
    private const string _cacheKeyCategoryQuestionsList = "categoryQuestionsList_EntityCache";
    private const string _cacheKeyCategorySetsList = "categorySetsList_EntityCache";
    private const string _cacheKeyCategoryQuestionInSetList = "categoryQuestionInSetList_EntityCache";

    private static ConcurrentDictionary<int, Question> Questions => (ConcurrentDictionary<int, Question>)HttpRuntime.Cache[_cacheKeyQuestions];
    private static ConcurrentDictionary<int, Category> Categories => (ConcurrentDictionary<int, Category>)HttpRuntime.Cache[_cacheKeyCategories];
    private static ConcurrentDictionary<int, Set> Sets => (ConcurrentDictionary<int, Set>)HttpRuntime.Cache[_cacheKeySets];

    //In category lists last level ConcurrentDictionary is used for easy access to keys (item ids) only (value is always 0)
    private static ConcurrentDictionary<int, ConcurrentDictionary<int, int>> CategoryQuestionsList => 
        (ConcurrentDictionary<int, ConcurrentDictionary<int, int>>)HttpRuntime.Cache[_cacheKeyCategoryQuestionsList];
    private static ConcurrentDictionary<int, ConcurrentDictionary<int, int>> CategorySetsList =>
        (ConcurrentDictionary<int, ConcurrentDictionary<int, int>>)HttpRuntime.Cache[_cacheKeyCategorySetsList];

    /// <summary>
    /// CategoryIds > QuestionIds (for each category) > SetsIds (of sets that bind question to category)
    /// </summary>
    private static ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>> CategoryQuestionInSetList =>
        (ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>>)HttpRuntime.Cache[_cacheKeyCategoryQuestionInSetList];

    public static void Init(string customMessage = "")
    {
        var stopWatch = Stopwatch.StartNew();

        Logg.r().Information("EntityCache Start" + customMessage + "{Elapsed}", stopWatch.Elapsed);

        var questions = Sl.QuestionRepo.GetAll();
        var categories = Sl.CategoryRepo.GetAllEager();
        var sets = Sl.SetRepo.GetAllEager();
        var questionInSets = Sl.QuestionInSetRepo.GetAll();

        Logg.r().Information("EntityCache LoadAllEntities" + customMessage + "{Elapsed}", stopWatch.Elapsed);

        IntoForeverCache(_cacheKeyQuestions, questions.ToConcurrentDictionary());
        IntoForeverCache(_cacheKeyCategories, categories.ToConcurrentDictionary());
        IntoForeverCache(_cacheKeySets, sets.ToConcurrentDictionary());
        IntoForeverCache(_cacheKeyCategoryQuestionsList, GetCategoryQuestionsList(questions));
        IntoForeverCache(_cacheKeyCategorySetsList, GetCategorySetsList(sets));
        IntoForeverCache(_cacheKeyCategoryQuestionInSetList, GetCategoryQuestionInSetList(questionInSets));


        Logg.r().Information("EntityCache PutIntoCache" + customMessage + "{Elapsed}", stopWatch.Elapsed);
    }

    private static void IntoForeverCache<T>(string key, ConcurrentDictionary<int, T> objectToCache)
    {
        HttpRuntime.Cache.Insert(
            key,
            objectToCache,
            null,
            Cache.NoAbsoluteExpiration,
            Cache.NoSlidingExpiration,
            CacheItemPriority.NotRemovable,
            null);
    }

    private static ConcurrentDictionary<int, ConcurrentDictionary<int, int>> GetCategoryQuestionsList(IList<Question> questions)
    {
        var categoryQuestionList = new ConcurrentDictionary<int, ConcurrentDictionary<int, int>>();
        foreach (var question in questions)
        {
            UpdateCategoryQuestionList(categoryQuestionList, question);
        }

        return categoryQuestionList;
    }

    private static ConcurrentDictionary<int, ConcurrentDictionary<int, int>> GetCategorySetsList(IList<Set> sets)
    {
        var categorySetsList = new ConcurrentDictionary<int, ConcurrentDictionary<int, int>>();
        foreach (var set in sets)
        {
            UpdateCategorySetList(categorySetsList, set);
        }

        return categorySetsList;
    }

    private static ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>>
        GetCategoryQuestionInSetList(IList<QuestionInSet> questionInSetItems)
    {
        var categoryQuestionInSetList =
            new ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>>();

        foreach (var questionInSet in questionInSetItems)
        {
            AddQuestionInSetTo(categoryQuestionInSetList, questionInSet);
        }

        return categoryQuestionInSetList;
    }

    public static IList<Question> GetQuestionsForCategory(int categoryId)
    {
        return GetQuestionsByIds(GetQuestionsIdsForCategory(categoryId));
    }

    public static IList<int> GetQuestionsIdsForCategory(int categoryId)
    {
        return CategoryQuestionsList.ContainsKey(categoryId) 
            ? CategoryQuestionsList[categoryId].Keys.ToList() 
            : new List<int>();
    }

    public static IList<Question> GetQuestionsInSetsForCategory(int categoryId)
    {
        return GetQuestionsByIds(GetQuestionsInSetsIdsForCategory(categoryId));
    }

    public static IList<Set> GetSetsForCategory(int categoryId)
    {
        return GetSetsByIds(GetSetIdsForCategory(categoryId));
    }

    public static IList<Set> GetSetsForCategories(IList<Category> categories)
    {
        return categories.SelectMany(c => GetSetsForCategory(c.Id)).Distinct().ToList();
    }

    public static IList<int> GetSetIdsForCategory(int categoryId)
    {
        return CategorySetsList.ContainsKey(categoryId)
            ? CategorySetsList[categoryId].Keys.ToList()
            : new List<int>();
    }

    public static IList<int> GetQuestionsInSetsIdsForCategory(int categoryId)
    {
        var questionIds = new List<int>();

        if (!CategoryQuestionInSetList.TryGetValue(categoryId, out var questionSetIdLists)) return questionIds;

        foreach (var questionId in questionSetIdLists.Keys)
        {
            if (questionSetIdLists.TryGetValue(questionId, out var setIdList)
                && !setIdList.IsEmpty)
            {
                questionIds.Add(questionId);
            }
        }

        return questionIds;
    }

    public static IList<Question> GetQuestionsByIds(IList<int> questionIds)
    {
        var questions = new List<Question>();

        foreach (var questionId in questionIds)
        {
            if (Questions.TryGetValue(questionId, out var questionToAdd))
            {
                questions.Add(questionToAdd);
            }
        }

        return questions;
    }

    public static Question GetQuestionById(int questionId)
    {
        if (Questions.TryGetValue(questionId, out var question))
            return question;

        throw new Exception("Question not in Cache");
    }

    public static IList<Set> GetSetsByIds(IList<int> setIds)
    {
        var sets = new List<Set>();

        foreach (var id in setIds)
        {
            if (Sets.TryGetValue(id, out var setToAdd))
            {
                sets.Add(setToAdd);
            }
        }

        return sets;
    }

    public static IList<Set> GetAllSets() => Sets.Values.ToList();

    private static void AddQuestionInSetTo(
        ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>> categoryQuestionInSetList,
        QuestionInSet questionInSet)
    {
        foreach (var category in questionInSet.Set.Categories)
        {
            var set = questionInSet.Set;
            var question = questionInSet.Question;

            categoryQuestionInSetList.AddOrUpdate(category.Id, new ConcurrentDictionary<int, ConcurrentDictionary<int, int>>(), (k, existingList) => existingList);

            categoryQuestionInSetList[category.Id]?.AddOrUpdate(question.Id, new ConcurrentDictionary<int, int>(), (k, existingList) => existingList);

            categoryQuestionInSetList[category.Id]?[question.Id]?.AddOrUpdate(set.Id, 0, (k, v) => 0);
        }
    }

    private static void RemoveQuestionInSetFrom(
        ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>> categoryQuestionInSetList,
        QuestionInSet questionInSet)
    {
        foreach (var category in questionInSet.Set.Categories)
        {
            categoryQuestionInSetList[category.Id]?[questionInSet.Question.Id]?.TryRemove(questionInSet.Set.Id, out var removedQuestionInSet);
        }
    }

    private static void UpdateCategoryQuestionList(
        ConcurrentDictionary<int, ConcurrentDictionary<int, int>> categoryQuestionsList, 
        Question question, 
        List<int> affectedCategoryIds = null)
    {
        DeleteQuestionFromRemovedCategories(question, categoryQuestionsList, affectedCategoryIds);

        AddQuestionToCategories(question, categoryQuestionsList);
    }

    private static void UpdateCategorySetList(
        ConcurrentDictionary<int, ConcurrentDictionary<int, int>> categorySetsList,
        Set set,
        List<int> affectedCategoryIds = null)
    {
        DeleteSetFromRemovedCategories(set, categorySetsList, affectedCategoryIds);

        AddSetToCategories(set, categorySetsList);
    }

    private static void DeleteQuestionFromRemovedCategories(
        Question question, 
        ConcurrentDictionary<int, ConcurrentDictionary<int, int>> categoryQuestionsList,
        List<int> affectedCategoryIds = null)
    {
        if (affectedCategoryIds != null)
        {
            foreach (var categoryId in affectedCategoryIds.Except(question.Categories.GetIds()))
            {
                if (categoryQuestionsList.ContainsKey(categoryId))
                    categoryQuestionsList[categoryId]?.TryRemove(question.Id, out var outVar);
            }
        }
    }

    private static void DeleteSetFromRemovedCategories(
        Set set,
        ConcurrentDictionary<int, ConcurrentDictionary<int, int>> categorySetsList,
        List<int> affectedCategoryIds = null)
    {
        if (affectedCategoryIds != null)
        {
            foreach (var categoryId in affectedCategoryIds.Except(set.Categories.GetIds()))
            {
                if (categorySetsList.ContainsKey(categoryId))
                    categorySetsList[categoryId]?.TryRemove(set.Id, out var questionOut);
            }
        }
    }

    private static void UpdateCategoryQuestionInSetList(
        ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>> categoryQuestionsInSetList,
        Set set,
        List<int> affectedCategoryIds = null)
    {
        foreach (var questionInSet in set.QuestionsInSet)
        {
            DeleteQuestionInSetFromRemovedCategories(questionInSet, categoryQuestionsInSetList, affectedCategoryIds);

            AddQuestionInSetTo(categoryQuestionsInSetList, questionInSet);
        }
    }

    private static void DeleteQuestionInSetFromRemovedCategories(QuestionInSet questionInSet, ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>> categoryQuestionsInSetList, List<int> affectedCategoryIds)
    {
        if (affectedCategoryIds != null)
        {
            foreach (var categoryId in affectedCategoryIds.Except(questionInSet.Set.Categories.GetIds()))
            {
                if (categoryQuestionsInSetList.ContainsKey(categoryId))
                    categoryQuestionsInSetList[categoryId]?[questionInSet.Question.Id]?.TryRemove(questionInSet.Set.Id, out var outValue);
            }
        }
    }

    private static void AddQuestionToCategories(
        Question question,
        ConcurrentDictionary<int, ConcurrentDictionary<int, int>> categoryQuestionsList,
        IList<Category> categories = null)
    {
        if (categories == null)
        {
            categories = question.Categories;
        }

        foreach (var category in categories)
        {
            try
            {
                categoryQuestionsList.AddOrUpdate(category.Id, new ConcurrentDictionary<int, int>(),
                    (k, existingList) => existingList);

                categoryQuestionsList[category.Id]?.AddOrUpdate(question.Id, 0, (k, v) => 0);
            }
            catch { } 
        }
    }

    private static void RemoveQuestionFrom(ConcurrentDictionary<int, ConcurrentDictionary<int, int>> categoryQuestionList, Question question)
    {
        foreach (var category in question.Categories)
        {
            var questionsInCategory = categoryQuestionList[category.Id];
            questionsInCategory.TryRemove(question.Id, out var outVar);
        }
    }

    private static void AddSetToCategories(
        Set set,
        ConcurrentDictionary<int, ConcurrentDictionary<int, int>> categorySetsList)
    {
        foreach (var category in set.Categories)
        {
            categorySetsList.AddOrUpdate(category.Id, new ConcurrentDictionary<int, int>(), (k, existingList) => existingList);

            categorySetsList[category.Id]?.AddOrUpdate(set.Id, 0, (k, v) => 0);
        }
    }

    private static void RemoveSetFrom(ConcurrentDictionary<int, ConcurrentDictionary<int, int>> categorySetList, Set set)
    {
        foreach (var category in set.Categories)
        {
            var questionsInCategory = categorySetList[category.Id];
            questionsInCategory.TryRemove(set.Id, out var setOut);
        }
    }

    public static void AddOrUpdate(Question question, List<int> affectedCategoriesIds = null)
    {
        AddOrUpdate(Questions, question);
        UpdateCategoryQuestionList(CategoryQuestionsList, question, affectedCategoriesIds);
    }

    public static void Remove(Question question)
    {
        Remove(Questions, question);
        RemoveQuestionFrom(CategoryQuestionsList, question);
    }

    public static void AddOrUpdate(Category category)
    {
        AddOrUpdate(Categories, category);

        UpdateCategoryForQuestions(category);
        UpdateCategoryForSets(category);
    }

    private static void UpdateCategoryForSets(Category category)
    {
        var affectedSetIds = GetSetIdsForCategory(category.Id);

        foreach (var setId in affectedSetIds)
        {
            if (Sets.TryGetValue(setId, out var set))
            {
                var categoryToReplace = set.Categories.FirstOrDefault(c => c.Id == category.Id);

                if(categoryToReplace == null) return;

                var index = set.Categories.IndexOf(categoryToReplace);
                set.Categories[index] = category;
            }
        }
    }

    private static void UpdateCategoryForQuestions(Category category)
    {
        var affectedQuestionsIds = GetQuestionsIdsForCategory(category.Id);

        foreach (var questionId in affectedQuestionsIds)
        {
            if (Questions.TryGetValue(questionId, out var question))
            {
                var categoryToReplace = question.Categories.FirstOrDefault(c => c.Id == category.Id);

                if(categoryToReplace == null) return;

                var index = question.Categories.IndexOf(categoryToReplace);
                question.Categories[index] = category;
            }
        }
    }

    public static void Remove(Category category)
    {
        Remove(Categories, category);
        CategoryQuestionsList.TryRemove(category.Id, out var catOut);
        CategorySetsList.TryRemove(category.Id, out var catOut2);
        CategoryQuestionInSetList.TryRemove(category.Id, out var listOut);
    }

    public static void AddOrUpdate(Set set, List<int> affectedCategoriesIds = null)
    {
        AddOrUpdate(Sets, set);
        UpdateCategorySetList(CategorySetsList, set, affectedCategoriesIds);
        UpdateCategoryQuestionInSetList(CategoryQuestionInSetList, set, affectedCategoriesIds);
    }

    public static void Remove(Set set)
    {
        Remove(Sets, set);
        RemoveSetFrom(CategorySetsList, set);

        foreach (var questionInSet in set.QuestionsInSet)
        {
            RemoveQuestionInSetFrom(CategoryQuestionInSetList, questionInSet);
        }
    }

    public static void AddOrUpdate(QuestionInSet questionInSet)
    {
        AddOrUpdateQuestionsInSet(Sets, questionInSet);
        AddQuestionInSetTo(CategoryQuestionInSetList, questionInSet);
    }

    private static void AddOrUpdateQuestionsInSet(ConcurrentDictionary<int, Set> sets, QuestionInSet questionInSet)
    {
        if (sets.TryGetValue(questionInSet.Set.Id, out var outSet))
        {
            outSet.QuestionsInSet = new HashSet<QuestionInSet>(outSet.QuestionsInSet.Where(q => q.Id != questionInSet.Id));
            outSet.QuestionsInSet.Add(questionInSet);

            foreach (var questionInSetItem in outSet.QuestionsInSet)
            {
                questionInSetItem.Set = outSet;
            }
        }
    }

    public static void Remove(QuestionInSet questionInSet)
    {
        RemoveQuestionInSetFrom(CategoryQuestionInSetList, questionInSet);
        RemoveQuestionFromSet(Sets, questionInSet);
    }

    public static void RemoveQuestionFromSet(ConcurrentDictionary<int, Set> sets, QuestionInSet questionInSet)
    {
        if (sets.TryGetValue(questionInSet.Set.Id, out var outSet))
        {
            outSet.QuestionsInSet = new HashSet<QuestionInSet>(outSet.QuestionsInSet.Where(q => q.Id != questionInSet.Id));
        }
    }

    /// <summary>
    /// Not ThreadSafe! 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objectToCache"></param>
    /// <param name="obj"></param>
    private static void AddOrUpdate<T>(ConcurrentDictionary<int, T> objectToCache, T obj) where T : DomainEntity
    {
        objectToCache.AddOrUpdate(obj.Id, obj, (k, v) => obj);
    }

    private static void Remove<T>(ConcurrentDictionary<int, T> objectToCache, T obj) where T : DomainEntity
    {
        objectToCache.TryRemove(obj.Id, out var outObj);
    }


    public static Category GetCategory(int categoryId) => Categories[categoryId];

    public static IEnumerable<Category> GetCategories(IEnumerable<int> getIds) => 
        getIds.Select(categoryId => Categories[categoryId]);

    public static IEnumerable<Category> GetAllCategories() => Categories.Values.ToList();

    /// <summary>
    /// Helps do debug, e.g. filter CategoryQuestionInSetList for certain questions in certain categories and the belonging set ids
    /// </summary>
    public static ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>>
        FilterThreeLevelConcurrentDictionary(
            ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>> dictionary, IList<int> firstLevelFilterIds, IList<int> secondLevelFilterIds)
    {
        var filteredDict = new ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>>();

        foreach (var secondLevelFilterId in secondLevelFilterIds)
        {
            var listOfDict = firstLevelFilterIds.Select(id => new {Id = id, Dictionary = dictionary[id]}).ToList();

            listOfDict.ForEach(d =>
            {
                if (d.Dictionary.TryGetValue(secondLevelFilterId, out var outDict))
                {
                    filteredDict.TryAdd(d.Id, new ConcurrentDictionary<int, ConcurrentDictionary<int, int>>());
                    filteredDict[d.Id].TryAdd(secondLevelFilterId, outDict);
                }
            });

        }

        return filteredDict;
    }
}