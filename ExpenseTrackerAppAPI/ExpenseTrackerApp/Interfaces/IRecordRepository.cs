using ExpenseTrackerApp.Models;

namespace ExpenseTrackerApp.Interfaces
{
    public interface IRecordRepository
    {
        ICollection<Record> GetAllRecords();
        Record GetRecord(int id);
        ICollection<Record> GetRecordsBySubcategories(int subcategoryId);
        ICollection<Record> GetRecordsByAccount(int accountId);
        ICollection<Record> GetRecordsByUser(int userId);
        bool RecordExists(int recordId);
        bool CreateRecord(Record record);
        bool UpdateRecord(Record record);
        bool DeleteRecord(Record record);
        bool Save();
    }
}
