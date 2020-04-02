﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

public class CategoryHistoryModel : BaseModel
{
    public int CategoryId { get; set; }
    public string CategoryName;
    public string CategoryUrl;
    public IList<CategoryChangeDayModel> Days;

    public CategoryHistoryModel(Category category, IList<CategoryChange> categoryChanges)
    {
        CategoryName = category.Name;
        CategoryId = category.Id;
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

    public CategoryChangeDayModel(DateTime date, IList<CategoryChange> changes)
    {
        Date = date.ToString("dd.MM.yyyy");

        Items = changes.Select(cc =>
        {
            var categoryDelete = cc.Type == CategoryChangeType.Delete;
            var data = JsonConvert.DeserializeObject<Data>(cc.Data); 

            return new CategoryChangeDetailModel
            {
                Author = cc.Author,
                AuthorName = cc.Author.Name,
                AuthorImageUrl = new UserImageSettings(cc.Author.Id).GetUrl_85px_square(cc.Author).Url,
                ElapsedTime = TimeElapsedAsText.Run(cc.DateCreated),
                DateTime = cc.DateCreated.ToString("dd.MM.yyyy HH:mm"),
                Time = cc.DateCreated.ToString("HH:mm"),
                CategoryChangeId = cc.Id,
                CategoryId = cc.Category_Id,
                CategoryName = categoryDelete ? data.Name : cc.Category.Name,
               

            };
        }).ToList();
    }
}

public class CategoryChangeDetailModel
{
    public User Author ;
    public string AuthorName;
    public string AuthorImageUrl;
    public string ElapsedTime;
    public string DateTime;
    public string Time;
    public int CategoryChangeId;
    public int CategoryId;
    public string CategoryName;
}

public class Data
{
    public string Name;
    public string Description; 
}