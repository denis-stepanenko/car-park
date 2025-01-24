using CarPark.Models;

namespace CarPark.Repos.Interfaces
{
    public interface ICarRepo : IMongoRepo<Car>
    {
        Task<bool> IsThereSuchCarAsync(Car car);
        Task<bool> IsThereSuchCarWithDifferentIdAsync(Car car);
    }
}
