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

        // Opens the Add Budget page.
        public IActionResult Add()
        {
            if (!IsLoggedIn())
                return RedirectToLogin();

            return View();
        }

        [HttpPost]
        // Saves a new budget record submitted by the user.
        public IActionResult Add(Budget budget)
        {
            // If no date is manually selected, the system uses the current date.
            if (budget.Date == default)
            {
                budget.Date = DateTime.Now;
            }

            // Assigns a new unique ID to the record.
            budget.Id = nextId++;

            // Calculates the remaining balance for this entry.
            budget.RemainingBalance = budget.Allowance - budget.Expenses;

            // Adds the new entry to the in-memory list.
            BudgetList.Add(budget);

            // Redirects the user to the records page after saving.
            return RedirectToAction("Records");
        }

        // Displays all saved budget records.
        public IActionResult Records()
        {
            if (!IsLoggedIn())
                return RedirectToLogin();

            return View(BudgetList);
        }

        // Opens the Edit page for the selected record.
        public IActionResult Edit(int id)
        {
            var item = BudgetList.FirstOrDefault(x => x.Id == id);
            return View(item);
        }

        [HttpPost]
        // Updates the selected record with the new values entered by the user.
        public IActionResult Edit(Budget updated)
        {
            var item = BudgetList.FirstOrDefault(x => x.Id == updated.Id);

            if (item != null)
            {
                // Replaces the old values with the edited values.
                item.Date = updated.Date;
                item.Description = updated.Description;
                item.Allowance = updated.Allowance;
                item.Expenses = updated.Expenses;
                item.Category = updated.Category;

                // Recalculates the balance after editing.
                item.RemainingBalance = item.Allowance - item.Expenses;
            }

            return RedirectToAction("Records");
        }

        // Removes a selected record from the list.
        public IActionResult Delete(int id)
        {
            var item = BudgetList.FirstOrDefault(x => x.Id == id);

            if (item != null)
                BudgetList.Remove(item);

            return RedirectToAction("Records");
        }

        // Recomputes balances in date order when running balance logic is needed.
        private void RecalculateBalances()
        {
            decimal runningBalance = 0;

            // Sorts records by date before recalculating the running total.
            var sortedList = BudgetList.OrderBy(x => x.Date).ToList();

            foreach (var item in sortedList)
            {
                runningBalance += item.Allowance;
                runningBalance -= item.Expenses;

                item.RemainingBalance = runningBalance;
            }

            // Replaces the original list with the sorted and recalculated version.
            BudgetList = sortedList;
        }
    }
}
