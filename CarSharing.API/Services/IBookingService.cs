using MyWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyWebApi.Services
{
    public interface IBookingService
    {
        Task<BookingDto?> GetByIdAsync(string id); // Змінено на Task<BookingDto?>
        Task<List<BookingDto>> GetAllAsync();
        Task CreateAsync(BookingDto bookingDto);
        Task UpdateAsync(BookingDto bookingDto);
        Task DeleteAsync(string id);
    }
}