namespace ExpenseTrackerApp.Models
{
    public class Record
    {
        public int Id { get; set; }

        //record type meaning income or expense
        public Type Type { get; set; }
        public double Value { get; set; }
        public string Note { get; set; }
        public DateTime DateTime { get; set; }
        public User User { get; set; }
        public Account Account { get; set; }
        public Subcategory Subcategory { get; set; }
    }

    public enum Type
    {
        Expense = 0,
        Income = 1,
    };
}
