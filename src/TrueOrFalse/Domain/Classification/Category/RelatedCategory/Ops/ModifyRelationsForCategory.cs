using System.Collections.Generic;
using System.Linq;

public class ModifyRelationsForCategory
{
    /// <summary>
    /// Updates relations with relatedCategories (keeps existing and deletes missing) with possible restrictions on type of relation (IsChildOf etc.) and type of category (Standard, Book etc.)
    /// </summary>
    /// <param name="category"></param>
    /// <param name="relatedCategories">Existing relations are updated with this collection (existing are kept, non-included are deleted)</param>
    /// <param name="relationType">If specified only relations of this type will be updated</param>
    /// <param name="relatedCategoriesType">If specified only relations with related category of this type will be updated</param>
    public static void UpdateCategoryRelationsOfType(
        Category category,
        IList<Category> relatedCategories, 
        CategoryRelationType relationType,
        CategoryType relatedCategoriesType = CategoryType.None)
    {
        var existingRelationsOfType = category.CategoryRelations.Any()
            ? category.CategoryRelations?.Where(r => r.CategoryRelationType == relationType).ToList()
            : new List<CategoryRelation>();

        if (existingRelationsOfType.Any() && relatedCategoriesType != CategoryType.None)
        {
            existingRelationsOfType = existingRelationsOfType
                .Where(r => r.RelatedCategory.Type == relatedCategoriesType)
                .ToList();
        }

        var relationsToAdd = relatedCategories
            .Except(existingRelationsOfType.Select(r => r.RelatedCategory))
            .Select(c => new CategoryRelation{Category = category, RelatedCategory = c, CategoryRelationType = relationType});

        var relationsToRemove = existingRelationsOfType.Where(r => relatedCategories.All(c => c != r.RelatedCategory)).ToList();

        foreach (var relation in relationsToAdd)
            category.CategoryRelations.Add(relation);

        foreach (var relation in relationsToRemove)
            category.CategoryRelations.Remove(relation);
    }

    public static void AddCategoryRelationOfType(Category category, Category relatedCategory, CategoryRelationType relationType)
    {
        if(category.CategoryRelations.Any(r => r.RelatedCategory == relatedCategory && r.CategoryRelationType == relationType))
            return;

        category.CategoryRelations.Add(
            new CategoryRelation
            {
                Category = category,
                RelatedCategory = relatedCategory,
                CategoryRelationType = relationType 
            });
    }

    public static void AddParentCategory(Category category, Category relatedCategory)
    {
        AddCategoryRelationOfType(category, relatedCategory, CategoryRelationType.IsChildCategoryOf);
    }

    public static void UpdateRelationsOfTypeIncludesContentOf(Category category, bool persist = true)
    {
        var catRepo = Sl.CategoryRepo;

        var descendants = GetCategoriesDescendants.WithAppliedRules(category);

        UpdateCategoryRelationsOfType(category, descendants, CategoryRelationType.IncludesContentOf);

        if(!persist)
            return;

        catRepo.Update(category);

        category.UpdateAggregatedContentJson();

        catRepo.Update(category);

        KnowledgeSummaryUpdate.RunForCategory(category.Id);
    }
}
