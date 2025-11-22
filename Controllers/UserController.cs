    using Microsoft.AspNetCore.Mvc;
    using MyWebApi.Models;
    using MyWebApi.Services;
    using System.Threading.Tasks;

    namespace MyWebApi.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class UserController : ControllerBase
        {
            private readonly IUserService _userService;

            public UserController(IUserService userService)
            {
                _userService = userService;
            }

            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var users = await _userService.GetAllAsync();
                return Ok(users);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetById(string id)
            {
                try
                {
                    var user = await _userService.GetByIdAsync(id);
                    return Ok(user);
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }

            [HttpPost]
            public async Task<IActionResult> Create([FromBody] User user)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                await _userService.CreateAsync(user);
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Update(string id, [FromBody] User user)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (id != user.Id)
                    return BadRequest("ID mismatch");
                try
                {
                    await _userService.UpdateAsync(user);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(string id)
            {
                try
                {
                    await _userService.DeleteAsync(id);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
        }
    }