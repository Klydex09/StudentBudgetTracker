using Microsoft.AspNetCore.Mvc;

namespace StudentBudgetTracker.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            var budgets = BudgetController.BudgetList;

            ViewBag.TotalAllowance = budgets.Sum(x => x.Allowance);
            ViewBag.TotalExpenses = budgets.Sum(x => x.Expenses);
            ViewBag.RemainingBalance =
                budgets.LastOrDefault()?.RemainingBalance ?? 0;
            ViewBag.Username = "student";

            ViewBag.RecentTransactions = budgets.
                OrderByDescending(x => x.Date).
                Take(3).
                ToList();

            return View();
        }
    }
}