using Microsoft.AspNetCore.Mvc;

namespace DemoProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        //51
        private NameService NameService { get; }

        public WeatherForecastController(ILogger<WeatherForecastController> logger, NameService nameService)
        {
            _logger = logger;
            NameService = nameService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            //54
            throw new InvalidNameException();
            //55
            //HttpContext.Request.Headers
            //HttpContext.User.Claims     //email addresses,etc
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        //52
        [HttpPost(Name = "PostWeatherForecast")]
        public async Task<IActionResult> Post()
        {
            //Post Logic
            return Ok("Weather forecast posted.");
        }

        [HttpPut(Name = "PutWeatherForecast")]
        public async Task<IActionResult> Put(WeatherForecast weatherForecast)
        {
            //Put Logic
            return Ok("Weather forecast updated.");
        }

        [HttpDelete(Name = "DeleteWeatherForecast")]
        public async Task<IActionResult> Delete(int id)
        {
            //Delete Logic
            return Ok("Weather forecast deleted.");
        }
    }
}