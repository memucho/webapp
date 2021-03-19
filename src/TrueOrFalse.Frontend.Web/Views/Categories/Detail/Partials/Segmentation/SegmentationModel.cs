﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using NHibernate.Mapping;

public class SegmentationModel : BaseContentModule
{
    public CategoryCacheItem Category;

    public string Title;
    public string Text;
    public bool HasCustomSegments = false;

    public List<CategoryCacheItem> CategoryList;
    public List<CategoryCacheItem> NotInSegmentCategoryList;
    public List<Segment> Segments;

    public SegmentationModel(CategoryCacheItem category)
    {
        Category = category;
        
        var categoryList = UserCache.GetItem(_sessionUser.UserId).IsFiltered ? UserEntityCache.GetChildren(category.Id, UserId) : EntityCache.GetChildren(category.Id);
        CategoryList = categoryList.Where(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Standard).ToList();

        var segments = new List<Segment>();
        if (category.CustomSegments != null)
        {
            segments = GetSegments(category.Id);
            NotInSegmentCategoryList = GetNotInSegmentCategoryList(segments, categoryList.ToList());
            Segments = segments;
        }
        else
            NotInSegmentCategoryList = categoryList.OrderBy(c => c.Name).ToList();
    }

    public List<Segment> GetSegments(int id)
    {
        var segments = new List<Segment>();
        var segmentJson = JsonConvert.DeserializeObject<List<SegmentJson>>(EntityCache.GetCategoryCacheItem(id).CustomSegments);
        foreach (var s in segmentJson)
        {
            var segment = new Segment();
            segment.Item = EntityCache.GetCategoryCacheItem(s.CategoryId);
            segment.Title = s.Title;
            if (s.ChildCategoryIds != null)
                segment.ChildCategories = UserCache.GetItem(_sessionUser.UserId).IsFiltered
                    ? EntityCache.GetCategoryCacheItems(s.ChildCategoryIds).Where(c => c.IsInWishknowledge()).ToList()
                    : EntityCache.GetCategoryCacheItems(s.ChildCategoryIds).ToList();
            else
                segment.ChildCategories = UserCache.GetItem(_sessionUser.UserId).IsFiltered ? 
                    UserEntityCache.GetChildren(s.CategoryId, UserId).ToList() : 
                    EntityCache.GetChildren(s.CategoryId);

            segments.Add(segment);
        }

        return segments.Distinct().OrderBy(s => s.Title).ToList();
    }

    public List<CategoryCacheItem> GetNotInSegmentCategoryList(List<Segment> segments, List<CategoryCacheItem> categoryList)
    {
        var notInSegmentCategoryList = new List<CategoryCacheItem>();
        var inSegmentCategoryList = new List<CategoryCacheItem>();

        foreach (var segment in segments)
        {
            inSegmentCategoryList.Add(segment.Item);
            if (segment.ChildCategories != null)
            {
                var categoriesToAdd = segment.ChildCategories.Where(c => !inSegmentCategoryList.Any(s => s.Id == c.Id)).ToList();
                foreach (var c in categoriesToAdd)
                    inSegmentCategoryList.Add(c);
            }

            notInSegmentCategoryList.AddRange(categoryList.Where(c => !inSegmentCategoryList.Any(s => c.Id == s.Id) && !notInSegmentCategoryList.Any(s => s.Id == c.Id)));
        }

        HasCustomSegments = true;
        return notInSegmentCategoryList.OrderBy(c => c.Name).ToList();
    }
}






