﻿using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse.Core.Web.Uris;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Core;
using System.Web;

public class QuestionRowModel
{
    
    public QuestionRowModel(Question question) 
    {
        QuestionShort = question.GetShortTitle();
        QuestionId = question.Id;
        CreatorName = question.Creator.Name;

        CreatorUrlName = UriSegmentFriendlyUser.Run(question.Creator.Name);
        CreatorId = question.Creator.Id;
    }

    public string CreatorName {get; private set;}

    public string QuestionShort { get; private set; }
    public int QuestionId { get; private set; }

    public string CreatorUrlName { get; private set; }
    public int CreatorId { get; private set; }


}