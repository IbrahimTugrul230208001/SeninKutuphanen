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
        private readonly LibraryManager _libraryManager;
        private readonly IUserService _userService;

        public UserProfileController(IUserService userService)
        {
            _libraryManager = new LibraryManager(new EntityFrameworkCore());
            _userService = userService;
        }

        public IActionResult ProfileIndex()
        {
            if (_userService.UserName == null)
            {
                return RedirectToAction("LogInPage", "RegisterLogIn");
            }
            ViewData["UserName"] = _userService.UserName;
            ViewData["UserProfilePicture"] = _libraryManager.ProfilePictureImage(_userService.UserName);
            return View();
        }

        public IActionResult Settings()
        {
            if (_userService.UserName == null)
            {
                return RedirectToAction("LogInPage", "RegisterLogIn");
            }
            ViewData["UserName"] = _userService.UserName;
            ViewData["UserProfilePicture"] = _libraryManager.ProfilePictureImage(_userService.UserName);
            return View();
        }

        [HttpPost]
        public IActionResult SetNewUserName(IFormCollection receivedUserInput)
        {
            string newUserName = receivedUserInput["TbxNewUserName"].ToString();
            int userId = _libraryManager.UserID(_userService.UserName);
            _libraryManager.SetNewUserName(userId, newUserName);
            _userService.UserName = newUserName; 
            return RedirectToAction("Settings");
        }

        [HttpPost]
        public IActionResult SetNewResidementPlace(IFormCollection receivedUserInput)
        {
            string newCity = receivedUserInput["TbxSetNewCity"].ToString();
            string newCountry = receivedUserInput["TbxSetNewCountry"].ToString();
            _libraryManager.SetNewResidementPlaces(newCity, newCountry, _userService.UserName);
            return RedirectToAction("Settings");
        }

        [HttpPost]
        public IActionResult SetNewPassword(IFormCollection receivedUserInput)
        {
            string newPassword = receivedUserInput["TbxNewPassword"].ToString();
            string currentPassword = receivedUserInput["TbxCurrentPassword"].ToString();
            string newPasswordAgain = receivedUserInput["TbxNewPasswordAgain"].ToString();

            if (newPassword == newPasswordAgain && _libraryManager.VerifyPassword(_userService.UserName, currentPassword))
            {
                _libraryManager.UpdateUserPassword(newPassword, _userService.UserName);
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
                _libraryManager.SetNewUserProfile(_userService.UserName, imageData);
                _userService.ProfilePicture = _libraryManager.ProfilePictureImage(_userService.UserName);
            }
            return RedirectToAction("Settings");
        }
    }

}
