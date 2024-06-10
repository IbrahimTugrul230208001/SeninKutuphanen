using learningASP.NET_CORE.Services;
using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.ADO.NET;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace learningASP.NET_CORE.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILibraryService _libraryService;

        public UserProfileController(IUserService userService,ILibraryService libraryService)
        {
            _userService = userService;
            _libraryService = libraryService;
        }

        public IActionResult ProfileIndex()
        {
            if (_userService.UserName == null)
            {
                return RedirectToAction("LogInPage", "RegisterLogIn");
            }
            ViewData["UserName"] = _userService.UserName;
            ViewData["UserProfilePicture"] = _userService.ProfilePicture;
            return View();
        }
    }

}
