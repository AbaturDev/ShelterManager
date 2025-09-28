using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShelterManager.Database.Entities;

public sealed class User : IdentityUser<Guid>
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public bool MustChangePassword { get; set; }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User));
        
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(u => u.Surname)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(u => u.MustChangePassword)
            .HasDefaultValue(true);
    }
}