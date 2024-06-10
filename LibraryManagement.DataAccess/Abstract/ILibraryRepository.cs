using LibraryManagement.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DataAccess.Abstract
{
    public interface ILibraryRepository
    {
            void AddToLibrary(Library library);
            void UpdateLibrary(int ID, string userName, string bookName, string author, string category, int completedPages, int totalOfPages, string status);
            void DeleteFromLibrary(Library library);
            void AddToFavorites(string userName, int ID);
            void AddBookImage(byte[] imageFile, string userName, string bookName);
            void RemoveBookShowcase(string userName, string bookName);
            int GetTotalOfReadPages(string userName);
            int BookStatusCounter(string status, string userName);
            int CountCompletedBooks(string userName);
            int CountGenresOfBooks(string genre, string userName);
            int GetTotalOfBooks(string userName);
            int CountFavorites(string userName);
            string MostExistingCategoryInLibrary(string userName);
            bool CheckImageStatus(string userName, string bookName);
            string BookImage(string userName, string bookName);
            List<Library> ListBookShowcase(string userName);
    }
}
