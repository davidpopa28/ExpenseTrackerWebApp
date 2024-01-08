using AutoMapper;
using ExpenseTrackerApp.DTO;
using ExpenseTrackerApp.Interfaces;
using ExpenseTrackerApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordController : Controller
    {
        private readonly IRecordRepository _recordRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ISubcategoryRepository _subcategoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public RecordController(IRecordRepository recordRepository, IMapper mapper, 
            IAccountRepository accountRepository, ISubcategoryRepository subcategoryRepository, IUserRepository userRepository)
        {
            _recordRepository = recordRepository;
            _accountRepository = accountRepository;
            _subcategoryRepository = subcategoryRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Record>))]
        public IActionResult GetAllRecords()
        {
            var records = _mapper.Map<List<RecordDTO>>(_recordRepository.GetAllRecords());

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(records);
        }
        [HttpGet("{recordId}")]
        [ProducesResponseType(200, Type = typeof(Record))]
        [ProducesResponseType(400)]
        public IActionResult GetRecord(int recordId)
        {
            if (!_recordRepository.RecordExists(recordId))
            {
                return NotFound();
            }

            var record = _mapper.Map<RecordDTO>(_recordRepository.GetRecord(recordId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(record);
        }
        [HttpGet("recordsBySubcategory/{subcategoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Record>))]
        [ProducesResponseType(400)]
        public IActionResult GetRecordsBySubcategories(int subcategoryId)
        {
            var records = _mapper.Map<List<Record>>(_recordRepository.GetRecordsBySubcategories(subcategoryId));

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(records);
        }
        [HttpGet("recordsByAccount/{accountId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Record>))]
        [ProducesResponseType(400)]
        public IActionResult GetRecordsByAccount(int accountId)
        {
            var records = _mapper.Map<List<Record>>(_recordRepository.GetRecordsByAccount(accountId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(records);
        }
        [HttpGet("recordsByUser/{userId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Record>))]
        [ProducesResponseType(400)]
        public IActionResult GetRecordsByUser(int userId)
        {
            var records = _mapper.Map<List<Record>>(_recordRepository.GetRecordsByUser(userId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(records);
        }

        [HttpPost("{userId}/{accountId}/{subcategoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateRecord(int accountId, int subcategoryId, int userId, [FromBody] RecordDTO recordCreate)
        {
            if(recordCreate == null)
            {
                return BadRequest(ModelState);
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var recordMap = _mapper.Map<Record>(recordCreate);

            recordMap.Account = _accountRepository.GetAccount(accountId);
            recordMap.Subcategory = _subcategoryRepository.GetSubcategory(subcategoryId);
            recordMap.User = _userRepository.GetUser(userId);

            if (!_recordRepository.CreateRecord(recordMap))
            {
                ModelState.AddModelError("", "Something went wrong!");
                return StatusCode(500, ModelState);
            }
            var account = _accountRepository.GetAccount(accountId);
            if (recordMap.Type == "Expense")
            {
                account.Balance -= recordMap.Value;
            }
            else
            {
                account.Balance += recordMap.Value;
            }
            _accountRepository.UpdateAccount(account);
            return Ok();
        }

        [HttpPut("{recordId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateRecord(int recordId, [FromBody]RecordDTO updatedRecord)
        {
            if(updatedRecord == null)
            {
                return BadRequest(ModelState);
            }

            if(recordId != updatedRecord.Id)
            {
                return BadRequest(ModelState);
            }
            var record = _recordRepository.GetRecord(recordId);
            if (!_recordRepository.RecordExists(record.Id))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var account = record.Account;
            if(record.Type == "Expense" && updatedRecord.Type == "Expense")
            {
                account.Balance -= (updatedRecord.Value - record.Value);
            }
            else if (record.Type == "Income" && updatedRecord.Type == "Income")
            {
                account.Balance += (updatedRecord.Value - record.Value);
            }
            else if (record.Type == "Expense" && updatedRecord.Type == "Income")
            {
                account.Balance = account.Balance + record.Value + updatedRecord.Value;
            }
            else if (record.Type == "Income" && updatedRecord.Type == "Expense")
            {
                account.Balance = account.Balance - record.Value - updatedRecord.Value;
            }
            _mapper.Map(updatedRecord, record);

            if(!_recordRepository.UpdateRecord(record))
            {
                ModelState.AddModelError("", "Something went wrong while updating!");
                return StatusCode(500, ModelState);
            }
            _accountRepository.UpdateAccount(account);
            return NoContent();
        }

        [HttpDelete("{recordId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRecord(int recordId)
        {

            if (!_recordRepository.RecordExists(recordId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var record = _recordRepository.GetRecord(recordId);
            var account = record.Account;
            if (record.Type == "Expense")
            {
                account.Balance += record.Value;
            }
            else
            {
                account.Balance -= record.Value;
            }
            _accountRepository.UpdateAccount(account);

            if (!_recordRepository.DeleteRecord(record))
            {
                ModelState.AddModelError("", "Something went wrong while deleting!");
            }

            return NoContent();
        }
    }
}
