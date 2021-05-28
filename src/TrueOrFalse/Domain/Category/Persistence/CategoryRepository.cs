using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Search;

public class CategoryRepository : RepositoryDbBase<Category>
{
    private readonly SearchIndexCategory _searchIndexCategory;

    public CategoryRepository(ISession session, SearchIndexCategory searchIndexCategory)
        : base(session)
    {
        _searchIndexCategory = searchIndexCategory;
        _searchIndexCategory = searchIndexCategory;
    }

    public Category GetByIdEager(int categoryId) => GetByIdsEager(new[] { categoryId }).FirstOrDefault();
    public Category GetByIdEager(CategoryCacheItem category) => GetByIdsEager(new[] { category.Id }).FirstOrDefault();

    public IList<Category> GetByIdsEager(IEnumerable<int> categoryIds = null)
    {
        var query = _session.QueryOver<Category>();
        if (categoryIds != null)
            query = query.Where(Restrictions.In("Id", categoryIds.ToArray()));
        else
        {
            //warmup entity cache
            var users = _session
                .QueryOver<User>()
                .Fetch(SelectMode.Fetch, u => u.MembershipPeriods)
                .List();
        }

        var result = query.Left.JoinQueryOver<CategoryRelation>(s => s.CategoryRelations)
            .Left.JoinQueryOver(x => x.RelatedCategory)
            .List()
            .GroupBy(c => c.Id)
            .Select(c => c.First())
            .ToList();

        foreach (var category in result)
        {
            NHibernateUtil.Initialize(category.Creator);
            NHibernateUtil.Initialize(category.CategoryRelations);
        }
        return result;
    }

    public IList<Category> GetAllEager() => GetByIdsEager();

    public override void Create(Category category)
    {
        foreach (var related in category.ParentCategories().Where(x => x.DateCreated == default(DateTime)))
            related.DateModified = related.DateCreated = DateTime.Now;

        base.Create(category);
        Flush();

        UserActivityAdd.CreatedCategory(category);
        _searchIndexCategory.Update(category);

        var categoryCacheItem = CategoryCacheItem.ToCacheCategory(category);
        EntityCache.AddOrUpdate(categoryCacheItem);

        Sl.CategoryChangeRepo.AddCreateEntry(category, category.Creator);
        GraphService.AutomaticInclusionOfChildCategoriesForEntityCacheAndDbCreate(categoryCacheItem);

        if (UserEntityCache.HasUserCache(Sl.CurrentUserId))
        {
            UserEntityCache.Add(categoryCacheItem, Sl.CurrentUserId);
            ModifyRelationsUserEntityCache.AddToParents(categoryCacheItem);
        }
        UpdateCachedData(categoryCacheItem, CreateDeleteUpdate.Create); 
    }

