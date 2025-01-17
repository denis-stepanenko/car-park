using MongoDB.Driver;

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
