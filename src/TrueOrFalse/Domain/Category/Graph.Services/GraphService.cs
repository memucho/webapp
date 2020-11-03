﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Configuration;

public class GraphService
{
    public static IList<Category> GetAllParents(int categoryId) =>
        GetAllParents(Sl.CategoryRepo.GetById(categoryId));

    public static IList<Category> GetAllParents(Category category)
    {
        category = category == null ? new Category() : category;

        var currentGeneration = category.ParentCategories(); // get ParentCategorys
        var previousGeneration = new List<Category>(); // new List is empty
        var parents = new List<Category>();  // new List is empty
                                             //

        while (currentGeneration.Count > 0) // 
        {
            parents.AddRange(currentGeneration);

            foreach (var currentCategory in currentGeneration)  //go through Parents 
            {
                var directParents = currentCategory.ParentCategories(); // Get from all parents the Parents
                if (directParents.Count > 0) // go through ParentParents
                {
                    previousGeneration.AddRange(directParents); // Add the ParentParents
                }
            }

            currentGeneration = previousGeneration.Except(parents).Where(c => c.Id != category.Id).Distinct().ToList(); // ParentParents except the Parents and parentparentcategory.id is non equal categoryId 
            previousGeneration = new List<Category>(); // clear list
            // return in While loop
        }

        return parents;
    }


    public static List<Category> GetLastWuwiChildrenFromCategories(int categoryId)
    {
        var childrenReverse = EntityCache.GetDescendants(categoryId);

        var sUser = Sl.SessionUser.User;
        var lastChildren = childrenReverse.Where(c => EntityCache.GetChildren(c.Id).Count == 0 && c.IsInWishknowledge());

        return lastChildren.ToList(); 

    }



    public static IList<Category> GetAllPersonelCategoriesWithRealtions(int rootCategoryId)
    {

        var rootCategory = EntityCache.GetCategory(rootCategoryId);
        var children = EntityCache.GetDescendants(rootCategory); 
        var listWithUserPersonelCategories = new List<Category>();


        foreach (var child in children)
        {
            if (!child.IsInWishknowledge())
                continue;
             
            var parents = children.SelectMany(c => c.CategoryRelations.Where(cr=>cr.CategoryRelationType == CategoryRelationType.IsChildCategoryOf).Select(cr => cr.RelatedCategory)).ToList();

            if (parents.Count != 0)
            {
                foreach (var parent in parents)
                {
                    if (parent.IsInWishknowledge() || parent.Id == rootCategoryId)
                    {
                        child.CategoryRelations = null;
                        var categoryRelations = new List<CategoryRelation>();
                        var categoryRelation = new CategoryRelation
                        {
                            CategoryRelationType = CategoryRelationType.IsChildCategoryOf,
                            Category = child,
                            RelatedCategory = parent
                        };
                        categoryRelations.Add(categoryRelation);
                        child.CategoryRelations = categoryRelations;
                        listWithUserPersonelCategories.Add(child);
                    }
                        
                }

            }
        }

        return listWithUserPersonelCategories;

    }

    public static IList<Category> GetAllPersonelCategoriesWithRealtions(Category category) =>
        GetAllPersonelCategoriesWithRealtions(category.Id);

    public static void AutomaticInclusionFromSubthemes(Category category)
    {
        var parentsFromParentCategories = GraphService.GetAllParents(category);
        if (parentsFromParentCategories.Count != 0)
        {
            foreach (var parentCategory in parentsFromParentCategories)
            {
                ModifyRelationsForCategory.UpdateRelationsOfTypeIncludesContentOf(parentCategory);
            }
        }
    }

}
