using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KLD.CoachMoney.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("principal-name")]
    public ActionResult<string> GetUserPrincipalName()
    {
        if (User?.Identity?.IsAuthenticated != true)
        {
            return Unauthorized();
        }

        // Common claim types that may contain a user principal name
        var upn = User.FindFirst(ClaimTypes.Upn)?.Value
               ?? User.FindFirst("upn")?.Value
               ?? User.FindFirst("preferred_username")?.Value
               ?? User.Identity?.Name
               ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
               ?? User.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrWhiteSpace(upn))
        {
            return NotFound("User principal name not found.");
        }

        //throw new BaseBadRequestException();

        return Ok(upn);
    }
}
