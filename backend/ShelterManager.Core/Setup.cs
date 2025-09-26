using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShelterManager.Core.Extensions;
using ShelterManager.Core.Options;

namespace ShelterManager.Core;

public static class Setup
{
    public static void AddCore(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton(TimeProvider.System);

        builder.Services.AddOptionsWithValidation<ApiOptions>(ApiOptions.SectionName);
    }
}