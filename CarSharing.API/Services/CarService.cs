using MyWebApi.Models;
using MyWebApi.Repositories;
using MongoDB.Bson;

namespace MyWebApi.Services;

public class CarService : ICarService
{
    private readonly ICarRepository _repo;

    public CarService(ICarRepository repo) => _repo = repo;

    public async Task<List<Car>> GetAllAsync() => await _repo.GetAllAsync();

    public async Task<Car?> GetByIdAsync(string id) => await _repo.GetByIdAsync(id);

    public async Task CreateAsync(Car car)
    {
        car.Id = ObjectId.GenerateNewId().ToString();
        car.CreatedAt = DateTime.UtcNow;
        car.IsAvailable = true;
        await _repo.CreateAsync(car);
    }

    public async Task UpdateAsync(Car car) => await _repo.UpdateAsync(car);

    public async Task DeleteAsync(string id) => await _repo.DeleteAsync(id);
}