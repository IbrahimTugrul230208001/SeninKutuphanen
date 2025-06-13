namespace learningASP.NET_CORE.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public string? Password { get; set; }
        public string? NewPassword { get; set; }
        public string? NewPasswordAgain { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? VerificationCode { get; set; }
        public string? FavoriteBook { get; set; }
        public string? FavoriteAuthor { get; set; }
        public string? FavoriteGenre { get; set; }
    }
}
