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
    public DateTimeOffset? CompletedAt { get; set; }
    public decimal Cost { get; set; }
    public required string Location { get; set; }
    public Guid AnimalId { get; set; }
    public Guid UserId { get; set; }
    
    public User User { get; set; } = null!;
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
            
            builder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);
        }
    }
}