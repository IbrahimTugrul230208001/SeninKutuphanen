using LibraryManagement.DataAccess.Abstract;
using LibraryManagement.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace LibraryManagement.DataAccess.Concrete.EntityFrameworkCore
{
    public class EfUserRepository:IUserRepository
    {
        public async Task AddNewUserAsync(string userName, string password)
        {
            try
            {
                UserAccount userAccount = new()
                {
                    UserName = userName,
                    PasswordHash = HashPassword(password),
                    ResidementPlaceCity = "-",
                    ResidementPlaceCountry = "-"
                };
                using (var context = new LibraryContext())
                {
                    try
                    {
                        // Attempt to query the database to check the connection
                        var test = context.Database.CanConnect();
                        Console.WriteLine($"Database connection test result: {test}");
                    }
                    catch (Exception dbEx)
                    {
                        Console.WriteLine($"Database connection error: {dbEx.Message}");
                        throw; // Re-throw to catch in the main catch block
                    }

                    await context.UserAccounts.AddAsync(userAccount);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log the exception (use a logger in a real application)
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }


        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }
        public async Task<string> ResidementPlaceCityAsync(string userName)
        {
            using (var context = new LibraryContext())
            {
                var userAccount = await context.UserAccounts.FirstOrDefaultAsync(l => l.UserName == userName) ?? throw new ArgumentException("Null here");
                return  userAccount.ResidementPlaceCity.ToString();
            }
        }

        public async Task<string> ResidementPlaceCountryAsync(string userName)
        {
            using (var context = new LibraryContext())
            {
                var userAccount = await context.UserAccounts.FirstOrDefaultAsync(l => l.UserName == userName) ?? throw new ArgumentException("Null here");
                return userAccount.ResidementPlaceCountry.ToString();
            }
        }
        public async Task SetNewUserNameAsync(int ID, string userName)
        {
            using (var context = new LibraryContext())
            {
                var userAccount = await context.UserAccounts.FirstOrDefaultAsync(l => l.Id == ID) ?? throw new ArgumentException("Null here");
                userAccount.UserName = userName;
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateUserPasswordAsync(string password, string userName)
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

        public async Task<int> UserIDAsync(string userName)
        {
            using (var context = new LibraryContext())
            {
                var user = await context.UserAccounts.FirstOrDefaultAsync(u => u.UserName == userName) ?? throw new ArgumentException("Null here");
                return user.Id;
            }
        }

        public async Task<bool> ValidateUserAsync(string userName, string password)
        {
            using (var context = new LibraryContext())
            {
                var user = await context.UserAccounts.FirstOrDefaultAsync(u => u.UserName == userName);

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

        public async Task<bool> VerifyPasswordAsync(string userName, string password)
        {
            using (var context = new LibraryContext())
            {
                var user = await context.UserAccounts.FirstOrDefaultAsync(u => u.UserName == userName) ?? throw new ArgumentException("Null here");

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
        public async Task SetNewUserProfileAsync(string userName, byte[] imageFile)
        {
            using (var context = new LibraryContext())
            {
                var user = await context.UserAccounts.FirstOrDefaultAsync(u => u.UserName == userName);
                if (user != null)
                {
                    user.UserProfilePicture = imageFile;
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<string> ProfilePictureImageAsync(string userName)
        {
            using (var context = new LibraryContext())
            {
                var user = await context.UserAccounts.FirstOrDefaultAsync(u => u.UserName == userName);
                if (user != null && user.UserProfilePicture != null)
                {
                    string base64String = Convert.ToBase64String(user.UserProfilePicture);
                    return $"data:image/jpeg;base64,{base64String}";
                }
                else
                {
                    return "/img/profilepicture.jpeg";
                }
            }
        }

        public async Task SetNewResidementPlacesAsync(string city, string country, string userName)
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

        public async Task RemoveProfilePictureAsync(string userName)
        {
            using(var context  = new LibraryContext())
            {
                var user = await context.UserAccounts.SingleOrDefaultAsync(u=>u.UserName == userName) ?? throw new ArgumentException("Null object");
                user.UserProfilePicture = null;
                await context.SaveChangesAsync();
            }
        }
    }
}
