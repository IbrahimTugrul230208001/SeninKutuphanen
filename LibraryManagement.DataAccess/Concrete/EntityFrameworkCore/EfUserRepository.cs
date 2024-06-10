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
        public async void AddNewUser(string userName, string password)
        {
            UserAccount userAccount = new();
            userAccount.UserName = userName;
            userAccount.PasswordHash = HashPassword(password);
            userAccount.ResidementPlaceCity = "-";
            userAccount.ResidementPlaceCountry = "-";
            using (var context = new LibraryContext())
            {
                await context.UserAccounts.AddAsync(userAccount);
                await context.SaveChangesAsync();
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
        public async void SetNewUserProfile(string userName, byte[] imageFile)
        {
            using (var context = new LibraryContext())
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
    }
}
