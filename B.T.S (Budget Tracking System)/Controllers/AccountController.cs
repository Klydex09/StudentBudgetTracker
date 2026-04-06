using Microsoft.AspNetCore.Mvc;
using StudentBudgetTracker.Models;

namespace StudentBudgetTracker.Controllers
{
    // This controller manages user login and logout.
    public class AccountController : Controller
    {
        // Demo users stored in memory for prototype login validation.
        private static List<User> Users = new List<User>
        {
            new User { Username="student", Password="1234"},
            new User { Username="admin", Password="admin"}
        };

        // Displays the login page.
        public IActionResult Login() => View();

        [HttpPost]
        // Validates the submitted username and password.
        public IActionResult Login(string username, string password)
        {
            // Finds a matching user in the in-memory user list.
            var user = Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                // Saves the username in session so protected pages know the user is logged in.
                HttpContext.Session.SetString("Username", user.Username);

                // Sends the user to the Home page after successful login.
                return RedirectToAction("Index", "Home");
            }

            // Shows an error if the credentials do not match.
            ViewBag.Error = "Invalid login";
            return View();
        }

        // Ends the current session and returns the user to the login page.
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
