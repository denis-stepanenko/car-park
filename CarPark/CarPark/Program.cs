using CarPark.Repos.Interfaces;
using CarPark.Repos;
using MongoDB.Driver;
using CarPark.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// MongoDB
var mongoConnectionString = builder.Configuration["MONGODB_URL"] ??
    throw new ArgumentException("MongoDb URL must be specified");

var mongoDatabaseName = builder.Configuration["MONGODB_DATABASE_NAME"]
    ?? throw new ArgumentException("MongoDb database name must be specified");

var client = new MongoClient(mongoConnectionString);
var database = client.GetDatabase(mongoDatabaseName);

builder.Services.AddSingleton(database);

builder.Services.AddScoped<ICarRepo, CarRepo>();

builder.Services.AddScoped<CarService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

public partial class Program();