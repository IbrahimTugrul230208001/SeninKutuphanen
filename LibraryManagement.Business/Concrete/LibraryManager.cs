using LibraryManagement.DataAccess.Abstract;
using LibraryManagement.DataAccess.Concrete.ADO.NET;
using LibraryManagement.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Business.Concrete
{
    public class LibraryManager : ILibraryRepository
    {
        public string residementPlaceCity,residementPlaceCountry;
        private ILibraryRepository _libraryRepository;
        public LibraryManager(ILibraryRepository libraryDal)
        {
            _libraryRepository = libraryDal;
        }
     
        public void AddToShowcase(string userName, int ID)
        {
            _libraryRepository.AddToShowcase(userName, ID);
        }

        public void AddToLibrary(Library library)
        {
            _libraryRepository.AddToLibrary(library);
        }

        public int BookStatusCounter(string status,string userName)
        {
            return _libraryRepository.BookStatusCounter(status,userName);
        }
        public int CountCompletedBooks(string userName)
        {
            return _libraryRepository.CountCompletedBooks(userName);
        }

        public int CountGenresOfBooks(string genre, string userName)
        {
            return _libraryRepository.CountGenresOfBooks(genre, userName);
        }

        public void DeleteFromLibrary(Library library)
        {
            _libraryRepository.DeleteFromLibrary(library);
        }

        public int GetTotalOfBooks(string userName)
        {
            return _libraryRepository.GetTotalOfBooks(userName);
        }

        public int GetTotalOfReadPages(string userName)
        {
            return _libraryRepository.GetTotalOfReadPages(userName);
        }


        public string MostExistingCategoryInLibrary(string userName)
        {
            return _libraryRepository.MostExistingCategoryInLibrary(userName);
        }
        
        public int CountFavorites(string userName)
        {
            return _libraryRepository.CountFavorites(userName);
        }

        public void AddBookImage(byte[] imageFile, string userName, string bookName)
        {
            _libraryRepository.AddBookImage(imageFile, userName, bookName);
        }

        public List<Library> ListBookShowcase(string userName)
        {
            return _libraryRepository.ListBookShowcase(userName);
        }

        public bool CheckImageStatus(string userName, string bookName)
        {
            return _libraryRepository.CheckImageStatus(userName, bookName);
        }

        public string BookImage(string userName, string bookName)
        {
            return _libraryRepository.BookImage(userName, bookName);
        }

        public void UpdateLibrary(int ID, string userName, string bookName, string author, string category, int completedPages, int totalOfPages, string status)
        {
            _libraryRepository.UpdateLibrary(ID, userName, bookName, author, category, completedPages, totalOfPages, status);
        }

        public void RemoveBookShowcase(string userName, string bookName)
        {
            _libraryRepository .RemoveBookShowcase(userName, bookName);
        }

        public int BookID(string userName, string bookName)
        {
            return _libraryRepository.BookID(userName,bookName);
        }
    }
}
