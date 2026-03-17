namespace StudentBudgetTracker.Models
{
    public class User
    {
        public required string Username { get; set; }
        public required string Password { get; set; } // Simple plain text for demo; normally hashed
    }
}
