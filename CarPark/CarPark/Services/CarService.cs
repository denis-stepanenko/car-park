using CarPark.Exceptions;
using CarPark.Models;
using CarPark.Repos.Interfaces;
using System.Linq.Expressions;

namespace CarPark.Services
{
    public class CarService
    {
        private readonly ICarRepo _repo;

        public CarService(ICarRepo repo)
        {
            _repo = repo;
        }

        public async Task<Car?> GetByIdAsync(string id)
            => await _repo.GetByIdAsync(id);

        public async Task<List<Car>> GetAllAsync(Expression<Func<Car, bool>>? expression = null, int skip = 0, int limit = 0)
            => await _repo.GetAllAsync(expression, skip, limit);

        public async Task<PaginatedResult<Car>> GetPaginatedAsync(int pageNumber, Expression<Func<Car, bool>>? expression = null, int pageSize = 10)
            => await _repo.GetPaginatedAsync(pageNumber, expression, pageSize);

        public async Task AddAsync(Car entity)
        {
            if (await _repo.IsThereSuchCarAsync(entity))
                throw new BusinessLogicException("Такой автомобиль уже существует");

            await _repo.AddAsync(entity);
        }

        public async Task UpdateAsync(Car entity)
        {
            if (await _repo.IsThereSuchCarWithDifferentIdAsync(entity))
                throw new BusinessLogicException("Такой автомобиль уже существует");

            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(string id)
            => await _repo.DeleteAsync(id);
    }
}