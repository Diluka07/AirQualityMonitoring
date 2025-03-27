namespace AirQualityMonitoring.Models
{
    public class AQIReading
    {
        public int Id { get; set; } // Primary Key
        public int SensorId { get; set; } // Foreign Key
        public int AQIValue { get; set; } // AQI Value
        public DateTime ReadingTime { get; set; } = DateTime.UtcNow; // Timestamp

        // Navigation property
        public Sensor Sensor { get; set; }
    }
}
