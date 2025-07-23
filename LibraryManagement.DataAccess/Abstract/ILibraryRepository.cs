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
        Task AddToLibraryAsync(int userId, int bookId, string status);
        Task DeleteFromLibraryAsync(int bookId);
        Task AddToShowcaseAsync(int userId);
        Task RemoveBookShowcaseAsync(int bookId, int userId);
        Task<int> GetTotalOfReadPagesAsync(int id);
        Task<int> BookStatusCounterAsync(string status, int userId);
        Task<int> CountCompletedBooksAsync(int userId);
        Task<int> GetTotalOfBooksAsync(int userId);
        Task<int> CountFavoritesAsync(int userId);
        Task<List<Book>> ListBookShowcaseAsync(int userId);
        Task<Book> ReturnBook (int bookId);
        Task<bool> CheckBookAsync(int userId, int bookId);
        Task<List<Book>> ReturnBookListPerPageAsync(int pageNumber);
        Task<List<Book>> BookSearchResultAsync(string searchTerm, string criteria);
        Task<List<string>> BookSearchQueryResultAsync(string searchTerm, string criteria);
    }
}
