using System;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingAPI.Controllers.Requests;
using DatingAPI.Controllers.Response;
using DatingAPI.Core;
using DatingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingAPI.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthRepository _authRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthController(IConfiguration configuration, IAuthRepository authRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _configuration = configuration;
            _authRepository = authRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (registerRequest.Username != null) 
                registerRequest.Username = registerRequest.Username.ToLower();
            
            if (await _authRepository.UserExistsAsync(registerRequest.Username))
                ModelState.AddModelError("Username", "Username is already taken");
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newUser = new User { Username = registerRequest.Username };

            var createdUser = await _authRepository.RegisterAsync(newUser, registerRequest.Password);
            await _unitOfWork.CompleteAsync();

            if (createdUser == null)
                return BadRequest("Cannot create user.");


            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userRepo = await _authRepository.LoginAsync(loginRequest.Username.ToLower(), loginRequest.Password);

            if (userRepo == null)
                return Unauthorized();


            var token = GenerateToken(userRepo);
            var user = _mapper.Map<UserListResponse>(userRepo);

            return Ok(new {token, user});
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}