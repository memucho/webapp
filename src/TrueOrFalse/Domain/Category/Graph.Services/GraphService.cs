﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentNHibernate.Conventions;

public class GraphService
{
    public static IList<CategoryCacheItem> GetAllParents(int categoryId) =>
       GetAllParents(EntityCache.GetCategoryCacheItem(categoryId));

    public static IList<CategoryCacheItem> GetAllParents(CategoryCacheItem category)
    {
        category = category == null ? new CategoryCacheItem() : category;

        var currentGeneration = category.ParentCategories(); 
        var previousGeneration = new List<CategoryCacheItem>();
        var parents = new List<CategoryCacheItem>();  

        while (currentGeneration.Count > 0) 
        {
            parents.AddRange(currentGeneration);

            foreach (var currentCategory in currentGeneration)  //go through Parents 
            {
                var directParents = EntityCache.GetCategoryCacheItem(currentCategory.Id).ParentCategories(); //Parents of parents are not eagerly loaded. Because of that, we get the parents using the cache.
                if (directParents.Count > 0) // go through ParentParents
                {
                    previousGeneration.AddRange(directParents); // Add the ParentParents
                }
            }

            currentGeneration = previousGeneration
                .Except(parents)
                .Where(c => c.Id != category.Id)
                .Distinct()
                .ToList(); // ParentParents except the Parents and parentparentcategory.id is non equal categoryId 

            previousGeneration = new List<CategoryCacheItem>(); // clear list
        }
        return parents;
    }

    public static IList<CategoryCacheItem> GetAllPersonalCategoriesWithRelations_TP(CategoryCacheItem category, int userId = -1) =>
        GetAllPersonalCategoriesWithRelations(category.Id, userId, true);

    public static IList<CategoryCacheItem> GetAllPersonalCategoriesWithRelations(int rootCategoryId, int userId = -1, bool isFromUserEntityCache = false)
    {
        var rootCategory = EntityCache.GetCategoryCacheItem(rootCategoryId, isFromUserEntityCache).DeepClone();
        
        var children = EntityCache.GetDescendants(rootCategory.Id, true)
            .Distinct()
            .Where(c => c.IsInWishknowledge())
            .Select(c => c.DeepClone());

        var listWithUserPersonelCategories = new List<CategoryCacheItem>();

        userId = userId == -1 ? Sl.CurrentUserId : userId;

        var time = new Stopwatch();
        time.Start();

        foreach (var child in children)
        {
            var parents = GetParentsFromCategory(child.Id, isFromUserEntityCache).ToList();
            var hasRootInParents = parents.Any(id => id == rootCategoryId);
            child.CategoryRelations.Clear();
            listWithUserPersonelCategories.Add(child);

            while (parents.Count > 0)
            {
                var parentId = parents.First();

                if (UserCache.IsInWishknowledge(userId, parentId) || parentId == rootCategoryId && hasRootInParents)
                {
                    var categoryRelation = new CategoryCacheRelation()
                    {
                        CategoryRelationType = CategoryRelationType.IsChildCategoryOf,
                        CategoryId = child.Id,
                        RelatedCategoryId = parentId
                    };

                        var indexOfChild = listWithUserPersonelCategories.IndexOf(child);

                        if (listWithUserPersonelCategories[indexOfChild].CategoryRelations.All(cr =>
                            cr.RelatedCategoryId != categoryRelation.RelatedCategoryId)) // Not add if available
                            listWithUserPersonelCategories[indexOfChild]
                                .CategoryRelations
                                .Add(categoryRelation);

                        parents.Remove(parentId);
                }
                else
                {
                    var c = parentId;
                    var currentParents = GetParentsFromCategory(parentId, isFromUserEntityCache);
                    parents.Remove(parentId);

                    foreach (var cp in currentParents)
                    {
                        parents.Add(cp);
                        
                    }

                    parents = parents.Distinct().ToList();
                }
               
            }
        }
        
        foreach (var listWithUserPersonelCategory in listWithUserPersonelCategories)
        {
            if (listWithUserPersonelCategory.CategoryRelations.Count == 0)
            {
                listWithUserPersonelCategory.CategoryRelations.Add(new CategoryCacheRelation()
                {
                    CategoryRelationType = CategoryRelationType.IsChildCategoryOf,
                    RelatedCategoryId = rootCategory.Id,
                    CategoryId = listWithUserPersonelCategory.Id
                });
            }

            listWithUserPersonelCategory.CachedData.ChildrenIds = new List<int>(); 
        }
        rootCategory.CategoryRelations = new List<CategoryCacheRelation>();
        rootCategory.CachedData.ChildrenIds = new List<int>(); 
        listWithUserPersonelCategories.Add(rootCategory);

        var listAsConcurrentDictionary = listWithUserPersonelCategories.ToConcurrentDictionary();

        

        return AddChildrenToCategory(listAsConcurrentDictionary).Values.ToList();
    }

