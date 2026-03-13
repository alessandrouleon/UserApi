using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserApin.Entities;
using UserApin.ValueObjects;

namespace UserApistructure.Persistence.Mappings;

public sealed class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedNever();

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.Password)
            .HasColumnName("PasswordHash")
            .HasMaxLength(256)
            .IsRequired()
            .HasConversion(
                password => password.Hash,
                hash => Password.FromHash(hash));

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .IsRequired(false);

        builder.Property(u => u.DeletedAt)
            .IsRequired(false);

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasFilter("[DeletedAt] IS NULL");
    }
}
