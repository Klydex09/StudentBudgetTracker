using System;

namespace StudentBudgetTracker.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public required string Description { get; set; }

        public decimal Allowance { get; set; }

        public decimal Expenses { get; set; }

        public required string Category { get; set; }

        public decimal RemainingBalance { get; set; }
    }
}