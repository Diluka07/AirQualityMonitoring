namespace AirQualityMonitoring.Models
{
    public class Sensor
    {
        public int Id { get; set; } // Primary Key
        public string SensorId { get; set; } // Unique Sensor Identifier
        public string City { get; set; } // City/Area Name
        public double Latitude { get; set; } // GPS Lat
        public double Longitude { get; set; } // GPS Lon
        public bool IsActive { get; set; } = true; // Active/Inactive status

        // Navigation property
        public List<AQIReading>? AQIReadings { get; set; }
    }
}
