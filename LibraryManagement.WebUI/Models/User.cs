namespace learningASP.NET_CORE.Models
{
    public class User
    {
        public string UserName { get; set; }
        public byte[] ProfilePicture { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordAgain { get; set; }
    }
}