    public static void UpdateCachedData(CategoryCacheItem categoryCacheItem, CreateDeleteUpdate createDeleteUpdate)
    {
        //Create
        if (createDeleteUpdate == CreateDeleteUpdate.Create)
        {
            //UpdateUserCache
            if (UserEntityCache.GetUserCache(Sl.CurrentUserId) != null)
            {
                var parentIds = UserEntityCache.GetParentsIds(Sl.CurrentUserId, categoryCacheItem.Id);
                foreach (var parentId in parentIds)
                {
                    UserEntityCache.GetCategory(Sl.CurrentUserId, parentId).CachedData.ChildrenIds.Add(categoryCacheItem.Id); 
                }
            }

            //Update EntityCache
            var parents = EntityCache.GetCategoryCacheItems(GraphService.GetDirectParents(categoryCacheItem));
            foreach (var parent in parents)
            {
                parent.CachedData.ChildrenIds.Add(categoryCacheItem.Id);
            }
        }

        //Update
        if (createDeleteUpdate == CreateDeleteUpdate.Update)
        {
            var allUserEntityCaches = UserEntityCache.GetAllCaches();
            foreach (var userEntityCacheWithUser in allUserEntityCaches)
            {
                var userEntityCache = userEntityCacheWithUser.Value;

                if (userEntityCache.ContainsKey(categoryCacheItem.Id))
                {
                    userEntityCache.TryGetValue(categoryCacheItem.Id, out var oldCategoryCacheItem);

                    var parentIdsCacheItem = categoryCacheItem.CategoryRelations
                        .Where(cr => cr.CategoryRelationType == CategoryRelationType.IsChildOf)
                        .Select(cr => cr.RelatedCategoryId).ToList();

                    var parentIdsOldCategoryCacheItem = oldCategoryCacheItem.CategoryRelations
                        .Where(cr => cr.CategoryRelationType == CategoryRelationType.IsChildOf)
                        .Select(cr => cr.RelatedCategoryId).ToList();

                    var exceptIdsToDelete = parentIdsOldCategoryCacheItem.Except(parentIdsCacheItem).ToList();
                    var exceptIdsToAdd = parentIdsCacheItem.Except(parentIdsOldCategoryCacheItem).ToList();
                    var hasAllIds = true;


                    if (exceptIdsToAdd.Any())
                        foreach (var id in exceptIdsToAdd)
                            if (userEntityCache.ContainsKey(id))
                                userEntityCache[id].CachedData.ChildrenIds.Add(categoryCacheItem.Id);
                            else
                            {
                                hasAllIds = false;
                                break;
                            }

                    if (exceptIdsToDelete.Any())
                        foreach (var id in exceptIdsToDelete)
                            if (userEntityCache.ContainsKey(id) && hasAllIds)
                                userEntityCache[id].CachedData.ChildrenIds.Remove(categoryCacheItem.Id);
                            else
                            {
                                hasAllIds = false;
                                break;
                            }

                    var userId = userEntityCacheWithUser.Key;
                    if (!hasAllIds)
                        UserEntityCache.Init(userId);

                }
            }

            var oldCategoryCacheItem1 = EntityCache.GetCategoryCacheItem(categoryCacheItem.Id, getDataFromEntityCache: true);

            var parentIdsCacheItem1 = categoryCacheItem.CategoryRelations
                .Where(cr => cr.CategoryRelationType == CategoryRelationType.IsChildOf)
                .Select(cr => cr.RelatedCategoryId).ToList();

            var parentIdsOldCategoryCacheItem1 = oldCategoryCacheItem1.CategoryRelations
                .Where(cr => cr.CategoryRelationType == CategoryRelationType.IsChildOf)
                .Select(cr => cr.RelatedCategoryId).ToList();

            var exceptIdsToDelete1 = parentIdsOldCategoryCacheItem1.Except(parentIdsCacheItem1).ToList();
            var exceptIdsToAdd1 = parentIdsCacheItem1.Except(parentIdsOldCategoryCacheItem1).ToList();

            if (exceptIdsToAdd1.Any() || exceptIdsToDelete1.Any())
            {
                foreach (var id in exceptIdsToAdd1)
                    EntityCache.GetCategoryCacheItem(id, getDataFromEntityCache: true).CachedData.ChildrenIds
                        .Add(categoryCacheItem.Id);

                foreach (var id in exceptIdsToDelete1)
                    EntityCache.GetCategoryCacheItem(id, getDataFromEntityCache: true).CachedData.ChildrenIds.Remove(categoryCacheItem.Id);
            }
        }

        //Delete
        if (createDeleteUpdate == CreateDeleteUpdate.Delete)
        {
            // userEntityCaches
            var allCachesWithUser = UserEntityCache.GetAllCaches();
            foreach (var userCacheWithUser in allCachesWithUser)
            {
                var userCache = userCacheWithUser.Value;
                if (userCacheWithUser.Value.ContainsKey(categoryCacheItem.Id))
                {
                    userCache.TryGetValue(categoryCacheItem.Id, out var oldCategoryCacheItem);

                    foreach (var id in GetIdsToRemove(oldCategoryCacheItem))
                    {
                        userCache.TryGetValue(id, out var parent);
                        parent.CachedData.ChildrenIds.Remove(categoryCacheItem.Id);
                    }
                }
            }

            //EntityCache
            foreach (var parent in categoryCacheItem.ParentCategories(true))
            {
                parent.CachedData.ChildrenIds.Remove(categoryCacheItem.Id);
            }
        }
    }

    private static IEnumerable<int> GetIdsToRemove(CategoryCacheItem oldCategoryCacheItem)
    {
        return oldCategoryCacheItem.CategoryRelations
            .Where(cr => cr.CategoryRelationType == CategoryRelationType.IsChildOf)
            .Select(cr => cr.RelatedCategoryId);
    }

    public enum CreateDeleteUpdate
    {
        Create = 1,
        Delete = 2,
        Update = 3
    }

    /// <summary>
    /// Update method for internal purpose, takes care that no change sets are created.
    /// </summary>
    public override void Update(Category category) => Update(category);

