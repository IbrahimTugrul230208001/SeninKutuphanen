using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;

namespace learningASP.NET_CORE.Controllers
{
    public class ChatBotController : Controller
    {
        public IActionResult ChatBot()
        {
            return View();
        }
    }
}
