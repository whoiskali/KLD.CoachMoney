using KLD.CoachMoney.Application.Abstractions;
using KLD.CoachMoney.Application.Abstractions.AiServices;
using KLD.CoachMoney.Application.Abstractions.AIServices;
using KLD.CoachMoney.Application.Abstractions.Messaging;
using KLD.CoachMoney.Contracts.FinancialAdvice;
using KLD.CoachMoney.Domain.Enums.AiEnum;

namespace KLD.CoachMoney.Application.UseCases.FinancialAdvice.Queries;

public static class GetFinancialAdvice
{
    // =========================
    // QUERY
    // =========================
    public sealed class Query : IQuery<FinancialAdviceResultDto>
    {
        public AdviceIntent Intent { get; init; }
    }
    // =========================
    // HANDLER
    // =========================
    public sealed class Handler(
        ICurrentUser currentUser,
        IFinancialSnapshotProvider snapshotProvider,
        IFinancialAiService aiService)
        : IQueryHandler<Query, FinancialAdviceResultDto>
    {
        public async Task<FinancialAdviceResultDto> HandleAsync(
            Query query,
            CancellationToken ct)
        {
            // 1️⃣ Build deterministic snapshot (NO AI here)
            var snapshot = await snapshotProvider.BuildAsync(
                currentUser.UserId,
                ct);

            // 2️⃣ Let AI interpret the snapshot
            return await aiService.GetAdviceAsync(
                snapshot,
                query.Intent,
                ct);
        }
    }
}
