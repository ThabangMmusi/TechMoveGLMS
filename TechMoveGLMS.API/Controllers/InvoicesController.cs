using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Data;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/invoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
            return await _context.Invoices
                .Include(i => i.ServiceRequest)
                .ThenInclude(sr => sr!.Contract)
                .ThenInclude(c => c!.Client)
                .ToListAsync();
        }

        // GET: api/invoices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.ServiceRequest)
                .ThenInclude(sr => sr!.Contract)
                .ThenInclude(c => c!.Client)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            return invoice;
        }

        // POST: api/invoices
        [HttpPost]
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
            // Ensure service request exists
            var serviceRequest = await _context.ServiceRequests.FindAsync(invoice.ServiceRequestId);
            if (serviceRequest == null)
            {
                return BadRequest("Service request not found.");
            }

            // Generate invoice number if not provided
            if (string.IsNullOrEmpty(invoice.InvoiceNumber))
            {
                var invoiceCount = await _context.Invoices.CountAsync();
                invoice.InvoiceNumber = $"INV-{DateTime.Now.Year}-{(invoiceCount + 1):D4}";
            }

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInvoice", new { id = invoice.Id }, invoice);
        }

        // PATCH: api/invoices/5/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] InvoiceStatus status)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            invoice.Status = status;
            if (status == InvoiceStatus.Paid)
            {
                invoice.PaidDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
