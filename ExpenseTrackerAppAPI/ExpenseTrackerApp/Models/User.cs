namespace ExpenseTrackerApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public ICollection<Record> Records { get; set; }
        public ICollection<UserAccount> UserAccounts { get; set; }
    }
}
