using MongoDB.Bson;
using MongoUzduotis.Models;

namespace MongoUzduotis.Contracts
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(ObjectId id);
        Task CreateAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(ObjectId id);
    }
}
