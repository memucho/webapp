using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

[DebuggerDisplay("Id={Id} Name={Name}")]
[Serializable]
public class CategoryCacheItem
{
    public virtual int Id { get; set; }
    public virtual string Name { get; set; }

    public virtual DateTime DateCreated { get; set; }
    public virtual string Description { get; set; }

    public virtual string WikipediaURL { get; set; }

    public virtual string Url { get; set; }

    public virtual string UrlLinkText { get; set; }

    public virtual bool DisableLearningFunctions { get; set; }

    public virtual User Creator { get; set; }

    public virtual IList<CategoryCacheRelation> CategoryRelations { get; set; }
    public virtual int CountQuestions { get; set; }
    public virtual string TopicMarkdown { get; set; }
    public virtual string Content { get; set; }
    public virtual string CustomSegments { get; set; }

    public virtual CategoryType Type { get; set; }

    public virtual string TypeJson { get; set; }

    public virtual int CorrectnessProbability { get; set; }
    public virtual int CorrectnessProbabilityAnswerCount { get; set; }

    public virtual int TotalRelevancePersonalEntries { get; set; }
    public virtual bool IsHistoric { get; set; }

    public virtual int FormerSetId { get; set; }
    public virtual bool SkipMigration { get; set; }
    public virtual CategoryVisibility Visibility { get; set; }
    public virtual bool IsRootCategory => Id == RootCategory.RootCategoryId;

    public virtual IList<CategoryCacheItem> ParentCategories(bool getFromEntityCache = false)
    {
        return CategoryRelations.Any()
            ? CategoryRelations
                .Where(r => r.CategoryRelationType == CategoryRelationType.IsChildOf)
                .Select(x => EntityCache.GetCategoryCacheItem(x.RelatedCategoryId, getDataFromEntityCache: getFromEntityCache))
                .ToList()
            : new List<CategoryCacheItem>();
    }

    public virtual CategoryCachedData CachedData { get; set; } = new CategoryCachedData();

    public virtual string CategoriesToExcludeIdsString { get; set; }

