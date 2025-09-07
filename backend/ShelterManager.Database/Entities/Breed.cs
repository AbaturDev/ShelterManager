using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShelterManager.Database.Commons;

namespace ShelterManager.Database.Entities;

public sealed record Breed : BaseEntity
{
    public required string Name { get; set; }
    public Guid SpeciesId { get; set; }
    public Species Species { get; set; } = null!;
    public ICollection<Animal> Animals { get; set; } = null!;

    private sealed class Configuration : BaseEntityConfiguration<Breed>
    {
        public override void Configure(EntityTypeBuilder<Breed> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("Breeds");

            builder.HasMany(b => b.Animals)
                .WithOne(a => a.Breed)
                .HasForeignKey(a => a.BreedId);
            
            builder.HasOne(b => b.Species)
                .WithMany(s => s.Breeds)
                .HasForeignKey(b => b.SpeciesId);
        }
    }
}