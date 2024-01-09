using Microsoft.AspNetCore.Mvc;

namespace Test.Datadog.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogsController : ControllerBase
    {


        private readonly ILoggingService _loggingService;

        public LogsController(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        [HttpPost]
        public IActionResult AddLog()
        { 
            var metadata = new { UserId = 123, UserName = "John Doe" };

            _loggingService.LogInfo("AddLog", metadata);
            _loggingService.LogWarning("AddLog", metadata);
            _loggingService.LogError("AddLog", metadata);
            try
            {
                // Some code that might throw an exception
                throw new Exception("Something went wrong.");
            }
            catch (Exception ex)
            {
                _loggingService.LogError("An error occurred in the controller.", metadata, ex);
            }

            return Ok();
        }
        [HttpPost("/tag")]
        public IActionResult AddTag()
        {

            _loggingService.SetTag("user_id", "andreslontest");
            _loggingService.SetTag("user_type", "guest");

            var metadata = new { UserId = 123, UserName = "John Doe" };

            _loggingService.LogInfo("AddTag", metadata);
            _loggingService.LogWarning("AddTag", metadata);
            _loggingService.LogError("AddTag", metadata);
            try
            {
                // Some code that might throw an exception
                throw new Exception("Something went wrong.");
            }
            catch (Exception ex)
            {
                _loggingService.LogError("An error occurred in the controller.", metadata, ex);
            }

            return Ok();
        }
    }
}
