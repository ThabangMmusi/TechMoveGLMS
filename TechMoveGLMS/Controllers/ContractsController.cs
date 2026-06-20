using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Data;
using TechMoveGLMS.Models;
using TechMoveGLMS.Services;
using TechMoveGLMS.ViewModels;

namespace TechMoveGLMS.Controllers
{
    public class ContractsController : Controller
    {
        private readonly IApiService _apiService;
        private readonly IFileValidationService _fileValidationService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<ContractsController> _logger;

        public ContractsController(
            IApiService apiService,
            IFileValidationService fileValidationService,
            IWebHostEnvironment webHostEnvironment,
            ILogger<ContractsController> logger)
        {
            _apiService = apiService;
            _fileValidationService = fileValidationService;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        // GET: Contracts - Index with filtering
        public async Task<IActionResult> Index(ContractSearchViewModel search)
        {
            var allContracts = await _apiService.GetContractsAsync(
                search.StartDateFrom, 
                search.StartDateTo, 
                search.Status);

            // Additional client filtering if needed (API should ideally handle this)
            if (search.ClientId.HasValue && search.ClientId.Value > 0)
            {
                allContracts = allContracts.Where(c => c.ClientId == search.ClientId.Value);
            }

            var contracts = allContracts
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new ContractViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    ClientId = c.ClientId,
                    ClientName = c.Client != null ? c.Client.Name : "N/A",
                    ClientEmail = c.Client != null ? c.Client.Email : "N/A",
                    ClientPhone = c.Client != null ? c.Client.Phone : "N/A",
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Status = c.Status,
                    ServiceLevel = c.ServiceLevel,
                    Description = c.Description,
                    SignedAgreementFileName = c.SignedAgreementFileName
                })
                .ToList();

            ViewBag.Clients = await _apiService.GetClientsAsync();
            ViewBag.StatusList = Enum.GetValues(typeof(ContractStatus));
            ViewBag.SearchModel = search;

            return View(contracts);
        }

        // GET: Contracts/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var contract = await _apiService.GetContractByIdAsync(id);

            if (contract == null)
                return NotFound();

            return View(contract);
        }

        // GET: Contracts/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.StatusList = Enum.GetValues(typeof(ContractStatus));
            return View();
        }

        // POST: Contracts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateContractViewModel model)
        {
            if (ModelState.IsValid)
            {
                string? filePath = null;
                string? fileName = null;

                // Check if client already exists
                var clients = await _apiService.GetClientsAsync();
                var existingClient = clients.FirstOrDefault(c => c.Name == model.ClientName);

                Client client;

                if (existingClient != null)
                {
                    client = existingClient;
                    TempData["Info"] = $"Using existing client: {client.Name}";
                }
                else
                {
                    // Create new client
                    client = new Client
                    {
                        Name = model.ClientName,
                        Email = model.ClientEmail,
                        Phone = model.ClientPhone,
                        Address = model.ClientAddress,
                        Region = model.ClientRegion,
                        CreatedAt = DateTime.UtcNow
                    };
                    client = await _apiService.CreateClientAsync(client);
                    TempData["Success"] = $"New client '{client.Name}' created!";
                }

                // Handle file upload if present
                if (model.SignedAgreement != null)
                {
                    using var memoryStream = new MemoryStream();
                    await model.SignedAgreement.CopyToAsync(memoryStream);
                    var fileContent = memoryStream.ToArray();

                    // Validate file
                    var validationResult = _fileValidationService.ValidateFile(
                        model.SignedAgreement.FileName, fileContent);

                    if (validationResult != "Valid")
                    {
                        ModelState.AddModelError("SignedAgreement", validationResult);
                        ViewBag.StatusList = Enum.GetValues(typeof(ContractStatus));
                        return View(model);
                    }

                    // Save file to server
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "contracts");
                    Directory.CreateDirectory(uploadsFolder);

                    fileName = $"{Guid.NewGuid()}_{model.SignedAgreement.FileName}";
                    filePath = Path.Combine(uploadsFolder, fileName);

                    await System.IO.File.WriteAllBytesAsync(filePath, fileContent);
                    _logger.LogInformation($"File saved: {filePath}");
                }

                // Create contract
                var contract = new Contract
                {
                    Title = model.Title,
                    ClientId = client.Id,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Status = model.Status,
                    ServiceLevel = model.ServiceLevel,
                    Description = model.Description,
                    SignedAgreementPath = filePath,
                    SignedAgreementFileName = model.SignedAgreement?.FileName,
                    CreatedAt = DateTime.UtcNow
                };

                await _apiService.CreateContractAsync(contract);

                TempData["Success"] = $"Contract '{contract.Title}' created successfully for client '{client.Name}'!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.StatusList = Enum.GetValues(typeof(ContractStatus));
            return View(model);
        }

        // GET: Contracts/DownloadFile/5
        public async Task<IActionResult> DownloadFile(int id)
        {
            var contract = await _apiService.GetContractByIdAsync(id);

            if (contract?.SignedAgreementPath == null || !System.IO.File.Exists(contract.SignedAgreementPath))
            {
                TempData["Error"] = "File not found";
                return RedirectToAction(nameof(Details), new { id });
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(contract.SignedAgreementPath);
            var fileName = contract.SignedAgreementFileName ?? "agreement.pdf";

            return File(fileBytes, "application/pdf", fileName);
        }

        // POST: Contracts/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, ContractStatus status)
        {
            await _apiService.UpdateContractStatusAsync(id, status);

            TempData["Success"] = $"Contract status updated to {status}";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}