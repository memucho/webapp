﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TrueOrFalse.Web;

public class CategoryModel : BaseModel
{
    public string MetaTitle;
    public string MetaDescription;

    public int Id;
    public string Name;
    public string Description;
    public string Type;

    public KnowledgeSummary KnowledgeSummary;

    public IList<Category> BreadCrumb;

    public string CustomPageHtml;//Is set in controller because controller context is needed
    public IList<Set> FeaturedSets;

    public IList<Category> CategoriesParent;
    public IList<Category> CategoriesChildren;

    public IList<Set> AggregatedSets;
    public int AggregatedSetCount;
    public int AggregatedQuestionCount;
    public IList<Question> TopQuestions;
    public IList<Question> TopQuestionsWithReferences;
    public List<Question> TopQuestionsInSubCats = new List<Question>();
    public IList<Question> TopWishQuestions;
    public IList<Question> SingleQuestions;

    public IList<User> TopCreaters;

    public User Creator;
    public string CreatorName;
    public string CreationDate;
    public string CreationDateNiceText;

    public Category Category;

    public ImageFrontendData ImageFrontendData;

    public string InfoUrl;

    public bool IsOwnerOrAdmin;

    public int CountAggregatedQuestions;
    public int CountReferences;
    public int CountWishQuestions;
    public int CountSets;

    public const int MaxCountQuestionsToDisplay = 20;

    public int CorrectnesProbability;
    public int AnswersTotal;

    private readonly QuestionRepo _questionRepo;
    private readonly CategoryRepository _categoryRepo;

    public bool IsInWishknowledge;

    public CategoryModel(Category category, bool loadKnowledgeSummary = true)
    {
        MetaTitle = category.Name;
        MetaDescription = SeoUtils.ReplaceDoubleQuotes(category.Description).Truncate(250, true);

        _questionRepo = R<QuestionRepo>();
        _categoryRepo = R<CategoryRepository>();

        if(loadKnowledgeSummary)
            KnowledgeSummary = KnowledgeSummaryLoader.RunFromMemoryCache(category.Id, UserId);

        IsInWishknowledge = Sl.CategoryValuationRepo.IsInWishKnowledge(category.Id, UserId);

        InfoUrl = category.WikipediaURL;
        Category = category;

        Id = category.Id;
        Name = category.Name;
        Description = string.IsNullOrEmpty(category.Description?.Trim())
                        ? null 
                        : MarkdownMarkdig.ToHtml(category.Description);
       
        Type = category.Type.GetShortName();
        BreadCrumb = GetBreadCrumb.For(Category);

        FeaturedSets = category.FeaturedSets();

        IsOwnerOrAdmin = _sessionUser.IsLoggedInUserOrAdmin(category.Creator.Id);

        CategoriesParent = category.ParentCategories();
        CategoriesChildren = _categoryRepo.GetChildren(category.Id);

        CorrectnesProbability = category.CorrectnessProbability;
        AnswersTotal = category.CorrectnessProbabilityAnswerCount;

        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(category.Id, ImageType.Category);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        var wishQuestions = _questionRepo.GetForCategoryAndInWishCount(category.Id, UserId, 5);

        CountAggregatedQuestions = category.CountQuestionsAggregated;
        CountReferences = ReferenceCount.Get(category.Id);

        if (category.Type != CategoryType.Standard)
            TopQuestionsWithReferences = Sl.R<ReferenceRepo>().GetQuestionsForCategory(category.Id);

        CountSets = category.GetCountSets();
        CountWishQuestions = wishQuestions.Total;

        TopQuestions = category.GetAggregatedQuestionsFromMemoryCache().Take(MaxCountQuestionsToDisplay).ToList();

        if (category.Type == CategoryType.Standard)
            TopQuestionsInSubCats = GetTopQuestionsInSubCats();

        TopWishQuestions = wishQuestions.Items;

        SingleQuestions = GetQuestionsForCategory.QuestionsNotIncludedInSet(Id);

        AggregatedSets = category.GetAggregatedSetsFromMemoryCache();
        AggregatedSetCount = AggregatedSets.Count;

        AggregatedQuestionCount = Category.CountQuestionsAggregated;
    }

    private List<Question> GetTopQuestionsInSubCats()
    {
        var topQuestions = new List<Question>();

        var categoryIds = CategoriesChildren.Take(10).Select(c => c.Id);
        topQuestions.AddRange(_questionRepo.GetForCategory(categoryIds, 15, UserId));

        if(topQuestions.Count < 7)
            GetTopQuestionsFromChildrenOfChildren(topQuestions);
                
        return topQuestions
            .Distinct(ProjectionEqualityComparer<Question>.Create(x => x.Id))
            .ToList();
    }

    private void GetTopQuestionsFromChildrenOfChildren(List<Question> topQuestions)
    {
        foreach (var childCat in CategoriesChildren)
            foreach (var childOfChild in _categoryRepo.GetChildren(childCat.Id))
                if (topQuestions.Count < 6)
                    topQuestions.AddRange(_questionRepo.GetForCategory(childOfChild.Id, UserId, 5));
    }

    public string GetViews() => Sl.CategoryViewRepo.GetViewCount(Id).ToString();
}