using Microsoft.AspNetCore.Mvc;

namespace StudentBudgetTracker.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            var budgets = BudgetController.BudgetList;

            decimal totalAllowance = budgets.Sum(x => x.Allowance);
            decimal totalExpenses = budgets.Sum(x => x.Expenses);
            decimal remaining = totalAllowance - totalExpenses;

            ViewBag.TotalAllowance = totalAllowance;
            ViewBag.TotalExpenses = totalExpenses;
            ViewBag.RemainingBalance = remaining;
            ViewBag.Username = "student";

            ViewBag.RecentTransactions = budgets.
                OrderByDescending(x => x.Date).
                Take(3).
                ToList();

            return View();
        }
    }
}