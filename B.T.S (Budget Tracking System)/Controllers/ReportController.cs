using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace StudentBudgetTracker.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Summary(string filter = "daily")
        {
            var budgets = BudgetController.BudgetList;

            // FILTER LOGIC (we'll use this for #2 also)
            DateTime today = DateTime.Now;

            if (filter == "daily")  
                budgets = budgets.Where(x => x.Date.Date == today.Date).ToList();

            else if (filter == "weekly")
                budgets = budgets.Where(x => x.Date >= today.AddDays(-7)).ToList();

            else if (filter == "monthly")
                budgets = budgets.Where(x => x.Date.Month == today.Month && x.Date.Year == today.Year).ToList();

            // TOTALS
            ViewBag.TotalAllowance = budgets.Sum(x => x.Allowance);
            ViewBag.TotalExpenses = budgets.Sum(x => x.Expenses);
            ViewBag.RemainingBalance = budgets.LastOrDefault()?.RemainingBalance ?? 0;

            // CHART DATA
            var labels = budgets.Select(x => x.Description).ToList();
            var data = budgets.Select(x => x.Expenses).ToList();

            ViewBag.Labels = JsonSerializer.Serialize(labels);
            ViewBag.Data = JsonSerializer.Serialize(data);

            ViewBag.Filter = filter;

            return View();
        }
    }
}