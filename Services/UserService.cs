using MyWebApi.Models;
using MyWebApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyWebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetAllAsync()
            => await _userRepository.GetAllAsync();

        public async Task<User?> GetByIdAsync(string id)
            => await _userRepository.GetByIdAsync(id);

        public async Task<User?> GetByEmailAsync(string email) // ДОДАЙ ЦЕ
            => await _userRepository.GetByEmailAsync(email);

        public async Task CreateAsync(User user)
            => await _userRepository.CreateAsync(user);

        public async Task UpdateAsync(User user)
            => await _userRepository.UpdateAsync(user);

        public async Task DeleteAsync(string id)
            => await _userRepository.DeleteAsync(id);
            // Services/UserService.cs
    }
    
}