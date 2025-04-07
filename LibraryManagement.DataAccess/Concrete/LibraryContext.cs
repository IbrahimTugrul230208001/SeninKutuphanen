using LibraryManagement.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DataAccess.Concrete
{
    public class LibraryContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server = (localdb)\\mssqllocaldb;Database=LibraryManagement;integrated security=true");
        }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<AudioLibrary> AudioLibraries { get; set; }
    }
}
