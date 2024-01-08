using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Datadog.Logs;


namespace Test.Datadog.Api
{
    public static class LoggingConfiguration
    {
        public static IServiceCollection AddMyLogging(this IServiceCollection services)
        {
            var loggerConfig = new LoggerConfiguration();
            loggerConfig
                .Enrich.WithThreadName()
                .Enrich.FromLogContext()
                .Enrich.WithThreadId();
            var datadogConfiguration = new DatadogConfiguration { Url = "https://http-intake.logs.datadoghq.com" };
            loggerConfig
                .WriteTo.DatadogLogs(
                    apiKey: "",
                    source: "andreslon-backend",
                    service: "andreslon-test",
                    tags: new string[] { $"env:local" },
                    configuration: datadogConfiguration,
                    logLevel: LogEventLevel.Information
                )
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning);
            var logger = loggerConfig.CreateLogger();
            services.AddLogging(builder => builder.AddSerilog(logger));
            services.AddSingleton<IMyLogging, MyLogging>();

            return services;
        }
    }
}
