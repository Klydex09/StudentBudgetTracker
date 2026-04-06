using Microsoft.AspNetCore.Mvc;

namespace StudentBudgetTracker.Controllers
{
    // This controller prepares summary information for the dashboard page.
    public class DashboardController : BaseController
    {
        // Displays the dashboard and computes the values shown in the summary cards.
        public IActionResult Dashboard()
        {
            // Prevents access to the dashboard if the user is not logged in.
            if (!IsLoggedIn())
                return RedirectToLogin();

            // Reads the saved budget entries from the shared in-memory list.
            var budgets = BudgetController.BudgetList;

            // Computes the total allowance, expenses, and remaining balance.
            decimal totalAllowance = budgets.Sum(x => x.Allowance);
            decimal totalExpenses = budgets.Sum(x => x.Expenses);
            decimal remaining = totalAllowance - totalExpenses;

            // Sends computed totals to the view.
            ViewBag.TotalAllowance = totalAllowance;
            ViewBag.TotalExpenses = totalExpenses;
            ViewBag.RemainingBalance = remaining;

            // Sends the username to the view for the welcome message.
            ViewBag.Username = "student";

            // Gets the three most recent records for the Recent Transactions table.
            ViewBag.RecentTransactions = budgets
                .OrderByDescending(x => x.Date)
                .Take(3)
                .ToList();

            return View();
        }
    }
}
