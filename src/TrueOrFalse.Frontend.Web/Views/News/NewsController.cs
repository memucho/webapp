﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;

public class NewsController : Controller
{
    [SetMenu(MenuEntry.News)]
    public ActionResult News()
    {
        return View(new NewsModel());

    }
}
