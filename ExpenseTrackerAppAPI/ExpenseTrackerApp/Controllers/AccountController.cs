using AutoMapper;
using ExpenseTrackerApp.DTO;
using ExpenseTrackerApp.Interfaces;
using ExpenseTrackerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace ExpenseTrackerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public AccountController(IAccountRepository accountRepository, IMapper mapper, IUserRepository userRepository)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type=typeof(IEnumerable<Account>))]
        public IActionResult GetAccounts()
        {
            var accounts = _mapper.Map<List<AccountDTO>>(_accountRepository.GetAccounts());

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(accounts);
        }
        [HttpGet("{accountId}")]
        [ProducesResponseType(200, Type=typeof(Account))]
        [ProducesResponseType(400)]
        public IActionResult GetAccount(int accountId)
        {
            if(!_accountRepository.AccountExists(accountId))
            {
                return NotFound();
            }

            var account = _mapper.Map<AccountDTO>(_accountRepository.GetAccount(accountId));

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(account);
        }

        [HttpGet("accountsByUser/{userId}")]
        [ProducesResponseType(200, Type=typeof(IEnumerable<Account>))]
        [ProducesResponseType(400)]
        public IActionResult GetAccountsByUser(int userId)
        {
            var accounts = _mapper.Map<List<AccountDTO>>(_accountRepository.GetAccountsByUserId(userId));

            if(!ModelState.IsValid)
            { 
                return BadRequest(ModelState); 
            }

            return Ok(accounts);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateAccount([FromQuery] int userId, [FromBody] AccountDTO accountCreate)
        {
            if(accountCreate == null)
            {
                return BadRequest(ModelState);
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accountMap = _mapper.Map<Account>(accountCreate);

            if(!_accountRepository.CreateAccount(userId, accountMap))
            {
                ModelState.AddModelError("", "Something went wrong!");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully created!");
        }

        [HttpPut("{accountId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateAccount(int accountId, [FromQuery] int userId, [FromBody] AccountDTO updatedAccount)
        {
            if(updatedAccount == null)
            {
                return BadRequest(ModelState);
            }

            if(accountId != updatedAccount.Id)
            {
                return BadRequest(ModelState);
            }

            if(!_accountRepository.AccountExists(accountId))
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accountMap = _mapper.Map<Account>(updatedAccount);

            if(!_accountRepository.UpdateAccount(userId, accountMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{accountId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteAccount(int accountId)
        {
            if(!_accountRepository.AccountExists(accountId))
            {
                return NotFound();
            }

            var accountToDelete= _accountRepository.GetAccount(accountId);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!_accountRepository.DeleteAccount(accountToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting!");
            }

            return NoContent();
        }
    }
}
