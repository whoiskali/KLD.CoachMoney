using KLD.CoachMoney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KLD.CoachMoney.Infrastructure.Persistence.Configurations;

public sealed class AuditTrailConfiguration : IEntityTypeConfiguration<AuditTrail>
{
    public void Configure(EntityTypeBuilder<AuditTrail> builder)
    {
        builder.ToTable("AuditTrails");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.TableName)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.EntityId)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.Action)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Changes)
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.PerformedById)
            .IsRequired();

        builder.Property(x => x.PerformedByName)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.Timestamp)
            .IsRequired();

        builder.HasIndex(x => x.TableName);
        builder.HasIndex(x => x.EntityId);
        builder.HasIndex(x => x.PerformedById);
        builder.HasIndex(x => x.Timestamp);
    }
}
