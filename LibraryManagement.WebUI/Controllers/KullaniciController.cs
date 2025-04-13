using learningASP.NET_CORE.Models;
using learningASP.NET_CORE.Services;
using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using LibraryManagement.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
            ViewData["UserName"] = _userService.UserName;
            ViewData["UserProfilePicture"] = _userService.ProfilePicture;
            return View();
        }
        public IActionResult Duzenle()
        {
            ViewData["UserName"] = _userName;
            ViewData["UserProfilePicture"] = _userService.ProfilePicture;
            return View();
        }
        public IActionResult Bildirimler()
        {
            return View();
        }
        public IActionResult Profil()
        {
            ViewData["UserName"] = _userService.UserName;
            ViewData["UserProfilePicture"] = _userService.ProfilePicture;
            return View();
        }
        public IActionResult Ayarlar()
        {
            ViewData["UserName"] = _userService.UserName;
            ViewData["UserProfilePicture"] = _userService.ProfilePicture;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Book b)
        {
            if (b != null)
            {
                await _libraryManager.AddToLibraryAsync(new Library
                {
                    UserName = _userName,
                    Name = b.Name,
                    Author = b.Author,
                    Category = b.Category,
                    CompletedPages = b.CompletedPages,
                    TotalOfPages = b.TotalOfPages,
                    Status = b.Status,
                });
                int bookId = await _libraryManager.BookIDAsync(_userName, b.Name);
                return Json(new { success = true, message = "Book updated successfully", redirectUrl = Url.Action("EditLibrary"), newBookId = bookId });
            }
            else
            {
                return Json(new { success = false, message = "Book could not be added", redirectUrl = Url.Action("EditLibrary") });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Book b)
        {
            if (b != null)
            {
                await _libraryManager.UpdateLibraryAsync(b.Id, _userName, b.Name, b.Author, b.Category, b.CompletedPages, b.TotalOfPages, b.Status);
                return Json(new { success = true, redirectUrl = Url.Action("EditLibrary") });
            }
            else
            {
                return Json(new { success = false, redirectUrl = Url.Action("EditLibrary") });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] int bookId)
        {
            if (bookId != 0)
            {
                await _libraryManager.DeleteFromLibraryAsync(new Library { Id = bookId });
                return Json(new { success = true, redirectUrl = Url.Action("EditLibrary") });
            }
            else
            {
                return Json(new { success = false, redirectUrl = Url.Action("EditLibrary") });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToShowcase([FromBody] int bookId)
        {
            if (bookId != 0)
            {
                await _libraryManager.AddToShowcaseAsync(_userName, bookId);
                string name = await _libraryManager.BookNameAsync(bookId);
                return Json(new { success = true, bookName = name, redirectUrl = Url.Action("EditLibrary") });
            }
            else
            {
                return Json(new { success = false, redirectUrl = Url.Action("EditLibrary") });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveBookShowcase([FromBody] string bookName)
        {
            if (bookName != null)
            {
                await _libraryManager.RemoveBookShowcaseAsync(_userName, bookName);
                return Json(new { success = true, redirectUrl = Url.Action("EditLibrary") });
            }
            else
            {
                return Json(new { success = false, redirectUrl = Url.Action("EditLibrary") });
            }
        }
        [HttpPost]
        public IActionResult UploadImage(IFormFile imageFile, IFormCollection receivedUserInput)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                byte[] imageData;
                using (MemoryStream stream = new())
                {
                    imageFile.CopyTo(stream);
                    imageData = stream.ToArray();
                }
                string? bookName = receivedUserInput["booktitle"].ToString();
                _libraryManager.AddBookImageAsync(imageData, _userName, bookName);
                return RedirectToAction("EditLibrary");
            }
            else
            {
                return RedirectToAction("EditLibrary");
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
