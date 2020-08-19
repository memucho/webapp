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
    public IList<Category> CategoriesParent;
    public IList<Category> CategoriesChildren;
    public IList<Question> AggregatedQuestions;
    public IList<Question> CategoryQuestions;
    public int AggregatedTopicCount;
    public int AggregatedQuestionCount;
    public int CategoryQuestionCount;
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
    public Category Category;
    public ImageFrontendData ImageFrontendData;
    public string WikipediaURL;
    public string Url;
    public bool IsOwnerOrAdmin;
    public bool IsTestSession => !IsLoggedIn;
    public bool IsLearningSession => IsLoggedIn;
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
    public LearningTabModel LearningTabModel;
    public UserTinyModel UserTinyModel;
    public AnalyticsFooterModel AnalyticsFooterModel;
    public bool CategoryIsDeleted;
    public CategoryModel()
    {

    }
    public CategoryModel(Category category, bool loadKnowledgeSummary = true, bool isCategoryNull = false)
    {
        CategoryIsDeleted = isCategoryNull;

        AnalyticsFooterModel = new AnalyticsFooterModel(category, false, isCategoryNull);
        MetaTitle = category.Name;
        MetaDescription = SeoUtils.ReplaceDoubleQuotes(category.Description).Truncate(250, true);

        _questionRepo = R<QuestionRepo>();
        _categoryRepo = R<CategoryRepository>();

        if(loadKnowledgeSummary)
            KnowledgeSummary = isCategoryNull ? null :  KnowledgeSummaryLoader.RunFromMemoryCache(category.Id, UserId);

        IsInWishknowledge = Sl.CategoryValuationRepo.IsInWishKnowledge(category.Id, UserId);

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
        SidebarModel.Fill(authors, UserId);

        IsOwnerOrAdmin = _sessionUser.IsLoggedInUserOrAdmin(Creator.Id);

        CategoriesParent = category.ParentCategories();
        CategoriesChildren = _categoryRepo.GetChildren(category.Id);

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

        CountSets = category.GetCountSets();
        CountWishQuestions = wishQuestions.Total;

        TopQuestions = AggregatedQuestions.Take(MaxCountQuestionsToDisplay).ToList();

        if (category.Type == CategoryType.Standard)
            TopQuestionsInSubCats = GetTopQuestionsInSubCats();


        TopWishQuestions = wishQuestions.Items;

        SingleQuestions = GetQuestionsForCategory.QuestionsNotIncludedInSet(Id);

        AggregatedTopicCount = new TopicNavigationModel().GetTotalTopicCount(category);

        AggregatedQuestionCount = Category.GetCountQuestionsAggregated();
        CategoryQuestionCount = Category.GetCountQuestionsAggregated(true, category.Id);
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

    public bool ShowContent(Category category)
    {
        return !_sessionUser.ShowMyWorld || category.IsInWishknowledge();
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

    public ImageUrl GetCategoryImageUrl(Category category)
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


}