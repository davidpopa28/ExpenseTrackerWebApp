namespace ExpenseTrackerApp.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
        public ICollection<Record> Records { get; set; }
        public ICollection<UserAccount> UserAccounts { get; set; }
    }
}
