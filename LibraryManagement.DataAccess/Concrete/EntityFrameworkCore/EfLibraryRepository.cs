using LibraryManagement.DataAccess.Abstract;
using LibraryManagement.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Reflection.Metadata.Ecma335;

namespace LibraryManagement.DataAccess.Concrete.EntityFrameworkCore
{
    public class EfLibraryRepository : ILibraryRepository
    {

        public async Task AddToLibraryAsync(int userId, int bookId, string status)
        {
            using (var context = new LibraryContext())
            {
                var userBook = new UserBook
                {
                    UserAccountId = userId,
                    BookId = bookId,
                    Status = status,
                };

                await context.UserBooks.AddAsync(userBook);
                await context.SaveChangesAsync();
            }
        }
        public async Task<bool> CheckBookAsync(int userId, int bookId)
        {
            using (var context = new LibraryContext())
            {
                return await context.UserBooks
                    .AnyAsync(ub => ub.UserAccountId == userId && ub.BookId == bookId);
            }
        }
        public async Task<int> BookStatusCounterAsync(string status, int userId)
        {
            using (var context = new LibraryContext())
            {
                return await context.UserBooks.CountAsync(l => l.UserAccountId == userId && l.Status == status);
            }
        }

        public async Task<int> CountCompletedBooksAsync(int userId)
        {
            using (var context = new LibraryContext())
            {
                return await context.UserBooks.CountAsync(l => l.UserAccountId == userId && l.Status == "tamamlandı");
            }
        }

        public async Task DeleteFromLibraryAsync(int bookId)
        {
            using (var context = new LibraryContext())
            {
                var book = await context.UserBooks.FindAsync(bookId);
                context.UserBooks.Remove(book);
                await context.SaveChangesAsync();
            }
        }

        public async Task<int> TotalOfBooksAsync(int userId)
        {
            using (var context = new LibraryContext())
            {
                return await context.UserBooks.CountAsync(l => l.UserAccountId == userId);
            }
        }

        public async Task AddToShowcaseAsync(int userId)
        {
            using (var context = new LibraryContext())
            {
                var book = await context.UserBooks.SingleAsync(l => l.UserAccountId == userId);
                await context.SaveChangesAsync();
            }
        }
        public async Task<int> CountFavoritesAsync(int id)
        {
            using (var context = new LibraryContext())
            {
                return await context.FavoriteBooks.CountAsync(l => l.UserAccountId == id);
            }
        }
        public async Task<List<Book>> ListBookShowcaseAsync(int id)
        {
            using (var context = new LibraryContext())
            {
                return await context.UserBooks
                    .Where(l => l.UserAccountId == id)
                    .Select(ub => ub.Book)
                    .ToListAsync();
            }
        }

        public async Task RemoveBookShowcaseAsync(int bookId, int userId)
        {
            using (var context = new LibraryContext())
            {
                var book = await context.FavoriteBooks.SingleAsync(l => l.UserAccountId == userId && l.BookId == bookId);
                context.FavoriteBooks.Remove(book);
                await context.SaveChangesAsync();
            }

        }

        public async Task<int> GetTotalOfBooksAsync(int id)
        {
            using (var context = new LibraryContext())
            {
                return await context.UserBooks.CountAsync(l => l.UserAccountId == id);
            }
        }
        public async Task<int> GetTotalOfReadPagesAsync(int id)
        {
            using (var context = new LibraryContext())
            {
                return await context.UserBooks.CountAsync(l => l.UserAccountId == id);
            }
        }

        // Replace the ReturnBook method with the following to suppress CS8603 warning
        public async Task<Book> ReturnBook(int bookId)
        {
            using (var context = new LibraryContext())
            {
                var book = await context.Books.FirstOrDefaultAsync(b => b.Id == bookId);
                return book;
            }
        }
        public async Task<List<Book>> ReturnBookListPerPageAsync(int pageNumber)
        {
            using (var context = new LibraryContext())
            {
                int pageSize = 30;
                int page = pageNumber;
                var totalBooks = await context.Books.CountAsync();
                var books = context.Books
                    .OrderBy(b => b.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                return books;
            }
        }

        public async Task<List<Book>> BookSearchResultAsync(string searchTerm, string criteria)
        {
            using(var context = new LibraryContext())
            {
                IQueryable<Book> query = context.Books;
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    switch (criteria.ToLower())
                    {
                        case "title":
                            query = query.Where(b => b.Title.Contains(searchTerm));
                            break;
                        case "author":
                            query = query.Where(b => b.Author.Contains(searchTerm));
                            break;
                        default:
                            throw new ArgumentException("Invalid search criteria");
                    }
                }
                return await query.ToListAsync();
            }
        }
    }
}
                     