using Microsoft.AspNetCore.Mvc;
using StudentBudgetTracker.Models;

namespace StudentBudgetTracker.Controllers
{
    public class BudgetController : Controller
    {
        public static List<Budget> BudgetList = new List<Budget>();
        private static int nextId = 1;

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Budget budget)
        {
            budget.Date = DateTime.Now; // set current date
            budget.Id = nextId++; // assign ID

            decimal previousBalance =
                BudgetList.LastOrDefault()?.RemainingBalance ?? 0;

            budget.RemainingBalance =
                previousBalance + budget.Allowance - budget.Expenses;

            BudgetList.Add(budget);

            return RedirectToAction("Records");
        }

        public IActionResult Records()
        {
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
            }

            RecalculateBalances();

            return RedirectToAction("Records");
        }

        // ===== DELETE =====
        public IActionResult Delete(int id)
        {
            var item = BudgetList.FirstOrDefault(x => x.Id == id);

            if (item != null)
                BudgetList.Remove(item);

            RecalculateBalances();

            return RedirectToAction("Records");
        }

        // ===== RECALCULATE BALANCES =====
        private void RecalculateBalances()
        {
            decimal runningBalance = 0;

            foreach (var item in BudgetList.OrderBy(x => x.Date))
            {
                runningBalance += item.Allowance - item.Expenses;
                item.RemainingBalance = runningBalance;
            }
        }
    }
}