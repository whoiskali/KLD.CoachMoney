namespace KLD.CoachMoney.Infrastructure.AI;

public sealed class FinancialAiOptions
{
    public string ApiKey { get; init; } = null!;
    public string BaseUrl { get; init; } = "https://api.openai.com/v1";
    public string Model { get; init; } = "gpt-5-mini"; // START CHEAP
    public int MaxTokens { get; init; } = 800;
}
