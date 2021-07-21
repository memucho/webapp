using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

public class CategoryHistoryModel : BaseModel
{
    public int CategoryId { get; set; }
    public string CategoryName;
    public string CategoryUrl;
    public IList<CategoryChangeDayModel> Days;

    public CategoryHistoryModel(Category category, IList<CategoryChange> categoryChanges, int categoryId )
    {
        Data data = new Data(); ;

        if (category == null)
        {
            data = JsonConvert.DeserializeObject<Data>(categoryChanges.First().Data); 
        }

        CategoryName = category == null ?  data.Name :  category.Name;
        CategoryId = categoryId;
        CategoryUrl = Links.CategoryDetail(CategoryName, CategoryId);

        Days = categoryChanges
            .GroupBy(change => change.DateCreated.Date)
            .OrderByDescending(group => group.Key)
            .Select(group => new CategoryChangeDayModel(
                group.Key, 
                (IList<CategoryChange>) group.OrderByDescending(g => g.DateCreated).ToList()
            ))
            .ToList();
    }
}

public class CategoryChangeDayModel
{
    public string Date;
    public IList<CategoryChangeDetailModel> Items;
    private string _catName;
    public DateTime DateTime;

    public CategoryChangeDayModel(DateTime date, IList<CategoryChange> changes)
    {
        Date = date.ToString("dd.MM.yyyy");
        DateTime = date;
        Items = changes.Select(cc =>
        {
            var typ = "";
            var categoryId = cc.Category == null ? Sl.CategoryChangeRepo.GetCategoryId(cc.Id): -1;
            if (cc.Category == null)
            {
                var allVersions = Sl.CategoryChangeRepo.GetForCategory(categoryId); 
                var prevVersionData =allVersions[allVersions.Count - 2].GetCategoryChangeData();
                _catName = prevVersionData.Name;
            }

            switch (cc.Type)
            {
                case CategoryChangeType.Create: 
                    typ = "Erstellt";
                    break;
                case CategoryChangeType.Update: 
                    typ = "Update";
                    break;
                case CategoryChangeType.Delete: 
                    typ = "Gelöscht";
                    break;
                default: 
                    Logg.r().Error("CategoryHistoryModel CategoryChangeType is invalid");
                    break;
            }

            return new CategoryChangeDetailModel
            {

                Author = new UserTinyModel(cc.Author),
                AuthorName = new UserTinyModel(cc.Author).Name,
                AuthorImageUrl = new UserImageSettings(new UserTinyModel( cc.Author).Id).GetUrl_85px_square( new UserTinyModel(cc.Author)).Url,
                ElapsedTime = TimeElapsedAsText.Run(cc.DateCreated),
                DateTime = cc.DateCreated.ToString("dd.MM.yyyy HH:mm"),
                Time = cc.DateCreated.ToString("HH:mm"),
                CategoryChangeId = cc.Id,
                CategoryId = cc.Category == null ? categoryId :  cc.Category.Id,
                CategoryName = cc.Category == null ? _catName : cc.Category.Name,
                Typ = typ,
                CreatorId = new UserTinyModel(cc.Category.Creator).Id,
                Visibility = cc.Category.Visibility,
            };
        }).ToList();
    }
}

public class CategoryChangeDetailModel
{
    public UserTinyModel Author;
    public string AuthorName;
    public string AuthorImageUrl;
    public string ElapsedTime;
    public string DateTime;
    public string Time;
    public int CategoryChangeId;
    public int CategoryId;
    public string CategoryName;
    public string Typ;
    public CategoryVisibility Visibility;
    public bool IsPrivate => Visibility == CategoryVisibility.Owner || Visibility == CategoryVisibility.OwnerAndFriends;
    public int CreatorId;
    public bool IsVisibleToCurrentUser()
    {
        return Visibility == CategoryVisibility.All || Sl.SessionUser.IsLoggedInUser(CreatorId);
    }
}

public class Data
{
    public string Name;
    public string Description; 
}