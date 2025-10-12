using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShelterManager.Database.Commons;

namespace ShelterManager.Database.Entities;

public sealed record DailyTask : BaseEntity
{
    public DateOnly Date { get; init; }
    public Guid AnimalId { get; init; }

    public Animal Animal { get; init; } = null!;
    public ICollection<DailyTaskEntry> Entries { get; init; } = new List<DailyTaskEntry>();
    
    private sealed class Configuration : BaseEntityConfiguration<DailyTask>
    {
        public override void Configure(EntityTypeBuilder<DailyTask> builder)
        {
            base.Configure(builder);

            builder.HasIndex(d => d.Date);
            builder.HasIndex(d => d.AnimalId);
            
            builder.HasOne(d => d.Animal)
                .WithMany(a => a.DailyTasks)
                .HasForeignKey(d => d.AnimalId);
            
            builder.HasMany(d => d.Entries)
                .WithOne(e => e.DailyTask)
                .HasForeignKey(e => e.DailyTaskId);
        }
    }
}