using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShelterManager.Database.Contexts;
using ShelterManager.Database.Entities;

namespace ShelterManager.Services.BackgroundServices;

/// <summary>
/// Background service that generates daily tasks for all animals in the shelter.
/// 
/// The service waits until 00:01 UTC of the next day after startup, then runs daily at the same time.
/// For each animal, it creates a new DailyTask entity for the current date if one does not already exist,
/// populating it with entries based on the animal's default daily task entries.
/// 
/// Errors during execution are logged using the provided ILogger.
/// </summary>
public class DailyTaskBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DailyTaskBackgroundService> _logger;
    private readonly TimeProvider _timeProvider;
    
    public DailyTaskBackgroundService(IServiceProvider serviceProvider, ILogger<DailyTaskBackgroundService> logger, TimeProvider timeProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _timeProvider = timeProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var now = _timeProvider.GetUtcNow();
        var nextRun = now.Date.AddDays(1).AddMinutes(1);
        
        var delay = nextRun - now;
        await Task.Delay(delay, stoppingToken);

        var timer = new PeriodicTimer(TimeSpan.FromDays(1));
        do
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var context = scope.ServiceProvider.GetRequiredService<ShelterManagerContext>();

                var animals = await context.Animals
                    .Include(a => a.DailyTaskDefaultEntries)
                    .Include(a => a.DailyTasks)
                    .ToListAsync(stoppingToken);

                var date = DateOnly.FromDateTime(_timeProvider.GetUtcNow().UtcDateTime);

                foreach (var animal in animals)
                {
                    if (animal.DailyTasks.Any(a => a.Date == date))
                    {
                        continue;
                    }

                    var dailyTask = new DailyTask
                    {
                        Date = date,
                        Entries = animal.DailyTaskDefaultEntries
                            .Where(x => x.AnimalId == animal.Id)
                            .Select(x => new DailyTaskEntry
                            {
                                Title = x.Title,
                                Description = x.Description,
                                IsCompleted = false
                            }).ToList()
                    };

                    animal.DailyTasks.Add(dailyTask);
                }

                await context.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while running daily task background service");
            }
        } while (await timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested);
    }
}