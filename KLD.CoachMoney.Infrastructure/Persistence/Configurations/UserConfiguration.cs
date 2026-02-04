using KLD.CoachMoney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KLD.CoachMoney.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Email)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.DisplayName)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasIndex(x => x.Email)
            .IsUnique();

        // Relationships
        //builder.HasMany<Debt>()
        //    .WithOne()
        //    .HasForeignKey(d => d.UserId)
        //    .OnDelete(DeleteBehavior.Cascade);

        //builder.HasMany<CoachSession>()
        //    .WithOne()
        //    .OnDelete(DeleteBehavior.Cascade);
    }
}
