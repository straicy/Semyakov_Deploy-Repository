using AutoMapper;
using MyWebApi.Models;
using MyWebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyWebApi.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public BookingService(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<BookingDto?> GetByIdAsync(string id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            return booking == null ? null : _mapper.Map<BookingDto>(booking);
        }

        public async Task<List<BookingDto>> GetAllAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return _mapper.Map<List<BookingDto>>(bookings);
        }

        public async Task CreateAsync(BookingDto bookingDto)
        {
            if (bookingDto == null)
                throw new ArgumentNullException(nameof(bookingDto));
            var booking = _mapper.Map<Booking>(bookingDto);
            if (string.IsNullOrEmpty(booking.Id))
                booking.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            await _bookingRepository.CreateAsync(booking);
        }

        public async Task UpdateAsync(BookingDto bookingDto)
        {
            var booking = _mapper.Map<Booking>(bookingDto);
            await _bookingRepository.UpdateAsync(booking);
        }

        public async Task DeleteAsync(string id)
        {
            await _bookingRepository.DeleteAsync(id);
        }
    }
}