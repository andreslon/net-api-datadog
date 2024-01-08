using Microsoft.AspNetCore.Mvc;

namespace Test.Datadog.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly IMyLogging _myLogging;

        public WeatherForecastController(IMyLogging myLogging)
        {
            _myLogging = myLogging;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {

            _myLogging.SetTag("user_id", "andreslontest");
            _myLogging.SetTag("user_type", "Patient");

            var metadata = new { UserId = 123, UserName = "John Doe" };

            _myLogging.LogInfo("GetWeatherForecast", metadata);
            _myLogging.LogWarning("GetWeatherForecast", metadata);
            _myLogging.LogError("GetWeatherForecast", metadata); 
            try
            {
                // Some code that might throw an exception
                throw new Exception("Something went wrong.");
            }
            catch (Exception ex)
            {
                _myLogging.LogError("An error occurred in the controller.", metadata, ex);
            }

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
