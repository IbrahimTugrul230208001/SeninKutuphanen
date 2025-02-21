using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DataAccess.Abstract
    {
        public interface IUserRepository
        {
            Task AddNewUserAsync(string email,string userName, string password);
            Task SetNewUserNameAsync(int ID, string userName);
            Task SetNewUserProfileAsync(string userName, byte[] imageFile);
            Task SetNewResidementPlacesAsync(string city, string country, string userName);
            Task UpdateUserPasswordAsync(string password, string userName);
            Task<int> UserIDAsync(string userName);
            Task<string> ResidementPlaceCityAsync(string userName);
            Task<string> ResidementPlaceCountryAsync(string userName);
            Task<string> ProfilePictureImageAsync(string userName);
            Task<bool> ValidateUserAsync(string userName, string password);
            Task<bool> VerifyPasswordAsync(string userName, string password);
            Task RemoveProfilePictureAsync(string userName);
            Task<int> CompletedPagesOfTodayAsync(string userName);
            Task<string> UserNameAsync(string email);
            Task<bool> ValidateUserNameAsync(string userName);
            Task<bool> ValidateEmailAsync(string email);
    }
}

