using ExpenseTrackerApp.Models;

namespace ExpenseTrackerApp.DTO
{
    public class RecordDTO
    {
        public int Id { get; set; }

        //record type meaning income or expense
        public string Type { get; set; }
        public double Value { get; set; }
        public string Note { get; set; }
        public DateTime DateTime { get; set; }
    }
}
