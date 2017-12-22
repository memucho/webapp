using System.Web.Mvc;

public class CategoryStatisticsApiController : BaseController
{
    public JsonResult ViewsByDayByName(int categoryId, int? amountOfDays)
    {
        var viewsPerDay = Sl.CategoryViewRepo.GetPerDay(categoryId, amountOfDays ?? 30);
         
        return Json(viewsPerDay, JsonRequestBehavior.AllowGet);
    }
}
