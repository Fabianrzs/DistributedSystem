using Authentications.Domain.Modules.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentications.Infrastructure.Implementations.Persistence.EFCore.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired();

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.Password)
            .IsRequired();

        builder.HasMany(e => e.UserRoles)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Sessions)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
