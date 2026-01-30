using KLD.CoachMoney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KLD.CoachMoney.Infrastructure.Persistence.Configurations;

public sealed class CoachSessionConfiguration : IEntityTypeConfiguration<CoachSession>
{
    public void Configure(EntityTypeBuilder<CoachSession> builder)
    {
        builder.ToTable("CoachSessions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Topic)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.StartedAt)
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.Sessions)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
