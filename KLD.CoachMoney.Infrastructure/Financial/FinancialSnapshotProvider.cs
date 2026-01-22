using KLD.CoachMoney.Application.Abstractions;
using KLD.CoachMoney.Application.Abstractions.AiServices;
using KLD.CoachMoney.Contracts.Debts;
using KLD.CoachMoney.Contracts.FinancialAdvice;
using KLD.CoachMoney.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KLD.CoachMoney.Infrastructure.Financial;

public sealed class FinancialSnapshotProvider(IApplicationDbContext db) : IFinancialSnapshotProvider
{
    public async Task<FinancialSnapshotDto> BuildAsync(
        Guid userId,
        CancellationToken ct)
    {
        // --------------------
        // Debts
        // --------------------
        var debts = await db.Debts
            .Where(d => d.UserId == userId)
            .AsNoTracking()
            .ToListAsync(ct);

        var debtItems = debts.Select(d => new DebtDto
        {
            Id = d.Id,
            Creditor = d.Creditor,
            CurrentBalance = d.CurrentBalance,
            DueDate = d.DueDate,
            Type = d.Type.ToString(),
            MinimumPayment = d.CreditorMinimumPayment ?? CalculateMinimumPayment(d),
            IsEstimatedMinimum = d.CreditorMinimumPayment is null,
        }).ToList();

        var totalDebt = debts.Sum(d => d.CurrentBalance);
        var monthlyDebtPayments = debtItems.Sum(d => d.MinimumPayment);

        // --------------------
        // Income & Expenses (placeholder for now)
        // --------------------
        var monthlyIncome = 0m;   // TODO: integrate income module
        var monthlyExpenses = 0m; // TODO: integrate expense module

        // --------------------
        // Risk Signals
        // --------------------
        var hasOverdueDebt = debts.Any(d => d.DueDate < DateTime.UtcNow && d.CurrentBalance > 0);
        var disposableIncome = monthlyIncome - monthlyExpenses - monthlyDebtPayments;

        var isPaycheckToPaycheck =
            monthlyIncome > 0 &&
            disposableIncome <= (monthlyIncome * 0.1m);

        // --------------------
        // Snapshot
        // --------------------
        return new FinancialSnapshotDto
        {
            UserId = userId,

            MonthlyIncome = monthlyIncome,
            MonthlyExpenses = monthlyExpenses,
            DisposableIncome = disposableIncome,

            TotalDebt = totalDebt,
            DebtCount = debts.Count,
            MonthlyDebtPayments = monthlyDebtPayments,

            SavingsBalance = 0m, // TODO
            EmergencyFundTarget = monthlyExpenses * 3,

            HasOverdueDebt = hasOverdueDebt,
            IsLivingPaycheckToPaycheck = isPaycheckToPaycheck,

            Debts = debtItems
        };
    }

    private static decimal CalculateMinimumPayment(Domain.Entities.Debt debt)
    {
        // Conservative default: 3% of balance or ₱500, whichever is higher
        return Math.Max(debt.CurrentBalance * 0.03m, 500m);
    }
}
