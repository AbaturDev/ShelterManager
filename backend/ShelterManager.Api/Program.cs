using ShelterManager.Api.Extensions;
using ShelterManager.Api.Middlewares;
using ShelterManager.Core;
using ShelterManager.Database;
using ShelterManager.Database.Seeders;
using ShelterManager.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddCore();
builder.AddDatabase();
builder.AddServices();

builder.AddApiConfiguration();

builder.Services.AddExceptionHandler<ErrorExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();

app.UseApiConfiguration();

app.RegisterEndpoints();

await app.SeedAsync();

app.Run();
