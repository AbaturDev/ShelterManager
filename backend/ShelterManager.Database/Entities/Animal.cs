using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShelterManager.Database.Commons;

namespace ShelterManager.Database.Entities;

public sealed record Animal : BaseEntity
{
    public required string Name { get; set; }

    
    private sealed class Configuration : BaseEntityConfiguration<Animal>
    {
        public override void Configure(EntityTypeBuilder<Animal> builder)
        {
            base.Configure(builder);

            builder.ToTable("Animals");
        }
    }
}
