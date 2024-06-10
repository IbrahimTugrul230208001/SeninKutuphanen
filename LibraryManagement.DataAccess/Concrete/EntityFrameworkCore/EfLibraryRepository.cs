using LibraryManagement.DataAccess.Abstract;
using LibraryManagement.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace LibraryManagement.DataAccess.Concrete.EntityFrameworkCore
{
    public class EfLibraryRepository:ILibraryRepository
    {

        public async void AddToLibrary(Library library)
        {
            using (var context = new LibraryContext())
            {
                await context.Libraries.AddAsync(library);
                await context.SaveChangesAsync();
            }
        }
        public int BookStatusCounter(string status,string userName)
        {
            using (var context = new LibraryContext())
            {
                return context.Libraries.Count(l => l.Status == status && l.UserName == userName);
            }
        }

        public int CountCompletedBooks(string userName)
        { 
            using (var context = new LibraryContext())
            {
                return context.Libraries.Count(l => l.UserName == userName && l.CompletedPages == l.TotalOfPages);
            }
        }

        public int CountGenresOfBooks(string genre, string userName)
        {
            using (var context = new LibraryContext())
            {
                return context.Libraries.Count(l => l.UserName == userName && l.Category == genre);
            }
        }

        public async void DeleteFromLibrary(Library library)
        {
            using (var context = new LibraryContext())
            {
                var entity = context.Entry(library);
                entity.State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }

        public int TotalOfBooks(string userName)
        {
           using (var context = new LibraryContext())
           {
                return context.Libraries.Count(l => l.UserName == userName);
           }
        }

        public int GetTotalOfReadPages(string userName)
        {
            using (var context = new LibraryContext())
            {
                int totalOfReadPages = context.Libraries
                    .Where(l => l.UserName == userName)
                    .Sum(l => l.CompletedPages);
                return totalOfReadPages;
            }
        }

      
        public string MostExistingCategoryInLibrary(string userName)
        {
            using (var context = new LibraryContext())
            {
                var historyCount = context.Libraries.Count(l => l.Category == "Tarih" && l.UserName == userName);
                var literatureCount = context.Libraries.Count(l => l.Category == "Edebiyat" && l.UserName == userName);
                var psychologyCount = context.Libraries.Count(l => l.Category == "Psikoloji" && l.UserName == userName);
                var philosophyReligionCount = context.Libraries.Count(l => l.Category == "Din-Felsefe" && l.UserName == userName);
                var scienceCount = context.Libraries.Count(l => l.Category == "Fen-Bilim" && l.UserName == userName);

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

        public async void AddToFavorites(string userName, int ID)
        {
            using (var context = new LibraryContext())
            {
                var book = await context.Libraries.SingleAsync(l=>l.UserName==userName && l.Id == ID);
                book.IsAddedToShowcase = true;
                await context.SaveChangesAsync();
            }
        }
        public int CountFavorites(string userName)
        {
            using (var context = new LibraryContext())
            {
                return context.Libraries.Count(l => l.UserName == userName && l.IsAddedToShowcase == true);
            }
        }
        public async void AddBookImage(byte[] imageFile,string userName,string bookName)
        {
            using (var context = new LibraryContext())
            {
                var book = await context.Libraries.FirstOrDefaultAsync(l => l.Name == bookName && l.UserName == userName);
                book.BookImage = imageFile;
                await context.SaveChangesAsync();
            }
        }
        public List<Library> ListBookShowcase(string userName)
        {
            using (var context = new LibraryContext())
            {
                var showcase = context.Libraries.Where(l => l.IsAddedToShowcase == true && l.UserName == userName).ToList();
                return showcase;
            }
        }
        public bool CheckImageStatus(string userName, string bookName)
        {
            using (var context = new LibraryContext())
            {
                var book = context.Libraries.FirstOrDefault(l => l.UserName == userName && l.Name == bookName);

                if (book == null || book.BookImage == null)
                {
                    return false;
                }
                return true;
            }
        }

        public string BookImage(string userName, string bookName)
        {
            using (var context = new LibraryContext())
            {
                var book = context.Libraries.FirstOrDefault(l => l.UserName == userName && l.Name == bookName);
                if (book != null && book.BookImage != null)
                {
                    string base64String = Convert.ToBase64String(book.BookImage);
                    string dataUri = $"data:image/jpeg;base64,{base64String}";
                    return dataUri;
                }
                return null;
            }
        }
        public async void RemoveBookShowcase(string userName,string bookName) 
        {
            using (var context = new LibraryContext())
            {
                var book = await context.Libraries.SingleAsync(l=>l.Name == bookName && l.UserName == userName);
                book.BookImage = null;
                book.IsAddedToShowcase = false;
                await context.SaveChangesAsync();
            }
        
        }
        public async void UpdateLibrary(int ID, string userName, string bookName, string author, string category, int completedPages, int totalOfPages, string status)
        {
            using(var context = new LibraryContext())
            {
                var book = await context.Libraries.FirstAsync(l=>l.Id == ID && l.UserName == userName);
                book.Name = bookName;
                book.Author = author;
                book.Category = category;
                book.CompletedPages = completedPages;
                book.TotalOfPages = totalOfPages;
                book.Status = status;
                await context.SaveChangesAsync();
            }
        }

        public int GetTotalOfBooks(string userName)
        {
            using(var context = new LibraryContext())
            {
                int books = context.Libraries.Count(l => l.UserName == userName);
                return books;
            }
        }
    }
}
