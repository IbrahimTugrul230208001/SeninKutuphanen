using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Entities.Concrete
{
    public class UserFavoriteGenre
    {
        [Key]
        public int Id { get; set; }
        public int UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; }

        public int GenreId { get; set; }
        public Book Book { get; set; }
    }
}
