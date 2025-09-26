using Microsoft.AspNetCore.RateLimiting;
using ShelterManager.Api.Constants;
using ShelterManager.Core.Extensions;

namespace ShelterManager.Api.Extensions;

public static class ApiExtensions
{
    public static void AddApiConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails();
        AddRateLimiting(builder);
        AddOpenApiDocs(builder);
        AddCorsConfiguration(builder);
    }

    public static void UseApiConfiguration(this WebApplication app)
    {
        app.UseRateLimiter();
        app.UseCors();
        
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

    private static void AddCorsConfiguration(IHostApplicationBuilder builder)
    {
        var cors = builder.Configuration.GetSection(CorsOptions.SectionName).Get<CorsOptions>();
        
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(b =>
            {
                if (cors is null)
                {
                    return;
                }
                
                b.WithOrigins(cors.Origins)
                    .WithMethods(cors.Methods)
                    .WithHeaders(cors.Headers)
                    .WithExposedHeaders(cors.ExposedHeaders);

                if (cors.AllowCredentials)
                {
                    b.AllowCredentials();
                }
                else
                {
                    b.DisallowCredentials();
                }
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