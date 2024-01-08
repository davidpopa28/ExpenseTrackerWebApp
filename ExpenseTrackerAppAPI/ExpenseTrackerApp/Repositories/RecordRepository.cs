using ExpenseTrackerApp.Data;
using ExpenseTrackerApp.Interfaces;
using ExpenseTrackerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApp.Repositories
{
    public class RecordRepository : IRecordRepository
    {
        private readonly DataContext _context;
        public RecordRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateRecord(Record record)
        {
            _context.Add(record);
            return Save();
        }

        public bool DeleteRecord(Record record)
        {
            _context.Remove(record);
            return Save();
        }

        public ICollection<Record> GetAllRecords()
        {
            return _context.Records.OrderBy(r => r.Id).ToList();
        }

        public Record GetRecord(int id)
        {
            return _context.Records
                .Include(r => r.Account)
                .Include(r => r.User)
                .Include(r => r.Subcategory)
                .Where(r => r.Id == id).FirstOrDefault();
        }

        public ICollection<Record> GetRecordsByAccount(int accountId)
        {
            return _context.Records.Include(r => r.Account).Include(r => r.User).Include(r => r.Subcategory)
                .Where(r => r.Account.Id == accountId).ToList();
        }

        public ICollection<Record> GetRecordsBySubcategories(int subcategoryId)
        {
            return _context.Records
                .Include(r => r.Account).Include(r => r.User)
                .Include(r => r.Subcategory)
                .Where(r => r.Subcategory.Id == subcategoryId).ToList();
        }

        public ICollection<Record> GetRecordsByUser(int userId)
        {
            return _context.Records
                .Include(r => r.Account)
                .Include(r => r.User)
                .Include(r => r.Subcategory)
                .Where(r => r.User.Id == userId).ToList();
        }

        public bool RecordExists(int recordId)
        {
            return _context.Records.Any(r => r.Id == recordId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateRecord(Record record)
        {
            _context.Update(record);
            return Save();
        }
    }
}