    public static ConcurrentDictionary<int, CategoryCacheItem> AddChildrenToCategory(ConcurrentDictionary<int, CategoryCacheItem> categoryList)
    {
        foreach (var category in categoryList.Values)
        {
            foreach (var categoryRelation in category.CategoryRelations)
            {
                if (categoryRelation.CategoryRelationType == CategoryRelationType.IsChildCategoryOf && categoryList.ContainsKey(categoryRelation.RelatedCategoryId))
                {
                    categoryList[categoryRelation.RelatedCategoryId].CachedData.ChildrenIds
                        .Add(categoryList[categoryRelation.CategoryId].Id);
                }
            }
        }

        foreach (var category in categoryList)
        {
            category.Value.CachedData.ChildrenIds = category.Value.CachedData.ChildrenIds.Distinct().ToList();
        }
        return categoryList;
    }


    private static IEnumerable<int> GetParentsFromCategory(int categoryId, bool isFromUserEntityCache = false)
    {

        if (!isFromUserEntityCache) {
            var userCacheCategory = UserEntityCache.GetCategory(Sl.CurrentUserId, categoryId);
            return userCacheCategory.CategoryRelations
                .Where(cr => cr.CategoryRelationType == CategoryRelationType.IsChildCategoryOf)
                .Select(cr => cr.RelatedCategoryId);
        }

        return EntityCache.GetCategoryCacheItem(categoryId, getDataFromEntityCache: true)
            .CategoryRelations.Where(cr => cr.CategoryRelationType == CategoryRelationType.IsChildCategoryOf)
            .Select(cr => cr.RelatedCategoryId);
    }

    public static void AutomaticInclusionOfChildCategories(Category category)
    {
        var parentsFromParentCategories = GetAllParents(category.Id);

        foreach (var parentCategory in parentsFromParentCategories)
            ModifyRelationsForCategory.UpdateRelationsOfTypeIncludesContentOf(EntityCache.GetCategoryCacheItem(parentCategory.Id));
    }

    public static bool IsCategoryParentEqual(IList<CategoryCacheItem> parent1 , IList<CategoryCacheItem> parent2)
    {
        if (parent1 == null || parent2 == null)
        {
            Logg.r().Error("parent1 or parent2 have a NullReferenceException");
            return false; 
        }
           

        if (parent1.Count != parent2.Count)
            return false;

        if (parent1.Count == 0 && parent2.Count == 0)
            return true;

        var result = parent1.Where(p => !parent2.Any(p2 => p2.Id == p.Id)).Count();

        return result == 0;
    }

    public static bool IsCategoryRelationEqual(CategoryCacheItem category1, CategoryCacheItem category2)
    {
        if (category1 != null && category2 != null || category1.CategoryRelations != null && category2.CategoryRelations != null)
        {
            var relations1 = category1.CategoryRelations;
            var relations2 = category2.CategoryRelations;

            if (relations2.Count != relations1.Count)
                return false;

            if (relations2.Count == 0 && relations1.Count == 0)
                return true;

            var count = 0; 

            var countVariousRelations = relations1.Count(r => !relations2.Any(r2 => r2.RelatedCategoryId == r.RelatedCategoryId && r2.CategoryId == r.CategoryId && r2.CategoryRelationType.ToString().Equals(r.CategoryRelationType.ToString())));
            return countVariousRelations == 0;
        }
        Logg.r().Error("Category or CategoryRelations have a NullReferenceException");
        return false;
    }
}
