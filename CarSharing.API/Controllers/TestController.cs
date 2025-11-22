using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok("Доступно всім");
        }

        [HttpGet("auth")]
        [Authorize]
        public IActionResult Auth()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Message = "Авторизований доступ",
                UserId = userId,
                Email = email,
                Role = role
            });
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnly()
        {
            return Ok("Тільки для Admin");
        }

        [HttpGet("manager")]
        [Authorize(Roles = "Manager,Admin")]
        public IActionResult ManagerOnly()
        {
            return Ok("Для Manager або Admin");
        }

        [HttpGet("user")]
        [Authorize(Roles = "User,Manager,Admin")]
        public IActionResult ForUsers() // ← ПЕРЕЙМЕНОВАНО!
        {
            return Ok("Для User, Manager або Admin");
        }
    }
}