using MongoDB.Driver;
using MyWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyWebApi.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IMongoCollection<Booking> _bookings;

        public BookingRepository(IMongoDatabase database)
        {
            _bookings = database.GetCollection<Booking>("Bookings");
        }

        public async Task<Booking> GetByIdAsync(string id)
        {
            return await _bookings.Find(booking => booking.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            return await _bookings.Find(_ => true).ToListAsync();
        }

        public async Task CreateAsync(Booking booking)
        {
            await _bookings.InsertOneAsync(booking);
        }

        public async Task UpdateAsync(Booking booking)
        {
            await _bookings.ReplaceOneAsync(b => b.Id == booking.Id, booking);
        }

        public async Task DeleteAsync(string id)
        {
            await _bookings.DeleteOneAsync(booking => booking.Id == id);
        }
    }
}