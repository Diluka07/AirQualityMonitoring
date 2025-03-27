using AirQualityMonitoring.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AirQualityMonitoring
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<AQIReading> AQIReadings { get; set; }
    }
}
