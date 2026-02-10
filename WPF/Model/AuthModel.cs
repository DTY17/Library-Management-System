using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WPF.Models;
using WPF.Repository;

namespace WPF.Model
{
    internal class AuthModel
    {
        private readonly UserRepository userRepository;

        public AuthModel()
        {
            userRepository = new UserRepository();
        }

        public List<User> GetAllUsers()
        {
            try
            {
                return userRepository.GetAllUsers();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving users: {ex.Message}", ex);
            }
        }

        public User GetUserById(int userId)
        {
            try
            {
                if (userId <= 0)
                    throw new ArgumentException("Invalid user ID");

                return userRepository.GetUserById(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user: {ex.Message}", ex);
            }
        }

        public List<User> SearchUsers(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    throw new ArgumentException("Search term cannot be empty");

                return userRepository.SearchUsers(searchTerm);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching users: {ex.Message}", ex);
            }
        }

        public bool CreateUser(string fullName, string username, string email, string password, string role)
        {
            try
            {
                ValidateUserInputs(fullName, username, email, password, role);

                if (userRepository.UsernameExists(username))
                {
                    throw new InvalidOperationException("Username already exists");
                }

                User user = new User
                {
                    FullName = fullName.Trim(),
                    Username = username.Trim(),
                    Email = email.Trim(),
                    PasswordHash = HashPassword(password),
                    Role = role,
                    CreatedDate = DateTime.Now
                };

                return userRepository.CreateUser(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating user: {ex.Message}", ex);
            }
        }

        public bool UpdateUser(int userId, string fullName, string username, string email, string password, string role)
        {
            try
            {
                if (userId <= 0)
                    throw new ArgumentException("Invalid user ID");

                ValidateUserInputs(fullName, username, email, null, role);

                if (userRepository.UsernameExists(username, userId))
                {
                    throw new InvalidOperationException("Username already exists");
                }

                User user = new User
                {
                    Id = userId,
                    FullName = fullName.Trim(),
                    Username = username.Trim(),
                    Email = email.Trim(),
                    Role = role
                };

                bool updatePassword = !string.IsNullOrEmpty(password);
                if (updatePassword)
                {
                    ValidatePassword(password);
                    user.PasswordHash = HashPassword(password);
                }

                return userRepository.UpdateUser(user, updatePassword);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating user: {ex.Message}", ex);
            }
        }

        public bool DeleteUser(int userId)
        {
            try
            {
                if (userId <= 0)
                    throw new ArgumentException("Invalid user ID");

                return userRepository.DeleteUser(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting user: {ex.Message}", ex);
            }
        }

        public int GetUserCount()
        {
            try
            {
                return userRepository.GetUserCount();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user count: {ex.Message}", ex);
            }
        }

        #region Validation Methods

        private void ValidateUserInputs(string fullName, string username, string email, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full name is required");

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required");

            if (username.Length < 3)
                throw new ArgumentException("Username must be at least 3 characters long");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required");

            if (!IsValidEmail(email))
                throw new ArgumentException("Invalid email format");

            if (password != null)
                ValidatePassword(password);

            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("User role is required");
        }

        private void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required");

            if (password.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters long");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        #endregion
        #region Security Methods

        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public bool VerifyPassword(string password, string hash)
        {
            string passwordHash = HashPassword(password);
            return passwordHash.Equals(hash, StringComparison.OrdinalIgnoreCase);
        }
        #endregion

        public bool ValidateUser(User user)
        {
            return userRepository.CheckCredentials(user.Username, user.PasswordHash);
        }
        public bool UsernameExists(string username, int excludeUserId = 0)
        {
            return userRepository.UsernameExists(username, excludeUserId);
        }
        public bool RegisterUser(User user)
        {
            return userRepository.SaveUser(user);
        }

        public string getRole(string username)
        {
            return userRepository.getRoleRepo(username);
        }
    }
}
