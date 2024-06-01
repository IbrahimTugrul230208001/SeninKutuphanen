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
    public class LibraryManager : ILibraryDal
    {
        public string residementPlaceCity,residementPlaceCountry;
        private ILibraryDal _libraryDal;
        public LibraryManager(ILibraryDal libraryDal)
        {
            _libraryDal = libraryDal;
        }

        public void AddNewUser(string userName, string password)
        {
            _libraryDal.AddNewUser(userName,password);
        }

        public void AddToFavorites(string userName, int ID)
        {
            _libraryDal.AddToFavorites(userName, ID);
        }

        public void AddToLibrary(Library library)
        {
            _libraryDal.AddToLibrary(library);
        }

        public int BookStatusCounter(string status,string userName)
        {
            return _libraryDal.BookStatusCounter(status,userName);
        }

        public void CheckUserLogInStatus(string stayLoggedIn, string userName)
        {
            _libraryDal.CheckUserLogInStatus(stayLoggedIn, userName);
        }

        public int CountCompletedBooks(string userName)
        {
            return _libraryDal.CountCompletedBooks(userName);
        }

        public int CountGenresOfBooks(string genre, string userName)
        {
            return _libraryDal.CountGenresOfBooks(genre, userName);
        }

        public void DeleteFromLibrary(Library library)
        {
            _libraryDal.DeleteFromLibrary(library);
        }

        public int GetTotalOfBooks(string userName)
        {
            return _libraryDal.GetTotalOfBooks(userName);
        }

        public int GetTotalOfReadPages(string userName)
        {
            return _libraryDal.GetTotalOfReadPages(userName);
        }

        public string HashPassword(string password)
        {
            return _libraryDal.HashPassword(password);
        }

        public string MostExistingCategoryInLibrary(string userName)
        {
            return _libraryDal.MostExistingCategoryInLibrary(userName);
        }

        public string ResidementPlaceCity(string userName)
        {
            return _libraryDal.ResidementPlaceCity(userName);
        }

        public string ResidementPlaceCountry(string userName)
        {
            return _libraryDal.ResidementPlaceCountry(userName);
        }

        public void SetNewResidementPlaces(string city, string country, string userName)
        {
            _libraryDal.SetNewResidementPlaces(city, country, userName);
        }

        public void SetNewUserName(int ID, string userName)
        {
            _libraryDal.SetNewUserName(ID,userName);
        }

        public void UpdateUserPassword(string password, string userName)
        {
            _libraryDal.UpdateUserPassword(password, userName);
        }

        public int UserID(string userName)
        {
            return _libraryDal.UserID(userName);
        }

        public bool ValidateUser(string userName, string password)
        {
            return _libraryDal.ValidateUser(userName, password);
        }

        public bool VerifyHashedPassword(string password, string hashedPassword)
        {
            return _libraryDal.VerifyHashedPassword(password, hashedPassword);
        }

        public bool VerifyPassword(string userName, string password)
        {
            return _libraryDal.VerifyPassword(userName,password);
        }
        public int CountFavorites(string userName)
        {
            return _libraryDal.CountFavorites(userName);
        }

        public void AddBookImage(byte[] imageFile, string userName, string bookName)
        {
            _libraryDal.AddBookImage(imageFile, userName, bookName);
        }

        public List<Library> ListBookShowcase(string userName)
        {
            return _libraryDal.ListBookShowcase(userName);
        }

        public bool CheckImageStatus(string userName, string bookName)
        {
            return _libraryDal.CheckImageStatus(userName, bookName);
        }

        public string BookImage(string userName, string bookName)
        {
            return _libraryDal.BookImage(userName, bookName);
        }

        public void UpdateLibrary(int ID, string userName, string bookName, string author, string category, int completedPages, int totalOfPages, string status)
        {
            _libraryDal.UpdateLibrary(ID, userName, bookName, author, category, completedPages, totalOfPages, status);
        }

        public void RemoveBookShowcase(string userName, string bookName)
        {
            _libraryDal.RemoveBookShowcase(userName, bookName);
        }

        public string ProfilePictureImage(string userName)
        {
            return _libraryDal.ProfilePictureImage(userName);
        }

        public void SetNewUserProfile(string userName, byte[] imageFile)
        {
            _libraryDal.SetNewUserProfile(userName, imageFile);
        }
    }
}
