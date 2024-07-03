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

namespace LibraryManagement.DataAccess.Concrete.EntityFrameworkCore
{
    public class EfLibraryRepository:ILibraryRepository
    {

        public async Task AddToLibraryAsync(Library library)
        {
            using (var context = new LibraryContext())
            {
                await context.Libraries.AddAsync(library);
                await context.SaveChangesAsync();
            }
        }
        public async Task<int> BookStatusCounterAsync(string status, string userName)
        {
            using (var context = new LibraryContext())
            {
                return await context.Libraries.CountAsync(l => l.UserName == userName && l.Status == status);
            }
        }

        public async Task<int> CountCompletedBooksAsync(string userName)
        {
            using (var context = new LibraryContext())
            {
                return await context.Libraries.CountAsync(l => l.UserName == userName && l.CompletedPages == l.TotalOfPages);
            }
        }

        public async Task<int> CountGenresOfBooksAsync(string genre, string userName)
        {
            using (var context = new LibraryContext())
            {
                return await context.Libraries.CountAsync(l => l.UserName == userName && l.Category == genre);
            }
        }

        public async Task DeleteFromLibraryAsync(Library library)
        {
            using (var context = new LibraryContext())
            {
                var entity = context.Entry(library);
                entity.State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }

        public async Task<int> TotalOfBooksAsync(string userName)
        {
            using (var context = new LibraryContext())
            {
                return await context.Libraries.CountAsync(l => l.UserName == userName);
            }
        }

        public async Task<int> GetTotalOfReadPagesAsync(string userName)
        {
            using (var context = new LibraryContext())
            {
                int totalOfReadPages = await context.Libraries
                .Where(l => l.UserName == userName)
                .SumAsync(l => l.CompletedPages);
                return totalOfReadPages;
            }
        }


        public async Task<string> MostExistingCategoryInLibraryAsync(string userName)
        {
            using (var context = new LibraryContext())
            {
                var historyCount = await context.Libraries.CountAsync(l => l.Category == "Tarih" && l.UserName == userName);
                var literatureCount = await context.Libraries.CountAsync(l => l.Category == "Edebiyat" && l.UserName == userName);
                var psychologyCount = await context.Libraries.CountAsync(l => l.Category == "Psikoloji" && l.UserName == userName);
                var philosophyReligionCount = await context.Libraries.CountAsync(l => l.Category == "Din-Felsefe" && l.UserName == userName);
                var scienceCount = await context.Libraries.CountAsync(l => l.Category == "Fen-Bilim" && l.UserName == userName);

                int maxCount = Math.Max(historyCount, Math.Max(literatureCount, Math.Max(psychologyCount, Math.Max(philosophyReligionCount, scienceCount))));

                string mostExistingCategory;
                if (maxCount == historyCount && historyCount > 0)
                    mostExistingCategory = "Tarih";
                else if (maxCount == literatureCount && literatureCount > 0)
                    mostExistingCategory = "Edebiyat";
                else if (maxCount == psychologyCount && psychologyCount > 0)
                    mostExistingCategory = "Psikoloji";
                else if (maxCount == philosophyReligionCount && philosophyReligionCount > 0)
                    mostExistingCategory = "Din-Felsefe";
                else if (maxCount == scienceCount && scienceCount > 0)
                    mostExistingCategory = "Fen-Bilim";
                else if (maxCount == 0)
                    mostExistingCategory = "-";
                else
                    return "Birden Fazla Tür";
                return mostExistingCategory;
            }
        }

        public async Task AddToShowcaseAsync(string userName, int ID)
        {
            using (var context = new LibraryContext())
            {
                var book = await context.Libraries.SingleAsync(l => l.UserName == userName && l.Id == ID);
                book.IsAddedToShowcase = true;
                await context.SaveChangesAsync();
            }
        }
        public async Task<int> CountFavoritesAsync(string userName)
        {
            using (var context = new LibraryContext())
            {
                return await context.Libraries.CountAsync(l => l.UserName == userName && l.IsAddedToShowcase == true);
            }
        }
        public async Task AddBookImageAsync(byte[] imageFile, string userName, string bookName)
        {
            using (var context = new LibraryContext())
            {
                var book = await context.Libraries.FirstOrDefaultAsync(l => l.Name == bookName && l.UserName == userName)
                ?? throw new ArgumentException("Book not found.");
                book.BookImage = imageFile;
                await context.SaveChangesAsync();
            }
        }
        public async Task<List<Library>> ListBookShowcaseAsync(string userName)
        {
            using (var context = new LibraryContext())
            {
                return await context.Libraries.Where(l => l.IsAddedToShowcase == true && l.UserName == userName).ToListAsync();
            }
        }
        public async Task<bool> CheckImageStatusAsync(string userName, string bookName)
        {
            using (var context = new LibraryContext())
            {
                var book = await context.Libraries.FirstOrDefaultAsync(l => l.UserName == userName && l.Name == bookName) ?? throw new ArgumentException("Null here");
                if (book.BookImage == null || book == null) return false;
                else return true;
            }
        }

        public async Task<string> BookImageAsync(string userName, string bookName)
        {
            using (var context = new LibraryContext())
            {
                var book = await context.Libraries.FirstOrDefaultAsync(l => l.UserName == userName && l.Name == bookName);
                if (book != null && book.BookImage != null)
                {
                    string base64String = Convert.ToBase64String(book.BookImage);
                    string dataUri = $"data:image/jpeg;base64,{base64String}";
                    return dataUri;
                }
                return "";
            }
        }
        public async Task RemoveBookShowcaseAsync(string userName, string bookName)
        {
            using (var context = new LibraryContext())
            {
                var book = await context.Libraries.SingleAsync(l => l.Name == bookName && l.UserName == userName);
                book.BookImage = null;
                book.IsAddedToShowcase = false;
                await context.SaveChangesAsync();
            }

        }
        public async Task UpdateLibraryAsync(int ID, string userName, string bookName, string author, string category, int completedPages, int totalOfPages, string status)
        {
            using (var context = new LibraryContext())
            {
                var book = await context.Libraries.FirstAsync(l => l.Id == ID && l.UserName == userName);
                book.Name = bookName;
                book.Author = author;
                book.Category = category;
                book.CompletedPages = completedPages;
                book.TotalOfPages = totalOfPages;
                book.Status = status;
                await context.SaveChangesAsync();
            }
        }

        public async Task<int> GetTotalOfBooksAsync(string userName)
        {
            using (var context = new LibraryContext())
            {
                return await context.Libraries.CountAsync(l => l.UserName == userName);
            }
        }

        public async Task<int> BookIDAsync(string userName, string bookName)
        {
            using (var context = new LibraryContext())
            {
                return await context.Libraries
                .Where(l => l.UserName == userName && l.Name == bookName)
                .Select(l => l.Id)
                .SingleOrDefaultAsync();
            }
        }
        public async Task<string> BookNameAsync(int id)
        {
            using (var context = new LibraryContext())
            {
                var book = await context.Libraries.SingleOrDefaultAsync(l => l.Id == id) ?? throw new ArgumentException("Book not found.");
                return book.Name;
            }
        }
    }
}
