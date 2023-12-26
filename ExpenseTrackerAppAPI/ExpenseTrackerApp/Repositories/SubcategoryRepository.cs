using ExpenseTrackerApp.Data;
using ExpenseTrackerApp.Interfaces;
using ExpenseTrackerApp.Models;

namespace ExpenseTrackerApp.Repositories
{
    public class SubcategoryRepository : ISubcategoryRepository
    {
        private readonly DataContext _context;
        public SubcategoryRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateSubcategory(Subcategory subcategory)
        {
            _context.Add(subcategory);
            return Save();
        }

        public bool DeleteSubcategory(Subcategory category)
        {
            _context.Remove(category);
            return Save();
        }

        public ICollection<Subcategory> GetSubcategories()
        {
            return _context.Subcategories.OrderBy(s => s.Id).ToList();
        }

        public ICollection<Subcategory> GetSubcategoriesByCategory(int categoryid)
        {
            return _context.Subcategories.Where(s => s.Category.Id == categoryid).ToList();
        }

        public Subcategory GetSubcategory(int id)
        {
            return _context.Subcategories.Where(s => s.Id == id).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool SubcategoryExists(int id)
        {
            return _context.Subcategories.Any(s => s.Id == id);
        }

        public bool UpdateSubcategory(Subcategory subcategory)
        {
            _context.Update(subcategory);
            return Save();
        }
    }
}
