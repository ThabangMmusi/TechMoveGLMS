using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Data;
using TechMoveGLMS.Models;
using TechMoveGLMS.ViewModels;
using TechMoveGLMS.Services;

namespace TechMoveGLMS.Controllers
{
    public class DriverSchedulesController : Controller
    {
        private readonly IApiService _apiService;

        public DriverSchedulesController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // ==========================================
        // DASHBOARD - Visual dashboard with statistics and trip cards
        // ==========================================
        public async Task<IActionResult> Dashboard(string filter = "upcoming")
        {
            var allSchedules = await _apiService.GetDriverSchedulesAsync();
            var sortedSchedules = allSchedules
                .OrderBy(ds => ds.ScheduleDate)
                .ThenBy(ds => ds.DepartureTime)
                .ToList();

            // Statistics
            ViewBag.TodayTrips = sortedSchedules.Count(s => s.ScheduleDate.Date == DateTime.Today);
            ViewBag.InProgressTrips = sortedSchedules.Count(s => s.Status == TripStatus.InProgress);
            ViewBag.ScheduledTrips = sortedSchedules.Count(s => s.Status == TripStatus.Scheduled && s.ScheduleDate.Date >= DateTime.Today);
            
            var drivers = await _apiService.GetDriversAsync();
            ViewBag.TotalDrivers = drivers.Count();
            ViewBag.CurrentFilter = filter;

            // Filter schedules based on selected tab
            if (filter == "inprogress")
            {
                return View(sortedSchedules.Where(s => s.Status == TripStatus.InProgress).ToList());
            }
            else if (filter == "all")
            {
                return View(sortedSchedules);
            }
            else // upcoming
            {
                return View(sortedSchedules.Where(s => s.ScheduleDate.Date >= DateTime.Today && s.Status != TripStatus.Completed).ToList());
            }
        }

        // ==========================================
        // INDEX - Complete list in table format
        // ==========================================
        public async Task<IActionResult> Index()
        {
            var allSchedules = await _apiService.GetDriverSchedulesAsync();
            var schedules = allSchedules
                .OrderByDescending(ds => ds.ScheduleDate)
                .ThenBy(ds => ds.DepartureTime)
                .ToList();
            return View(schedules);
        }

        // ==========================================
        // DETAILS - Detailed view of a single schedule
        // ==========================================
        public async Task<IActionResult> Details(int id)
        {
            var schedule = await _apiService.GetDriverScheduleByIdAsync(id);
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
            ViewBag.ExistingDrivers = await _apiService.GetDriversAsync();
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

                await _apiService.CreateDriverScheduleAsync(schedule);

                TempData["Success"] = $"Trip scheduled for {schedule.DriverName} on {schedule.ScheduleDate:yyyy-MM-dd}!";
                return RedirectToAction(nameof(Dashboard));
            }

            ViewBag.ExistingDrivers = await _apiService.GetDriversAsync();
            return View(model);
        }

        // ==========================================
        // UPDATE STATUS - Change trip status
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, TripStatus status)
        {
            await _apiService.UpdateDriverScheduleStatusAsync(id, status);
            TempData["Success"] = $"Trip status updated to {status}";
            return RedirectToAction(nameof(Dashboard));
        }

        // ==========================================
        // DELETE - Remove a schedule
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _apiService.DeleteDriverScheduleAsync(id);
            TempData["Success"] = "Schedule deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}