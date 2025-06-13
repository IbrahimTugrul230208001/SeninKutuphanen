﻿using System;
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
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? ResidementPlaceCity { get; set; }
        public string? ResidementPlaceCountry { get; set; }
        public byte[]? UserProfilePicture { get; set; }
        public ICollection<UserBook> UserBooks { get; set; } = [];
        public ICollection<UserFavoriteBook> UserFavoriteBooks { get; set; } = [];
        public ICollection<UserFavoriteAuthor> UserFavoriteAuthors { get; set; } = [];
        public ICollection<UserFavoriteGenre> UserFavoriteGenres { get; set; } = [];
    }
}
