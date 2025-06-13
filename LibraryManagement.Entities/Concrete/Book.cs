using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Entities.Concrete
{
    public class Book
    {
        [Key]                     // Primary key
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Author { get; set; }
        public string? Genre { get; set; }
        public string? Theme { get; set; }
        public string? Audience { get; set; }
        public string? ImageLink { get; set; }
    }
}
