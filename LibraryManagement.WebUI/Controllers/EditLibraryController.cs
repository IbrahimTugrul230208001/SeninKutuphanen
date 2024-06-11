using learningASP.NET_CORE.Models;
using learningASP.NET_CORE.Services;
using Microsoft.AspNetCore.Mvc;

namespace learningASP.NET_CORE.Controllers
{
    public class EditLibraryController : Controller
    {
        private readonly ILibraryService _libraryService;
        private readonly IUserService _userService;

        public EditLibraryController(ILibraryService libraryService, IUserService userService)
        {
            _libraryService = libraryService;
            _userService = userService;
        }

        public IActionResult EditLibrary()
        {
                ViewData["UserName"] = _userService.UserName;
                ViewData["UserProfilePicture"] = _userService.ProfilePicture;
                return View();
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] Book book)
        {
                book.UserName = _userService.UserName;
                await _libraryService.AddAsync(book);
                return RedirectToAction("EditLibrary");           
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Book book)
        {
                book.UserName = _userService.UserName;
                await _libraryService.UpdateAsync(book);
                return RedirectToAction("EditLibrary");     
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int bookId)
        {          
                await _libraryService.DeleteAsync(bookId);
                return RedirectToAction("EditLibrary");        
        }

        [HttpPost("AddToShowcase/{bookId}")]
        public async Task<IActionResult> AddToShowcase(int bookId)
        {
                await _libraryService.AddToFavoritesAsync(bookId, _userService.UserName);
                return RedirectToAction("EditLibrary"); 
        }

        [HttpPost("RemoveBookShowcase/{bookId}")]
        public async Task<IActionResult> RemoveBookShowcase(int bookId)
        {          
                await _libraryService.RemoveBookShowcaseAsync(bookId, _userService.UserName);
                return RedirectToAction("EditLibrary");            
        }
    }
}
