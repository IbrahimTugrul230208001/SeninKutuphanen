using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Entities.Concrete
{
    public class Library
    {
        public string? UserName { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Author { get; set; }
        public string? Category { get; set; }
        public int CompletedPages { get; set; }
        public int TotalOfPages { get; set; }
        public string? Status { get; set; }
        public bool IsAddedToShowcase { get; set; }
        public byte[]? BookImage { get; set; }
    }
}
