using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShelterManager.Api.Constants;
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
        builder.Services.AddOpenApi();
        
        var apiOptions = builder.Configuration.GetSection(ApiOptions.SectionName).Get<ApiOptions>();
        if (apiOptions is null)
        {
            return;
        }

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = apiOptions.Title,
                Version = $"v{apiOptions.Version}",
                Description = apiOptions.Description,
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme.\n" +
                              "Enter 'Bearer' [space] and then your token in the text input below.\n" +
                              "Example: 'Bearer 12345abcdef'"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                }
            });
        });
    }

    private static void UseApiDocs(WebApplication app)
    {
        app.MapOpenApi();

        //var apiOptions = app.Services.GetRequiredService<IOptions<ApiOptions>>();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/v1/swagger.json", "API");
        });
    }
}