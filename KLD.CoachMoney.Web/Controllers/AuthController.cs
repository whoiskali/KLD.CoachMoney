using KLD.CoachMoney.Application.Abstractions.Messaging;
using KLD.CoachMoney.Application.UseCases.Auth.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KLD.CoachMoney.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateToken(
            [FromServices] ICommandHandler<GenerateToken.Command> handler,
            CancellationToken ct)
        {
            var userId = new Guid("AB1ED3DB-D321-48E5-879D-4CEBB5722FE5"); // TEMP

            var command = new GenerateToken.Command
            {
                UserId = userId
            };

            await handler.HandleAsync(command, ct);

            return Ok(command.Result);
        }

    }
}
