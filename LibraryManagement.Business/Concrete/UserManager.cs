using LibraryManagement.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Business.Concrete
{
    public class UserManager:IUserRepository
    {
        private readonly IUserRepository _userRepository;
        public UserManager(IUserRepository userRepository)
        {
              _userRepository = userRepository;
        }

        public async Task AddNewUserAsync(string email,string userName, string password)
        {
            await _userRepository.AddNewUserAsync(email,userName, password);
        }

        public async Task SetNewUserNameAsync(int ID, string userName)
        {
            await _userRepository.SetNewUserNameAsync(ID, userName);
        }

        public async Task SetNewUserProfileAsync(string userName, byte[] imageFile)
        {
            await _userRepository.SetNewUserProfileAsync(userName, imageFile);
        }

        public async Task SetNewResidementPlacesAsync(string city, string country, string userName)
        {
            await _userRepository.SetNewResidementPlacesAsync(city, country, userName);
        }

        public async Task UpdateUserPasswordAsync(string password, string userName)
        {
            await _userRepository.UpdateUserPasswordAsync(password, userName);
        }

        public async Task<int> UserIDAsync(string userName)
        {
            return await _userRepository.UserIDAsync(userName);
        }

        public async Task<string> ResidementPlaceCityAsync(string userName)
        {
            return await _userRepository.ResidementPlaceCityAsync(userName);
        }

        public async Task<string> ResidementPlaceCountryAsync(string userName)
        {
            return await _userRepository.ResidementPlaceCountryAsync(userName);
        }

        public async Task<string> ProfilePictureImageAsync(string userName)
        {
            return await _userRepository.ProfilePictureImageAsync(userName);
        }

        public async Task<bool> ValidateUserAsync(string email, string password)
        { 
           return await _userRepository.ValidateUserAsync(email, password);
        }

        public async Task<bool> VerifyPasswordAsync(string userName, string password)
        {
            return await _userRepository.VerifyPasswordAsync(userName, password);
        }

        public async Task RemoveProfilePictureAsync(string userName)
        {
            await _userRepository.RemoveProfilePictureAsync(userName);
        }

        public async Task<int> CompletedPagesOfTodayAsync(string userName)
        {
            return await _userRepository.CompletedPagesOfTodayAsync(userName);
        }

        public async Task<string> UserNameAsync(string email)
        {
            return await _userRepository.UserNameAsync(email);
        }

        public async Task<bool> ValidateUserNameAsync(string userName)
        {
            return await _userRepository.ValidateUserNameAsync(userName);
        }

        public async Task<bool> ValidateEmailAsync(string email)
        {
            return await _userRepository.ValidateEmailAsync(email);
        }
    }
}
