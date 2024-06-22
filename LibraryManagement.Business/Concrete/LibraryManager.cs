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
     
        public async Task AddToShowcaseAsync(string userName, int ID)
        {
            await _libraryRepository.AddToShowcaseAsync(userName, ID);
        }

        public async Task AddToLibraryAsync(Library library)
        {
            await _libraryRepository.AddToLibraryAsync(library);
        }

        public async Task<int> BookStatusCounterAsync(string status,string userName)
        {
            return await _libraryRepository.BookStatusCounterAsync(status,userName);
        }
        public async Task<int> CountCompletedBooksAsync(string userName)
        {
            return await _libraryRepository.CountCompletedBooksAsync(userName);
        }

        public async Task<int> CountGenresOfBooksAsync(string genre, string userName)
        {
            return await _libraryRepository.CountGenresOfBooksAsync(genre, userName);
        }

        public async Task DeleteFromLibraryAsync(Library library)
        {
            await _libraryRepository.DeleteFromLibraryAsync(library);
        }

        public async Task<int> GetTotalOfBooksAsync(string userName)
        {
            return await _libraryRepository.GetTotalOfBooksAsync(userName);
        }

        public async Task<int> GetTotalOfReadPagesAsync(string userName)
        {
            return await _libraryRepository.GetTotalOfReadPagesAsync(userName);
        }


        public async Task<string> MostExistingCategoryInLibraryAsync(string userName)
        {
            return await _libraryRepository.MostExistingCategoryInLibraryAsync(userName);
        }
        
        public async Task<int> CountFavoritesAsync(string userName)
        {
            return await _libraryRepository.CountFavoritesAsync(userName);
        }

        public async Task AddBookImageAsync(byte[] imageFile, string userName, string bookName)
        {
            await _libraryRepository.AddBookImageAsync(imageFile, userName, bookName);
        }

        public async Task<List<Library>> ListBookShowcaseAsync(string userName)
        {
            return await _libraryRepository.ListBookShowcaseAsync(userName);
        }

        public async Task<bool> CheckImageStatusAsync(string userName, string bookName)
        {
            return await _libraryRepository.CheckImageStatusAsync(userName, bookName);
        }

        public async Task<string> BookImageAsync(string userName, string bookName)
        {
            return await _libraryRepository.BookImageAsync(userName, bookName);
        }

        public async Task UpdateLibraryAsync(int ID, string userName, string bookName, string author, string category, int completedPages, int totalOfPages, string status)
        {
            await _libraryRepository.UpdateLibraryAsync(ID, userName, bookName, author, category, completedPages, totalOfPages, status);
        }

        public async Task RemoveBookShowcaseAsync(string userName, string bookName)
        {
           await _libraryRepository .RemoveBookShowcaseAsync(userName, bookName);
        }

        public async Task<int> BookIDAsync(string userName, string bookName)
        {
            return await _libraryRepository.BookIDAsync(userName,bookName);
        }
        public async Task<string> BookNameAsync(int id)
        {
            return await _libraryRepository.BookNameAsync(id);
        }
     
    }
}
