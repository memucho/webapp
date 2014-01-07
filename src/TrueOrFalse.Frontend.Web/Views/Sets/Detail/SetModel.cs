﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;

public class SetModel : BaseModel
{
    public int Id;
    public string Name;

    public Set Set;
    
    public IList<QuestionInSet> QuestionsInSet;

    public User Creator;
    public string CreatorName;
    public string CreationDate;
    public string CreationDateNiceText;

    public string ImageUrl;

    public bool IsOwner;

    public Func<UrlHelper, string> DetailLink;

    public SetModel(Set set)
    {
        Id = set.Id;
        Name = set.Name;

        Set = set;
        QuestionsInSet = set.QuestionsInSet;

        IsOwner = _sessionUser.IsOwner(set.Creator.Id);

        Creator = set.Creator;
        CreatorName = set.Creator.Name;
        CreationDate = set.DateCreated.ToString("dd.MM.yyyy HH:mm:ss");
        CreationDateNiceText = TimeElapsedAsText.Run(set.DateCreated);

        ImageUrl = QuestionSetImageSettings.Create(set.Id).GetUrl_350px_square().Url;
    }
}
