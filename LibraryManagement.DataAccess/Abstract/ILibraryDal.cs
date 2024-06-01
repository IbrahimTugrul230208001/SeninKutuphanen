using LibraryManagement.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DataAccess.Abstract
{
    public interface ILibraryDal
    {
        bool ValidateUser(string userName, string password);
        bool VerifyHashedPassword(string password, string hashedPassword);
        bool VerifyPassword(string userName, string password);
        void SetNewResidementPlaces(string city, string country, string userName);
        void AddNewUser(string userName, string password);
        string HashPassword(string password);
        void UpdateUserPassword(string password, string userName);
        int GetTotalOfReadPages(string userName);
        void CheckUserLogInStatus(string stayLoggedIn, string userName);
        void AddToLibrary(Library library);
        void UpdateLibrary(int ID,string userName,string bookName,string author,string category,int completedPages,int totalOfPages,string status);
        int BookStatusCounter(string status,string userName);
        int CountCompletedBooks(string userName);
        int CountGenresOfBooks(string genre, string userName);
        int GetTotalOfBooks(string userName);
        string ResidementPlaceCity(string userName);
        string ResidementPlaceCountry(string userName);
        void DeleteFromLibrary(Library library);
        int UserID(string userName);
        void SetNewUserName(int ID,string userName);
        string MostExistingCategoryInLibrary(string userName);
        void AddToFavorites(string userName, int ID);
        int CountFavorites(string userName);
        void AddBookImage(byte[] imageFile, string userName, string bookName);
        List<Library> ListBookShowcase(string userName);
        bool CheckImageStatus(string userName, string bookName);
        string BookImage(string userName, string bookName);
        void RemoveBookShowcase(string userName, string bookName);
        string ProfilePictureImage(string userName);
        void SetNewUserProfile(string userName, byte[] imageFile);
    }
}
