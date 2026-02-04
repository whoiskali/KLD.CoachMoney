using System.Security.Claims;

namespace KLD.CoachMoney.Application.Abstractions.Auth;

public interface ITokenService
{
    string GenerateAccessToken(
        Guid userId,
        IEnumerable<Claim>? additionalClaims = null);
}
