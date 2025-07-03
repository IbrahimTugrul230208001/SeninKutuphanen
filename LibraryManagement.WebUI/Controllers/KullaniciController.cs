using learningASP.NET_CORE.Models;
using learningASP.NET_CORE.Services;
using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using LibraryManagement.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;

namespace learningASP.NET_CORE.Controllers
{
    
    public class KullaniciController : Controller
    {
        private readonly IUserService _userService;
        private readonly string _userName;
        LibraryManager _libraryManager = new(new EfLibraryRepository());
        UserManager _userManager = new(new EfUserRepository());


        public KullaniciController(IUserService userService)
        {
            _userService = userService;
            _userName = _userService.UserName;
        }
        public IActionResult Kitaplik()
        {
            ViewData["UserId"] = _userService.UserId;
            ViewData["UserName"] = _userService.UserName;
            ViewData["UserProfilePicture"] = _userService.ProfilePicture;
            return View();
        }
        public async Task<IActionResult> Duzenle()
        {
            ViewData["UserId"] = _userService.UserId;
            ViewData["UserName"] = _userName;
            ViewData["UserProfilePicture"] = _userService.ProfilePicture;
            var booklist = await _libraryManager.ListBookShowcaseAsync(_userService.UserId);
            var checkedIds = new HashSet<int>(booklist.Select(b => b.Id));
            return View();
        }
        public IActionResult Bildirimler()
        {
            return View();
        }
        public IActionResult Profil()
        {
            ViewData["UserId"] = _userService.UserId;
            ViewData["UserName"] = _userService.UserName;
            ViewData["UserProfilePicture"] = _userService.ProfilePicture;
            return View();
        }
        public IActionResult Ayarlar()
        {
            ViewData["UserId"] = _userService.UserId;
            ViewData["UserName"] = _userService.UserName;
            ViewData["UserProfilePicture"] = _userService.ProfilePicture;
            return View();
        }
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> AnaSayfa(int id)
        {
            int userId = _userService.UserId;
            var booklist = await _libraryManager.ListBookShowcaseAsync(userId);
            var checkedIds = new HashSet<int>(booklist.Select(b => b.Id));
            var listedBooksPerPage = await _libraryManager.ReturnBookListPerPageAsync(id);
            ViewData["PageNumber"] = id;
            ViewData["CheckedIds"] = checkedIds;
            ViewData["UserId"] = userId;
            ViewData["UserName"] = _userService.UserName;
            ViewData["UserProfilePicture"] = _userService.ProfilePicture;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BookData b)
        {
            if (b != null)
            {
                await _libraryManager.AddToLibraryAsync(
                    _userService.UserId,
                    b.Id,
                    "");
                return Json(new { success = true, message = "Book updated successfully", redirectUrl = Url.Action("EditLibrary") });
            }

            else
            {
                return Json(new { success = false, message = "Book could not be added", redirectUrl = Url.Action("EditLibrary") });
            }
        }


        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] int bookId)
        {
            if (bookId != 0)
            {
                await _libraryManager.DeleteFromLibraryAsync(bookId);
                return Json(new { success = true, redirectUrl = Url.Action("EditLibrary") });
            }
            else
            {
                return Json(new { success = false, redirectUrl = Url.Action("EditLibrary") });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToShowcase([FromBody] int userId)
        {
            if (userId != 0)
            {
                await _libraryManager.AddToShowcaseAsync(userId);
                return Json(new { success = true, redirectUrl = Url.Action("EditLibrary") });
            }
            else
            {
                return Json(new { success = false, redirectUrl = Url.Action("EditLibrary") });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveBookShowcase([FromBody] int bookId,int userId)
        {
            if (bookId != 0 && userId != 0)
            { 
                await _libraryManager.RemoveBookShowcaseAsync(bookId, userId);
                return Json(new { success = true, redirectUrl = Url.Action("EditLibrary") });
            }
            else
            {
                return Json(new { success = false, redirectUrl = Url.Action("EditLibrary") });
            }
        }
 
        [HttpPut]
        public async Task<IActionResult> SetNewUserName([FromBody] string userName)
        {
            if (userName != null)
            {
                int userId = await _userManager.UserIDAsync(_userService.UserName);
                await _userManager.SetNewUserNameAsync(userId, userName);
                _userService.UserName = userName;
                return Json(new { success = true, redirectUrl = Url.Action("Settings") });
            }
            else
            {
                return Json(new { success = false, redirectUrl = Url.Action("Settings") });
            }
        }

        [HttpPut]
        public async Task<IActionResult> SetNewResidementPlaces([FromBody] User user)
        {
            if (user.City != null && user.Country != null)
            {
                await _userManager.SetNewResidementPlacesAsync(user.City, user.Country, _userService.UserName);
                return Json(new { success = true, redirectUrl = Url.Action("Settings") });
            }
            else
            {
                return Json(new { success = false, redirectUrl = Url.Action("Settings") });
            }
        }

        [HttpPut]
        public async Task<IActionResult> SetNewPassword([FromBody] User user)
        {
            if (user.NewPasswordAgain == user.NewPassword && await _userManager.VerifyPasswordAsync(_userService.UserName, user.Password))
            {
                await _userManager.UpdateUserPasswordAsync(user.NewPassword, _userService.UserName);
                return Json(new { success = true, redirectUrl = Url.Action("Settings") });
            }
            else
            {
                return Json(new { success = false, redirectUrl = Url.Action("Settings") });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetNewUserProfilePicture(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                byte[] imageData;
                using (MemoryStream stream = new())
                {
                    imageFile.CopyTo(stream);
                    imageData = stream.ToArray();
                }
                await _userManager.SetNewUserProfileAsync(_userService.UserName, imageData);
                _userService.ProfilePicture = await _userManager.ProfilePictureImageAsync(_userService.UserName);
                return RedirectToAction("Settings");
            }
            else
            {
                return RedirectToAction("Settings");
            }
        }
        [HttpPut]
        public async Task<IActionResult> RemoveProfilePicture([FromBody] string userName)
        {
            if (User == null)
            {
                return Json(new { success = false, redirectUrl = Url.Action("Settings") });
            }
            else
            {
                await _userManager.RemoveProfilePictureAsync(userName);
                return Json(new { success = true, redirectUrl = Url.Action("Settings") });
            }
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            return RedirectToAction("Giris", "Dogrulama");
        }
    }
}
