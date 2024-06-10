namespace LibraryManagementWeb.WebAPI.Services
{
    public class UserService:IUserService
    {
        private string _profilePicture;
        private string _userName;

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
    }
}
