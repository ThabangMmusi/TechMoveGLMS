using Microsoft.AspNetCore.Mvc;
using TechMoveGLMS.Data;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Get user info from session
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");

            // Get statistics for dashboard
            ViewBag.ActiveContracts = _context.Contracts.Count(c => c.Status == ContractStatus.Active);
            ViewBag.PendingRequests = _context.ServiceRequests.Count(sr => sr.Status == RequestStatus.Pending);
            ViewBag.TotalContracts = _context.Contracts.Count();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}