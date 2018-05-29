﻿using System;
using NHibernate.Criterion;

public class KnowledgeCardMiniSetModel:BaseModel
{
    public Set Set;
    public bool isInWishKnowledge;

    public KnowledgeCardMiniSetModel(Set set)
    {
        Set = set;
        isInWishKnowledge = Sl.SetValuationRepo.IsInWishKnowledge(Set.Id, UserId );
    }

    public ImageFrontendData GetSetImage(Set set)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(set.Id, ImageType.QuestionSet);
        return new ImageFrontendData(imageMetaData);
    }
}