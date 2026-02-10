using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using WPF.Dto;
using WPF.Models;
using WPF.User_Control;
using WPF.Utils;

namespace WPF.Repository
{
    public class UserRepository
    {

        public UserRepository()
        {
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (var reader = DB.ExecuteReader("SELECT Id, FullName, Username, Email, Role, CreatedDate FROM Users ORDER BY Id DESC"))
            {
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FullName = reader["FullName"].ToString(),
                        Username = reader["Username"].ToString(),
                        Email = reader["Email"].ToString(),
                        Role = reader["Role"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                    });
                }
            }
            return users;
        }

        public User GetUserById(int userId)
        {
            User user = null;

            using (var reader = DB.ExecuteReader("SELECT Id, FullName, Username, Email, Role, CreatedDate FROM Users WHERE Id = @Id"))
            {
                while (reader.Read())
                {
                    user = new User
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FullName = reader["FullName"].ToString(),
                        Username = reader["Username"].ToString(),
                        Email = reader["Email"].ToString(),
                        Role = reader["Role"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                    };
                }
            }
            return user;
        }

        public List<User> SearchUsers(string searchTerm)
        {            
            List<User> users = new List<User>();

            using (var reader = DB.ExecuteReader(@"SELECT Id, FullName, Username, Email, Role, CreatedDate 
                               FROM Users 
                               WHERE FullName LIKE @Search 
                               OR Username LIKE @Search 
                               OR Email LIKE @Search
                               ORDER BY Id DESC", 
                          new MySqlParameter("@Search", "%" + searchTerm + "%")))
            {
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FullName = reader["FullName"].ToString(),
                        Username = reader["Username"].ToString(),
                        Email = reader["Email"].ToString(),
                        Role = reader["Role"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                    });
                }
            }
            return users;
        }

        public bool UsernameExists(string username, int excludeUserId = 0)
        {
            object result = DB.ExecuteScalar(
                "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Id != @ExcludeId",
                new MySqlParameter("@Username", username),
                new MySqlParameter("@ExcludeId", excludeUserId)
            );

            if (result != null && int.TryParse(result.ToString(), out int count))
            {
                return count > 0;
            }
            return false;
        }


        public bool CreateUser(User user)
        {
            bool isSaved = false;

            int rowsAffected = DB.ExecuteNonQuery(
                @"INSERT INTO Users (FullName, Username, Email, PasswordHash, Role, CreatedDate) 
                               VALUES (@FullName, @Username, @Email, @PasswordHash, @Role, @CreatedDate)",
                new MySqlParameter("@FullName", user.FullName),
                new MySqlParameter("@Username", user.Username),
                new MySqlParameter("@Email", user.Email),
                new MySqlParameter("@PasswordHash", user.PasswordHash),
                new MySqlParameter("@Role", user.Role),
                new MySqlParameter("@CreatedDate", user.CreatedDate)
            );

            if (rowsAffected > 0)
            {
                isSaved = true;
            }
            return isSaved;
        }

        public bool UpdateUser(User user, bool updatePassword = false)
        {
            bool isSaved = false;
            int rowsAffected = 0;

            if (updatePassword)
            {
                rowsAffected = DB.ExecuteNonQuery(
                 @"UPDATE Users SET 
                            FullName = @FullName, 
                            Username = @Username, 
                            Email = @Email, 
                            PasswordHash = @PasswordHash,
                            Role = @Role 
                            WHERE Id = @Id",
                new MySqlParameter("@PasswordHash", user.PasswordHash)
            );
            }
            else
            {
                rowsAffected = DB.ExecuteNonQuery(
                @"UPDATE Users SET 
                            FullName = @FullName, 
                            Username = @Username, 
                            Email = @Email, 
                            Role = @Role 
                            WHERE Id = @Id",
                new MySqlParameter("@FullName", user.FullName),
                new MySqlParameter("@Username", user.Username),
                new MySqlParameter("@Email", user.Email),
                new MySqlParameter("@Role", user.Role),
                new MySqlParameter("@Id", user.Id)
            );
            }

            if (rowsAffected > 0)
            {
                isSaved = true;
            }
            return isSaved;
        }

        public bool DeleteUser(int userId)
        {
            bool isSaved = false;

            int rowsAffected = DB.ExecuteNonQuery(
                "DELETE FROM Users WHERE Id = @Id",
                new MySqlParameter("@id", userId)
            );

            if (rowsAffected > 0)
            {
                isSaved = true;
            }
            return isSaved;
        }

        public int GetUserCount()
        {
            int count = 0;
            object result = DB.ExecuteScalar(
                "SELECT COUNT(*) FROM users"
            );

            if (result != null && int.TryParse(result.ToString(), out int parsed))
            {
                count = parsed;
            }
            return count;
        }

        public bool CheckCredentials(string username, string password) {
            var result = DB.ExecuteReader(
                "SELECT PasswordHash FROM Users WHERE Username=@Username",
                new MySqlParameter("@Username", username)
            );
            Debug.WriteLine(result.Read());
            if (result["PasswordHash"].ToString() == password)
            {
                return true;
            }
            return false; 
        }
        public bool SaveUser(User user) { int rows = DB.ExecuteNonQuery("INSERT INTO Users (Username, PasswordHash, Email) VALUES(@Username, @PasswordHash, @Email)", new MySqlParameter("@Username", user.Username), new MySqlParameter("@PasswordHash", user.PasswordHash), new MySqlParameter("@Email", user.Email)); return rows > 0; }

        internal string getRoleRepo(string username)
        {
            var result = DB.ExecuteReader(
               "SELECT role FROM Users WHERE Username=@Username",
               new MySqlParameter("@Username", username)
            );

            result.Read();
            return (string)result["Role"];
        }
    }
}