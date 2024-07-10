using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Entities.Concrete
{
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? PasswordHash { get; set; }
        public string? ResidementPlaceCity { get; set; }
        public string? ResidementPlaceCountry { get; set; }
        public byte[]? UserProfilePicture { get; set; } 
        public int CompletedPagesOfToday { get; set; }
    }
}
