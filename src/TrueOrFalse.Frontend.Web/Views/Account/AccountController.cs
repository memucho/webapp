﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrueOrFalse.View.Web
{
    public class AccountController : Controller
    {
        public ActionResult LogOn()
        {
            return View();
        }

        public ActionResult LogOff()
        {
            return View();
        }
    }
}