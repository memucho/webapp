using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace TrueOrFalse.View.Web.Views.Api
{
    public class InternalController : Controller
    {

        public void UpdateCategory(string categoryAsString ="")
        {
            var category = JsonConvert.DeserializeObject<Category>(categoryAsString); 
            Sl.CategoryRepo.Update(category);
        }
    }
}