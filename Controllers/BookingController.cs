using Microsoft.AspNetCore.Mvc;
using MyWebApi.Models;
using MyWebApi.Services;
using MongoDB.Bson;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MyWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<BookingController> _logger;

        public BookingController(IBookingService bookingService, ILogger<BookingController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Getting all bookings");
            var bookings = await _bookingService.GetAllAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                _logger.LogInformation("Getting booking with id {Id}", id);
                var booking = await _bookingService.GetByIdAsync(id);
                if (booking == null)
                {
                    _logger.LogWarning("Booking with id {Id} not found", id);
                    return NotFound("Booking not found");
                }
                return Ok(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving booking with id {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookingDto bookingDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid ModelState: {Errors}", string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            if (!ObjectId.TryParse(bookingDto.UserId, out _) || !ObjectId.TryParse(bookingDto.CarId, out _))
            {
                _logger.LogWarning("Invalid UserId: {UserId} or CarId: {CarId}", bookingDto.UserId, bookingDto.CarId);
                return BadRequest("Invalid UserId or CarId");
            }

            try
            {
                _logger.LogInformation("Creating booking with id {Id}", bookingDto.Id);
                await _bookingService.CreateAsync(bookingDto);
                return CreatedAtAction(nameof(GetById), new { id = bookingDto.Id }, bookingDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating booking with id {Id}", bookingDto.Id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] BookingDto bookingDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid ModelState: {Errors}", string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            if (id != bookingDto.Id)
            {
                _logger.LogWarning("ID mismatch: Route id {RouteId} does not match booking id {BookingId}", id, bookingDto.Id);
                return BadRequest("ID mismatch");
            }

            if (!ObjectId.TryParse(bookingDto.UserId, out _) || !ObjectId.TryParse(bookingDto.CarId, out _))
            {
                _logger.LogWarning("Invalid UserId: {UserId} or CarId: {CarId}", bookingDto.UserId, bookingDto.CarId);
                return BadRequest("Invalid UserId or CarId");
            }

            try
            {
                _logger.LogInformation("Updating booking with id {Id}", id);
                await _bookingService.UpdateAsync(bookingDto);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating booking with id {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                _logger.LogInformation("Deleting booking with id {Id}", id);
                await _bookingService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting booking with id {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}