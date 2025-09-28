using Serilog;
using ShelterManager.Api.Extensions;
using ShelterManager.Api.Middlewares;
using ShelterManager.Core;
using ShelterManager.Database;
using ShelterManager.Services;

try
{
    var builder = WebApplication.CreateBuilder(args);

    //builder.AddSerilog();

    builder.AddCore();
    builder.AddDatabase();
    builder.AddServices();

    builder.AddApiConfiguration();

    builder.Services.AddExceptionHandler<ErrorExceptionHandler>();
    
    var app = builder.Build();

    //app.UseSerilogRequestLogging();

    app.UseExceptionHandler();

    app.UseApiConfiguration();

    app.RegisterEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}