using Asp.Versioning;
using KLD.CoachMoney.Application.Abstractions.Messaging;
using KLD.CoachMoney.Application.UseCases.Debts.Commands;
using KLD.CoachMoney.Application.UseCases.FinancialAdvice.Queries;
using KLD.CoachMoney.Contracts.FinancialAdvice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KLD.CoachMoney.Web.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/finance")]
[Authorize]
public sealed class FinanceController : ControllerBase
{
    [HttpGet("advice")]
    public async Task<ActionResult<FinancialAdviceResultDto>> GetAdvice(
        [FromServices] IQueryHandler<GetFinancialAdvice.Query, FinancialAdviceResultDto> handler,
        CancellationToken ct)
    {
        var result = await handler.HandleAsync(
            new GetFinancialAdvice.Query(),
            ct);

        return Ok(result);
    }
}
