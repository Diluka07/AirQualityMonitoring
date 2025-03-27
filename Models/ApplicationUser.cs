using Microsoft.AspNetCore.Identity;

namespace AirQualityMonitoring.Models
{
    public class ApplicationUser : IdentityUser 
    {
        public string FullName { get; set; }
    }
}
