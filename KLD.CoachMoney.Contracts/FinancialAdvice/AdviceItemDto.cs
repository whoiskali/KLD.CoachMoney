namespace KLD.CoachMoney.Contracts.FinancialAdvice;

public sealed class AdviceItemDto
{
    public string Title { get; init; } = null!;
    public string Reason { get; init; } = null!;

    // Optional link to action (NOT executed automatically)
    public Guid? RelatedDebtId { get; init; }
    public decimal? SuggestedAmount { get; init; }
}
