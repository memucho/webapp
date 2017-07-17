using System.Web.Mvc;

public class LoginController : BaseController
{
    [HttpPost]
    public JsonResult IsUserNameAvailable(string selectedName) =>
        new JsonResult { Data = new { isAvailable = global::IsUserNameAvailable.Yes(selectedName) } };

    [HttpPost]
    public JsonResult IsEmailAvailable(string selectedEmail) =>
        new JsonResult { Data = new { isAvailable = IsEmailAddressAvailable.Yes(selectedEmail) } };

    [HttpPost]
    public JsonResult Login(LoginModel loginModel)
    {
        //loginModel.Password = Request["Password"];

        var credentialsAreValid = R<CredentialsAreValid>();

        if (credentialsAreValid.Yes(loginModel.EmailAddress, loginModel.Password))
        {

            if (loginModel.PersistentLogin)
            {
                WritePersistentLoginToCookie.Run(credentialsAreValid.User.Id);
            }

            _sessionUser.Login(credentialsAreValid.User);

            TransferActivityPoints.FromSessionToUser();
            Sl.UserRepo.UpdateActivityPointsData();

            return Json(new
            {
                Success = true
            });
        }

        return Json(new
        {
            Success = false,
            Message = "Du konntest nicht eingeloggt werden. Bitte überprüfe deine E-Mail-Adresse und das Passwort."
        });
    }

    public string LoginModal() => 
        ViewRenderer.RenderPartialView("~/Views/Welcome/Login/Login.ascx", new LoginModel(), ControllerContext);
}