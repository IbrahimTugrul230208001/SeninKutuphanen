using learningASP.NET_CORE.Services;
using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace learningASP.NET_CORE.Controllers
{
    public class NotificationsController : Controller
    {
        UserManager _userManager = new UserManager(new EfUserRepository());
        private readonly IUserService _userService;
        public NotificationsController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Notifications()
        {
            return View();
        }
    }
}
