using AirQualityMonitoring.Models;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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
            var existingSensor = await _context.Sensors.FirstOrDefaultAsync(s => s.SensorId == sensor.SensorId);
            if (existingSensor != null)
            {
                return Conflict("Sensor with the same ID already exists.");
            }

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

        // Activate Sensor
        [HttpPut("activate/{id}")]
        public async Task<IActionResult> AactivateSensor(int id)
        {
            var sensor = await _context.Sensors.FindAsync(id);
            if (sensor == null) return NotFound("Sensor not found.");

            sensor.IsActive = true;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Sensor activated" });
        }

        // Delete sensor
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensor(int id)
        {
            var sensor = await _context.Sensors
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sensor == null)
            {
                return NotFound(new { message = "Sensor not found" });
            }

            // Delete related AQI readings first
            var aqiReadings = _context.AQIReadings.Where(a => a.SensorId == id);
            _context.AQIReadings.RemoveRange(aqiReadings);
            await _context.SaveChangesAsync(); // Save changes after deleting AQI readings

            // Now delete the sensor
            _context.Sensors.Remove(sensor);
            await _context.SaveChangesAsync(); // Save changes after deleting the sensor

            return Ok(new { message = "Sensor and its related AQI readings deleted successfully" });
        }


    }
}
