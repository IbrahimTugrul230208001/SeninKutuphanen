using Microsoft.AspNetCore.Mvc;

namespace learningASP.NET_CORE.Controllers
{
    public class KullanıcıController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
