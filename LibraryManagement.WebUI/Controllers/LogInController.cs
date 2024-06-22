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
        public async Task<IActionResult> UserLogIn(IFormCollection userData)
        {
            string userName = userData["TbxUserName"].ToString();
            string password = userData["TbxPassword"].ToString();
            if (await _userManager.ValidateUserAsync(userName, password) == true)
            {
                _userService.UserName = userName;
                _userService.ProfilePicture = await _userManager.ProfilePictureImageAsync(userName);
                return RedirectToAction("ProfileIndex", "UserProfile");
            }
            else
            {
                return View("LogIn");
            }
        }
    }
}
