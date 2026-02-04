using KLD.CoachMoney.Contracts.Debts;

namespace KLD.CoachMoney.Contracts.FinancialAdvice;

public sealed class FinancialSnapshotDto
{
    // Identity (optional, but useful for logging)
    public Guid UserId { get; init; }

    // Income
    public decimal MonthlyIncome { get; init; }

    // Debt
    public decimal TotalDebt { get; init; }
    public int DebtCount { get; init; }
    public decimal MonthlyDebtPayments { get; init; }

    // Savings
    public decimal SavingsBalance { get; init; }
    public decimal EmergencyFundTarget { get; init; }

    // Cash flow
    public decimal MonthlyExpenses { get; init; }
    public decimal DisposableIncome { get; init; }

    // Risk signals (pre-calculated, NOT AI)
    public bool HasOverdueDebt { get; init; }
    public bool IsLivingPaycheckToPaycheck { get; init; }

    // Optional: debt breakdown (AI loves structured lists)
    public IReadOnlyList<DebtDto> Debts { get; init; } = [];
}
