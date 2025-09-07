using Microsoft.AspNetCore.RateLimiting;
using ShelterManager.Api.Constants;

namespace ShelterManager.Api.Extensions;

public static class ApiExtensions
{
    public static void AddApiConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails();
        AddRateLimiting(builder);
        AddOpenApiDocs(builder);
    }

    public static void UseApiConfiguration(this WebApplication app)
    {
        app.UseRateLimiter();
        
        if (app.Environment.IsDevelopment())
        {
            UseApiDocs(app);
        }
    }
    
    private static void AddRateLimiting(IHostApplicationBuilder builder)
    {
        builder.Services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter(RateLimiters.DefaultRateLimiterName, o =>
            {
                o.Window = TimeSpan.FromMinutes(1);
                o.PermitLimit = 50;
                o.QueueLimit = 20;
            });
            
            options.AddFixedWindowLimiter(RateLimiters.LoginRateLimiterName, o =>
            {
                o.Window = TimeSpan.FromMinutes(1);
                o.PermitLimit = 5;
                o.QueueLimit = 0;
            });
        });
    }

    private static void AddOpenApiDocs(IHostApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();
    }

    private static void UseApiDocs(WebApplication app)
    {
        app.MapOpenApi();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "Shelter Manager API");
        });
    }
}