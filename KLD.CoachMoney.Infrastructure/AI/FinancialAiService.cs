using KLD.CoachMoney.Application.Abstractions;
using KLD.CoachMoney.Application.Abstractions.AIServices;
using KLD.CoachMoney.Contracts.FinancialAdvice;
using KLD.CoachMoney.Domain.Enums.AiEnum;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace KLD.CoachMoney.Infrastructure.AI;

public sealed class FinancialAiService(
        HttpClient http,
        FinancialAiOptions options) : IFinancialAiService
{
    public async Task<FinancialAdviceResultDto> GetAdviceAsync(
        FinancialSnapshotDto snapshot,
        AdviceIntent intent,
        CancellationToken ct)
    {
        var request = new
        {
            model = options.Model,
            max_tokens = options.MaxTokens,
            temperature = 0.3,
            messages = new[]
            {
                new { role = "system", content = PromptBuilder.BuildSystemPrompt() },
                new { role = "user", content = PromptBuilder.BuildUserPrompt(snapshot, intent) }
            }
        };

        var response = await http.PostAsync(
            "/chat/completions",
            new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"),
            ct);

        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync(ct);
        using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

        var content = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        if (string.IsNullOrWhiteSpace(content))
            throw new InvalidOperationException("AI returned empty response.");

        return JsonSerializer.Deserialize<FinancialAdviceResultDto>(content!)
               ?? throw new InvalidOperationException("Invalid AI JSON response.");
    }
}
