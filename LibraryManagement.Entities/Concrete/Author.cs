using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Entities.Concrete
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        public string? AuthorName { get; set; } 
    }
}
