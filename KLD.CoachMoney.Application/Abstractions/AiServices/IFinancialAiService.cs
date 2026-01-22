using KLD.CoachMoney.Contracts.FinancialAdvice;
using KLD.CoachMoney.Domain.Enums.AiEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLD.CoachMoney.Application.Abstractions.AIServices
{
    public interface IFinancialAiService
    {
        Task<FinancialAdviceResultDto> GetAdviceAsync(
            FinancialSnapshotDto snapshot,
            AdviceIntent intent,
            CancellationToken ct);
    }
}
