using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Web;

public class CategoryModel : BaseContentModule 
{
    public string MetaTitle;
    public string MetaDescription;

    public int Id;
    public string Name;
    public string Description;

    public KnowledgeSummary KnowledgeSummary;

    public string CustomPageHtml;//Is set in controller because controller context is needed
    public CategoryChange CategoryChange;//Is set in controller because controller context is needed
    public bool NextRevExists;   //Is set in controller because controller context is needed
    public IList<CategoryCacheItem> CategoriesParent;
    public IList<CategoryCacheItem> CategoriesChildren;
    public IList<Question> AggregatedQuestions;
    public IList<Question> CategoryQuestions;
    public int AggregatedTopicCount;
    public IList<Question> TopQuestions;
    public IList<Question> TopQuestionsWithReferences;
    public List<Question> TopQuestionsInSubCats = new List<Question>();
    public IList<Question> TopWishQuestions;
    public IList<Question> SingleQuestions;
    public Question EasiestQuestion;
    public Question HardestQuestion;
    public bool IsInTopic = false;
    public bool IsInLearningTab = false;
    public bool IsInAnalyticsTab = false; 
    public UserTinyModel Creator;
    public string CreatorName;
    public string CreationDate;
    public string ImageUrl_250;
    public CategoryCacheItem Category;
    public ImageFrontendData ImageFrontendData;
    public string WikipediaURL;
    public string Url;
    public bool IsOwnerOrAdmin;
    public bool IsTestSession = false;
    public bool IsLearningSession = true;
    public int CountAggregatedQuestions;
    public int CountCategoryQuestions;
    public int CountReferences;
    public int CountWishQuestions;
    public int CountSets;
    public const int MaxCountQuestionsToDisplay = 20;
    public int CorrectnesProbability;
    public int AnswersTotal;
    private readonly QuestionRepo _questionRepo;
    private readonly CategoryRepository _categoryRepo;
    public bool IsInWishknowledge;
    public bool IsLearningTab;
    public string TotalPins;
    public bool CategoryIsDeleted;
    public IList<AuthorViewModel> Authors = new List<AuthorViewModel>();
    public bool ShowLearningSessionConfigurationMessageForTab { get; set; }
    public bool ShowLearningSessionConfigurationMessageForQuestionList { get; set; }
    public bool IsFilteredUserWorld;
    public string ImageIsNew { get; set; }
    public string ImageSource { get; set; }
    public string ImageWikiFileName { get; set; }

    public string ImageGuid { get; set; }
    public string ImageLicenseOwner { get; set; }
    public bool IsMyWorld { get; set; }

    public CategoryModel()
    {
    }

