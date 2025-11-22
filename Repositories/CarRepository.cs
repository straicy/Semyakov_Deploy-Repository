using MongoDB.Driver;
using MyWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyWebApi.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly IMongoCollection<Car> _cars;

        public CarRepository(IMongoDatabase database)
        {
            _cars = database.GetCollection<Car>("Cars");
        }

        public async Task<Car> GetByIdAsync(string id)
        {
            return await _cars.Find(car => car.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Car>> GetAllAsync()
        {
            return await _cars.Find(_ => true).ToListAsync();
        }

        public async Task CreateAsync(Car car)
        {
            await _cars.InsertOneAsync(car);
        }

        public async Task UpdateAsync(Car car)
        {
            await _cars.ReplaceOneAsync(c => c.Id == car.Id, car);
        }

        public async Task DeleteAsync(string id)
        {
            await _cars.DeleteOneAsync(car => car.Id == id);
        }
    }
}