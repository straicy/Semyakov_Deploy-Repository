using MyWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyWebApi.Services
{
    public interface ICarService
    {
        Task<Car> GetByIdAsync(string id);
        Task<List<Car>> GetAllAsync();
        Task CreateAsync(Car car);
        Task UpdateAsync(Car car);
        Task DeleteAsync(string id);
    }
}