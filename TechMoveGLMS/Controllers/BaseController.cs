using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TechMoveGLMS.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                context.Result = RedirectToAction("Login", "Account", new { returnUrl = context.HttpContext.Request.Path });
            }
            base.OnActionExecuting(context);
        }
    }
}