using Microsoft.AspNetCore.Mvc;

namespace StudentBudgetTracker.Controllers
{
    // This controller displays the informational pages of the system.
    public class HomeController : BaseController
    {
        // Displays the Home page after login.
        public IActionResult Index()
        {
            if (!IsLoggedIn())
                return RedirectToLogin();

            return View();
        }

        // Displays the About page that explains the project.
        public IActionResult About()
        {
            return View();
        }

        // Displays the Settings page placeholder.
        public IActionResult Settings()
        {
            return View();
        }
    }
}
