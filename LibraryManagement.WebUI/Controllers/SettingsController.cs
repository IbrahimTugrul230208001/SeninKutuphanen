using learningASP.NET_CORE.Models;
using learningASP.NET_CORE.Services;
using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace learningASP.NET_CORE.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IUserService _userService;
        UserManager _userManager = new(new EfUserRepository());
        public SettingsController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Settings()
        {
            ViewData["UserName"] = _userService.UserName;
            ViewData["UserProfilePicture"] = _userService.ProfilePicture;
            return View();
        }
        [HttpPost]
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
        public IActionResult SetNewPassword([FromBody]User user)
        {
            if (user.NewPasswordAgain == user.NewPassword && _userManager.VerifyPassword(_userService.UserName, user.Password))
            {
                _userManager.UpdateUserPassword(user.NewPassword, _userService.UserName);
                return Json(new { success = true, message = "Password is updated successfully", redirectUrl = Url.Action("Settings")});
            }
            else
            {
                return Json(new { success = false, message = "An error occured during update." , redirectUrl = Url.Action("Settings")});
            }
        }

        [HttpPost]
        public IActionResult SetNewUserProfilePicture(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                byte[] imageData;
                using (MemoryStream stream = new())
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