    // ReSharper disable once MethodOverloadWithOptionalParameter
    public void Update(Category category, User author = null, bool imageWasUpdated = false, bool isFromModifiyRelations = false)
    {
        if (!isFromModifiyRelations) 
            _searchIndexCategory.Update(category);

        base.Update(category);

        if (author != null)
            Sl.CategoryChangeRepo.AddUpdateEntry(category, author, imageWasUpdated);

        Flush();

        Sl.R<UpdateQuestionCountForCategory>().Run(category);
    }

    public void UpdateWithoutCaches(Category category, User author = null, bool imageWasUpdated = false,
        bool isFromModifiyRelations = false)
    {
        if (!isFromModifiyRelations)
            _searchIndexCategory.Update(category);

        base.Update(category);

        if (author != null)
            Sl.CategoryChangeRepo.AddUpdateEntry(category, author, imageWasUpdated);

        Flush();

        Sl.R<UpdateQuestionCountForCategory>().Run(category);
    }




    public void UpdateBeforeEntityCacheInit(Category category)
    {
        _searchIndexCategory.Update(category);
        base.Update(category);

        Flush();
    }

    public override  void Delete(Category category)
    {
        _searchIndexCategory.Delete(category);
        base.Delete(category);

    }

    public override void DeleteWithoutFlush(Category category)
    {
        _searchIndexCategory.Delete(category);
        base.DeleteWithoutFlush(category);
        EntityCache.Remove(EntityCache.GetCategoryCacheItem(category.Id, getDataFromEntityCache: true));
        UserCache.RemoveAllForCategory(category.Id);
    }

    public IList<Category> GetByName(string categoryName)
    {
        categoryName = categoryName ?? "";

        return _session.CreateQuery("from Category as c where c.Name = :categoryName")
                        .SetString("categoryName", categoryName)
                        .List<Category>();
    }

    public IList<Category> GetByIds(List<int> questionIds)
    {
        return GetByIds(questionIds.ToArray());
    }

