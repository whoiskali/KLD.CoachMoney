using System;
using System.Collections.Generic;
using System.Text;

namespace KLD.CoachMoney.Application.Abstractions
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync(CancellationToken ct = default);
    }
}
