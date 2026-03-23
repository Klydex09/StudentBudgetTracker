using Microsoft.AspNetCore.Mvc;

namespace StudentBudgetTracker.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            if (!IsLoggedIn())
                return RedirectToLogin();

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Settings()
        {
            return View();
        }


    }
}