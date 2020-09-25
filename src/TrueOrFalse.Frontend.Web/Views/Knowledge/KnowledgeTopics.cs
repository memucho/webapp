using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class KnowledgeTopics : BaseModel
{
    readonly IList<Category> Categories;
    readonly IList<Set> Sets;

    public KnowledgeTopics(bool isAuthor)
    {
        var categoriesIds = UserCache.GetCategoryValuations(UserId)
            .Where(v => v.IsInWishKnowledge())
            .Select(i => i.CategoryId)
            .ToList();

        Categories = isAuthor
            ? EntityCache.GetCategories(categoriesIds).Where(v => v.Creator != null && v.Creator.Id == UserId).ToList()
            : EntityCache.GetCategories(categoriesIds).ToList();

        var setIds = UserCache.GetSetValuations(UserId)
            .Where(v => v.IsInWishKnowledge())
            .Select(i => i.SetId)
            .ToList();

        Sets = isAuthor ? EntityCache.GetSetsByIds(setIds).Where(v => v.Creator != null && v.Creator.Id == UserId).ToList()
            : EntityCache.GetSetsByIds(setIds);
    }

    public List<CategoryAndSetWishKnowledge> FilteredCategoryWishKnowledge(ControllerContext controllerContext)
    {
        return GetCategoryAndSetWishKnowledgeItems(Categories, Sets, controllerContext);
    }

    public IList<CategoryAndSetWishKnowledge> SortList(List<CategoryAndSetWishKnowledge> unSortList, string sortCondition)
    {
        switch (sortCondition)
        {
            case "name|asc":
                unSortList.Sort((x, y) => String.CompareOrdinal(x.Title, y.Title));
                break;
            case "name|desc":
                unSortList.Sort((x, y) => String.CompareOrdinal(y.Title, x.Title));
                break;
            case "knowledgeBar|asc":
                unSortList.Sort((x, y) => y.KnowledgeWishAVGPercantage.CompareTo(x.KnowledgeWishAVGPercantage));
                break;
            case "knowledgeBar|desc":
                unSortList.Sort((x, y) => -1 * y.KnowledgeWishAVGPercantage.CompareTo(x.KnowledgeWishAVGPercantage));
                break;
        }

        var sortList = unSortList;

        return sortList;
    }

    private List<CategoryAndSetWishKnowledge> GetCategoryAndSetWishKnowledgeItems(IList<Category> CategorieWishes, IList<Set> setWishes, ControllerContext controllerContext)
    {
        List<CategoryAndSetWishKnowledge> filteredCategoryAndSetWishKnowledges = new List<CategoryAndSetWishKnowledge>();
        var countList = CategorieWishes.Count + setWishes.Count;

        foreach (var categoryWish in CategorieWishes)
        {
            var facebookLink = "https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2F" + changeUrlToFacebookCompatible(Settings.CanonicalHost + Links.CategoryDetail(categoryWish.Name, categoryWish.Id)) + "%2F&amp;src=sdkpreparse";
            var categoryAndSetWishKnowledge = new CategoryAndSetWishKnowledge();

            categoryAndSetWishKnowledge.Description = categoryWish.Description;
            categoryAndSetWishKnowledge.Title = categoryWish.Name;
            categoryAndSetWishKnowledge.ImageFrontendData = new ImageFrontendData(categoryWish.Id, ImageType.Category).GetImageUrl(128);
            categoryAndSetWishKnowledge.KnowlegdeWishPartial = KnowledgeWishPartial(categoryWish, controllerContext);
            categoryAndSetWishKnowledge.Id = categoryWish.Id;
            categoryAndSetWishKnowledge.IsCategory = true;
            categoryAndSetWishKnowledge.LinkStartLearningSession = Links.StartCategoryLearningSession(categoryWish.Id);
            categoryAndSetWishKnowledge.CreateQuestionLink = Links.CreateQuestion(categoryId: categoryWish.Id);
            categoryAndSetWishKnowledge.LearnSetsCount = categoryWish.GetCountSets();
            categoryAndSetWishKnowledge.QuestionsCount = categoryWish.CountQuestionsAggregated;
            categoryAndSetWishKnowledge.EditCategoryOrSetLink = Links.CategoryEdit(categoryWish);
            categoryAndSetWishKnowledge.ShareFacebookLink = facebookLink;
            categoryAndSetWishKnowledge.HasVideo = false;
            categoryAndSetWishKnowledge.KnowledgeWishAVGPercantage = CountDesiredKnowledge(categoryWish);
            categoryAndSetWishKnowledge.LinkToSetOrCategory = Links.GetUrl(categoryWish);
            categoryAndSetWishKnowledge.ListCount = countList;
            
            filteredCategoryAndSetWishKnowledges.Add(categoryAndSetWishKnowledge);
        }

        foreach (var setWish in setWishes)
        {
            var facebookLink = "https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2F" + changeUrlToFacebookCompatible(Settings.CanonicalHost + Links.SetDetail(setWish.Name, setWish.Id)) + "%2F&amp;src=sdkpreparse";
            var categoryAndSetWishKnowledge = new CategoryAndSetWishKnowledge
            {
                Description = setWish.Text,
                Title = setWish.Name,
                ImageFrontendData = new ImageFrontendData(setWish.Id, ImageType.QuestionSet).GetImageUrl(128),
                KnowlegdeWishPartial = KnowledgeWishPartial(setWish, controllerContext),
                Id = setWish.Id,
                IsCategory = false,
                LinkStartLearningSession = Links.StartLearningSessionForSet(setWish.Id),
                CreateQuestionLink = Links.CreateQuestion(setId: setWish.Id),
                LearnSetsCount = 1,
                QuestionsCount = setWish.QuestionCount(),
                EditCategoryOrSetLink = Links.SetEdit(setWish),
                ShareFacebookLink = facebookLink,
                HasVideo = setWish.HasVideo,
                KnowledgeWishAVGPercantage = CountDesiredKnowledge(setWish),
                LinkToSetOrCategory = Links.GetUrl(setWish),
                ListCount = countList
            };

            filteredCategoryAndSetWishKnowledges.Add(categoryAndSetWishKnowledge);
        }
        return filteredCategoryAndSetWishKnowledges;
    }

    public class CategoryAndSetWishKnowledge
    {
        public int Id;
        public string Description;
        public string Title;
        public ImageUrl ImageFrontendData;
        public string KnowlegdeWishPartial;
        public bool IsCategory;
        public string LinkStartLearningSession;
        public string CreateQuestionLink { get; set; }
        public int LearnSetsCount = 0;
        public int QuestionsCount = 0;
        public string EditCategoryOrSetLink { get; set; }
        public string ShareFacebookLink { get; set; }
        public bool HasVideo { get; set; }
        public int KnowledgeWishAVGPercantage { get; set; }
        public string LinkToSetOrCategory { get; set; }
        public int ListCount { get; set; }
    }

    public string changeUrlToFacebookCompatible(string url)
    {
        return url.Replace("https://", "www.");
    }

    private string KnowledgeWishPartial(Category category, ControllerContext controllerContext)
    {
        var KnowledgeBarPartial = ViewRenderer.RenderPartialView("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(category), controllerContext); // 128 ms

        return KnowledgeBarPartial;
    }

    private string KnowledgeWishPartial(Set set, ControllerContext controllerContext)
    {
        var KnowledgeBarPartial = ViewRenderer.RenderPartialView("~/Views/Sets/Detail/SetKnowledgeBar.ascx", new SetKnowledgeBarModel(set), controllerContext);

        return KnowledgeBarPartial;
    }

    private int CountDesiredKnowledge(object categoryOrSet)
    {
        var solid = 0;
        var needsConsolidation = 0;
        var notLearned = 0;
        var notInWishKnowledge = 0;
        var needsLearning = 0;
        var questionAVGPercantage = 0;
        var questionTotalNumber = 0;

        if (categoryOrSet is Category)
        {
            var category = (Category)categoryOrSet;
            var categoryKnowledge = KnowledgeSummaryLoader.RunFromMemoryCache(category.Id, UserId);

            solid = categoryKnowledge.Solid;
            needsConsolidation = categoryKnowledge.NeedsConsolidation;
            needsLearning = categoryKnowledge.NeedsLearning;
            notLearned = categoryKnowledge.NotLearned;
            notInWishKnowledge = categoryKnowledge.NotInWishknowledge;

        }
        else if (categoryOrSet is Set)
        {
            var set = (Set)categoryOrSet;
            var setKnowledge = KnowledgeSummaryLoader.Run(UserId, set);

            solid = setKnowledge.Solid;
            needsConsolidation = setKnowledge.NeedsConsolidation;
            notLearned = setKnowledge.NotLearned;
            needsLearning = setKnowledge.NeedsLearning;
            notInWishKnowledge = setKnowledge.NotInWishknowledge;
        }
        else
        {
            // error Handling
            return -1;
        }

        questionTotalNumber = (solid + needsConsolidation + needsLearning + notInWishKnowledge + notLearned) == 0  ? -1 : solid + needsConsolidation + needsLearning + notInWishKnowledge + notLearned;  // Die Bedingung ist für die Möglichkeit das ein Set hinzugefügt wird das keine Fragen enthält ,damit wird der Wert negativ und sie landet am Ende 
        questionAVGPercantage = ((solid * 100) + (needsConsolidation * 50) + (needsLearning * 20) + (notLearned * 2))/ questionTotalNumber;
        return questionAVGPercantage;
    }
}
