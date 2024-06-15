using learningASP.NET_CORE.Models;
using learningASP.NET_CORE.Services;
using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace learningASP.NET_CORE.Controllers
{
    public class LogInController : Controller
    {

        UserManager _userManager = new UserManager(new EfUserRepository());
        private readonly IUserService _userService;

        public LogInController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserLogIn([FromBody] User user)
        {
            if (_userManager.ValidateUser(user.UserName, user.Password) == true)
            {
                _userService.UserName = user.UserName;
                _userService.ProfilePicture = _userManager.ProfilePictureImage(user.UserName);
                return Json(new { success = true, redirectUrl = Url.Action("LogInPage") });
            }
            else
            {
                return Json(new { success = false, redirectUrl = Url.Action("LogInPage") });
            }
        }
    }
}
