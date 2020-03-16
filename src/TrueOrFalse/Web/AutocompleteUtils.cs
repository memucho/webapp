using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

public class AutocompleteUtils
{
    public static IList<Category> GetRelatedCategoriesFromPostData(NameValueCollection postData)
    {
        var _categoryRepo = ServiceLocator.Resolve<CategoryRepository>();
        return
            (from key in postData.AllKeys
                where key.StartsWith("cat-")
                select _categoryRepo.GetById(Convert.ToInt32(postData[key])))
                .Where(category => category != null)
                .ToList();
    }
}