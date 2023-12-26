using ExpenseTrackerApp.Models;

namespace ExpenseTrackerApp.Interfaces
{
    public interface ISubcategoryRepository
    {
        ICollection<Subcategory> GetSubcategories();
        Subcategory GetSubcategory(int id);
        ICollection<Subcategory> GetSubcategoriesByCategory(int categoryid);
        bool SubcategoryExists(int id);
        bool CreateSubcategory(Subcategory subcategory);
        bool UpdateSubcategory(Subcategory subcategory);
        bool DeleteSubcategory(Subcategory category);
        bool Save();
    }
}
