using MyWebApi.Models;
using MyWebApi.Services;
using System;
using System.Threading.Tasks;

namespace MyWebApi.Infrastructure
{
    public class DataSeeder
    {
        private readonly ICarService _carService;
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;

        // КОНСТРУКТОР ПЕРШИЙ!
        public DataSeeder(ICarService carService, IUserService userService, IBookingService bookingService)
        {
            _carService = carService;
            _userService = userService;
            _bookingService = bookingService;
        }

        public async Task SeedDataAsync()
        {
            // === АВТОМОБІЛЬ ===
            var car = await _carService.GetByIdAsync("68ff185e566c5c8856f735fc");
            if (car == null)
            {
                await _carService.CreateAsync(new Car
                {
                    Id = "68ff185e566c5c8856f735fc",
                    Brand = "Tesla",
                    Model = "Model 3",
                    Year = 2023,
                    LicensePlate = "AB123456"
                });
            }

            // === КОРИСТУВАЧ ===
            var user = await _userService.GetByIdAsync("68ff1977566c5c8856f735fd");
            if (user == null)
            {
                var emailUser = await _userService.GetByEmailAsync("john@example.com");
                if (emailUser == null)
                {
                    await _userService.CreateAsync(new User
                    {
                        Id = "68ff1977566c5c8856f735fd",
                        FullName = "John Doe",
                        Email = "john@example.com",
                        Role = UserRole.User // ПРАВИЛЬНО!
                    });
                }
            }

            // === БРОНЮВАННЯ ===
            var booking = await _bookingService.GetByIdAsync("68ff1977566c5c8856f73601");
            if (booking == null)
            {
                await _bookingService.CreateAsync(new BookingDto
                {
                    Id = "68ff1977566c5c8856f73601",
                    CarId = "68ff185e566c5c8856f735fc",
                    UserId = "68ff1977566c5c8856f735fd",
                    StartDate = DateTime.Parse("2025-10-28T00:00:00"),
                    EndDate = DateTime.Parse("2025-10-31T00:00:00"),
                    Status = "Active"
                });
            }
        }
    }
}