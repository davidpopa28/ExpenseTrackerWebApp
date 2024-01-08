namespace ExpenseTrackerApp.Models
{
    public class Subcategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public double Value { get; set; }
        public ICollection<Record> Records { get; set; }
    }
}
