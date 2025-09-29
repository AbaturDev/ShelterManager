using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShelterManager.Database.Commons;

namespace ShelterManager.Database.Entities;

public sealed record Event : BaseEntity
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required DateTimeOffset Date { get; set; }
    public bool IsDone { get; set; }
    public Guid AnimalId { get; set; }
    public Animal Animal { get; set; } = null!;

    private sealed class Configuration : BaseEntityConfiguration<Event>
    {
        public override void Configure(EntityTypeBuilder<Event> builder)
        {
            base.Configure(builder);
            
            builder.Property(e => e.IsDone)
                .HasDefaultValue(false);
            
            builder.HasQueryFilter(e => !e.IsDone);

            builder.HasOne(e => e.Animal)
                .WithMany(a => a.Events)
                .HasForeignKey(e => e.AnimalId);
        }
    }
}