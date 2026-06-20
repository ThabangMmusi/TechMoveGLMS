using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Data;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServiceRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/servicerequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetServiceRequests()
        {
            return await _context.ServiceRequests.ToListAsync();
        }

        // GET: api/servicerequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceRequest>> GetServiceRequest(int id)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            return serviceRequest;
        }

        // POST: api/servicerequests
        [HttpPost]
        public async Task<ActionResult<ServiceRequest>> PostServiceRequest(ServiceRequest serviceRequest)
        {
            // Business Logic: Cannot create ServiceRequest if Contract is Expired or On Hold
            var contract = await _context.Contracts.FindAsync(serviceRequest.ContractId);
            if (contract == null)
            {
                return BadRequest("Contract not found.");
            }

            if (contract.Status == ContractStatus.Expired || contract.Status == ContractStatus.OnHold)
            {
                return BadRequest($"Cannot create service request. Contract is {contract.Status}.");
            }

            _context.ServiceRequests.Add(serviceRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServiceRequest", new { id = serviceRequest.Id }, serviceRequest);
        }

        // PATCH: api/servicerequests/5/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] RequestStatus status)
        {
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            request.Status = status;
            if (status == RequestStatus.Completed)
            {
                request.CompletedDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
