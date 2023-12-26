using ExpenseTrackerApp.Models;

namespace ExpenseTrackerApp.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int id);
        bool CreateCategory(Category category);
        bool Save();
        bool CategoryExists(int id);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
    }
}
