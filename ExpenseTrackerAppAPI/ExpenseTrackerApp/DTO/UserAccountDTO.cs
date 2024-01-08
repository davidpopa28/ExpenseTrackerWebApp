using ExpenseTrackerApp.Models;

namespace ExpenseTrackerApp.DTO
{
    public class UserAccountDTO
    {
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public UserRole Role { get; set; }
    }
}
