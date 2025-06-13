using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Entities.Concrete
{
    public class UserFavoriteBook
    {
        [Key]
        public int Id { get; set; }
        public int UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
