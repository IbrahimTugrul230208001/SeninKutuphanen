using LibraryManagement.Entities.Concrete;

namespace learningASP.NET_CORE.Models
{
    public class AnaSayfaViewModel
    {
        public List<Book> Books { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public HashSet<int> CheckedIds { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserProfilePicture { get; set; }
    }
}
