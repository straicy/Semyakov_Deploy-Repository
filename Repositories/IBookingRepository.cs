using MyWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyWebApi.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking> GetByIdAsync(string id);
        Task<List<Booking>> GetAllAsync();
        Task CreateAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAsync(string id);
    }
}