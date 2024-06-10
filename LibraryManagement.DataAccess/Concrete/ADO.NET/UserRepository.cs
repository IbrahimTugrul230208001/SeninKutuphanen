using LibraryManagement.DataAccess.Abstract;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace LibraryManagement.DataAccess.Concrete.ADO.NET
{
    public class UserRepository:IUserRepository
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
            if (_connection.State == ConnectionState.Closed)
            {
                await _connection.OpenAsync();
            }
        }
        public async void SetNewResidementPlaces(string city, string country, string userName)
        {
            ConnectionControl();
            SqlCommand command = new("UPDATE UserAccounts SET ResidementPlaceCity = @newCity,ResidementPlaceCountry = @newCountry WHERE UserName=@UserName", _connection);
            command.Parameters.AddWithValue("newCity", city);
            command.Parameters.AddWithValue("newCountry", country);
            command.Parameters.AddWithValue("UserName", userName);
            await command.ExecuteNonQueryAsync();
            _connection.Close();
        }
        public async void AddNewUser(string userName, string password)
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
        public async void UpdateUserPassword(string password, string userName)
        {
            ConnectionControl();
            SqlCommand command = new("UPDATE UserAccounts SET PasswordHash=@PasswordHash WHERE UserName = @UserName", _connection);
            command.Parameters.AddWithValue("UserName", userName);
            command.Parameters.AddWithValue("PasswordHash", HashPassword(password));
            await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }
        public async void CheckUserLogInStatus(string stayLoggedIn, string userName)
        {
            ConnectionControl();
            SqlCommand command = new("UPDATE UserAccounts SET StayLoggedIn = @StayLoggedIn WHERE UserName=@UserName", _connection);
            command.Parameters.AddWithValue("@StayLoggedIn", stayLoggedIn);
            command.Parameters.AddWithValue("@UserName", userName);
            await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }
        public string ResidementPlaceCity(string userName)
        {
            ConnectionControl();
            SqlCommand command = new("SELECT ResidementPlaceCity FROM UserAccounts WHERE UserName = @UserName", _connection);
            command.Parameters.AddWithValue("UserName", userName);
            SqlDataReader reader = command.ExecuteReader();
            string? residementPlaceCity = null;
            while (reader.Read())
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
        public int UserID(string userName)
        {
            ConnectionControl();
            SqlCommand command = new("Select ID From UserAccounts WHERE UserName = @UserName", _connection);
            command.Parameters.AddWithValue("UserName", userName);
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

        public async void SetNewUserName(int ID, string userName)
        {
            ConnectionControl();
            SqlCommand command = new("UPDATE UserAccounts SET UserName=@UserName WHERE Id=@Id", _connection);
            command.Parameters.AddWithValue("UserName", userName);
            command.Parameters.AddWithValue("Id", ID);
            await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
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
