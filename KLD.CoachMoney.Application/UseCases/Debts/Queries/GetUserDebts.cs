
using KLD.CoachMoney.Application.Abstractions;
using KLD.CoachMoney.Application.Abstractions.Messaging;
using KLD.CoachMoney.Contracts.Debts;
using Microsoft.EntityFrameworkCore;

namespace KLD.CoachMoney.Application.UseCases.Debts.Queries;

public static class GetUserDebts
{
    // =========================
    // QUERY
    // =========================
    public sealed class Query : IQuery<IReadOnlyList<DebtDto>>
    {
    }

    // =========================
    // HANDLER
    // =========================
    public sealed class Handler(
            IApplicationDbContext db,
            ICurrentUser currentUser)
        : IQueryHandler<Query, IReadOnlyList<DebtDto>>
    {

        public async Task<IReadOnlyList<DebtDto>> HandleAsync(
            Query query,
            CancellationToken ct)
        {
            var userId = currentUser.UserId;

            return await db.Debts
                .Where(d => d.UserId == userId)
                .OrderBy(d => d.DueDate)
                .AsNoTracking()
                .Select(d => new DebtDto
                {
                    Id = d.Id,
                    Creditor = d.Creditor,
                    OriginalAmount = d.OriginalAmount,
                    CurrentBalance = d.CurrentBalance,
                    DueDate = d.DueDate,
                    Type = d.Type.ToString()
                })
                .ToListAsync(ct);
        }
    }
}
