using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;

public class SortSetsIntoListOfCategories
{

    /// <summary>
    /// Inserts each set to the most appropriate position within a list of categories, assuming the list of categories is ordered from less to more specific
    /// (e.g. via OrderCategoriesByQuestionCountAndLevel.Run)
    /// </summary>
    /// <returns>List of objects, each object either being a Set or a Category</returns>
    public static IList<object> Run(IList<Category> categories)
    {
        var output = new List<object>();
        categories.ForEach(c => output.Add(c));
        return output;
    }
}
