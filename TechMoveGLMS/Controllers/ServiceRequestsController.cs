using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Data;
using TechMoveGLMS.Models;
using TechMoveGLMS.Services;
using TechMoveGLMS.ViewModels;

namespace TechMoveGLMS.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrencyService _currencyService;
        private readonly ILogger<ServiceRequestsController> _logger;

        public ServiceRequestsController(
            ApplicationDbContext context,
            ICurrencyService currencyService,
            ILogger<ServiceRequestsController> logger)
        {
            _context = context;
            _currencyService = currencyService;
            _logger = logger;
        }

        // GET: ServiceRequests
        public async Task<IActionResult> Index()
        {
            var requests = await _context.ServiceRequests
                .Include(sr => sr.Contract)
                .ThenInclude(c => c!.Client)
                .OrderByDescending(sr => sr.RequestDate)
                .Select(sr => new ServiceRequestViewModel
                {
                    Id = sr.Id,
                    Title = sr.Title,
                    Description = sr.Description,
                    AmountUSD = sr.AmountUSD,
                    AmountZAR = sr.AmountZAR,
                    ExchangeRate = sr.ExchangeRate,
                    Status = sr.Status,
                    RequestDate = sr.RequestDate,
                    CompletedDate = sr.CompletedDate,
                    ContractTitle = sr.Contract != null ? sr.Contract.Title : "N/A",
                    ClientName = sr.Contract != null && sr.Contract.Client != null ? sr.Contract.Client.Name : "N/A",
                    ContractId = sr.ContractId
                })
                .ToListAsync();

            return View(requests);
        }

        // GET: ServiceRequests/Create
        public async Task<IActionResult> Create()
        {
            // Only show active contracts for service requests
            var activeContracts = await _context.Contracts
                .Include(c => c.Client)
                .Where(c => c.Status == ContractStatus.Active)
                .ToListAsync();

            if (!activeContracts.Any())
            {
                TempData["Warning"] = "No active contracts available. Please create an active contract first.";
            }

            ViewBag.Contracts = activeContracts;
            ViewBag.ExchangeRate = await _currencyService.GetExchangeRateAsync("USD", "ZAR");

            return View();
        }

        // POST: ServiceRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateServiceRequestViewModel model)
        {
            // Validate contract exists and is active
            var contract = await _context.Contracts.FindAsync(model.ContractId);

            if (contract == null)
            {
                ModelState.AddModelError("ContractId", "Invalid contract selected");
                ViewBag.Contracts = await _context.Contracts
                    .Include(c => c.Client)
                    .Where(c => c.Status == ContractStatus.Active)
                    .ToListAsync();
                return View(model);
            }

            // Business rule: Service request cannot be created for expired or on-hold contracts
            if (contract.Status != ContractStatus.Active)
            {
                ModelState.AddModelError("", "Service requests can only be created for ACTIVE contracts");
                ViewBag.Contracts = await _context.Contracts
                    .Include(c => c.Client)
                    .Where(c => c.Status == ContractStatus.Active)
                    .ToListAsync();
                return View(model);
            }

            if (ModelState.IsValid)
            {
                // Get current exchange rate
                var rate = await _currencyService.GetExchangeRateAsync("USD", "ZAR");

                var serviceRequest = new ServiceRequest
                {
                    ContractId = model.ContractId,
                    Title = model.Title,
                    Description = model.Description,
                    AmountUSD = model.AmountUSD,
                    ExchangeRate = rate,
                    AmountZAR = model.AmountUSD * rate,
                    RequestDate = DateTime.UtcNow,
                    Status = RequestStatus.Pending
                };

                _context.ServiceRequests.Add(serviceRequest);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Service request created: {serviceRequest.Title} for contract {contract.Title}");

                TempData["Success"] = $"Service request created! Amount: {model.AmountUSD:C} USD = {serviceRequest.AmountZAR:C} ZAR (Rate: {rate:F2})";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Contracts = await _context.Contracts
                .Include(c => c.Client)
                .Where(c => c.Status == ContractStatus.Active)
                .ToListAsync();
            return View(model);
        }

        // GET: ServiceRequests/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var request = await _context.ServiceRequests
                .Include(sr => sr.Contract)
                .ThenInclude(c => c!.Client)
                .FirstOrDefaultAsync(sr => sr.Id == id);

            if (request == null)
                return NotFound();

            return View(request);
        }

        // GET: ServiceRequests/GetExchangeRate (API endpoint)
        [HttpGet]
        public async Task<IActionResult> GetExchangeRate()
        {
            var rate = await _currencyService.GetExchangeRateAsync("USD", "ZAR");
            return Json(new { rate = rate, timestamp = DateTime.UtcNow });
        }

        // POST: ServiceRequests/UpdateStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, RequestStatus status)
        {
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request == null)
                return NotFound();

            request.Status = status;

            if (status == RequestStatus.Completed)
                request.CompletedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Request status updated to {status}";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}