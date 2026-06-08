using BCrypt.Net;
using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            var res = await _userRepository.GetAllUsers();   
            return Ok(res);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var res = await _userRepository.GetUserById(id);
            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(FinanceAPI.DTOs.UserDto data)
        {
            var res = new DAL.Models.User
            {
                Name = data.Name,
                Email = data.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(data.Password),
                CreatedAt = DateTime.UtcNow
            };
            await _userRepository.CreateUser(res);
            return Ok("User Created");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, FinanceAPI.DTOs.UserDto data)
        {
            var res = new DAL.Models.User
            {
                Id = id,
                Name = data.Name,
                Email = data.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(data.Password),
            };
            await _userRepository.UpdateUser(res);
            return Ok("User Updated");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
           await _userRepository.DeleteUser(id);
            return Ok("User Deleted");
        }
    }
}
