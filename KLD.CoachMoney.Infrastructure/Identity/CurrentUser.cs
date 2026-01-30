using KLD.CoachMoney.Application.Abstractions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace KLD.CoachMoney.Infrastructure.Identity
{

    public sealed class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid UserId
        {
            get
            {
                var value = _httpContextAccessor.HttpContext?.User?
                    .FindFirstValue(ClaimTypes.NameIdentifier);

                if (Guid.TryParse(value, out var userId))
                    return userId;
                return Guid.Empty;
            }
        }
        public string Username
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?
                    .FindFirstValue(ClaimTypes.Name) ?? "";
            }
        }
        public string Name
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?
                    .FindFirstValue(ClaimTypes.GivenName) ?? "";
            }
        }
    }
}
