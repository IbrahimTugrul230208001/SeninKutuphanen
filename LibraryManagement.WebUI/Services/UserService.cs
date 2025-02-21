namespace learningASP.NET_CORE.Services
{
    public class UserService : IUserService
    {
        private string _profilePicture;
        private string _userName;
        private string _verificationCode;
        private string _email;
        private string _password;
        public string ProfilePicture
        {
            get => _profilePicture;
            set => _profilePicture = value;
        }

        public string UserName
        {
            get => _userName;
            set => _userName = value;
        }

        public string VerificationCode
        {
            get=> _verificationCode;
            set => _verificationCode = value;
        }
        public string Email 
        {
            get => _email;
            set => _email = value; 
        }
        public string Password
        {
            get => _password;
            set => _password = value;
        }
    }


}
