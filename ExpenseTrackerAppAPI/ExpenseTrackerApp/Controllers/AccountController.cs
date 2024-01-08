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

        [HttpPost("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateAccount(int userId, [FromBody] AccountDTO accountCreate)
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

            return Ok();
        }

        [HttpPut("{accountId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateAccount(int accountId, [FromBody] AccountDTO updatedAccount)
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

            if(!_accountRepository.UpdateAccount(accountMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{accountId}/{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteAccount(int accountId, int userId)
        {
            if(!_accountRepository.AccountExists(accountId))
            {
                return NotFound();
            }

            if (!_userRepository.UserExists(userId))
            {
                return NotFound();
            }

            if (_accountRepository.GetUserRoleByAccountIdAndUserId(accountId, userId) == UserRole.Member)
            {
                return BadRequest(ModelState);
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

        [HttpPost("associateAccount/{accountId}/{userId}/{userRole}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AssociateAccount(int userId, int accountId, UserRole userRole)
        {
            if (!_userRepository.UserExists(userId))
            {
                return BadRequest(ModelState);
            }

            if (!_accountRepository.AccountExists(accountId))
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_accountRepository.AddUserToAccount(userId, accountId, userRole))
            {
                ModelState.AddModelError("", "Something went wrong!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        [HttpDelete("disassociateAccount/{userId}/{accountId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DisassociateAccount(int userId, int accountId)
        {
            if (!_userRepository.UserExists(userId))
            {
                return BadRequest(ModelState);
            }

            if (!_accountRepository.AccountExists(accountId))
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_accountRepository.RemoveUserFromAccount(userId, accountId))
            {
                ModelState.AddModelError("", "Something went wrong!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