    public CategoryModel(CategoryCacheItem category, bool loadKnowledgeSummary = true, bool isCategoryNull = false)
    {
        IsMyWorld = UserCache.GetItem(Sl.CurrentUserId).IsFiltered; 
        TopNavMenu.BreadCrumbCategories = CrumbtrailService.Get(category, RootCategory.Get);
        CategoryIsDeleted = isCategoryNull;
        MetaTitle = category.Name;
        MetaDescription = SeoUtils.ReplaceDoubleQuotes(category.Description).Truncate(250, true);

        _questionRepo = R<QuestionRepo>();
        _categoryRepo = R<CategoryRepository>();

        if(loadKnowledgeSummary)
            KnowledgeSummary = isCategoryNull ? null :  KnowledgeSummaryLoader.RunFromMemoryCache(category.Id, UserId);

        var userValuationCategory = UserCache.GetCategoryValuations(UserId).Where(cv => cv.CategoryId == category.Id).ToList();
   
        if (userValuationCategory.Count() == 0)
            IsInWishknowledge = false;
        else 
            IsInWishknowledge = userValuationCategory.First().IsInWishKnowledge();

        WikipediaURL = category.WikipediaURL;
        Url = category.Url;
        Category = category;
        if (CategoryIsDeleted)
            Category.IsHistoric = true;

        Id = category.Id;
        Name = category.Name;
        Description = string.IsNullOrEmpty(category.Description?.Trim())
                        ? null 
                        : MarkdownMarkdig.ToHtml(category.Description);
       
        Type = category.Type.GetShortName();

        Creator = new UserTinyModel(category.Creator);
        CreatorName = Creator.Name;

        var imageResult = new UserImageSettings(Creator.Id).GetUrl_250px(Creator);
        ImageUrl_250 = imageResult.Url;
    
        var authors = _categoryRepo.GetAuthors(Id, filterUsersForSidebar: true);
        Authors = AuthorViewModel.Convert(authors);

        IsOwnerOrAdmin = _sessionUser.IsLoggedInUserOrAdmin(Creator.Id);

        CategoriesParent = category.ParentCategories();
        CategoriesChildren = UserCache.GetItem(_sessionUser.UserId).IsFiltered ? 
            UserEntityCache.GetChildren(category.Id, UserId) :
           EntityCache.GetChildren(category.Id, true);

        CorrectnesProbability = category.CorrectnessProbability;
        AnswersTotal = category.CorrectnessProbabilityAnswerCount;

        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(category.Id, ImageType.Category);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        var wishQuestions = _questionRepo.GetForCategoryAndInWishCount(category.Id, UserId, 5);

        AggregatedQuestions = category.GetAggregatedQuestionsFromMemoryCache();
        CountAggregatedQuestions = AggregatedQuestions.Count;
        CategoryQuestions = category.GetAggregatedQuestionsFromMemoryCache(true, false, category.Id);
        CountCategoryQuestions = CategoryQuestions.Count;

        CountReferences = ReferenceCount.Get(category.Id);

        if (category.Type != CategoryType.Standard)
            TopQuestionsWithReferences = Sl.R<ReferenceRepo>().GetQuestionsForCategory(category.Id);

        CountWishQuestions = wishQuestions.Total;

        TopQuestions = AggregatedQuestions.Take(MaxCountQuestionsToDisplay).ToList();

        if (category.Type == CategoryType.Standard)
            TopQuestionsInSubCats = GetTopQuestionsInSubCats();


        TopWishQuestions = wishQuestions.Items;

        SingleQuestions = GetQuestionsForCategory.QuestionsNotIncludedInSet(Id);
        IsFilteredUserWorld = UserCache.GetItem(_sessionUser.UserId).IsFiltered;

        AggregatedTopicCount = IsMyWorld ? CategoriesChildren.Count : GetTotalTopicCount(category);
        HardestQuestion = GetQuestion(true);
        EasiestQuestion = GetQuestion(false);

        TotalPins = category.TotalRelevancePersonalEntries.ToString();
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

    private Question GetQuestion(bool hardestQuestion)
    {
        if (CountAggregatedQuestions < 1)
        {
            return null;
        }
        var questions = AggregatedQuestions;
        if (hardestQuestion)
        {
            var question = questions.OrderByDescending(q => q.CorrectnessProbability).Last();
            return question;
        }
        else
        {
            var question = questions.OrderByDescending(q => q.CorrectnessProbability).First();
            return question;
        }
    }

    public ImageUrl GetCategoryImageUrl(CategoryCacheItem category)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(category.Id, ImageType.Category);
        return new ImageFrontendData(imageMetaData).GetImageUrl(232);
    }

    private void GetTopQuestionsFromChildrenOfChildren(List<Question> topQuestions)
    {
        foreach (var childCat in CategoriesChildren)
            foreach (var childOfChild in _categoryRepo.GetChildren(childCat.Id))
                if (topQuestions.Count < 6)
                    topQuestions.AddRange(_questionRepo.GetForCategory(childOfChild.Id, UserId, 5));
    }

    public string GetViews() => Sl.CategoryViewRepo.GetViewCount(Id).ToString();

    public string GetViewsPerDay()
    {
         var views =  Sl.CategoryViewRepo
            .GetPerDay(Id)
            .Select(item => item.Date.ToShortDateString() + " " + item.Views)
            .ToList();

         return !views.Any() 
            ? "" 
            : views.Aggregate((a, b) => a + " " + b + System.Environment.NewLine);
    }

    public Question GetDummyQuestion()
    {
        var questionId = 0; 
        if (IsMyWorld)
             questionId = Category
                .GetAggregatedQuestionsFromMemoryCache()
                .Where(q => q.IsVisibleToCurrentUser() && q.IsInWishknowledge())
                .Select(q => q.Id)
                .FirstOrDefault();
        else
         questionId = Category
                .GetAggregatedQuestionsFromMemoryCache()
                .Where(q => q.IsVisibleToCurrentUser())
                .Select(q => q.Id)
                .FirstOrDefault();

            return EntityCache.GetQuestionById(questionId);
    }
    public int GetTotalTopicCount(CategoryCacheItem category)
    {
        return EntityCache.GetChildren(category.Id).Count(c =>
                c.Type == CategoryType.Standard);


    }
}