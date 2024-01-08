namespace ExpenseTrackerApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public ICollection<Subcategory> Subcategories { get; set; }
    }
}
