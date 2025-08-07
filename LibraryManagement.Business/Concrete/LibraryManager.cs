using LibraryManagement.DataAccess.Abstract;
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
        private ILibraryRepository _libraryRepository;
        public LibraryManager(ILibraryRepository libraryDal)
        {
            _libraryRepository = libraryDal;
        }

        public async Task AddToLibraryAsync(int userId, int bookId)
        {
            await _libraryRepository.AddToLibraryAsync(userId, bookId);
        }

        public async Task AddToShowcaseAsync(int userId)
        {
             await _libraryRepository.AddToShowcaseAsync(userId);
        }

        public Task<List<string>> BookSearchQueryResultAsync(string searchTerm, string criteria)
        {
            return _libraryRepository.BookSearchQueryResultAsync(searchTerm, criteria);
        }

        public async Task<List<Book>> BookSearchResultAsync(string searchTerm, string criteria)
        {
            return await _libraryRepository.BookSearchResultAsync(searchTerm, criteria);
        }

        public async Task<int> BookStatusCounterAsync(string status, int userId)
        {
            return await _libraryRepository.BookStatusCounterAsync(status, userId);
        }

        public Task<bool> CheckBookAsync(int userId, int bookId)
        {
            return _libraryRepository.CheckBookAsync(userId, bookId);
        }

        public async Task<int> CountCompletedBooksAsync(int userId)
        {
            return await _libraryRepository.CountCompletedBooksAsync(userId);
        }

        public async Task<int> CountFavoritesAsync(int userId)
        {
            return await _libraryRepository.CountFavoritesAsync(userId);
        }

        public async Task DeleteFromLibraryAsync(int bookId)
        {
            await _libraryRepository.DeleteFromLibraryAsync(bookId);
        }

        public async Task<int> GetTotalOfBooksAsync(int userId)
        {
            return await _libraryRepository.GetTotalOfBooksAsync(userId);
        }

        public async Task<int> GetTotalOfReadPagesAsync(int id)
        {
            return await _libraryRepository.GetTotalOfReadPagesAsync(id);
        }

        public async Task<List<Book>> ListBookShowcaseAsync(int userId)
        {
            return await _libraryRepository.ListBookShowcaseAsync(userId);
        }

        public async Task RemoveBookShowcaseAsync(int bookId, int userId)
        {
            await _libraryRepository.RemoveBookShowcaseAsync(bookId, userId);
        }

        public async Task<Book> ReturnBook(int bookId)
        {
            return await _libraryRepository.ReturnBook(bookId);
        }

        public async Task<List<Book>> ReturnBookListPerPageAsync(int pageNumber)
        {
            return await _libraryRepository.ReturnBookListPerPageAsync(pageNumber);
        }
    }
}
