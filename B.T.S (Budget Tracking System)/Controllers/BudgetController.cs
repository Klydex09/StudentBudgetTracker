using Microsoft.AspNetCore.Mvc;
using StudentBudgetTracker.Models;

namespace StudentBudgetTracker.Controllers
{
    // This controller handles adding, viewing, editing, and deleting budget records.
    public class BudgetController : BaseController
    {
        // In-memory list used to temporarily store all budget entries.
        public static List<Budget> BudgetList = new List<Budget>();

        // Counter used to assign a unique ID to each new record.
        private static int nextId = 1;

        private static readonly string[] PresetCategories = new[] { "None", "Food", "Transportation", "School" };

        // Opens the Add Budget page.
        public IActionResult Add()
        {
            if (!IsLoggedIn())
                return RedirectToLogin();

            return View();
        }

        [HttpPost]
        // Saves a new budget record submitted by the user.
        public IActionResult Add(Budget budget, string? customCategory)
        {
            if (!IsLoggedIn())
                return RedirectToLogin();

            if (budget.Date == default)
            {
                budget.Date = DateTime.Now;
            }

            // Replaces the "Other" option with the user's custom category text.
            if (string.Equals(budget.Category, "Other", StringComparison.OrdinalIgnoreCase))
            {
                budget.Category = string.IsNullOrWhiteSpace(customCategory) ? "Other" : customCategory.Trim();
            }

            budget.Id = nextId++;
            budget.RemainingBalance = budget.Allowance - budget.Expenses;
            BudgetList.Add(budget);

            return RedirectToAction("Records");
        }

        // Displays all saved budget records and applies search/filter options.
        public IActionResult Records(string? searchTerm, string category = "all", DateTime? startDate = null, DateTime? endDate = null)
        {
            if (!IsLoggedIn())
                return RedirectToLogin();

            IEnumerable<Budget> filteredBudgets = BudgetList;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                filteredBudgets = filteredBudgets.Where(x =>
                    x.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    x.Category.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (!string.IsNullOrWhiteSpace(x.Remarks) && x.Remarks.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.Equals(category, "all", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(category, "Other", StringComparison.OrdinalIgnoreCase))
                {
                    filteredBudgets = filteredBudgets.Where(x => !PresetCategories.Contains(x.Category, StringComparer.OrdinalIgnoreCase));
                }
                else
                {
                    filteredBudgets = filteredBudgets.Where(x => x.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
                }
            }

            if (startDate.HasValue)
            {
                filteredBudgets = filteredBudgets.Where(x => x.Date.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                filteredBudgets = filteredBudgets.Where(x => x.Date.Date <= endDate.Value.Date);
            }

            var records = filteredBudgets
                .OrderByDescending(x => x.Date)
                .ToList();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.Category = category;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(records);
        }

        // Opens the Edit page for the selected record.
        public IActionResult Edit(int id)
        {
            if (!IsLoggedIn())
                return RedirectToLogin();

            var item = BudgetList.FirstOrDefault(x => x.Id == id);
            return View(item);
        }

        [HttpPost]
        // Updates the selected record with the new values entered by the user.
        public IActionResult Edit(Budget updated, string? customCategory)
        {
            if (!IsLoggedIn())
                return RedirectToLogin();

            var item = BudgetList.FirstOrDefault(x => x.Id == updated.Id);

            if (item != null)
            {
                item.Date = updated.Date;
                item.Description = updated.Description;
                item.Allowance = updated.Allowance;
                item.Expenses = updated.Expenses;
                item.Category = string.Equals(updated.Category, "Other", StringComparison.OrdinalIgnoreCase)
                    ? (string.IsNullOrWhiteSpace(customCategory) ? "Other" : customCategory.Trim())
                    : updated.Category;
                item.Remarks = updated.Remarks;
                item.RemainingBalance = item.Allowance - item.Expenses;
            }

            return RedirectToAction("Records");
        }

        // Removes a selected record from the list.
        public IActionResult Delete(int id)
        {
            if (!IsLoggedIn())
                return RedirectToLogin();

            var item = BudgetList.FirstOrDefault(x => x.Id == id);

            if (item != null)
                BudgetList.Remove(item);

            return RedirectToAction("Records");
        }

        // Recomputes balances in date order when running balance logic is needed.
        private void RecalculateBalances()
        {
            decimal runningBalance = 0;

            var sortedList = BudgetList.OrderBy(x => x.Date).ToList();

            foreach (var item in sortedList)
            {
                runningBalance += item.Allowance;
                runningBalance -= item.Expenses;

                item.RemainingBalance = runningBalance;
            }

            BudgetList = sortedList;
        }
    }
}
