namespace KLD.CoachMoney.Contracts.FinancialAdvice;

public sealed class FinancialAdviceResultDto
{
    // Metadata
    public DateTime GeneratedAt { get; init; }

    // Overall assessment
    public string Summary { get; init; } = default!;

    // Main recommendation
    public string PrimaryRecommendation { get; init; } = default!;

    // Step-by-step plan
    public IReadOnlyList<string> ActionSteps { get; init; }
        = Array.Empty<string>();

    // Warnings / red flags
    public IReadOnlyList<string> RiskAlerts { get; init; }
        = Array.Empty<string>();

    // Financial snapshot
    public decimal TotalDebt { get; init; }
    public decimal TotalMonthlyPayments { get; init; }
    public decimal EstimatedPayoffMonths { get; init; }

    // Confidence score (AI self-eval)
    public int ConfidenceScore { get; init; } // 0–100
}
