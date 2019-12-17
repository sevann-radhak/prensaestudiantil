using Microsoft.AspNetCore.Mvc;

namespace prensaestudiantil.Web.Controllers
{
    public class PrensaController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Prensa", "Account");
        }
    }
}