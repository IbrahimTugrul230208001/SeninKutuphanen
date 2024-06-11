using learningASP.NET_CORE.Models;
using learningASP.NET_CORE.Services;
using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using LibraryManagement.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

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
        public async Task<IActionResult> Add([FromBody] Book b)
        {
            if (b != null)
            {
                _libraryManager.AddToLibrary(new Library
                {
                    UserName = _userName,
                    Id = b.Id,
                    Name = b.Name,
                    Author = b.Author,
                    Category = b.Category,
                    CompletedPages = b.CompletedPages,
                    TotalOfPages = b.TotalOfPages,
                    Status = b.Status,
                });
            }           
                return RedirectToAction("EditLibrary");           
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Book b)
        {
            if (b != null)
            {
                _libraryManager.UpdateLibrary(b.Id, _userName, b.Name, b.Author, b.Category, b.CompletedPages, b.TotalOfPages, b.Status);
            }
            return RedirectToAction("EditLibrary");     
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody]int bookId)
        {
            if (bookId != 0)
            {
                _libraryManager.DeleteFromLibrary(new Library { Id = bookId});
            }
            return RedirectToAction("EditLibrary");        
        }

        [HttpPost]
        public async Task<IActionResult> AddToShowcase([FromBody] int bookId)
        {
            if (bookId != 0)
            {
                _libraryManager.AddToShowcase(_userName, bookId);
            }
            return RedirectToAction("EditLibrary"); 
        }

        [HttpPost]
        public async Task<IActionResult> RemoveBookShowcase([FromBody] string bookName)
        {          
                _libraryManager.RemoveBookShowcase(_userName, bookName);
                return RedirectToAction("EditLibrary");            
        }
    }
}
