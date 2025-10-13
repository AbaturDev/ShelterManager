using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShelterManager.Database.Commons;

namespace ShelterManager.Database.Entities;

public sealed record DailyTaskEntry : BaseEntity
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public Guid DailyTaskId { get; init; }
    public Guid? UserId { get; set; }

    public DailyTask DailyTask { get; init; } = null!;
    public User? User { get; set; }

    private sealed class Configuration : BaseEntityConfiguration<DailyTaskEntry>
    {
        public override void Configure(EntityTypeBuilder<DailyTaskEntry> builder)
        {
            base.Configure(builder);

            builder.HasOne(e => e.DailyTask)
                .WithMany(d => d.Entries)
                .HasForeignKey(e => e.DailyTaskId);
            
            builder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);
        }
    }

}