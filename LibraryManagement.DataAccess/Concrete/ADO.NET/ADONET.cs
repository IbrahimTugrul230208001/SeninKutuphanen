using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Runtime.InteropServices;
using Microsoft.Data.SqlClient;
using LibraryManagement.Entities.Concrete;
using System.Security.Cryptography;
using LibraryManagement.DataAccess.Abstract;

namespace LibraryManagement.DataAccess.Concrete.ADO.NET
{
    public class ADONET : ILibraryDal
    {
        SqlConnection _connection = new("server = (localdb)\\mssqllocaldb;initial catalog = LibraryManagement;integrated security = true");


        public bool ValidateUser(string userName, string password)
        {
            ConnectionControl();
            string query = "SELECT PasswordHash FROM UserAccounts WHERE Username = @Username";
            using (_connection)
            {
                
                SqlCommand command = new(query, _connection);
                command.Parameters.AddWithValue("@Username", userName);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string storedHashedPassword = reader["PasswordHash"].ToString();
                        return VerifyHashedPassword(password, storedHashedPassword);
                    }
                }
            }
            return false;
        }
   
        public bool VerifyHashedPassword(string password, string hashedPassword)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                string inputHashedPassword = Convert.ToBase64String(hashBytes);
                return inputHashedPassword == hashedPassword;
            }
        }
        public bool VerifyPassword(string userName, string password)
        {
            ConnectionControl();
            SqlCommand command = new("SELECT PasswordHash FROM UserAccounts WHERE UserName = @UserName", _connection);
            command.Parameters.AddWithValue("@UserName", userName);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string storedPasswordHash = reader["PasswordHash"].ToString();

                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] inputPasswordHashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    string inputPasswordHash = Convert.ToBase64String(inputPasswordHashBytes);

                    bool passwordsMatch = storedPasswordHash == inputPasswordHash;

                    reader.Close();

                    return passwordsMatch;
                }
            }

            reader.Close();

            return false;
        }

        private async void ConnectionControl()
        {
            if(_connection.State == ConnectionState.Closed)
            {
                await _connection.OpenAsync();
            }
        }
        public async void SetNewResidementPlaces(string city,string country,string userName)
        {
            ConnectionControl();
            SqlCommand command = new("UPDATE UserAccounts SET ResidementPlaceCity = @newCity,ResidementPlaceCountry = @newCountry WHERE UserName=@UserName",_connection);
            command.Parameters.AddWithValue("newCity",city);
            command.Parameters.AddWithValue("newCountry",country);
            command.Parameters.AddWithValue("UserName",userName);
            await command.ExecuteNonQueryAsync();
            _connection.Close();
        }
        public async void AddNewUser(string userName,string password)
        {
            ConnectionControl();
            string hashedPassword = HashPassword(password);
            SqlCommand command = new("INSERT INTO UserAccounts (UserName, PasswordHash) VALUES (@UserName, @PasswordHash)", _connection);
            command.Parameters.AddWithValue("@UserName", userName);
            command.Parameters.AddWithValue("@PasswordHash", hashedPassword);
            await command.ExecuteNonQueryAsync();
            _connection.Close();
        }
        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }
        public async void UpdateUserPassword(string password,string userName)
        {
            ConnectionControl();
            SqlCommand command = new("UPDATE UserAccounts SET PasswordHash=@PasswordHash WHERE UserName = @UserName", _connection);
            command.Parameters.AddWithValue("UserName",userName);
            command.Parameters.AddWithValue("PasswordHash",HashPassword(password));
            await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }
        public  int GetTotalOfReadPages(string userName)
        {
            ConnectionControl();
            SqlCommand command = new("SELECT completedPages FROM Libraries WHERE UserName = @UserName", _connection);
            command.Parameters.AddWithValue("@UserName", userName);
            SqlDataReader reader = command.ExecuteReader();
            int totalOfReadPages = 0;
            while (reader.Read())
            {
                totalOfReadPages += Convert.ToInt32(reader["completedPages"]);
            }
             reader.Close();
             _connection.Close();
            return totalOfReadPages;
        }
        public async void CheckUserLogInStatus(string stayLoggedIn,string userName)
        {
                ConnectionControl();
                SqlCommand command = new("UPDATE UserAccounts SET StayLoggedIn = @StayLoggedIn WHERE UserName=@UserName", _connection);
                command.Parameters.AddWithValue("@StayLoggedIn",stayLoggedIn);
                command.Parameters.AddWithValue("@UserName",userName);
                await command.ExecuteNonQueryAsync();
                await _connection.CloseAsync();   
        }

        public async void AddToLibrary(Library library)
        {
            ConnectionControl();
            SqlCommand command = new("INSERT INTO Libraries VALUES(@UserName,@Name,@Author,@Category,@CompletedPages,@TotalOfPages,@Status)",_connection);
            command.Parameters.AddWithValue("@UserName",library.UserName);
            command.Parameters.AddWithValue("@Name", library.Name);
            command.Parameters.AddWithValue("@Author",library.Author);
            command.Parameters.AddWithValue("@Category", library.Category);
            command.Parameters.AddWithValue("@CompletedPages", library.CompletedPages);
            command.Parameters.AddWithValue("@TotalOfPages", library.TotalOfPages);
            command.Parameters.AddWithValue("@Status", library.Status);
            await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }

        public int BookStatusCounter(string status,string userName)
        {
            ConnectionControl();
            SqlCommand command = new("SELECT Status FROM Libraries WHERE UserName = @UserName",_connection);
            command.Parameters.AddWithValue("UserName",userName);
            SqlDataReader reader = command.ExecuteReader();
            int bookCounter = 0;
            while (reader.Read())
            {
                if (status == reader["Status"].ToString())
                {
                    ++bookCounter;
                }
            }
            reader.Close();
            _connection.Close();
            return bookCounter;
        }
        public int CountCompletedBooks(string userName)
        {
            ConnectionControl();
            SqlCommand command = new("SELECT (CONVERT(decimal(5, 2), CompletedPages) / NULLIF(TotalofPages, 0)) * 100 AS [CompletionRate] FROM Libraries WHERE (CONVERT(decimal(5, 2), CompletedPages) / NULLIF(TotalofPages, 0)) * 100 = 100 AND UserName = @UserName", _connection);
            command.Parameters.AddWithValue("UserName",userName);
            SqlDataReader reader = command.ExecuteReader();
            int totalOfReadBooks = 0;
            while (reader.Read())
            {
                ++totalOfReadBooks;
            }   
            reader.Close();
            _connection.Close();
            return totalOfReadBooks;
        }
        public int CountGenresOfBooks(string genre, string userName)
        {
            ConnectionControl();
            SqlCommand command = new("SELECT Category FROM Libraries WHERE UserName = @UserName",_connection);
            command.Parameters.AddWithValue("UserName",userName);
            SqlDataReader reader = command.ExecuteReader();
            int totalOfBooksWithGivenGenre = 0;
            while (reader.Read())
            {
                if (genre == reader["Category"].ToString())
                {
                    ++totalOfBooksWithGivenGenre;
                } 
            }
            reader.Close();
            _connection.Close();
            return totalOfBooksWithGivenGenre;
        }
        
        public int GetTotalOfBooks(string userName)
        {
            ConnectionControl();
            SqlCommand command = new("SELECT * FROM Libraries WHERE UserName = @UserName",_connection);
            command.Parameters.AddWithValue("UserName", userName);
            SqlDataReader reader = command.ExecuteReader();
            int totalOfReadBooks = 0;
            while (reader.Read())
            {
                ++totalOfReadBooks;
            }
            reader.Close();
            _connection.Close();
            return totalOfReadBooks;

        }

        public string ResidementPlaceCity(string userName)
        {
            ConnectionControl();
            SqlCommand command = new("SELECT ResidementPlaceCity FROM UserAccounts WHERE UserName = @UserName", _connection);
            command.Parameters.AddWithValue("UserName", userName);
            SqlDataReader reader = command.ExecuteReader();
            string? residementPlaceCity = null;
            while(reader.Read())
            {
                residementPlaceCity = Convert.ToString(reader["ResidementPlaceCity"]);
            }
            return residementPlaceCity;
        }

        public string ResidementPlaceCountry(string userName)
        {
            ConnectionControl();
            SqlCommand command = new("SELECT ResidementPlaceCountry FROM UserAccounts WHERE UserName = @UserName", _connection);
            command.Parameters.AddWithValue("UserName", userName);
            SqlDataReader reader = command.ExecuteReader();
            string? residementPlaceCountry = null;
            while (reader.Read())
            {
                residementPlaceCountry = Convert.ToString(reader["ResidementPlaceCountry"]);
            }
            return residementPlaceCountry;
        }

        public async void DeleteFromLibrary(Library library)
        {
            ConnectionControl();
            SqlCommand command = new("DELETE FROM Libraries WHERE ID=@Id", _connection);
            command.Parameters.AddWithValue("ID",library.Id);
            await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }

        public int UserID(string userName)
        {
            ConnectionControl();
            SqlCommand command = new("Select ID From UserAccounts WHERE UserName = @UserName", _connection);
            command.Parameters.AddWithValue("UserName",userName);
            SqlDataReader reader = command.ExecuteReader();
            int Id = 0;
            while (reader.Read())
            {
                Id = Convert.ToInt32(reader["ID"]);
            }
            reader.Close();
            _connection.Close();
            return Id;
        }

        public async void SetNewUserName(int ID,string userName)
        {
            ConnectionControl();
            SqlCommand command = new("UPDATE UserAccounts SET UserName=@UserName WHERE Id=@Id", _connection);
            command.Parameters.AddWithValue("UserName",userName);
            command.Parameters.AddWithValue("Id", ID);
            await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }

        public string MostExistingCategoryInLibrary(string userName)
        {
            ConnectionControl();
            SqlCommand command = new SqlCommand("SELECT Category FROM Libraries WHERE UserName = @UserName", _connection);
            command.Parameters.AddWithValue("@UserName", userName);
            SqlDataReader reader = command.ExecuteReader();

            int historyCount = 0;
            int literatureCount = 0;
            int psychologyCount = 0;
            int philosophyReligionCount = 0;
            int scienceCount = 0;

            while (reader.Read())
            {
                string category = reader["Category"].ToString();

                switch (category)
                {
                    case "Tarih":
                        historyCount++;
                        break;
                    case "Edebiyat":
                        literatureCount++;
                        break;
                    case "Psikoloji":
                        psychologyCount++;
                        break;
                    case "Din-Felsefe":
                        philosophyReligionCount++;
                        break;
                    case "Fen-Bilim":
                        scienceCount++;
                        break;
                    default:
                        break;
                }
            }

            int maxCount = Math.Max(historyCount, Math.Max(literatureCount, Math.Max(psychologyCount, Math.Max(philosophyReligionCount, scienceCount))));

            string mostExistingCategory;
            if (maxCount == historyCount)
                mostExistingCategory = "Tarih";
            else if (maxCount == literatureCount)
                mostExistingCategory = "Edebiyat";
            else if (maxCount == psychologyCount)
                mostExistingCategory = "Psikoloji";
            else if (maxCount == philosophyReligionCount)
                mostExistingCategory = "Din-Felsefe";
            else if (maxCount == scienceCount)
                mostExistingCategory = "Fen-Bilim";
            else
                return "Birden Fazla Tür";

            return mostExistingCategory;
        }

        public void AddToFavorites(string username, int ID)
        {
            throw new NotImplementedException();
        }

        public int CountFavorites(string userName)
        {
            throw new NotImplementedException();
        }

        public void AddBookImage(byte[] imageFile, string userName, string bookName)
        {
            throw new NotImplementedException();
        }

        public List<Library> ListBookShowcase(string userName)
        {
            throw new NotImplementedException();
        }

        public bool CheckImageStatus(string userName, string bookName)
        {
            throw new NotImplementedException();
        }

        public string BookImage(string userName, string bookName)
        {
            throw new NotImplementedException();
        }

        public void UpdateLibrary(int ID, string userName, string bookName, string author, string category, int completedPages, int totalOfPages, string status)
        {
            throw new NotImplementedException();
        }

        public void RemoveBookShowcase(string userName, string bookName)
        {
            throw new NotImplementedException();
        }

        public string ProfilePictureImage(string userName)
        {
            throw new NotImplementedException();
        }

        public void SetNewUserProfile(string userName, byte[] imageFile)
        {
            throw new NotImplementedException();
        }
    }
}
