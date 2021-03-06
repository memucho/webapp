﻿using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;

public class CategoryGraphModel : BaseModel
{
    public CategoryCacheItem Category;
    public JsonResult GraphData;
    public string GraphDataString;

    public CategoryGraphModel(CategoryCacheItem category)
    {
        Category = category;
        GraphData = GetCategoryGraph.AsJson(category);
        GraphDataString = EscapeChars(new JavaScriptSerializer().Serialize(GraphData.Data));
    }

    private string EscapeChars(string objectString)
    {
        return objectString.Replace(Environment.NewLine, String.Empty).Replace("\'", @"\'").Replace("\"", "\\\"").Replace(@"\t", " ");
    }
}