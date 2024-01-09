using Datadog.Trace;
using Datadog.Trace.Configuration;
using Microsoft.Extensions.Logging;

namespace Test.Datadog.Api
{
    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;

            this.LogWarning("Warning: MyLogging instance created");
            this.LogInfo("Info: MyLogging instance created");
            this.LogError("Error: MyLogging instance created", new Exception());
        }

        public void SetTag(string key, string value)
        {
            try
            {
                // Create a settings object using the existing
                // environment variables and config sources
                var settings = TracerSettings.FromDefaultSources();
                // Override a value
                settings.GlobalTags.Add(key, value);

                // Replace the tracer configuration
                Tracer.Configure(settings);

                var span = Tracer.Instance.ActiveScope?.Span;
                if (span != null)
                {
                    span.SetTag(key, value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed creating tag {ex?.Message}");
            }
        }

        private void Log(LogLevel? level, string? message, object? metadata = null, Exception? exception = null)
        {
            try
            {
                switch (level)
                {
                    case LogLevel.Information:
                        if (metadata != null)
                            _logger.LogInformation(message + " {@Metadata}", metadata);
                        else
                            _logger.LogInformation(message);
                        break;
                    case LogLevel.Warning:
                        if (metadata != null)
                            _logger.LogWarning(message + " {@Metadata}", metadata);
                        else
                            _logger.LogWarning(message);
                        break;
                    case LogLevel.Error:
                        if (exception != null)
                            if (metadata != null)
                                _logger.LogError(exception, message + " {@Metadata}", metadata);
                            else
                                _logger.LogError(exception, message);
                        else
                            if (metadata != null)
                            _logger.LogError(message + " {@Metadata}", metadata);
                        else
                            _logger.LogError(message);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(level), level, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed logging process: {ex?.Message}");
            }
        }

        public void LogInfo(string message, object? metadata = null)
        {
            Log(LogLevel.Information, message, metadata);
        }

        public void LogWarning(string message, object? metadata = null)
        {
            Log(LogLevel.Warning, message, metadata);
        }

        public void LogError(string message, object? metadata = null, Exception? exception = null)
        {
            Log(LogLevel.Error, message, metadata, exception);
        }

        public void LogError(string message, Exception? exception = null, object? metadata = null)
        {
            Log(LogLevel.Error, message, metadata, exception);
        }
    }
}