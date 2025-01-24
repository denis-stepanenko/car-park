using CarPark.Models;
using System.Linq.Expressions;

namespace CarPark.Repos.Interfaces
{
    public interface IMongoRepo<T> where T : BaseModel
    {
        Task<T?> GetByIdAsync(string id);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? expression = null, int skip = 0, int limit = 0);
        Task<PaginatedResult<T>> GetPaginatedAsync(int pageNumber, Expression<Func<T, bool>>? expression = null, int pageSize = 10);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }
}
