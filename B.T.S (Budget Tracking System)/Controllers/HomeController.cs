using Microsoft.AspNetCore.Mvc;

namespace StudentBudgetTracker.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
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