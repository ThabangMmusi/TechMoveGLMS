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
        private readonly IApiService _apiService;
        private readonly ICurrencyService _currencyService;
        private readonly ILogger<ServiceRequestsController> _logger;

        public ServiceRequestsController(
            IApiService apiService,
            ICurrencyService currencyService,
            ILogger<ServiceRequestsController> logger)
        {
            _apiService = apiService;
            _currencyService = currencyService;
            _logger = logger;
        }

        // GET: ServiceRequests
        public async Task<IActionResult> Index()
        {
            var allRequests = await _apiService.GetServiceRequestsAsync();
            var requests = allRequests
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
                .ToList();

            return View(requests);
        }

        // GET: ServiceRequests/Create
        public async Task<IActionResult> Create()
        {
            // Only show active contracts for service requests
            var contracts = await _apiService.GetContractsAsync();
            var activeContracts = contracts
                .Where(c => c.Status == ContractStatus.Active)
                .ToList();

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
            var contract = await _apiService.GetContractByIdAsync(model.ContractId);

            if (contract == null)
            {
                ModelState.AddModelError("ContractId", "Invalid contract selected");
                var contracts = await _apiService.GetContractsAsync();
                ViewBag.Contracts = contracts.Where(c => c.Status == ContractStatus.Active).ToList();
                return View(model);
            }

            // Business rule: Service request cannot be created for expired or on-hold contracts
            if (contract.Status != ContractStatus.Active)
            {
                ModelState.AddModelError("", "Service requests can only be created for ACTIVE contracts");
                var contracts = await _apiService.GetContractsAsync();
                ViewBag.Contracts = contracts.Where(c => c.Status == ContractStatus.Active).ToList();
                return View(model);
            }

            if (ModelState.IsValid)
            {
                try
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

                    await _apiService.CreateServiceRequestAsync(serviceRequest);

                    _logger.LogInformation($"Service request created: {serviceRequest.Title} for contract {contract.Title}");

                    TempData["Success"] = $"Service request created! Amount: {model.AmountUSD:C} USD = {serviceRequest.AmountZAR:C} ZAR (Rate: {rate:F2})";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"API Error: {ex.Message}");
                }
            }

            var allContracts = await _apiService.GetContractsAsync();
            ViewBag.Contracts = allContracts.Where(c => c.Status == ContractStatus.Active).ToList();
            return View(model);
        }

        // GET: ServiceRequests/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var request = await _apiService.GetServiceRequestByIdAsync(id);

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
            await _apiService.UpdateServiceRequestStatusAsync(id, status);

            TempData["Success"] = $"Request status updated to {status}";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}