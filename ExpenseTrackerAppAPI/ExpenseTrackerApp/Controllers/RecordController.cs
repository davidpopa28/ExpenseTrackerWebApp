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
            var records = _mapper.Map<List<RecordDTO>>(_recordRepository.GetRecordsBySubcategories(subcategoryId));

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
            var records = _mapper.Map<List<RecordDTO>>(_recordRepository.GetRecordsByAccount(accountId));

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
            var records = _mapper.Map<List<RecordDTO>>(_recordRepository.GetRecordsByUser(userId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(records);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateRecord([FromQuery] int accountId, [FromQuery] int subcategoryId,
            [FromQuery] int userId, [FromBody] RecordDTO recordCreate)
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

            return Ok("Succesfully created!");
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

            if(!_recordRepository.RecordExists(recordId))
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var recordMap = _mapper.Map<Record>(updatedRecord);

            if(!_recordRepository.UpdateRecord(recordMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating!");
                return StatusCode(500, ModelState);
            }

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

            var recordToDelete = _recordRepository.GetRecord(recordId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_recordRepository.DeleteRecord(recordToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting!");
            }

            return NoContent();
        }
    }
}
