using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KLD.CoachMoney.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLD.CoachMoney.Infrastructure.Persistence.Configurations
{
    internal class AuditTrailEntityConfiguration
    : IEntityTypeConfiguration<AuditTrail>
    {
        public void Configure(EntityTypeBuilder<AuditTrail> builder)
        {
            // Table
            builder.ToTable("AuditTrails");

            // Primary key
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            // Entity info
            builder.Property(x => x.TableName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.EntityId)
                .IsRequired()
                .HasMaxLength(50);

            // Action (enum stored as string)
            builder.Property(x => x.Action)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(x => x.Changes)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(x => x.PerformedByName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.Timestamp)
                .IsRequired();

            // Indexes
            builder.HasIndex(x => new { x.TableName, x.EntityId });
            builder.HasIndex(x => x.PerformedByName);
            builder.HasIndex(x => x.Timestamp);
        }
    }
}
