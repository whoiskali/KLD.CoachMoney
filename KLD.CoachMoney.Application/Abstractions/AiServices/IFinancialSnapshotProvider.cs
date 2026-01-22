using KLD.CoachMoney.Contracts.FinancialAdvice;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLD.CoachMoney.Application.Abstractions.AiServices
{
    public interface IFinancialSnapshotProvider
    {
        Task<FinancialSnapshotDto> BuildAsync(
            Guid userId,
            CancellationToken ct);
    }

}
