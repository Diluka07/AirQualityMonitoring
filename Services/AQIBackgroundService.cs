using AirQualityMonitoring.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AirQualityMonitoring.Services
{
    public class AQIBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly Random _random = new Random();

        public AQIBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var sensors = await _context.Sensors.Where(s => s.IsActive).ToListAsync();

                    foreach (var sensor in sensors)
                    {
                        int randomAQI = _random.Next(0, 500); // AQI range 0-500

                        var aqiReading = new AQIReading
                        {
                            SensorId = sensor.Id,
                            AQIValue = randomAQI,
                            ReadingTime = DateTime.UtcNow
                        };

                        _context.AQIReadings.Add(aqiReading);
                    }

                    await _context.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); // Wait for 5 minutes
            }
        }
    }
}