    private IEnumerable<int> _categoriesToExcludeIds;
    public virtual IEnumerable<int> CategoriesToExcludeIds() =>
        _categoriesToExcludeIds ?? (_categoriesToExcludeIds = CategoriesToExcludeIdsString
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Convert.ToInt32(x)));

    private IEnumerable<int> _categoriesToIncludeIds;
    public virtual string CategoriesToIncludeIdsString { get; set; }

    public virtual IEnumerable<int> CategoriesToIncludeIds() =>
        _categoriesToIncludeIds ?? (_categoriesToIncludeIds = CategoriesToIncludeIdsString
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Convert.ToInt32(x)));

    public virtual IList<CategoryCacheItem> CategoriesToInclude()
    {
        return !string.IsNullOrEmpty(CategoriesToIncludeIdsString)
            ? ToCacheCategories(Sl.R<CategoryRepository>().GetByIdsFromString(CategoriesToIncludeIdsString)).ToList()
            : new List<CategoryCacheItem>();
    }

    public virtual IList<CategoryCacheItem> CategoriesToExclude()
    {
        return !string.IsNullOrEmpty(CategoriesToExcludeIdsString)
            ? ToCacheCategories(Sl.R<CategoryRepository>().GetByIdsFromString(CategoriesToExcludeIdsString)).ToList()
            : new List<CategoryCacheItem>();
    }

    public virtual IList<CategoryCacheItem> AggregatedCategories(bool includingSelf = true)
    {
        var list = new List<CategoryCacheItem>();
        list = EntityCache.GetCategoryCacheItem((Id)).CategoryRelations
            .Where(r => r.CategoryRelationType == CategoryRelationType.IncludesContentOf)
            .Select(r => EntityCache.GetCategoryCacheItem(r.RelatedCategoryId)).ToList();
        
        if (includingSelf)
            list.Add(this);

        return list;
    }
    public virtual int CountQuestionsAggregated { get; set; }

    public virtual void UpdateCountQuestionsAggregated()
    {
        CountQuestionsAggregated = GetCountQuestionsAggregated();
    }

    public virtual int GetCountQuestionsAggregated(bool inCategoryOnly = false, int categoryId = 0)
    {
        if (inCategoryOnly)
            return GetAggregatedQuestionsFromMemoryCache(true, false, categoryId).Count;

        return GetAggregatedQuestionsFromMemoryCache().Count;
    }

    public virtual IList<Question> GetAggregatedQuestionsFromMemoryCache(bool onlyVisible = true, bool fullList = true, int categoryId = 0)
    {
        IEnumerable<Question> questions;

        if (fullList)
            questions = AggregatedCategories()
                .SelectMany(c => EntityCache.GetQuestionsForCategory(c.Id))
                .Distinct();
        else
            questions = EntityCache.GetQuestionsForCategory(categoryId)
                .Distinct();


        if (onlyVisible)
            questions = questions.Where(q => q.IsVisibleToCurrentUser());

        return questions.ToList();
    }

    public virtual IList<int> GetAggregatedQuestionIdsFromMemoryCache()
    {
        return AggregatedCategories()
            .SelectMany(c => EntityCache.GetQuestionsIdsForCategory(c.Id))
            .Distinct()
            .ToList();
    }

    public virtual bool IsInWishknowledge() => UserCache.IsInWishknowledge(Sl.CurrentUserId, Id);

    public CategoryCacheItem()
    {
    }

    public CategoryCacheItem(string name)
    {
        Name = name;
    }

    public virtual bool IsSpoiler(Question question) =>
        IsSpoilerCategory.Yes(Name, question);

    public ConcurrentDictionary<int, CategoryCacheItem> ToConcurrentDictionary(ConcurrentDictionary<int, Category> concurrentDictionary)
    {
        var concDic = new ConcurrentDictionary<int, CategoryCacheItem>();

        foreach (var keyValuePair in concurrentDictionary)
        {
            concDic.TryAdd(keyValuePair.Key, ToCacheCategory(keyValuePair.Value));
        }

        return concDic;
    }

    public IEnumerable<CategoryCacheItem> ToIEnumerable(IEnumerable<Category> categoryList, bool withCachedData = false, bool withRealtions = false)
    {
        var categories = new List<CategoryCacheItem>();

        foreach (var category in categoryList)
        {
            categories.Add(ToCacheCategory(category));
        }

        return categories;
    }

    public static IEnumerable<CategoryCacheItem> ToCacheCategories(List<Category> categories) => categories.Select(c => ToCacheCategory(c));
    public static IEnumerable<CategoryCacheItem> ToCacheCategories(IEnumerable<Category> categories) => categories.Select(c => ToCacheCategory(c));

    public static CategoryCacheItem ToCacheCategory(Category category)
    {
        var userEntityCacheCategoryRelations = new CategoryCacheRelation();
        return new CategoryCacheItem
        {
            Id = category.Id,
            CachedData = new CategoryCachedData(),
            CategoryRelations = userEntityCacheCategoryRelations.ToListCategoryRelations(category.CategoryRelations),
            CategoriesToExcludeIdsString = category.CategoriesToExcludeIdsString,
            CategoriesToIncludeIdsString = category.CategoriesToIncludeIdsString,
            Content = category.Content,
            CorrectnessProbability = category.CorrectnessProbability,
            CorrectnessProbabilityAnswerCount = category.CorrectnessProbabilityAnswerCount,
            CountQuestions = category.CountQuestions,
            CountQuestionsAggregated = category.CountQuestionsAggregated,
            Creator = category.Creator,
            CustomSegments = category.CustomSegments,
            Description = category.Description,
            DisableLearningFunctions = category.DisableLearningFunctions,
            FormerSetId = category.FormerSetId,
            IsHistoric = category.IsHistoric,
            Name = category.Name,
            SkipMigration = category.SkipMigration,
            Visibility = category.Visibility,
            TopicMarkdown = category.TopicMarkdown,
            TotalRelevancePersonalEntries = 50,
            Type = category.Type,
            TypeJson = category.TypeJson,
            Url = category.Url,
            UrlLinkText = category.UrlLinkText,
            WikipediaURL = category.WikipediaURL,
            DateCreated = category.DateCreated
        };
    }

    public virtual bool IsVisibleToCurrentUser()
    {
        var creator = new UserTinyModel(Creator);
        return Visibility == CategoryVisibility.All || creator.Id == Sl.SessionUser.UserId;
    }

    public virtual bool IsNotVisibleToCurrentUser => !IsVisibleToCurrentUser();

    public virtual bool HasPublicParent() {
        return ParentCategories().Any(c => c.Visibility == CategoryVisibility.All);
    }
    public virtual bool HasRelation(CategoryCacheRelation newRelation)
    {
        foreach (var categoryRelation in this.CategoryRelations)
            if (CategoryCacheRelation.IsCategorRelationEqual(categoryRelation, newRelation))
                return true;

        return false;
    }
}