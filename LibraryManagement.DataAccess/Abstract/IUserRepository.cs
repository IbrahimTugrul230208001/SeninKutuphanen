using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DataAccess.Abstract
{
    public interface IUserRepository
    {
        bool ValidateUser(string userName, string password);
        bool VerifyHashedPassword(string password, string hashedPassword);
        bool VerifyPassword(string userName, string password);
        void SetNewResidementPlaces(string city, string country, string userName);
        void AddNewUser(string userName, string password);
        string HashPassword(string password);
        void UpdateUserPassword(string password, string userName);
        int UserID(string userName);
        void SetNewUserName(int ID, string userName);
        string ResidementPlaceCity(string userName);
        string ResidementPlaceCountry(string userName);
        string ProfilePictureImage(string userName);
        void SetNewUserProfile(string userName, byte[] imageFile);
        void CheckUserLogInStatus(string stayLoggedIn, string userName);

    }
}
