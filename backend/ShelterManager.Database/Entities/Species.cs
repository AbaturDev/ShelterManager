using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShelterManager.Database.Commons;

namespace ShelterManager.Database.Entities;

public sealed record Species : BaseEntity
{
    public required string Name { get; set; }
    public ICollection<Breed> Breeds { get; set; } = null!;

    private sealed class Configuration : BaseEntityConfiguration<Species>
    {
        public override void Configure(EntityTypeBuilder<Species> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("Species");

            builder.HasMany(s => s.Breeds)
                .WithOne(b => b.Species)
                .HasForeignKey(b => b.SpeciesId);
        }
    }
}