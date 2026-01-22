namespace KLD.CoachMoney.Contracts.FinancialAdvice;

public sealed class FinancialAdviceResultDto
{
    // Short summary (1–3 sentences)
    public string Summary { get; init; } = null!;

    // Key recommendations (ordered)
    public IReadOnlyList<AdviceItemDto> Recommendations { get; init; } = [];

    // Risk notes / warnings
    public IReadOnlyList<string> Warnings { get; init; } = [];

    // Coaching / encouragement (optional)
    public string? CoachingNote { get; init; }
}
