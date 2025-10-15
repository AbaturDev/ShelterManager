using System.Collections;
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
    public required Sex Sex { get; init; }
    public Guid BreedId { get; set; }
    public Breed Breed { get; set; } = null!;
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<DailyTask> DailyTasks { get; set; } = new List<DailyTask>();
    public ICollection<DailyTaskDefaultEntry> DailyTaskDefaultEntries { get; set; } = new List<DailyTaskDefaultEntry>();
    public ICollection<Adoption> Adoptions { get; set; } = new List<Adoption>();
    
    private sealed class Configuration : BaseEntityConfiguration<Animal>
    {
        public override void Configure(EntityTypeBuilder<Animal> builder)
        {
            base.Configure(builder);

            builder.ToTable("Animals");
            
            builder.Property(a => a.Status)
                .HasConversion<string>();
            
            builder.Property(a => a.Sex)
                .HasConversion<string>();
            
            builder.HasOne(a => a.Breed)
                .WithMany(b => b.Animals)
                .HasForeignKey(a => a.BreedId);
            
            builder.HasMany(a => a.Events)
                .WithOne(e => e.Animal)
                .HasForeignKey(e => e.AnimalId);
            
            builder.HasMany(a => a.DailyTasks)
                .WithOne(d => d.Animal)
                .HasForeignKey(d => d.AnimalId);
            
            builder.HasMany(a => a.DailyTaskDefaultEntries)
                .WithOne(d => d.Animal)
                .HasForeignKey(d => d.AnimalId);
            
            builder.HasMany(a => a.Adoptions)
                .WithOne(ad => ad.Animal)
                .HasForeignKey(ad => ad.AnimalId);
        }
    }
}
