using System.Web.Mvc;

public class AccessOnlyLocalAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var request = filterContext.RequestContext.HttpContext.Request;

        if (!request.IsLocal)
            throw new InvalidAccessException();

        base.OnActionExecuting(filterContext);  
    }
}

