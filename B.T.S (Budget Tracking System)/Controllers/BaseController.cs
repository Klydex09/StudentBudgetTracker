using Microsoft.AspNetCore.Mvc;

namespace StudentBudgetTracker.Controllers
{
    public class BaseController : Controller
    {
        protected bool IsLoggedIn()
        {
            return HttpContext.Session.GetString("Username") != null;
        }

        protected IActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}