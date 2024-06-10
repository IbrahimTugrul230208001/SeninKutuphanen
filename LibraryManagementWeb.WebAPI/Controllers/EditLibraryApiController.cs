using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using LibraryManagement.Entities.Concrete;
using LibraryManagementWeb.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementWeb.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryManager _libraryManager = new LibraryManager(new EfLibraryRepository());
      
        [HttpPost("Add")]
        public IActionResult Add([FromBody] Book b)
        {
            if (b == null)
            {
                return BadRequest();
            }

            _libraryManager.AddToLibrary(new Library { 
            UserName = b.UserName,
            Name = b.Name,
            Author = b.Author,
            Category = b.Category,
            CompletedPages = b.CompletedPages,
            TotalOfPages = b.TotalPages,
            Status = b.Status,
            });
            return Ok();
        }

        [HttpPut]
        public IActionResult Update([FromBody] Book b)
        {
            if (b == null)
            {
                return BadRequest();
            }
            _libraryManager.UpdateLibrary(b.Id,b.UserName,b.Name,b.Author,b.Category,b.CompletedPages,b.TotalPages,b.Status);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] Book b)
        {
            if (b == null)
            {
                return BadRequest();
            }
            _libraryManager.DeleteFromLibrary(new Library
            {
                Id = b.Id,
            });
            return Ok();
        }

        [HttpPost("AddToFavorites")]
        public IActionResult AddToFavorites([FromBody] Book b)
        {
            if (b == null)
            {
                return BadRequest();
            }
            _libraryManager.AddToFavorites(b.UserName, b.Id);
            return Ok();
        }

        [HttpPost("RemoveBookShowcase")]
        public IActionResult RemoveBookShowcase([FromBody] Book b)
        {
            if (b == null)
            {
                return BadRequest();
            }

            _libraryManager.RemoveBookShowcase(b.UserName, b.Name);
            return Ok();
        }
    }
}
