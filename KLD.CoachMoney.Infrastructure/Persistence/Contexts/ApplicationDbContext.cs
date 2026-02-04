using KLD.CoachMoney.Application.Abstractions;
using KLD.CoachMoney.Domain.Abstracts;
using KLD.CoachMoney.Domain.Entities;
using KLD.CoachMoney.Domain.Entities;
using KLD.CoachMoney.Domain.Enums;
using KLD.CoachMoney.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KLD.CoachMoney.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUser currentUser) : DbContext(options),
      IApplicationDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Debt> Debts { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }

        #region Overrides   
        public override async Task<int> SaveChangesAsync(
    CancellationToken cancellationToken = default)
        {
            var auditEntries = new List<AuditTrail>();
            var utcNow = DateTime.UtcNow;

            var entries = ChangeTracker.Entries<AuditableEntity>()
                .Where(e => e.State is EntityState.Added
                         or EntityState.Modified
                         or EntityState.Deleted)
                .ToList();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.SetCreated(currentUser.UserId, utcNow);
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.SetUpdated(currentUser.UserId, utcNow);
                }

                auditEntries.Add(new AuditTrail
                {
                    TableName = entry.Metadata.ClrType.Name,
                    EntityId = GetPrimaryKey(entry),
                    Action = MapAction(entry.State),
                    Timestamp = DateTimeOffset.UtcNow,
                    PerformedById = currentUser.UserId,
                    PerformedByName = currentUser.Name,
                    Changes = BuildNestedDiff(entry)
                });
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            if (auditEntries.Any())
            {
                ChangeTracker.AutoDetectChangesEnabled = false;

                AuditTrails.AddRange(auditEntries);
                await base.SaveChangesAsync(cancellationToken);

                ChangeTracker.AutoDetectChangesEnabled = true;
            }

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        #endregion

        #region Private
        private static string GetPrimaryKey(EntityEntry entry)
        {
            var key = entry.Properties.First(p => p.Metadata.IsPrimaryKey());
            return key.CurrentValue?.ToString() ?? string.Empty;
        }

        private static AuditAction MapAction(EntityState state) =>
        state switch
        {
            EntityState.Added => AuditAction.Added,
            EntityState.Modified => AuditAction.Modified,
            EntityState.Deleted => AuditAction.Deleted,
            _ => throw new ArgumentOutOfRangeException()
        };
        private static string BuildNestedDiff(EntityEntry entry)
        {
            var changes = new Dictionary<string, object?>();

            foreach (var prop in entry.Properties)
            {
                if (prop.Metadata.IsPrimaryKey())
                    continue;

                var name = prop.Metadata.Name;

                switch (entry.State)
                {
                    case EntityState.Added:
                        changes[name] = new
                        {
                            oldValue = (object?)null,
                            newValue = prop.CurrentValue
                        };
                        break;

                    case EntityState.Deleted:
                        changes[name] = new
                        {
                            oldValue = prop.OriginalValue,
                            newValue = (object?)null
                        };
                        break;

                    case EntityState.Modified:
                        if (!Equals(prop.OriginalValue, prop.CurrentValue))
                        {
                            changes[name] = new
                            {
                                oldValue = prop.OriginalValue,
                                newValue = prop.CurrentValue
                            };
                        }
                        break;
                }
            }

            return JsonSerializer.Serialize(changes);
        }
        #endregion
    }
}
