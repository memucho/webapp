using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using TemplateMigration;


public class TopicNavigationModel : BaseContentModule
{
    public Category Category;

    public string Title;
    public string Text;

    public List<Category> CategoryList;

    public TopicNavigationModel()
    {
    }

    public TopicNavigationModel(Category category, TopicNavigationJson topicNavigation)
    {

        Category = category;

        var isLoadList = false;
        switch (topicNavigation.Load)
        {
            case null:
            case "All":
                CategoryList = Sl.CategoryRepo.GetChildren(category.Id).ToList();
                break;

            default:
                var categoryIdList = topicNavigation.Load.Split(',').ToList().ConvertAll(Int32.Parse);
                CategoryList = ConvertToCategoryList(categoryIdList);
                isLoadList = true;
                break;
        }

        switch (topicNavigation.Order)
        {
            case null:
            case "QuestionAmount":
                if(topicNavigation.Load == null || topicNavigation.Load == "All")
                    CategoryList = CategoryList.OrderByDescending(c => c.GetAggregatedQuestionsFromMemoryCache().Count).ToList();
                break;

            case "Name":
                CategoryList = CategoryList.OrderBy(c => c.Name).ToList();
                break;

            default:
                if (isLoadList)
                {
                    throw new Exception("\"Load: \" und \"Order: \" können nicht gleichzeitig mit Category-Id-Listen als Parameter verwendet werden!");
                }
                var firstCategories = ConvertToCategoryList(topicNavigation.Order.Split(',').ToList().ConvertAll(Int32.Parse));
                CategoryList = OrderByCategoryList(firstCategories);
                break;
        }


        CategoryList = CategoryList.Where(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Standard).ToList();

        Title = topicNavigation.Title;
        Text = topicNavigation.Text;
    }


    public int GetTotalQuestionCount(Category category)
    {
        return category.GetAggregatedQuestionsFromMemoryCache().Count;
    }



    public ImageFrontendData GetCategoryImage(Category category)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(category.Id, ImageType.Category);
        return new ImageFrontendData(imageMetaData);
    }

    private List<Category> ConvertToCategoryList(List<int> categoryIdList)
    {
        var categoryList = new List<Category>();
        foreach (var categoryId in categoryIdList)
        {
            var category = Sl.CategoryRepo.GetById(categoryId);
            categoryList.Add(category);
        }

        return categoryList;
    }

    private List<Category> OrderByCategoryList(List<Category> firstCategories)
    {
        foreach (var category in firstCategories)
        {
            CategoryList.Remove(category);
        }

        firstCategories.AddRange(CategoryList);
        return firstCategories;
    }

    public static string ReturnKnowledgeStatus(List<string> list, int counter)
    {  
        return list.ElementAt(counter);
    }

    public int GetTotalTopicCount(Category category)
    {
        return Sl.CategoryRepo.GetChildren(category.Id).Count(c => c.Type == CategoryType.Standard && c.GetAggregatedQuestionIdsFromMemoryCache().Count > 0);
      
    }

    public bool ShowCategory(Category category)
    {
        return !_sessionUser.ShowMyWorld || category.IsInWishknowledge();
    }

}






