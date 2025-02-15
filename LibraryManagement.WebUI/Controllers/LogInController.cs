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
        public async Task<IActionResult> UserLogIn([FromBody]User user)
        {
            string email = user.Email;
            string password = user.Password;
            if (await _userManager.ValidateUserAsync(email, password) == true)
            {

                _userService.UserName = await _userManager.UserNameAsync(email);
                _userService.ProfilePicture = await _userManager.ProfilePictureImageAsync(_userService.UserName);
                return Json(new { success = true, redirectUrl = Url.Action("ProfileIndex","UserProfile")});
            }
            else
            {
                return Json(new { success = false, redirectUrl = Url.Action("LogIn") });
            }
        }
    }
}
