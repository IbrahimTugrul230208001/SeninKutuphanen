using learningASP.NET_CORE.Services;
using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace learningASP.NET_CORE.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILibraryService _libraryService;
        private readonly UserManager _userManager = new(new EfUserRepository());
        public SettingsController(ILibraryService libraryService,IUserService userService)
        {
            _libraryService = libraryService;
            _userService = userService;
        }
        public IActionResult Settings()
        {
            ViewData["UserName"] = _userService.UserName;
            ViewData["UserProfilePicture"] = _userService.ProfilePicture;
            return View();
        }
        public IActionResult SetNewUserName(IFormCollection receivedUserInput)
        {
            return RedirectToAction("Settings");
        }

        [HttpPost]
        public IActionResult SetNewResidementPlace(IFormCollection receivedUserInput)
        {
            return RedirectToAction("Settings");
        }

        [HttpPost]
        public IActionResult SetNewPassword(IFormCollection receivedUserInput)
        {
            string newPassword = receivedUserInput["TbxNewPassword"].ToString();
            string currentPassword = receivedUserInput["TbxCurrentPassword"].ToString();
            string newPasswordAgain = receivedUserInput["TbxNewPasswordAgain"].ToString();

            if (newPassword == newPasswordAgain && _userManager.VerifyPassword(_userService.UserName, currentPassword))
            {
                _userManager.UpdateUserPassword(newPassword, _userService.UserName);
                return View("Settings");
            }

            ViewData["ErrorMessage"] = "Invalid current password or passwords don't match";
            return View("Settings");
        }

        [HttpPost]
        public IActionResult SetNewUserProfilePicture(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                byte[] imageData;
                using (MemoryStream stream = new MemoryStream())
                {
                    imageFile.CopyTo(stream);
                    imageData = stream.ToArray();
                }
                _userManager.SetNewUserProfile(_userService.UserName, imageData);
                _userService.ProfilePicture = _userManager.ProfilePictureImage(_userService.UserName);
            }
            return RedirectToAction("Settings");
        }
    }
}
