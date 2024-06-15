using learningASP.NET_CORE.Models;
using learningASP.NET_CORE.Services;
using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using LibraryManagement.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace learningASP.NET_CORE.Controllers
{
    public class EditLibraryController : Controller
    {
        private readonly IUserService _userService;
        private readonly string _userName;
        LibraryManager _libraryManager = new(new EfLibraryRepository());

        public EditLibraryController(IUserService userService)
        {
            _userService = userService;
            _userName = _userService.UserName;
        }

        public IActionResult EditLibrary()
        {
                ViewData["UserName"] = _userName;
                ViewData["UserProfilePicture"] = _userService.ProfilePicture;
                return View();
        }

        [HttpPost]
        public IActionResult Add([FromBody] Book b)
        {
            if (b != null)
            {          
                _libraryManager.AddToLibrary(new Library
                {
                    UserName = _userName,
                    Name = b.Name,
                    Author = b.Author,
                    Category = b.Category,
                    CompletedPages = b.CompletedPages,
                    TotalOfPages = b.TotalOfPages,
                    Status = b.Status,
                });
                int bookId = _libraryManager.BookID(_userName, b.Name);
                return Json(new { success = true, message = "Book updated successfully", redirectUrl = Url.Action("EditLibrary"), newBookId = bookId });
            }
            else
            {
                return Json(new { success = false, message = "Book could not be added", redirectUrl = Url.Action("EditLibrary") });
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] Book b)
        {
            if (b != null)
            {
                _libraryManager.UpdateLibrary(b.Id, _userName, b.Name, b.Author, b.Category, b.CompletedPages, b.TotalOfPages, b.Status);
                return Json(new { success = true, redirectUrl = Url.Action("EditLibrary") });
            }
            else
            {
                return Json(new { success = false, redirectUrl = Url.Action("EditLibrary")});
            }
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]int bookId)
        {
            if (bookId != 0)
            {
                _libraryManager.DeleteFromLibrary(new Library { Id = bookId });
                return Json(new { success = true, redirectUrl = Url.Action("EditLibrary") });
            }
            else
            {
                return Json(new { success = false, redirectUrl = Url.Action("EditLibrary") });
            }
        }

        [HttpPost]
        public IActionResult AddToShowcase([FromBody]int bookId)
        {
            if (bookId != 0)
            {
                _libraryManager.AddToShowcase(_userName, bookId);
                string name = _libraryManager.BookName(bookId);
                return Json(new { success = true, bookName = name , redirectUrl = Url.Action("EditLibrary")});
            }
            else
            {
                return Json(new { success = false, redirectUrl = Url.Action("EditLibrary") });
            }
        }

        [HttpDelete]
        public IActionResult RemoveBookShowcase([FromBody]string bookName)
        {          
            if(bookName != null)
            {
                _libraryManager.RemoveBookShowcase(_userName, bookName);
                return Json(new { success = true, redirectUrl = Url.Action("EditLibrary") });
            }
            else
            {
                return Json(new { success = false, redirectUrl = Url.Action("EditLibrary") });
            }
        }
    }
}
