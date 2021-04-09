﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentNHibernate.Conventions;

public class GraphService
{
    public static IList<CategoryCacheItem> GetAllParents(int categoryId, bool getFromEntityCache = false) =>
       GetAllParents(EntityCache.GetCategoryCacheItem(categoryId, getDataFromEntityCache: getFromEntityCache));

    public static IList<CategoryCacheItem> GetAllParents(CategoryCacheItem category)
    {
        var parentIds = GetDirektParents(category);
        var allParents = new List<CategoryCacheItem>();
        var deletedIds = new Dictionary<int, int>();

        while (parentIds.Count > 0)
        {
            var parent = EntityCache.GetCategoryCacheItem(parentIds[0]);

            if (!deletedIds.ContainsKey(parentIds[0]))
            {
                allParents.Add(parent);//Avoidance of circular references

                deletedIds.Add(parentIds[0], parentIds[0]);
                var currentParents = GetDirektParents(parent);
                foreach (var currentParent in currentParents)
                {
                    parentIds.Add(currentParent);
                }
            }

            parentIds.RemoveAt(0);
        }

        return allParents;
    }

    public static List<int> GetDirektParents(CategoryCacheItem category)
    {
        return category.CategoryRelations
            .Where(cr => cr.CategoryRelationType == CategoryRelationType.IsChildCategoryOf)
            .Select(cr => cr.RelatedCategoryId).ToList();
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
        var cacheItemWithChildren = AddChildrenToCategory(listAsConcurrentDictionary);

         foreach (var categoryCacheItem in cacheItemWithChildren.Values)
         {
             
             var childrenOuter = categoryCacheItem.CachedData.ChildrenIds.DeepClone(); 
             
             while (childrenOuter.Count > 0)
             {
                 
                 listAsConcurrentDictionary.TryGetValue(childrenOuter[0], out var value);
                 childrenOuter.RemoveAt(0);
                 categoryCacheItem.CategoryRelations.Add(new CategoryCacheRelation
                 {
                     CategoryRelationType = CategoryRelationType.IncludesContentOf,
                     RelatedCategoryId = value.Id,
                     CategoryId = categoryCacheItem.Id
                 });

                foreach (var cachedDataChildrenId in value.CachedData.ChildrenIds)
                {
                    childrenOuter.Add(cachedDataChildrenId);
                }
             } 
         }

         return cacheItemWithChildren.Values.ToList(); 
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

    public static void AutomaticInclusionOfChildCategoriesForEntityCacheAndDb(Category category)
    {
        var parentsFromParentCategories = GetAllParents(category.Id,true);

        foreach (var parentCategory in parentsFromParentCategories)
            ModifyRelationsForCategory.UpdateRelationsOfTypeIncludesContentOf(EntityCache.GetCategoryCacheItem(parentCategory.Id));
    }

    public static void AutomaticInclusionOfChildCategoriesForUsérEntityCache(CategoryCacheItem category, CategoryRepository.CreateDeleteUpdate createDeleteUpdate)
    {
        switch (createDeleteUpdate)
        {
            case CategoryRepository.CreateDeleteUpdate.Create:
                var parentsFromParentCategories = GetAllParents(category.Id).ToList();
                ModifyRelationsUserEntityCache.CreateRelationsIncludetContentOf(parentsFromParentCategories, category);
                break;
        }
        
        
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

}
