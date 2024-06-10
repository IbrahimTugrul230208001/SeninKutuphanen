using learningASP.NET_CORE.Models;
using Microsoft.AspNetCore.Mvc;

namespace learningASP.NET_CORE.Services
{
    public interface ILibraryService
    {
        Task<Book> AddAsync(Book book);
        Task<Book> UpdateAsync(Book book);
        Task DeleteAsync(int Id);
        Task AddToFavoritesAsync(int bookId, string userId);
        Task RemoveBookShowcaseAsync(int bookId, string userId);
    }
}
