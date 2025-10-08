using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShelterManager.Database.Commons;

namespace ShelterManager.Database.Entities;

public sealed record RefreshToken : BaseEntity
{
    public required string Token { get; init; }
    public bool IsRevoked { get; set; }
    public DateTimeOffset ExpiresAt { get; init; }
    public required Guid UserId { get; init; }
    
    public User User { get; init; } = null!;

    private sealed class Configuration : BaseEntityConfiguration<RefreshToken>
    {
        public override void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }
    }
}