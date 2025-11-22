// Repositories/IUserRepository.cs
using MyWebApi.Models;
using System.Threading.Tasks;

namespace MyWebApi.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(string id);
        Task<User?> GetByEmailAsync(string email); // ДОДАЙ ЦЕ
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(string id);
    }
}