using MongoDB.Driver;
using MongoUzduotis.Contracts;
using MongoUzduotis.Repository;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Konfigūruojame MongoDB
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(builder.Configuration.GetConnectionString("MongoDb")));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

Log.Logger = log;
Log.Information("Application Started");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

