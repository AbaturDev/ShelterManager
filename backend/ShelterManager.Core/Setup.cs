using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShelterManager.Core.Extensions;
using ShelterManager.Core.Options;
using ShelterManager.Core.Services;
using ShelterManager.Core.Services.Abstractions;
using ShelterManager.Core.Utils;

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
        builder.Services.AddOptionsWithValidation<ShelterConfigurationOptions>(ShelterConfigurationOptions.SectionName);
        builder.Services.AddOptionsWithValidation<FrontendOptions>(FrontendOptions.SectionName);

        var wkthmlPath = Path.Combine(AppContext.BaseDirectory, "native", $"libwkhtmltox{OsUtils.GetWkhtmlExtension()}");
        var context = new CustomAssemblyLoadContext();
        context.LoadUnmanagedLibrary(wkthmlPath);
        builder.Services.AddSingleton<IConverter>(new SynchronizedConverter(new PdfTools()));

        builder.Services.AddSingleton<ITemplateService, TemplateService>();
        builder.Services.AddScoped<IUserContextService, UserContextService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddSingleton<IFileService, AzureFileService>();
        builder.Services.AddSingleton<IPdfService, PdfService>();
    }
}