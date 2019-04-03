﻿using System;

public class SingleCategoryFullWidthModel : BaseContentModule
{
    public Category Category;
    public int CategoryId;
    public string CategoryType;
    public string Name;
    public string Description;
    public int AggregatedQuestionCount;
    public int AggregatedSetCount;
    public ImageFrontendData ImageFrontendData;
    public bool IsInWishknowledge;

    public SingleCategoryFullWidthModel(int categoryId) : this(new SingleCategoryFullWidthJson{CategoryId = categoryId})
    {
    }

    public SingleCategoryFullWidthModel(SingleCategoryFullWidthJson singleCategoryFullWidthJson)
    {
        Category = Sl.CategoryRepo.GetById(singleCategoryFullWidthJson.CategoryId);
        CategoryId = Category.Id;
        if (Category== null)
            throw new Exception("Die angegebene Themen-ID verweist nicht auf ein existierendes Thema.");
        Name = singleCategoryFullWidthJson.Name ?? Category.Name;
        Description = singleCategoryFullWidthJson.Description ?? Category.Description;
        AggregatedQuestionCount = Category.CountQuestionsAggregated;
        AggregatedSetCount = Category.GetAggregatedSetsFromMemoryCache().Count;
        CategoryType = Category.Type.GetShortName();
        

        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(Category.Id, ImageType.Category);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        IsInWishknowledge = R<CategoryValuationRepo>().GetBy(CategoryId, _sessionUser.UserId)?.IsInWishKnowledge() ?? false;
    }

}