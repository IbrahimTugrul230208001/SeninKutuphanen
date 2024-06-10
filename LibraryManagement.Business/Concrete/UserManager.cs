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
        public void AddNewUser(string userName, string password)
        {
            _userRepository.AddNewUser(userName, password);
        }
        public void CheckUserLogInStatus(string stayLoggedIn, string userName)
        {
            _userRepository.CheckUserLogInStatus(stayLoggedIn, userName);
        }

        public string HashPassword(string password)
        {
            return _userRepository.HashPassword(password);
        }
        public string ResidementPlaceCity(string userName)
        {
            return _userRepository.ResidementPlaceCity(userName);
        }

        public string ResidementPlaceCountry(string userName)
        {
            return _userRepository.ResidementPlaceCountry(userName);
        }
        public void SetNewResidementPlaces(string city, string country, string userName)
        {
            _userRepository.SetNewResidementPlaces(city, country, userName);
        }

        public void SetNewUserName(int ID, string userName)
        {
            _userRepository.SetNewUserName(ID, userName);
        }

        public void UpdateUserPassword(string password, string userName)
        {
            _userRepository.UpdateUserPassword(password, userName);
        }

        public int UserID(string userName)
        {
            return _userRepository.UserID(userName);
        }

        public bool ValidateUser(string userName, string password)
        {
            return _userRepository.ValidateUser(userName, password);
        }

        public bool VerifyHashedPassword(string password, string hashedPassword)
        {
            return _userRepository.VerifyHashedPassword(password, hashedPassword);
        }

        public bool VerifyPassword(string userName, string password)
        {
            return _userRepository.VerifyPassword(userName, password);
        }
        public string ProfilePictureImage(string userName)
        {
            return _userRepository.ProfilePictureImage(userName);
        }

        public void SetNewUserProfile(string userName, byte[] imageFile)
        {
            _userRepository.SetNewUserProfile(userName, imageFile);
        }
    }
}
