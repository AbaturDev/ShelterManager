using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using ShelterManager.Api.Constants;
using ShelterManager.Api.Utils;
using ShelterManager.Core.Options;

namespace ShelterManager.Api.Extensions;

public static class ApiExtensions
{
    public static void AddApiConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails();
        
        AddAuthentication(builder);
        builder.Services.AddAuthorization();
        
        AddRateLimiting(builder);
        
        AddCultureConfiguration(builder);
        AddOpenApiDocs(builder);
        AddCorsConfiguration(builder);
        
    }

    public static void UseApiConfiguration(this WebApplication app)
    {
        app.UseCors();
        
        app.UseHttpsRedirection();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
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

    private static void AddCultureConfiguration(IHostApplicationBuilder builder)
    {
        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[] { new CultureInfo("en-US") };
            options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
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

    private static void AddAuthentication(IHostApplicationBuilder builder)
    {
        var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();
        if (jwtOptions is null)
        {
            return;
        }
        
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
            };
        });
    }
    
    private static void AddOpenApiDocs(IHostApplicationBuilder builder)
    {
        var apiOptions = builder.Configuration.GetSection(ApiOptions.SectionName).Get<ApiOptions>();
        if (apiOptions is null)
        {
            return;
        }
        
        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info = new()
                {
                    Title = apiOptions.Title,
                    Version = $"v{apiOptions.Version}",
                    Description = apiOptions.Description,
                };
                return Task.CompletedTask;
            });
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });
    }

    private static void UseApiDocs(WebApplication app)
    {
        var apiOptions = app.Services.GetRequiredService<IOptions<ApiOptions>>();

        var openApiRoutePattern = $"/open-api/v{apiOptions.Value.Version}.json";
        
        app.MapOpenApi(openApiRoutePattern);

        app.MapScalarApiReference(options =>
        {
            options.Title = apiOptions.Value.Title;
            options.OpenApiRoutePattern = openApiRoutePattern;
            options.HideClientButton = true;
        });
    }
}