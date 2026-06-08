using DAL.Interfaces;
using FinanceAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace FinanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        public AuthController(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto data)
        {
            var user = await _userRepository.GetUserByEmail(data.Email);
            bool isValid = BCrypt.Net.BCrypt.Verify(data.Password, user.Password);
            if(user == null)
            {
                return Unauthorized("Invalid email");
            }
            if(!isValid)
            {
                return Unauthorized("Invalid password");
            }

            //creating claim
            var claim = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };
            //Creating key and credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Token creation here
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claim = claim,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
                );
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}
