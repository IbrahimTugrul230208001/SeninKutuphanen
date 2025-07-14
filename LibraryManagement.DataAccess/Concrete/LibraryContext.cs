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
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=ibrahim06;Database=LibraryManagement;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            const string trCollation = "tr_icu_det";   // PostgreSQL deterministic ICU collation

            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(b => b.Title)
                      .UseCollation(trCollation);

                entity.Property(b => b.Author)
                      .UseCollation(trCollation);
            });

            // ── Existing composite index ────────────────────────────────────────────
            modelBuilder.Entity<UserBook>()
                .HasIndex(ub => new { ub.UserAccountId, ub.BookId })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }



        public DbSet<Book> Books { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<UserBook> UserBooks { get; set; }
        public DbSet<UserFavoriteAuthor> FavoriteAuthors { get; set; }
        public DbSet<UserFavoriteGenre> FavoriteGenres { get; set; }
        public DbSet<UserFavoriteBook> FavoriteBooks { get; set; }
    }
}

