using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Entities.Concrete
{
    public class AudioLibrary
    {
        public string? UserName { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Author { get; set; }
        public string? Category { get; set; }
        public int CompletedDuration { get; set; }
        public int TotalDuration { get; set; }
        public string? Status { get; set; }
        public bool IsAddedToShowcase { get; set; }
        public byte[]? BookImage { get; set; }
    }
}
