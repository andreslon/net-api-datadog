using System;

namespace Test.Datadog.Api
{
    public interface ILoggingService
    {
        void SetTag(string key, string value);
        void LogInfo(string message, object? metadata = null);
        void LogWarning(string message, object? metadata = null);
        void LogError(string message, object? metadata = null, Exception? exception = null);
        void LogError(string message, Exception? exception = null, object? metadata = null);
    }
}