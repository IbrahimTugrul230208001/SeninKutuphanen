using learningASP.NET_CORE.Services;
using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace learningASP.NET_CORE.Controllers
{
    public class UserLibraryController : Controller
    {
        private readonly IUserService _userService;
        public UserLibraryController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult UserLibrary()
        {
            ViewData["UserName"] = _userService.UserName;
            ViewData["UserProfilePicture"] = _userService.ProfilePicture;
            return View();
        }
      
    }
}
