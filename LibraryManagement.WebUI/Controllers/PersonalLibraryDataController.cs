using learningASP.NET_CORE.Services;
using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace learningASP.NET_CORE.Controllers
{
    public class PersonalLibraryDataController : Controller
    {
        private readonly IUserService _userService;
        private readonly LibraryManager _libraryManager;
        public PersonalLibraryDataController(IUserService userService)
        {
            _userService = userService;
            _libraryManager = new LibraryManager(new EfLibraryRepository());
        }
        public IActionResult PersonalLibraryData()
        {
            ViewData["UserName"] = _userService.UserName;
            ViewData["UserProfilePicture"] = _userService.ProfilePicture;
            return View();
        }
      
    }
}
