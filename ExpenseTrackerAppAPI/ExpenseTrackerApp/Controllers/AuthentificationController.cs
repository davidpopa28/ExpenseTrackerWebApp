using AutoMapper;
using ExpenseTrackerApp.Data;
using ExpenseTrackerApp.DTO;
using ExpenseTrackerApp.Interfaces;
using ExpenseTrackerApp.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExpenseTrackerApp.Controllers
{
    [Controller]
    [Route("/api/[controller]")]
    public class AuthentificationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly AbstractValidator<User> _validator;
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public AuthentificationController(IConfiguration config, DataContext context, IUserRepository userRepository,
            IMapper mapper, AbstractValidator<User> validator)
        {
            _config = config;
            _userRepository = userRepository;
            _mapper = mapper;
            _context = context; 
            _validator = validator;
        }
        [HttpPost("/signup")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult SignUp([FromBody] UserDTO userCreate)
        {
            if (userCreate == null)
            {
                return BadRequest(ModelState);
            }

            var user = _userRepository.GetUsers()
                .Where(u => u.Email.Trim().ToUpper() == userCreate.Email.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (user != null)
            {
                ModelState.AddModelError("", "Category exists already");
                return StatusCode(422, ModelState);
            }

            var userMap = _mapper.Map<User>(userCreate);

            var validation = _validator.Validate(userMap);
            if(!validation.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong!");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully created!");
        }

        [AllowAnonymous]
        [HttpPost("/login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            try
            {
                var user = Authenticate(userLogin);
                var token = GenerateToken(user);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        private User Authenticate(UserLogin userLogin)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.Email.ToLower() == userLogin.Email.ToLower() && u.Password == userLogin.Password);

            return user == null ? throw new KeyNotFoundException($"UserName or password are incorrect!") : user;
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"] ?? ""));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
            };
            var token = new JwtSecurityToken(_config["JWT:Issuer"],
                _config["JWT:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
