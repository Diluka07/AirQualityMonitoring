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
    public class SensorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SensorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Register a New Sensor
        [HttpPost("register")]
        public async Task<IActionResult> RegisterSensor([FromBody] Sensor sensor)
        {
            if (sensor == null) return BadRequest("Invalid data.");

            sensor.SensorId = Guid.NewGuid().ToString(); // Generate Unique ID
            _context.Sensors.Add(sensor);
            await _context.SaveChangesAsync();
            return Ok(sensor);
        }

        // Get All Sensors
        [HttpGet("all")]
        public async Task<IActionResult> GetSensors()
        {
            var sensors = await _context.Sensors.ToListAsync();
            return Ok(sensors);
        }

        // Update Sensor (Edit or Deactivate)
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateSensor(int id, [FromBody] Sensor updatedSensor)
        {
            var sensor = await _context.Sensors.FindAsync(id);
            if (sensor == null) return NotFound("Sensor not found.");

            sensor.City = updatedSensor.City;
            sensor.Latitude = updatedSensor.Latitude;
            sensor.Longitude = updatedSensor.Longitude;
            sensor.IsActive = updatedSensor.IsActive;

            await _context.SaveChangesAsync();
            return Ok(sensor);
        }

        // Deactivate Sensor
        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> DeactivateSensor(int id)
        {
            var sensor = await _context.Sensors.FindAsync(id);
            if (sensor == null) return NotFound("Sensor not found.");

            sensor.IsActive = false;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Sensor deactivated" });
        }
    }
}
