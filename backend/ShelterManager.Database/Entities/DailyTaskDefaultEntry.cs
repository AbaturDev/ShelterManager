using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShelterManager.Database.Commons;

namespace ShelterManager.Database.Entities;

public sealed record DailyTaskDefaultEntry : BaseEntity
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public Guid AnimalId { get; set; }

    public Animal Animal { get; set; } = null!;

    public sealed class Configuration : BaseEntityConfiguration<DailyTaskDefaultEntry>
    {
        public override void Configure(EntityTypeBuilder<DailyTaskDefaultEntry> builder)
        {
            base.Configure(builder);

            builder.HasOne(d => d.Animal)
                .WithMany(a => a.DailyTaskDefaultEntries)
                .HasForeignKey(d => d.AnimalId);
        }
    }
}