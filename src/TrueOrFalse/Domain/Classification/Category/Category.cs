﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Seedworks.Lib.Persistence;

[DebuggerDisplay("Id={Id} Name={Name}")]
[Serializable]
public class Category : DomainEntity, ICreator
{
    public virtual string Name { get; set; }

    public virtual string Description { get; set; }

    public virtual string WikipediaURL { get; set; }

    public virtual bool DisableLearningFunctions { get; set; }

    public virtual User Creator { get; set; }

    public virtual IList<CategoryRelation> CategoryRelations { get; set; }

    public virtual IList<Category> ParentCategories()
    {
        return CategoryRelations.Any()
            ? CategoryRelations
                .Where(r => r.CategoryRelationType == CategoryRelationType.IsChildCategoryOf)
                .Select(x => x.RelatedCategory)
                .ToList()
            : new List<Category>();
    }

    public virtual string CategoriesToExcludeIdsString { get; set; }

    public virtual string CategoriesToIncludeIdsString { get; set; }

    public virtual IList<Category> CategoriesToInclude()
    {
        return !string.IsNullOrEmpty(CategoriesToIncludeIdsString)
            ? Sl.R<CategoryRepository>().GetByIdsFromString(CategoriesToIncludeIdsString)
            : new List<Category>();
    }

    public virtual IList<Category> CategoriesToExclude()
    {
        return !string.IsNullOrEmpty(CategoriesToExcludeIdsString)
            ? Sl.R<CategoryRepository>().GetByIdsFromString(CategoriesToExcludeIdsString)
            : new List<Category>();
    }

    public virtual IList<Category> AggregatedCategories()
    {
        return Sl.R<CategoryRepository>().GetAggregatedCategories(this);
    }

    public virtual IList<Category> NonAggregatedCategories()
    {
        return Sl.R<CategoryRepository>()
            .GetDescendants(Id)
            .Except(AggregatedCategories())
            .Except(CategoriesToExclude())
            .Distinct()
            .ToList();
    }

    public virtual string FeaturedSetsIdsString { get; set; }

    public virtual string TopicMarkdown { get; set; }
        
    public virtual int CountQuestions { get; set; }
    public virtual int CountSets { get; set; }
    public virtual int CountCreators { get; set; }

    public virtual CategoryType Type { get; set; }

    public virtual string TypeJson { get; set; }

    public virtual int CorrectnessProbability { get; set; }
    public virtual int CorrectnessProbabilityAnswerCount { get; set; }

    public virtual int TotalRelevancePersonalEntries { get; set; }

    public Category(){
        CategoryRelations = new List<CategoryRelation>();
        Type = CategoryType.Standard;
    }

    public Category(string name) : this(){
        Name = name;
    }

    public virtual bool IsSpoiler(Question question) => 
        IsSpoilerCategory.Yes(Name, question);

    public virtual IList<Set> FeaturedSets()
    {
        if (string.IsNullOrEmpty(FeaturedSetsIdsString))
            return new List<Set>();

        var setIds = FeaturedSetsIdsString
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Convert.ToInt32(x));

        var setRepo = Sl.R<SetRepo>();

        return setIds
            .Select(setId => setRepo.GetById(setId))
            .Where(set => set != null)
            .ToList();
    }

    public virtual IList<Set> GetSets(bool featuredSetsOnlyIfAny = false)
    {
        var featuredSets = FeaturedSets();
        if (featuredSets.Count > 0 && featuredSetsOnlyIfAny)
        {
            return featuredSets;
        }
                    
        return Sl.R<SetRepo>().GetForCategory(Id);
    }

    public virtual IList<Set> GetAggregatedSets()
    {
        var aggregatedCategories = Sl.R<CategoryRepository>().GetAggregatedCategories(this);
        var aggregatedSets = new List<Set>();

        foreach (var category in aggregatedCategories)
        {
            aggregatedSets.AddRange(category.GetSets());
        }

        return aggregatedSets.Distinct().ToList();
    }

    public virtual object GetTypeModel()
    {
        if (Type == CategoryType.Standard)
            return CategoryTypeStandard.FromJson(this);

        if (Type == CategoryType.Book)
            return CategoryTypeBook.FromJson(this);

        if (Type == CategoryType.Daily)
            return CategoryTypeDaily.FromJson(this);

        if (Type == CategoryType.DailyArticle)
            return CategoryTypeDailyArticle.FromJson(this);

        if (Type == CategoryType.DailyIssue)
            return CategoryTypeDailyIssue.FromJson(this);

        if (Type == CategoryType.Magazine)
            return CategoryTypeMagazine.FromJson(this);

        if (Type == CategoryType.MagazineArticle)
            return CategoryTypeMagazineArticle.FromJson(this);

        if (Type == CategoryType.MagazineIssue)
            return CategoryTypeMagazineIssue.FromJson(this);

        if (Type == CategoryType.VolumeChapter)
            return CategoryTypeVolumeChapter.FromJson(this);

        if (Type == CategoryType.Website)
            return CategoryTypeWebsite.FromJson(this);

        if (Type == CategoryType.WebsiteArticle)
            return CategoryTypeWebsiteArticle.FromJson(this);

        if (Type == CategoryType.WebsiteVideo)
            return CategoryTypeWebsiteVideo.FromJson(this);

        if (Type == CategoryType.SchoolSubject)
            return CategoryTypeSchoolSubject.FromJson(this);

        if (Type == CategoryType.FieldOfStudy)
            return CategoryTypeFieldOfStudy.FromJson(this);

        if (Type == CategoryType.FieldOfTraining)
            return CategoryTypeFieldOfTraining.FromJson(this);

        throw new Exception("Invalid type.");
    }
}