using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Data;
using TechMoveGLMS.Models;
using TechMoveGLMS.Services;

namespace TechMoveGLMS.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly IApiService _apiService;

        public InvoicesController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var allInvoices = await _apiService.GetInvoicesAsync();
            var invoices = allInvoices
                .OrderByDescending(i => i.InvoiceDate)
                .ToList();

            // Statistics
            ViewBag.TotalInvoices = invoices.Count;
            ViewBag.TotalAmount = invoices.Sum(i => i.AmountZAR);
            ViewBag.PaidAmount = invoices.Where(i => i.Status == InvoiceStatus.Paid).Sum(i => i.AmountZAR);
            ViewBag.OverdueCount = invoices.Count(i => i.DueDate < DateTime.Today && i.Status != InvoiceStatus.Paid);

            return View(invoices);
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var invoice = await _apiService.GetInvoiceByIdAsync(id);

            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create/5
        public async Task<IActionResult> Create(int serviceRequestId)
        {
            var serviceRequest = await _apiService.GetServiceRequestByIdAsync(serviceRequestId);

            if (serviceRequest == null)
            {
                TempData["Error"] = "Service request not found";
                return RedirectToAction("Index", "ServiceRequests");
            }

            // Check if invoice already exists
            var invoices = await _apiService.GetInvoicesAsync();
            var existingInvoice = invoices.FirstOrDefault(i => i.ServiceRequestId == serviceRequestId);

            if (existingInvoice != null)
            {
                TempData["Warning"] = $"Invoice {existingInvoice.InvoiceNumber} already exists for this service request";
                return RedirectToAction("Details", new { id = existingInvoice.Id });
            }

            ViewBag.ServiceRequest = serviceRequest;
            return View();
        }

        // POST: Invoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int serviceRequestId, DateTime invoiceDate, DateTime dueDate, string? notes)
        {
            var serviceRequest = await _apiService.GetServiceRequestByIdAsync(serviceRequestId);

            if (serviceRequest == null)
            {
                TempData["Error"] = "Service request not found";
                return RedirectToAction("Index", "ServiceRequests");
            }

            var invoice = new Invoice
            {
                ServiceRequestId = serviceRequestId,
                AmountUSD = serviceRequest.AmountUSD,
                AmountZAR = serviceRequest.AmountZAR,
                ExchangeRate = serviceRequest.ExchangeRate,
                InvoiceDate = invoiceDate,
                DueDate = dueDate,
                Status = InvoiceStatus.Draft,
                Notes = notes,
                CreatedAt = DateTime.UtcNow
            };

            var createdInvoice = await _apiService.CreateInvoiceAsync(invoice);

            TempData["Success"] = $"Invoice {createdInvoice.InvoiceNumber} created successfully!";
            return RedirectToAction(nameof(Details), new { id = createdInvoice.Id });
        }

        // POST: Invoices/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, InvoiceStatus status)
        {
            await _apiService.UpdateInvoiceStatusAsync(id, status);
            TempData["Success"] = $"Invoice status updated to {status}";

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: Invoices/MarkAsPaid
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            await _apiService.UpdateInvoiceStatusAsync(id, InvoiceStatus.Paid);

            TempData["Success"] = $"Invoice marked as paid!";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}