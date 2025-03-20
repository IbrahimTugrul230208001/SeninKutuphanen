using learningASP.NET_CORE.Services;
using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;

namespace learningASP.NET_CORE.Controllers
{
    public class ChatBotController : Controller
    {
        private readonly IUserService _userService;
        private readonly string _userName;
        LibraryManager _libraryManager = new(new EfLibraryRepository());

        public ChatBotController(IUserService userService)
        {
            _userService = userService;
            _userName = _userService.UserName;
        }

    
        public IActionResult ChatBot()
        {
            ViewData["UserName"] = _userName;
            ViewData["UserProfilePicture"] = _userService.ProfilePicture;
            return View();
        }
    }
}
