using Microsoft.AspNetCore.Mvc;
using StudentBudgetTracker.Models;

namespace StudentBudgetTracker.Controllers
{
    public class BudgetController : BaseController
    {
        public static List<Budget> BudgetList = new List<Budget>();
        private static int nextId = 1;

        public IActionResult Add()
        {
            if (!IsLoggedIn())
                return RedirectToLogin();

            return View();
        }

        [HttpPost]
        public IActionResult Add(Budget budget)
        {
            if (budget.Date == default)
            {
                budget.Date = DateTime.Now;
            }
            budget.Id = nextId++; // assign ID


            budget.RemainingBalance = budget.Allowance - budget.Expenses;

            BudgetList.Add(budget);

            return RedirectToAction("Records");
        }

        public IActionResult Records()
        {
            if (!IsLoggedIn())
                return RedirectToLogin();

            return View(BudgetList);
        }

        // ===== EDIT =====
        public IActionResult Edit(int id)
        {
            var item = BudgetList.FirstOrDefault(x => x.Id == id);
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(Budget updated)
        {
            var item = BudgetList.FirstOrDefault(x => x.Id == updated.Id);

            if (item != null)
            {
                item.Date = updated.Date;
                item.Description = updated.Description;
                item.Allowance = updated.Allowance;
                item.Expenses = updated.Expenses;
                item.Category = updated.Category;

                // ✅ RECALCULATE ONLY THIS ITEM
                item.RemainingBalance = item.Allowance - item.Expenses;
            }

            return RedirectToAction("Records");
        }

        // ===== DELETE =====
        public IActionResult Delete(int id)
        {
            var item = BudgetList.FirstOrDefault(x => x.Id == id);

            if (item != null)
                BudgetList.Remove(item);

            return RedirectToAction("Records");
        }

        // ===== RECALCULATE BALANCES =====
        private void RecalculateBalances()
        {
            decimal runningBalance = 0;

            // 🔥 SORT BY DATE FIRST
            var sortedList = BudgetList.OrderBy(x => x.Date).ToList();

            foreach (var item in sortedList)
            {
                runningBalance += item.Allowance;
                runningBalance -= item.Expenses;

                item.RemainingBalance = runningBalance;
            }

            // 🔥 UPDATE ORIGINAL LIST ORDER
            BudgetList = sortedList;
        }
    }
}