using Microsoft.AspNetCore.Mvc;
using StudentBudgetTracker.Models;

namespace StudentBudgetTracker.Controllers
{
    public class AccountController : Controller
    {
        private static List<User> Users = new List<User>
        {
            new User { Username="student", Password="1234"},
            new User { Username="admin", Password="admin"}
        };

        public IActionResult Login() => View();
      
         

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = Users.FirstOrDefault(u  => u.Username == username && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid login";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}