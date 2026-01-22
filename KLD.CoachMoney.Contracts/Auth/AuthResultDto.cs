namespace KLD.CoachMoney.Contracts.Auth;

public sealed class AuthResultDto
{
    public string AccessToken { get; init; } = null!;

    public DateTime ExpiresAtUtc { get; init; }

    // Optional but useful for clients
    public string TokenType { get; init; } = "Bearer";
}
