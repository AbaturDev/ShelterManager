using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ShelterManager.Database.Commons;
using ShelterManager.Database.Entities;

namespace ShelterManager.Database.Contexts;

public sealed class ShelterManagerContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    private readonly TimeProvider _timeProvider;
    
    public ShelterManagerContext(DbContextOptions<ShelterManagerContext> options, TimeProvider timeProvider) : base(options)
    {
        _timeProvider = timeProvider;

        ChangeTracker.StateChanged += UpdateTimestamps;
        ChangeTracker.Tracked += UpdateTimestamps;
    }

    public DbSet<Animal> Animals { get; init; }
    public DbSet<Species> Species { get; init; }
    public DbSet<Breed> Breeds { get; init; }
    public DbSet<Event> Events { get; init; }
    public DbSet<RefreshToken> RefreshTokens { get; init; }
    public DbSet<DailyTask> DailyTasks { get; init; }
    public DbSet<DailyTaskEntry> DailyTaskEntries { get; init; }
    public DbSet<DailyTaskDefaultEntry> DailyTaskDefaultEntries { get; init; }
    public DbSet<Adoption> Adoptions { get; init; }
    public DbSet<ShelterConfiguration> ShelterConfigurations { get; init; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(BaseEntityConfiguration<>).Assembly);
        
        builder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
    }

    private void UpdateTimestamps(object? sender, EntityEntryEventArgs e)
    {
        if (e.Entry.Entity is not ITimeTrackable timeTrackable)
        {
            return;
        }

        switch (e.Entry.State)
        {
            case EntityState.Added:
                timeTrackable.CreatedAt = _timeProvider.GetUtcNow();
                timeTrackable.UpdatedAt = _timeProvider.GetUtcNow();
                return;
            case EntityState.Modified:
                timeTrackable.UpdatedAt = _timeProvider.GetUtcNow();
                return;
        }
    }
}