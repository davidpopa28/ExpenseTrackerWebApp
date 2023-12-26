namespace ExpenseTrackerApp.Models
{
    public class Subcategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public ICollection<Record> Records { get; set; }
    }
}
