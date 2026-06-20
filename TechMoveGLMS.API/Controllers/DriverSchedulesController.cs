using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Data;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverSchedulesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DriverSchedulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/driverschedules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverSchedule>>> GetDriverSchedules()
        {
            return await _context.DriverSchedules.ToListAsync();
        }

        // GET: api/driverschedules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DriverSchedule>> GetDriverSchedule(int id)
        {
            var schedule = await _context.DriverSchedules.FindAsync(id);

            if (schedule == null)
            {
                return NotFound();
            }

            return schedule;
        }

        // POST: api/driverschedules
        [HttpPost]
        public async Task<ActionResult<DriverSchedule>> PostDriverSchedule(DriverSchedule schedule)
        {
            _context.DriverSchedules.Add(schedule);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriverSchedule", new { id = schedule.Id }, schedule);
        }

        // PATCH: api/driverschedules/5/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] TripStatus status)
        {
            var schedule = await _context.DriverSchedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            schedule.Status = status;
            schedule.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/driverschedules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriverSchedule(int id)
        {
            var schedule = await _context.DriverSchedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            _context.DriverSchedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
