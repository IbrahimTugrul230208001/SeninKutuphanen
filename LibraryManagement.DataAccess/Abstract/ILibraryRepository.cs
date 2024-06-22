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
        Task AddToLibraryAsync(Library library);
        Task UpdateLibraryAsync(int ID, string userName, string bookName, string author, string category, int completedPages, int totalOfPages, string status);
        Task DeleteFromLibraryAsync(Library library);
        Task AddToShowcaseAsync(string userName, int ID);
        Task AddBookImageAsync(byte[] imageFile, string userName, string bookName);
        Task RemoveBookShowcaseAsync(string userName, string bookName);
        Task<int> GetTotalOfReadPagesAsync(string userName);
        Task<int> BookStatusCounterAsync(string status, string userName);
        Task<int> CountCompletedBooksAsync(string userName);
        Task<int> CountGenresOfBooksAsync(string genre, string userName);
        Task<int> GetTotalOfBooksAsync(string userName);
        Task<int> CountFavoritesAsync(string userName);
        Task<string> MostExistingCategoryInLibraryAsync(string userName);
        Task<bool> CheckImageStatusAsync(string userName, string bookName);
        Task<string> BookImageAsync(string userName, string bookName);
        Task<List<Library>> ListBookShowcaseAsync(string userName);
        Task<int> BookIDAsync(string userName, string bookName);
        Task<string> BookNameAsync(int id);

    }
}
