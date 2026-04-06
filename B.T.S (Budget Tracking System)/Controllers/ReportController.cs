using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace StudentBudgetTracker.Controllers
{
    // This controller prepares the financial summary and chart data.
    public class ReportController : BaseController
    {
        // Displays the summary page and filters the records based on the selected period.
        public IActionResult Summary(string filter = "overall")
        {
            var allBudgets = BudgetController.BudgetList;
            DateTime today = DateTime.Now;
            IEnumerable<Models.Budget> budgets = allBudgets;

            if (!IsLoggedIn())
                return RedirectToLogin();

            if (filter == "daily")
            {
                budgets = budgets.Where(x => x.Date.Date == today.Date);
            }
            else if (filter == "weekly")
            {
                budgets = budgets.Where(x => x.Date >= today.AddDays(-7));
            }
            else if (filter == "monthly")
            {
                budgets = budgets.Where(x => x.Date.Month == today.Month && x.Date.Year == today.Year);
            }

            var reportBudgets = budgets
                .OrderByDescending(x => x.Date)
                .ToList();

            decimal totalAllowance = reportBudgets.Sum(x => x.Allowance);
            decimal totalExpenses = reportBudgets.Sum(x => x.Expenses);
            decimal remaining = totalAllowance - totalExpenses;

            ViewBag.TotalAllowance = totalAllowance;
            ViewBag.TotalExpenses = totalExpenses;
            ViewBag.RemainingBalance = remaining;

            // Uses descriptions for labels so the selected report period is easier to recognize.
            var labels = reportBudgets.Select(x => x.Description).ToList();
            var data = reportBudgets.Select(x => x.Expenses).ToList();

            ViewBag.Labels = JsonSerializer.Serialize(labels);
            ViewBag.Data = JsonSerializer.Serialize(data);
            ViewBag.Filter = filter;

            if (!reportBudgets.Any())
            {
                ViewBag.EmptySummaryMessage = "No budget data matches the selected filter.";
            }

            return View(reportBudgets);
        }
    }
}
