using learningASP.NET_CORE.Services;
using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.ADO.NET;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using LibraryManagement.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace learningASP.NET_CORE.Controllers
{
    public class EditLibraryController : Controller
    {
        private readonly LibraryManager _libraryManager = new LibraryManager(new EntityFrameworkCore());
        private readonly IUserService _userService;

        public EditLibraryController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult EditLibrary()
        {
            ViewData["UserName"] = _userService.UserName;
            ViewData["UserProfilePicture"] = _userService.ProfilePicture;
            return View();
        }

        [HttpPost]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None, Duration = -1)]
        public IActionResult Update(IFormCollection receivedUserInput)
        {
            string userName = _userService.UserName;
            int Id = Convert.ToInt32(receivedUserInput["TbxID"]);
            string name = receivedUserInput["TbxUpdateName"].ToString();
            string author = receivedUserInput["TbxUpdateAuthor"].ToString();
            string category = receivedUserInput["CbxUpdateCategory"].ToString();
            int completedPages = Convert.ToInt32(receivedUserInput["TbxUpdateCompletedPages"]);
            int totalOfPages = Convert.ToInt32(receivedUserInput["TbxUpdateTotalOfPages"]);
            string status = receivedUserInput["CbxUpdateStatus"].ToString();
            _libraryManager.UpdateLibrary(Id, userName, name, author, category, completedPages, totalOfPages, status);
            return RedirectToAction("EditLibrary");
        }


        [HttpPost]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None, Duration = -1)]
        public IActionResult Add(IFormCollection receivedUserInput)
        {
            _libraryManager.AddToLibrary(new Library
            {
                UserName = _userService.UserName,
                Name = receivedUserInput["TbxAddName"].ToString(),
                Author = receivedUserInput["TbxAddAuthor"].ToString(),
                Category = receivedUserInput["CbxAddCategory"].ToString(),
                CompletedPages = Convert.ToInt32(receivedUserInput["TbxAddCompletedPages"]),
                TotalOfPages = Convert.ToInt32(receivedUserInput["TbxAddTotalOfPages"]),
                Status = receivedUserInput["CbxAddStatus"].ToString(),
            });
            return RedirectToAction("EditLibrary");
        }
        [HttpPost]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None, Duration = -1)]
        public IActionResult Delete(IFormCollection receivedUserInput)
        {
            _libraryManager.DeleteFromLibrary(new Library
            {
                Id = Convert.ToInt32(receivedUserInput["IdTextboxDEL"]),
            });
            return RedirectToAction("EditLibrary");
        }
        [HttpPost]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None, Duration = -1)]
        public IActionResult AddToFavorites(IFormCollection receivedUserInput)
        {
            _libraryManager.AddToFavorites(_userService.UserName, Convert.ToInt32(receivedUserInput["IdTextboxFAV"]));
            return RedirectToAction("EditLibrary");
        }
        [HttpPost]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None, Duration = -1)]
        public IActionResult UploadImage(IFormFile imageFile,IFormCollection receivedBookName)
        {
            string bookName = receivedBookName["booktitle"].ToString();
            byte[] imageData;
            using (MemoryStream stream = new MemoryStream())
            {
                imageFile.CopyTo(stream);
                imageData = stream.ToArray();
            }
            _libraryManager.AddBookImage(imageData, _userService.UserName, bookName);
            return RedirectToAction("EditLibrary");
        }
        [HttpPost]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None, Duration = -1)]
        public IActionResult RemoveBookShowcase(IFormCollection receivedUserInput)
        {
            string bookName = receivedUserInput["booktitle"].ToString();
            _libraryManager.RemoveBookShowcase(_userService.UserName, bookName);
            return RedirectToAction("EditLibrary");
        }
    }
}
