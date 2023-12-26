namespace ExpenseTrackerApp.Models
{
    public class UserAccount
    {
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public User User { get; set; }
        public Account Account { get; set; }
    }
}
