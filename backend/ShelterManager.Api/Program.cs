using ShelterManager.Core;
using ShelterManager.Database;

var builder = WebApplication.CreateBuilder(args);

builder.AddCore();
builder.AddDatabase();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