    public IList<Category> GetByIdsFromString(string idsString)
    {
        return idsString
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToInt32(x))
                .Select(GetById)
                .Where(set => set != null)
                .ToList();
    }

    public IList<Category> GetIncludingCategories(Category category, bool includingSelf = true)
    {
        var includingCategories = GetCategoriesIdsForRelatedCategory(category, CategoryRelationType.IncludesContentOf);

        if (includingSelf)
            includingCategories = includingCategories.Union(new List<Category> { category }).ToList();

        return includingCategories;
    }

    public IList<Category> GetCategoriesIdsForRelatedCategory(Category relatedCategory, CategoryRelationType relationType = CategoryRelationType.None)
    {
        var query = _session.QueryOver<CategoryRelation>()
            .Where(r => r.CategoryRelationType == CategoryRelationType.IncludesContentOf
                        && r.RelatedCategory == relatedCategory);

        if (relationType != CategoryRelationType.None)
        {
            query.Where(r => r.CategoryRelationType == relationType);
        }

        return query.List()
            .Select(r => r.Category)
            .ToList();
    }


    public IList<Category> GetChildren(int categoryId)
    {
        var categoryIds = _session.CreateSQLQuery($@"SELECT Category_id
            FROM relatedcategoriestorelatedcategories
            WHERE  Related_id = {categoryId} 
            AND CategoryRelationType = {(int)CategoryRelationType.IsChildOf}").List<int>();
        return GetByIds(categoryIds.ToArray());
    }

    public IList<Category> GetChildren(
        CategoryType parentType,
        CategoryType childrenType,
        int parentId,
        string searchTerm = "")
    {
        Category relatedCategoryAlias = null;
        Category categoryAlias = null;

        var query = Session
            .QueryOver<CategoryRelation>()
            .JoinAlias(c => c.RelatedCategory, () => relatedCategoryAlias)
            .JoinAlias(c => c.Category, () => categoryAlias)
            .Where(r =>
                r.CategoryRelationType == CategoryRelationType.IsChildOf
                && relatedCategoryAlias.Type == parentType
                && relatedCategoryAlias.Id == parentId
                && categoryAlias.Type == childrenType);

        if (!String.IsNullOrEmpty(searchTerm))
            query.WhereRestrictionOn(r => categoryAlias.Name)
                .IsLike(searchTerm);

        return query.Select(r => r.Category).List<Category>();
    }

    public IList<Category> GetDescendants(int parentId)
    {
        var currentGeneration = EntityCache
            .GetCategoryCacheItem(parentId)
            .CachedData.ChildrenIds
            .Select(id => Sl.CategoryRepo.GetByIdEager(id))
            .ToList();

        var nextGeneration = new List<Category>();
        var descendants = new List<Category>();

        while (currentGeneration.Count > 0)
        {
            descendants.AddRange(currentGeneration);

            foreach (var category in currentGeneration)
            {
                var children = EntityCache.GetCategoryCacheItem(category.Id)
                    .CachedData
                    .ChildrenIds
                    .Select(id => Sl.CategoryRepo.GetByIdEager(id))
                    .ToList();

                if (children.Count > 0)
                {
                    nextGeneration.AddRange(children);
                }
            }

            currentGeneration = nextGeneration.Except(descendants).Where(c => c.Id != parentId).Distinct().ToList();
            nextGeneration = new List<Category>();
        }

        return descendants;
    }

    public IList<UserTinyModel> GetAuthors(int categoryId, bool filterUsersForSidebar = false)
    {
        var allAuthors = Sl.CategoryChangeRepo
            .GetForCategory(categoryId, filterUsersForSidebar)
            .Select(categoryChange => new UserTinyModel(categoryChange.Author));

        return allAuthors.GroupBy(a => a.Id)
            .Select(groupedAuthor => groupedAuthor.First())
            .ToList();
    }

    public int CountAggregatedQuestions(int categoryId)
    {
        var count = _session.CreateSQLQuery($@"
        SELECT COUNT(DISTINCT(questionId)) FROM 
        (
	        SELECT DISTINCT(cq.Question_id) questionId
            FROM categories_to_questions cq
            WHERE cq.Category_id = {categoryId}

            UNION

            SELECT DISTINCT(qs.Question_id) questionId
	        FROM relatedcategoriestorelatedcategories rc
	        INNER JOIN category c
	        ON rc.Related_Id = c.Id
	        INNER JOIN categories_to_sets cs
	        ON c.Id = cs.Category_id
	        INNER JOIN questioninset qs
	        ON cs.Set_id = qs.Set_id
	        WHERE rc.Category_id = {categoryId}
	        AND rc.CategoryRelationType = {(int)CategoryRelationType.IncludesContentOf} 
	
	        UNION
	
	        SELECT DISTINCT(cq.Question_id) questionId 
	        FROM relatedcategoriestorelatedcategories rc
	        INNER JOIN category c
	        ON rc.Related_Id = c.Id
	        INNER JOIN categories_to_sets cs
	        ON c.Id = cs.Category_id
	        INNER JOIN categories_to_questions cq
	        ON c.Id = cq.Category_id
	        WHERE rc.Category_id = {categoryId}
	        AND rc.CategoryRelationType = {(int)CategoryRelationType.IncludesContentOf}
        ) c
        ").UniqueResult<long>();//Union is distinct by default
        return (int)count;
    }

    public override IList<Category> GetByIds(params int[] categoryIds)
    {
        var resultTmp = base.GetByIds(categoryIds);

        var result = new List<Category>();
        for (int i = 0; i < categoryIds.Length; i++)
        {
            if (resultTmp.Any(c => c.Id == categoryIds[i]))
                result.Add(resultTmp.First(c => c.Id == categoryIds[i]));
        }
        return result;
    }

    public IEnumerable<Category> GetWithMostQuestions(int amount)
    {
        return _session
            .QueryOver<Category>()
            .OrderBy(c => c.CountQuestionsAggregated).Desc
            .Take(amount)
            .List();
    }

    public IEnumerable<Category> GetMostRecent_WithAtLeast3Questions(int amount)
    {
        return _session
            .QueryOver<Category>()
            .Where(c => c.CountQuestionsAggregated > 3 || c.CountQuestions > 3)
            .OrderBy(c => c.DateCreated)
            .Desc
            .Take(amount)
            .List();
    }

    public bool Exists(string categoryName)
    {
        return GetByName(categoryName).Any(x => x.Type == CategoryType.Standard);
    }


    public int TotalCategoryCount()
    {
        return _session.QueryOver<Category>()
            .RowCount();
    }

    public const int AllgemeinwissenId = 709;
    public const int StudiumId = 687;
    public const int SchuleId = 682;
    public const int ZertifikateId = 689;

    public IEnumerable<int> GetRootCategoryInts() => GetRootCategoriesListÍds();

    public List<int> GetRootCategoriesListÍds()
    {
        return new List<int>
        {
            SchuleId,
            StudiumId,
            ZertifikateId,
            AllgemeinwissenId
        };
    }
}