using MongoDB.Bson;
using MongoDB.Driver;
using MyWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyWebApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _collection;

        public UserRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<User>("users");
        }

        public async Task<List<User>> GetAllAsync()
            => await _collection.Find(_ => true).ToListAsync();

        public async Task<User?> GetByIdAsync(string id)
            => await _collection.Find(u => u.Id == id).FirstOrDefaultAsync();

        public async Task<User?> GetByEmailAsync(string email) // ДОДАЙ ЦЕ
            => await _collection.Find(u => u.Email == email).FirstOrDefaultAsync();

        public async Task CreateAsync(User user)
            => await _collection.InsertOneAsync(user);

        public async Task UpdateAsync(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            await _collection.ReplaceOneAsync(filter, user);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            await _collection.DeleteOneAsync(filter);
        }
    }
}