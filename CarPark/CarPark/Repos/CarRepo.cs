using CarPark.Models;
using CarPark.Repos.Interfaces;
using MongoDB.Driver;

namespace CarPark.Repos
{
    public class CarRepo : MongoRepo<Car>, ICarRepo
    {
        public CarRepo(IMongoDatabase db) : base(db)
        {
        }

        public async Task<bool> IsThereSuchCarAsync(Car car)
        {
            return await _collection.CountDocumentsAsync(x => x.Make == car.Make && x.Name == car.Name) > 0;
        }

        public async Task<bool> IsThereSuchCarWithDifferentIdAsync(Car car)
        {
            return await _collection.CountDocumentsAsync(x => x.Id != car.Id && x.Make == car.Make && x.Name == car.Name) > 0;
        }
    }
}
