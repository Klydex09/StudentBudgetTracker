using Microsoft.AspNetCore.Mvc;

namespace StudentBudgetTracker.Controllers
{
    // This base controller contains shared helper methods used by secured pages.
    public class BaseController : Controller
    {
        // Checks whether the user already has an active login session.
        protected bool IsLoggedIn()
        {
            return HttpContext.Session.GetString("Username") != null;
        }

        // Sends unauthenticated users back to the login page.
        protected IActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}
