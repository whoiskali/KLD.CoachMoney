using KLD.CoachMoney.Contracts.FinancialAdvice;
using KLD.CoachMoney.Domain.Enums.AiEnum;
using System.Text;
using System.Text.Json;

namespace KLD.CoachMoney.Infrastructure.AI;

internal static class PromptBuilder
{
    public static string BuildSystemPrompt()
        => """
You are a financial coaching assistant.

Rules:
- Educational guidance only, not professional financial advice.
- Never suggest illegal, risky, or guaranteed outcomes.
- Do NOT recommend specific financial products.
- Do NOT execute actions.
- Output MUST be valid JSON only.
""";

    public static string BuildUserPrompt(
        FinancialSnapshotDto snapshot,
        AdviceIntent intent)
    {
        var payload = JsonSerializer.Serialize(snapshot);

        return intent switch
        {
            AdviceIntent.DebtPriority =>
                $"""
Analyze the financial snapshot below.
Identify the top 1–3 priorities related to debt.
Keep explanations short and factual.

Snapshot:
{payload}
""",

            AdviceIntent.Snapshot =>
                $"""
Summarize the user's financial situation in simple terms.
Avoid judgmental language.

Snapshot:
{payload}
""",

            AdviceIntent.Coaching =>
                $"""
Provide short, encouraging coaching advice.
Focus on behavior and mindset, not actions.

Snapshot:
{payload}
""",

            _ =>
                $"""
Provide general financial guidance.

Snapshot:
{payload}
"""
        };
    }
}
