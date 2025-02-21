namespace learningASP.NET_CORE.Services
{
    public interface IUserService
    {
        string ProfilePicture { get; set; }
        string UserName { get; set; }
        string Email { get; set; }
        string VerificationCode { get; set; }
        string Password { get; set; }
    }
}
