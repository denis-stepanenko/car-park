using CarPark.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]
namespace CarPark.IntegrationTests.Infrastructure
{
    public class CustomWebApplicationFactory<TProgram>
        : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("IntegrationTestsConfig.json")
                .Build();

            var mongoConnectionString = config["MONGODB_URL"] ??
                throw new ArgumentException("MongoDb URL должен быть указан для интеграционных тестов");

            var mongoDatabaseName = config["MONGODB_DATABASE_NAME"] ??
                throw new ArgumentException("Имя базы данных должено быть указано для интеграционных тестов");

            var client = new MongoClient(mongoConnectionString);
            var database = client.GetDatabase(mongoDatabaseName);

            client.DropDatabase(mongoDatabaseName);

            SeedData(database);

            builder.ConfigureServices(services =>
            {
                var databaseDescriptor = services.Single(x => x.ServiceType == typeof(IMongoDatabase));
                services.Remove(databaseDescriptor);

                services.AddSingleton(database);
                services.AddSingleton(client);

                services.AddAntiforgery(t =>
                {
                    t.FormFieldName = AntiForgeryTokenExtractor.Field;
                    t.Cookie.Expiration = TimeSpan.FromDays(356);
                });
            });

            builder.UseEnvironment("Development");
        }

        void SeedData(IMongoDatabase db)
        {
            var cars = new List<Car>()
            {
                new() { Id = "678a613c95634c673d425934", Make = "BMW", Name = "M4" },
                new() { Id = "678a613c95634c673d425935", Make = "BMW", Name = "M5" },
                new() { Id = "678a613c95634c673d425936", Make = "BMW", Name = "M8" },
                new() { Id = "678a613c95634c673d425937", Make = "Toyota", Name = "Camry" },
                new() { Id = "678a613c95634c673d425938", Make = "Toyota", Name = "Raize" },
                new() { Id = "678a613c95634c673d425939", Make = "Toyota", Name = "Hilux" },
                new() { Id = "678a613c95634c673d425940", Make = "Toyota", Name = "Corolla" },
                new() { Id = "678a613c95634c673d425941", Make = "Toyota", Name = "Yaris" },
                new() { Id = "678a613c95634c673d425942", Make = "Toyota", Name = "Crown" },
            };

            db.GetCollection<Car>("Car").InsertMany(cars);
        }
    }
}
