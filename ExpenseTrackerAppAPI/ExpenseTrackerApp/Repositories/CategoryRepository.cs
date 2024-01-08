using ExpenseTrackerApp.Data;
using ExpenseTrackerApp.Interfaces;
using ExpenseTrackerApp.Models;

namespace ExpenseTrackerApp.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;
        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            var categories = _context.Categories.OrderBy(c => c.Id).ToList();

            foreach (var category in categories)
            {
                category.Value = _context.Subcategories
                    .Where(s => s.Category.Id == category.Id)
                    .SelectMany(s => s.Records)
                    .Sum(s => s.Value);
            }

            return categories;

        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public bool Save()
        {
            var saved= _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }
    }
}
