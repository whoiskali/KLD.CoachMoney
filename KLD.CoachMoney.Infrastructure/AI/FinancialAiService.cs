using KLD.CoachMoney.Application.Abstractions;
using KLD.CoachMoney.Application.Abstractions.AIServices;
using KLD.CoachMoney.Contracts.FinancialAdvice;
using KLD.CoachMoney.Domain.Enums.AiEnum;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using System.Text.Json;

namespace KLD.CoachMoney.Infrastructure.AI;

public sealed class FinancialAiService
    : IFinancialAiService
{
    private readonly FinancialAiOptions _options;

    public FinancialAiService(
        IOptions<FinancialAiOptions> options)
    {
        _options = options.Value;
    }

    public async Task<FinancialAdviceResultDto> GetAdviceAsync(
        FinancialSnapshotDto snapshot,
        AdviceIntent intent,
        CancellationToken ct)
    {
        var client = new ChatClient(
            model: _options.Model,
            apiKey: _options.ApiKey);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(
                PromptBuilder.BuildSystemPrompt()),

            new UserChatMessage(
                PromptBuilder.BuildUserPrompt(snapshot, intent))
        };

        var response = await client.CompleteChatAsync(
            messages,
            new ChatCompletionOptions
            {
                MaxOutputTokenCount = _options.MaxTokens,
                EndUserId = snapshot.UserId.ToString(),
            },
            ct);

        // Extract AI text response
        var content = response.Value.Content[0].Text;

        if (string.IsNullOrWhiteSpace(content))
            throw new InvalidOperationException("AI returned empty response.");

        // Deserialize JSON result
        var result = JsonSerializer.Deserialize<FinancialAdviceResultDto>(
            content,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return result
            ?? throw new InvalidOperationException("Invalid AI JSON response.");
    }
}
