using Asp.Versioning;
using KLD.CoachMoney.Application.Abstractions.Messaging;
using KLD.CoachMoney.Application.UseCases.Debts.Commands;
using KLD.CoachMoney.Application.UseCases.Debts.Queries;
using KLD.CoachMoney.Contracts.Debts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KLD.CoachMoney.Web.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/debts")]
    [Authorize]
    public class DebtController : ControllerBase
    {
        // GET /api/v1/debts
        [HttpGet]
        public async Task<IActionResult> GetDebts(
            [FromServices] IQueryHandler<GetUserDebts.Query, IReadOnlyList<DebtDto>> handler,
            CancellationToken ct)
        {
            var result = await handler.HandleAsync(
                new GetUserDebts.Query(),
                ct);

            return Ok(result);
        }

        // POST /api/v1/debts
        [HttpPost]
        public async Task<IActionResult> AddDebt(
            [FromServices] ICommandHandler<AddDebt.Command> handler,
            [FromBody] AddDebt.Command command,
            CancellationToken ct)
        {
            await handler.HandleAsync(command, ct);
            return NoContent();
        }

        // POST /api/v1/debts/{id}/payment
        [HttpPost("{id:guid}/payment")]
        public async Task<IActionResult> ApplyPayment(
            Guid id,
            [FromBody] ApplyPayment.Command body,
            [FromServices] ICommandHandler<ApplyPayment.Command> handler,
            CancellationToken ct)
        {
            var command = body with { DebtId = id };
            await handler.HandleAsync(command, ct);
            return NoContent();
        }
    }
}

