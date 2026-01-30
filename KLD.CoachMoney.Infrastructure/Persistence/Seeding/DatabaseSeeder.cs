using KLD.CoachMoney.Application.Abstractions;
using KLD.CoachMoney.Domain.Entities;
using KLD.CoachMoney.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLD.CoachMoney.Infrastructure.Persistence.Seeding
{
    public sealed class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly ApplicationDbContext _db;

        public DatabaseSeeder(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task SeedAsync(CancellationToken ct = default)
        {
            await SeedUsersAsync(ct);
            await SeedDebtsAsync(ct);
        }

        private async Task SeedUsersAsync(CancellationToken ct)
        {
            if (await _db.Users.AnyAsync(ct))
                return;

            var user = new User(
                "demo@coachmoney.com",
                "Demo User");

            _db.Users.Add(user);

            await _db.SaveChangesAsync(ct);
        }

        private async Task SeedDebtsAsync(CancellationToken ct)
        {
            if (await _db.Debts.AnyAsync(ct))
                return;

            var user = await _db.Users.FirstAsync(ct);

            var debt = new Debt(
                user.Id,
                "BPI Credit Card",
                DebtType.CreditCard,
                15000,
                DateTime.UtcNow.AddDays(30));

            _db.Debts.Add(debt);

            await _db.SaveChangesAsync(ct);
        }
    }
}
