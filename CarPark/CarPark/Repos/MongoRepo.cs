using CarPark.Models;
using CarPark.Repos.Interfaces;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace CarPark.Repos
{
    public abstract class MongoRepo<T> : IMongoRepo<T> where T : BaseModel
    {
        protected readonly IMongoCollection<T> _collection;

        public MongoRepo(IMongoDatabase database)
        {
            _collection = database.GetCollection<T>(typeof(T).Name);
        }

        public async Task<T?> GetByIdAsync(string id)
            => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? expression = null, int skip = 0, int limit = 0)
            => await _collection.Find(expression ?? (_ => true)).Skip(skip).Limit(limit).ToListAsync();

        public async Task AddAsync(T entity)
            => await _collection.InsertOneAsync(entity);

        public async Task UpdateAsync(T entity)
            => await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);

        public async Task DeleteAsync(string id)
            => await _collection.DeleteOneAsync(x => x.Id == id);

        public async Task<PaginatedResult<T>> GetPaginatedAsync(int pageNumber, Expression<Func<T, bool>>? expression = null, int pageSize = 10)
        {
            List<T> items = await _collection.Find(expression ?? (_ => true))
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize).ToListAsync();

            long count = await _collection.Find(expression ?? (_ => true)).CountDocumentsAsync();
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedResult<T>(items, pageNumber, totalPages);
        }
    }
}
