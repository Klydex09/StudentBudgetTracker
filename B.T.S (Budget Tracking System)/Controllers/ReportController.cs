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
            // Gets all budget records from the shared in-memory list.
            var allBudgets = BudgetController.BudgetList;

            // Stores the current date so daily, weekly, and monthly filters can be compared correctly.
            DateTime today = DateTime.Now;
            List<Models.Budget> budgets;

            if (!IsLoggedIn())
                return RedirectToLogin();

            // Selects which records will be shown based on the chosen filter.
            if (filter == "daily")
            {
                // Keeps only records created today.
                budgets = allBudgets
                    .Where(x => x.Date.Date == today.Date)
                    .ToList();
            }
            else if (filter == "weekly")
            {
                // Keeps only records created within the last seven days.
                budgets = allBudgets
                    .Where(x => x.Date >= today.AddDays(-7))
                    .ToList();
            }
            else if (filter == "monthly")
            {
                // Keeps only records from the current month and year.
                budgets = allBudgets
                    .Where(x => x.Date.Month == today.Month && x.Date.Year == today.Year)
                    .ToList();
            }
            else
            {
                // Shows all records when no specific filter is selected.
                budgets = allBudgets;
            }

            // Computes the totals shown on the summary page.
            decimal totalAllowance = budgets.Sum(x => x.Allowance);
            decimal totalExpenses = budgets.Sum(x => x.Expenses);
            decimal remaining = totalAllowance - totalExpenses;

            ViewBag.TotalAllowance = totalAllowance;
            ViewBag.TotalExpenses = totalExpenses;
            ViewBag.RemainingBalance = remaining;

            // Prepares the chart labels and values from the filtered records.
            var labels = budgets.Select(x => x.Description).ToList();
            var data = budgets.Select(x => x.Expenses).ToList();

            // Converts chart data to JSON so JavaScript can read it in the view.
            ViewBag.Labels = JsonSerializer.Serialize(labels);
            ViewBag.Data = JsonSerializer.Serialize(data);

            // Sends the selected filter back to the view so the dropdown keeps its selected value.
            ViewBag.Filter = filter;

            return View();
        }
    }
}
