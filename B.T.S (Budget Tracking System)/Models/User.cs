namespace StudentBudgetTracker.Models
{
    // This model represents a user account for login.
    public class User
    {
        // Username entered in the login page.
        public required string Username { get; set; }

        // Password entered in the login page.
        public required string Password { get; set; } // Simple plain text for demo; normally hashed
    }
}
