using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class InfitechController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
