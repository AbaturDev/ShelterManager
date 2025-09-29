using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShelterManager.Database.Commons;
using ShelterManager.Database.Enums;

namespace ShelterManager.Database.Entities;

public sealed record Animal : BaseEntity
{
    public required string Name { get; set; }
    public required DateTimeOffset AdmissionDate { get; set; }
    public required AnimalStatus Status { get; set; }
    public int? Age { get; set; }
    public string? ImagePath { get; set; }
    public string? Description { get; set; }
    public Guid BreedId { get; set; }
    public Breed Breed { get; set; } = null!;
    public ICollection<Event> Events { get; set; } = new List<Event>();
    private sealed class Configuration : BaseEntityConfiguration<Animal>
    {
        public override void Configure(EntityTypeBuilder<Animal> builder)
        {
            base.Configure(builder);

            builder.ToTable("Animals");
            
            builder.Property(a => a.Status)
                .HasConversion<string>();
            
            builder.HasOne(a => a.Breed)
                .WithMany(b => b.Animals)
                .HasForeignKey(a => a.BreedId);
            
            builder.HasMany(a => a.Events)
                .WithOne(e => e.Animal)
                .HasForeignKey(e => e.AnimalId);
        }
    }
}
