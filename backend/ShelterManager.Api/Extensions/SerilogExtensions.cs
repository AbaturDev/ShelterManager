using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace ShelterManager.Api.Extensions;

public static class SerilogExtensions
{
    public static WebApplicationBuilder AddSerilog(
        this WebApplicationBuilder builder, string sectionName = "Serilog")
    {
        builder.Host.UseSerilog((_, _, loggerConfiguration) =>
        {

            loggerConfiguration
                .WriteTo.Console()
                .Enrich.WithProperty("Application", builder.Environment.ApplicationName)
                .Enrich.FromLogContext()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override( "Microsoft.AspNetCore", LogEventLevel.Warning )
                .MinimumLevel.Override( "Microsoft.Extensions.Hosting", LogEventLevel.Information )
                .MinimumLevel.Override( "Microsoft.Hosting", LogEventLevel.Information )  
                .MinimumLevel.Override("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogEventLevel.Fatal)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails();
        });

        return builder;
    }
}