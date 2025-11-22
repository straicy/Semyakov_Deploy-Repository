using Microsoft.AspNetCore.Mvc;
using MyWebApi.Models;
using MyWebApi.Services;

namespace MyWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;

        public AuthController(IJwtService jwtService, IUserService userService)
        {
            _jwtService = jwtService;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto dto)
        {
            if (await _userService.GetByEmailAsync(dto.Email) != null)
                return BadRequest("Email already exists");

            var user = new User
            {
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                FullName = dto.FullName,
                Email = dto.Email,
                Role = (UserRole)dto.Role // ЯВНЕ ПРИВЕДЕННЯ!
            };

            await _userService.CreateAsync(user);

            return Created("", new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString()
            });
        }
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginDto dto)
        {
            var user = await _userService.GetByEmailAsync(dto.Email);
            // if (user == null || dto.Password != "password") // Простий пароль для демо
            //     return Unauthorized("Invalid credentials");

            // Ген  еруємо токени
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // ЗБЕРЕЖИ REFRESH TOKEN У БАЗІ
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7); // 7 днів
            await _userService.UpdateAsync(user);

            return Ok(new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken, // ДОДАНО!
                User = new UserDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = user.Role.ToString()
                }
            });
        }
    }
}
    