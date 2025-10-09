using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShelterManager.Core.Extensions;
using ShelterManager.Core.Options;
using ShelterManager.Core.Services;
using ShelterManager.Core.Services.Abstractions;

namespace ShelterManager.Core;

public static class Setup
{
    public static void AddCore(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton(TimeProvider.System);

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddOptionsWithValidation<ApiOptions>(ApiOptions.SectionName);
        builder.Services.AddOptionsWithValidation<CorsOptions>(CorsOptions.SectionName);
        builder.Services.AddOptionsWithValidation<JwtOptions>(JwtOptions.SectionName);
        builder.Services.AddOptionsWithValidation<AdminOptions>(AdminOptions.SectionName);
        builder.Services.AddOptionsWithValidation<EmailOptions>(EmailOptions.SectionName);

        builder.Services.AddSingleton<ITemplateService, TemplateService>();
        builder.Services.AddScoped<IUserContextService, UserContextService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
    }
}