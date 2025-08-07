using learningASP.NET_CORE.Models;
using learningASP.NET_CORE.Services;
using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace learningASP.NET_CORE.Controllers
{
    public class SohbetController : Controller
    {
        private readonly IUserService _userService;
        private readonly string _userName;
        private readonly IAIServices _aiService;
        LibraryManager _libraryManager = new(new EfLibraryRepository());

        public SohbetController(IUserService userService, IAIServices AIService)
        {
            _userService = userService;
            _userName = _userService.UserName;
            _aiService = AIService;
        }


        public IActionResult BenimAsistanim()
        {
            ViewData["UserName"] = _userName;
            ViewData["UserProfilePicture"] = _userService.ProfilePicture;
            return View();
        }
     
    }
}
