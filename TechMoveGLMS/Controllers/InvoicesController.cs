using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Data;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var invoices = await _context.Invoices
                .Include(i => i.ServiceRequest)
                .ThenInclude(sr => sr.Contract)
                .ThenInclude(c => c.Client)
                .OrderByDescending(i => i.InvoiceDate)
                .ToListAsync();

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
            var invoice = await _context.Invoices
                .Include(i => i.ServiceRequest)
                .ThenInclude(sr => sr.Contract)
                .ThenInclude(c => c.Client)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create/5
        public async Task<IActionResult> Create(int serviceRequestId)
        {
            var serviceRequest = await _context.ServiceRequests
                .Include(sr => sr.Contract)
                .FirstOrDefaultAsync(sr => sr.Id == serviceRequestId);

            if (serviceRequest == null)
            {
                TempData["Error"] = "Service request not found";
                return RedirectToAction("Index", "ServiceRequests");
            }

            // Check if invoice already exists
            var existingInvoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.ServiceRequestId == serviceRequestId);

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
            var serviceRequest = await _context.ServiceRequests
                .FirstOrDefaultAsync(sr => sr.Id == serviceRequestId);

            if (serviceRequest == null)
            {
                TempData["Error"] = "Service request not found";
                return RedirectToAction("Index", "ServiceRequests");
            }

            // Generate invoice number
            var invoiceCount = await _context.Invoices.CountAsync();
            var invoiceNumber = $"INV-{DateTime.Now.Year}-{(invoiceCount + 1):D4}";

            var invoice = new Invoice
            {
                ServiceRequestId = serviceRequestId,
                InvoiceNumber = invoiceNumber,
                AmountUSD = serviceRequest.AmountUSD,
                AmountZAR = serviceRequest.AmountZAR,
                ExchangeRate = serviceRequest.ExchangeRate,
                InvoiceDate = invoiceDate,
                DueDate = dueDate,
                Status = InvoiceStatus.Draft,
                Notes = notes,
                CreatedAt = DateTime.UtcNow
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Invoice {invoiceNumber} created successfully!";
            return RedirectToAction(nameof(Details), new { id = invoice.Id });
        }

        // POST: Invoices/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, InvoiceStatus status)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            invoice.Status = status;
            if (status == InvoiceStatus.Paid)
            {
                invoice.PaidDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = $"Invoice {invoice.InvoiceNumber} status updated to {status}";

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: Invoices/MarkAsPaid
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            invoice.Status = InvoiceStatus.Paid;
            invoice.PaidDate = DateTime.Now;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Invoice {invoice.InvoiceNumber} marked as paid!";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}