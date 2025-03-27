using AirQualityMonitoring.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirQualityMonitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AQIReadingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AQIReadingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1️⃣ Store AQI Reading
        [HttpPost("add")]
        public async Task<IActionResult> AddAQIReading([FromBody] AQIReading aqiReading)
        {
            if (aqiReading == null) return BadRequest("Invalid data.");

            var sensor = await _context.Sensors.FindAsync(aqiReading.SensorId);
            if (sensor == null) return NotFound("Sensor not found.");

            _context.AQIReadings.Add(aqiReading);
            await _context.SaveChangesAsync();
            return Ok(aqiReading);
        }

        // 2️⃣ Get AQI Readings for a Sensor
        [HttpGet("sensor/{sensorId}")]
        public async Task<IActionResult> GetAQIReadings(int sensorId)
        {
            var readings = await _context.AQIReadings
                .Where(r => r.SensorId == sensorId)
                .OrderByDescending(r => r.ReadingTime)
                .ToListAsync();

            return Ok(readings);
        }
    }
}
