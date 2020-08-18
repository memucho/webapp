using System;
using System.Net.Mail;
using System.Web.Mvc;
using TrueOrFalse.Web;

public class AccountController : BaseController
{
    [SetMainMenu(MainMenuEntry.None)]
    [HttpGet]
    public ActionResult Membership()
    {
        return View("~/Views/Users/Account/Membership.aspx", new MembershipModel());
    }

    [HttpPost]
    public ActionResult Membership(MembershipModel model)
    {
        var membership = model.ToMembership();
        R<MembershipRepo>().Create(membership);
        
        _sessionUser.User.MembershipPeriods.Add(membership);

        SendEmail.Run(new MailMessage(
            Settings.EmailFrom,
            Settings.EmailToMemucho,
            "We have a new member",
            $"New member: {_sessionUser.User.Name} {_sessionUser.User.Id}"));

        return View("~/Views/Users/Account/Membership.aspx", new MembershipModel
        {
            Message = new SuccessMessage(String.Format(
                "Du hast dich erfolgreich als Mitglied angemeldet. " +
                "Die Rechnung für deinen Mitgliedsbeitrag wird an <b>{0}</b> geschickt.",
                model.BillingEmail)),
            IsMember = true,
            Membership = membership
        });
    }

    [AccessOnlyAsAdmin]
    public ActionResult RemoveAdminRights()
    {
        _sessionUser.IsInstallationAdmin = false;

        if (Request.UrlReferrer == null)
            Redirect("/");

        return Redirect(Request.UrlReferrer.AbsolutePath);
    }

    [AccessOnlyAsLoggedIn]
    public ActionResult ToggleWorld()
    {
        _sessionUser.ToggleOwnWorld();
        return Redirect(Request.UrlReferrer.AbsolutePath);
    }

    [SetMainMenu(MainMenuEntry.None)]
    [AccessOnlyAsLoggedIn]
    public ActionResult WidgetStats()
    {
        return View("~/Views/Users/Account/WidgetStats/WidgetStats.aspx", new WidgetStatsModel());
    }

    public string RenderWidgetStatsDetail(string host, string widgetKey)
    {
        return ViewRenderer.RenderPartialView(
            "~/Views/Users/Account/WidgetStats/Partials/WidgetStatsDetailViews.ascx",
            new WidgetStatsDetailViewsModel(host, widgetKey), 
            ControllerContext
        );

    }
}
