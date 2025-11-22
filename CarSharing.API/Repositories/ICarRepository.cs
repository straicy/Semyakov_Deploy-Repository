using MyWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyWebApi.Repositories
{
    public interface ICarRepository
    {
        Task<Car> GetByIdAsync(string id);
        Task<List<Car>> GetAllAsync();
        Task CreateAsync(Car car);
        Task UpdateAsync(Car car);
        Task DeleteAsync(string id);
    }
}