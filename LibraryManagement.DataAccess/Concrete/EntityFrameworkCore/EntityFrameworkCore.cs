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
    public class EntityFrameworkCore : ILibraryDal
    {
        public async void AddNewUser(string userName, string password)
        {
            UserAccount userAccount = new();
            userAccount.UserName = userName;
            userAccount.PasswordHash = HashPassword(password);
            userAccount.ResidementPlaceCity = "-";
            userAccount.ResidementPlaceCountry = "-";
            using(var context = new LibraryContext())
            {
                await context.UserAccounts.AddAsync(userAccount);
                await context.SaveChangesAsync();
            }
        }

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

        public async void CheckUserLogInStatus(string stayLoggedIn, string userName)
        {
            using (var context = new LibraryContext())
            {
                var user = await context.UserAccounts.FirstOrDefaultAsync(u => u.UserName == userName);

                if (user != null)
                {
                    user.StayLoggedIn = stayLoggedIn;
                    await context.SaveChangesAsync();
                }
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

        public int GetTotalOfBooks(string userName)
        {
           using (var context = new LibraryContext())
           {
                return context.Libraries.Count(l => l.UserName == userName);
           }
        }

        public int GetTotalOfReadPages(string userName)
        {
            int totalOfReadPages = 0;
            using (var context = new LibraryContext())
            {
                totalOfReadPages = context.Libraries.Where(l => l.UserName == userName).Sum(l => l.CompletedPages);
            }
            return totalOfReadPages;
        }

        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }

        public string ResidementPlaceCity(string userName)
        {
            using (var context = new LibraryContext())
            {
                var userAccount = context.UserAccounts.FirstOrDefault(l => l.UserName == userName);
                return userAccount.ResidementPlaceCity.ToString();
            }
        }

        public string ResidementPlaceCountry(string userName)
        {
            using (var context = new LibraryContext())
            {
                var userAccount = context.UserAccounts.FirstOrDefault(l => l.UserName == userName);
                return userAccount.ResidementPlaceCountry.ToString();
            }
        }

        public async void SetNewResidementPlaces(string city, string country, string userName)
        {
            using (var context = new LibraryContext())
            {
                var userAccount = await context.UserAccounts.FirstOrDefaultAsync(u => u.UserName == userName);

                if (userAccount != null)
                {
                    userAccount.ResidementPlaceCity = city;
                    userAccount.ResidementPlaceCountry = country;
                    await context.SaveChangesAsync();
                }
            }
        }

        public async void SetNewUserName(int ID, string userName)
        {
            using (var context = new LibraryContext())
            {
                var userAccount = await context.UserAccounts.FirstOrDefaultAsync(l => l.Id == ID);
                userAccount.UserName = userName;
                context.SaveChanges();
            }
        }

        public async void UpdateUserPassword(string password, string userName)
        {
            using (var context = new LibraryContext())
            {
                var user = await context.UserAccounts.FirstOrDefaultAsync(u => u.UserName == userName);

                if (user != null)
                {
                    user.PasswordHash = HashPassword(password);
                    await context.SaveChangesAsync();
                }
            }
        }

        public int UserID(string userName)
        {
            using (var context = new LibraryContext())
            {
                var user = context.UserAccounts.FirstOrDefault(u => u.UserName == userName);
                return user.Id;    
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            using (var context = new LibraryContext())
            {
                var user = context.UserAccounts.FirstOrDefault(u => u.UserName == userName);

                if (user != null)
                {
                    string storedHashedPassword = user.PasswordHash;
                    return VerifyHashedPassword(password, storedHashedPassword);
                }
            }

            return false;
        }


        public bool VerifyHashedPassword(string password, string hashedPassword)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                string inputHashedPassword = Convert.ToBase64String(hashBytes);
                return inputHashedPassword == hashedPassword;
            }
        }

        public bool VerifyPassword(string userName, string password)
        {
            using (var context = new LibraryContext())
            {
                var user = context.UserAccounts.FirstOrDefault(u => u.UserName == userName);

                if (user != null)
                {
                    string storedPasswordHash = user.PasswordHash;

                    using (SHA256 sha256 = SHA256.Create())
                    {
                        byte[] inputPasswordHashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                        string inputPasswordHash = Convert.ToBase64String(inputPasswordHashBytes);
                        return storedPasswordHash == inputPasswordHash;
                    }
                }
            }
            return false;
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
                if (maxCount == historyCount)
                    mostExistingCategory = "Tarih";
                else if (maxCount == literatureCount)
                    mostExistingCategory = "Edebiyat";
                else if (maxCount == psychologyCount)
                    mostExistingCategory = "Psikoloji";
                else if (maxCount == philosophyReligionCount)
                    mostExistingCategory = "Din-Felsefe";
                else if (maxCount == scienceCount)
                    mostExistingCategory = "Fen-Bilim";
                else if (literatureCount == 0 && philosophyReligionCount == 0 && psychologyCount == 0 && scienceCount == 0 && historyCount == 0)
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
        public async void SetNewUserProfile(string userName, byte[] imageFile)
        {
            using(var context = new LibraryContext())
            {
                var user = await context.UserAccounts.FirstOrDefaultAsync(u => u.UserName == userName);
                user.UserProfilePicture = imageFile;
                await context.SaveChangesAsync();
            }
        }
        public string ProfilePictureImage(string userName)
        {
            using (var context = new LibraryContext())
            {
                var user = context.UserAccounts.FirstOrDefault(u => u.UserName == userName);

                if (user != null && user.UserProfilePicture != null)
                {
                    string base64String = Convert.ToBase64String(user.UserProfilePicture);
                    string dataUri = $"data:image/jpeg;base64,{base64String}";
                    return dataUri;
                }
                else
                {
                    return "/img/profilepicture.jpeg";
                }
            }
        }

    }
}
