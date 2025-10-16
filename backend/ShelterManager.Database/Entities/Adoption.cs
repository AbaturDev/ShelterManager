using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShelterManager.Database.Commons;
using ShelterManager.Database.Entities.Owned;
using ShelterManager.Database.Enums;

namespace ShelterManager.Database.Entities;

public sealed record Adoption : BaseEntity
{
    public string? Note { get; set; }
    public DateTimeOffset StartAdoptionProcess { get; init; }
    public DateTimeOffset? AdoptionDate { get; set; }
    public AdoptionStatus Status { get; set; }
    public required AdoptionPerson Person { get; init; }
    public required Guid AnimalId { get; init; }
    
    public Animal Animal { get; init; } = null!;

    private sealed class Configuration : BaseEntityConfiguration<Adoption>
    {
        public override void Configure(EntityTypeBuilder<Adoption> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Status)
                .HasConversion<string>();

            builder.OwnsOne(x => x.Person);

            builder.HasOne(x => x.Animal)
                .WithMany(a => a.Adoptions)
                .HasForeignKey(x => x.AnimalId);
        }
    }
}