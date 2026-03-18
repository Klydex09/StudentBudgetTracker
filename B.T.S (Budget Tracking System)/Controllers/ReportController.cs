using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace StudentBudgetTracker.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Summary(string filter = "overall")
        {
            var allBudgets = BudgetController.BudgetList;

            DateTime today = DateTime.Now;
            List<Models.Budget> budgets;

            // 🎯 FILTERING
            if (filter == "daily")
            {
                budgets = allBudgets
                    .Where(x => x.Date.Date == today.Date)
                    .ToList();
            }
            else if (filter == "weekly")
            {
                budgets = allBudgets
                    .Where(x => x.Date >= today.AddDays(-7))
                    .ToList();
            }
            else if (filter == "monthly")
            {
                budgets = allBudgets
                    .Where(x => x.Date.Month == today.Month && x.Date.Year == today.Year)
                    .ToList();
            }
            else // ✅ OVERALL (default)
            {
                budgets = allBudgets;
            }

            // 🎯 TOTALS
            decimal totalAllowance = budgets.Sum(x => x.Allowance);
            decimal totalExpenses = budgets.Sum(x => x.Expenses);
            decimal remaining = totalAllowance - totalExpenses;

            ViewBag.TotalAllowance = totalAllowance;
            ViewBag.TotalExpenses = totalExpenses;
            ViewBag.RemainingBalance = remaining;

            // 🎯 CHART
            var labels = budgets.Select(x => x.Description).ToList();
            var data = budgets.Select(x => x.Expenses).ToList();

            ViewBag.Labels = System.Text.Json.JsonSerializer.Serialize(labels);
            ViewBag.Data = System.Text.Json.JsonSerializer.Serialize(data);

            ViewBag.Filter = filter;

            return View();
        }
    }
}