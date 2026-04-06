using System;

namespace StudentBudgetTracker.Models
{
    // This model represents one saved budget record in the system.
    public class Budget
    {
        // Unique identifier used to find, edit, or delete a record.
        public int Id { get; set; }

        // Stores the date of the budget entry.
        public DateTime Date { get; set; }

        // Short explanation of what the budget entry is for.
        public required string Description { get; set; }

        // Stores the money received or allocated by the user.
        public decimal Allowance { get; set; }

        // Stores the money spent by the user.
        public decimal Expenses { get; set; }

        // Stores the category of the expense such as Food or Transportation.
        public required string Category { get; set; }

        // Optional notes that explain more details about the record.
        public string? Remarks { get; set; }

        // Stores the computed balance after subtracting expenses from allowance.
        public decimal RemainingBalance { get; set; }
    }
}
