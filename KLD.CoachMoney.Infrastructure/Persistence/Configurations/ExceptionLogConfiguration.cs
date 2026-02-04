using KLD.CoachMoney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KLD.CoachMoney.Infrastructure.Persistence.Configurations;

public sealed class ExceptionLogConfiguration : IEntityTypeConfiguration<ExceptionLog>
{
    public void Configure(EntityTypeBuilder<ExceptionLog> builder)
    {
        builder.ToTable("ExceptionLogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Username)
            .HasMaxLength(256);

        builder.Property(x => x.ExceptionMessage)
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(x => x.InnerException)
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.StackTrace)
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.Source)
            .HasMaxLength(256);

        builder.Property(x => x.OccurredAt)
            .IsRequired();

        builder.HasIndex(x => x.OccurredAt);
    }
}
