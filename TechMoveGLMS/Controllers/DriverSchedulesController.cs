using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Data;
using TechMoveGLMS.Models;
using TechMoveGLMS.ViewModels;

namespace TechMoveGLMS.Controllers
{
    public class DriverSchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DriverSchedulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // DASHBOARD - Visual dashboard with statistics and trip cards
        // ==========================================
        public async Task<IActionResult> Dashboard(string filter = "upcoming")
        {
            var allSchedules = await _context.DriverSchedules
                .OrderBy(ds => ds.ScheduleDate)
                .ThenBy(ds => ds.DepartureTime)
                .ToListAsync();

            // Statistics
            ViewBag.TodayTrips = allSchedules.Count(s => s.ScheduleDate.Date == DateTime.Today);
            ViewBag.InProgressTrips = allSchedules.Count(s => s.Status == TripStatus.InProgress);
            ViewBag.ScheduledTrips = allSchedules.Count(s => s.Status == TripStatus.Scheduled && s.ScheduleDate.Date >= DateTime.Today);
            ViewBag.TotalDrivers = await _context.Drivers.CountAsync();
            ViewBag.CurrentFilter = filter;

            // Filter schedules based on selected tab
            if (filter == "inprogress")
            {
                return View(allSchedules.Where(s => s.Status == TripStatus.InProgress).ToList());
            }
            else if (filter == "all")
            {
                return View(allSchedules.ToList());
            }
            else // upcoming
            {
                return View(allSchedules.Where(s => s.ScheduleDate.Date >= DateTime.Today && s.Status != TripStatus.Completed).ToList());
            }
        }

        // ==========================================
        // INDEX - Complete list in table format
        // ==========================================
        public async Task<IActionResult> Index()
        {
            var schedules = await _context.DriverSchedules
                .OrderByDescending(ds => ds.ScheduleDate)
                .ThenBy(ds => ds.DepartureTime)
                .ToListAsync();
            return View(schedules);
        }

        // ==========================================
        // DETAILS - Detailed view of a single schedule
        // ==========================================
        public async Task<IActionResult> Details(int id)
        {
            var schedule = await _context.DriverSchedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            return View(schedule);
        }

        // ==========================================
        // CREATE - Form for new trip (text input for driver name)
        // ==========================================
        public async Task<IActionResult> Create()
        {
            // Get existing drivers for auto-suggest
            ViewBag.ExistingDrivers = await _context.Drivers.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDriverScheduleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var schedule = new DriverSchedule
                {
                    DriverName = model.DriverName,
                    DriverPhone = model.DriverPhone,
                    VehicleNumber = model.VehicleNumber,
                    ScheduleDate = model.ScheduleDate,
                    PickupLocation = model.PickupLocation,
                    DeliveryLocation = model.DeliveryLocation,
                    DepartureTime = model.DepartureTime,
                    ArrivalTime = model.ArrivalTime,
                    Status = model.Status,
                    ReferenceNumber = model.ReferenceNumber,
                    Description = model.Description,
                    CreatedAt = DateTime.UtcNow
                };

                _context.DriverSchedules.Add(schedule);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Trip scheduled for {schedule.DriverName} on {schedule.ScheduleDate:yyyy-MM-dd}!";
                return RedirectToAction(nameof(Dashboard));
            }

            ViewBag.ExistingDrivers = await _context.Drivers.ToListAsync();
            return View(model);
        }

        // ==========================================
        // UPDATE STATUS - Change trip status
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, TripStatus status)
        {
            var schedule = await _context.DriverSchedules.FindAsync(id);
            if (schedule != null)
            {
                schedule.Status = status;
                schedule.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Trip status updated to {status}";
            }
            return RedirectToAction(nameof(Dashboard));
        }

        // ==========================================
        // DELETE - Remove a schedule
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var schedule = await _context.DriverSchedules.FindAsync(id);
            if (schedule != null)
            {
                _context.DriverSchedules.Remove(schedule);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Schedule deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}