using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ShelterManager.Core;

public static class Setup
{
    public static void AddCore(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton(TimeProvider.System);
    }
}