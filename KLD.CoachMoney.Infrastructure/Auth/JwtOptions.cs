using System;
using System.Collections.Generic;
using System.Text;

namespace KLD.CoachMoney.Infrastructure.Auth
{
    public sealed class JwtOptions
    {
        public string Issuer { get; init; } = null!;
        public string Audience { get; init; } = null!;
        public string Key { get; init; } = null!;
        public int ExpiryMinutes { get; init; }
    }
}
