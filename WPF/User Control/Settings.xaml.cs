using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WPF.Repository;
using WPF.Models;
using WPF.Model;

namespace WPF.User_Control
{
    public partial class Settings : UserControl
    {
        private AuthModel userService;
        private int selectedUserId = 0;

        public Settings()
        {
            InitializeComponent();
            userService = new AuthModel();
            LoadUsers();
        }

        #region Data Loading Methods

        private void LoadUsers()
        {
            try
            {
                List<User> users = userService.GetAllUsers();
                UsersTable.ItemsSource = users;
                txtUserCount.Text = $"{users.Count} users";
            }
            catch (Exception ex)
            {
                ShowError($"Error loading users: {ex.Message}");
            }
        }

        #endregion
        #region Button Click Events

        private void btnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidatePasswordMatch())
                    return;

                string fullName = txtFullName.Text;
                string username = txtUsername.Text;
                string email = txtEmail.Text;
                string password = txtPassword.Password;
                string role = GetSelectedRole();

                bool success = userService.CreateUser(fullName, username, email, password, role);

                if (success)
                {
                    ShowSuccess("User account created successfully!");
                    ClearForm();
                    LoadUsers();
                }
                else
                {
                    ShowError("Failed to create user account.");
                }
            }
            catch (ArgumentException ex)
            {
                ShowWarning(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                ShowWarning(ex.Message);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void btnUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedUserId == 0)
                {
                    ShowInfo("Please select a user to update.");
                    return;
                }

                if (!string.IsNullOrEmpty(txtPassword.Password))
                {
                    if (!ValidatePasswordMatch())
                        return;
                }

                string fullName = txtFullName.Text;
                string username = txtUsername.Text;
                string email = txtEmail.Text;
                string password = txtPassword.Password;
                string role = GetSelectedRole();

                bool success = userService.UpdateUser(selectedUserId, fullName, username, email, password, role);

                if (success)
                {
                    ShowSuccess("User account updated successfully!");
                    ClearForm();
                    LoadUsers();
                }
                else
                {
                    ShowError("Failed to update user account.");
                }
            }
            catch (ArgumentException ex)
            {
                ShowWarning(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                ShowWarning(ex.Message);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedUserId == 0)
                {
                    ShowInfo("Please select a user to delete.");
                    return;
                }

                var result = MessageBox.Show(
                    $"Are you sure you want to delete the user '{txtFullName.Text}'? This action cannot be undone.",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    bool success = userService.DeleteUser(selectedUserId);

                    if (success)
                    {
                        ShowSuccess("User deleted successfully!");
                        ClearForm();
                        LoadUsers();
                    }
                    else
                    {
                        ShowError("Failed to delete user.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void btnClearForm_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchTerm = txtSearch.Text.Trim();

                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    ShowInfo("Please enter a search term.");
                    return;
                }

                List<User> users = userService.SearchUsers(searchTerm);
                UsersTable.ItemsSource = users;
                txtUserCount.Text = $"{users.Count} users";

                if (users.Count == 0)
                {
                    ShowInfo("No users found matching your search.");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            LoadUsers();
        }

        #endregion
        #region DataGrid Events

        private void UsersTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsersTable.SelectedItem != null)
            {
                User selectedUser = (User)UsersTable.SelectedItem;

                selectedUserId = selectedUser.Id;
                txtFullName.Text = selectedUser.FullName;
                txtUsername.Text = selectedUser.Username;
                txtEmail.Text = selectedUser.Email;

                foreach (ComboBoxItem item in cmbUserRole.Items)
                {
                    if (item.Content.ToString() == selectedUser.Role)
                    {
                        cmbUserRole.SelectedItem = item;
                        break;
                    }
                }

                txtPassword.Clear();
                txtConfirmPassword.Clear();
            }
        }

        #endregion
        #region Helper Methods

        private void ClearForm()
        {
            selectedUserId = 0;
            txtFullName.Clear();
            txtUsername.Clear();
            txtEmail.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            cmbUserRole.SelectedIndex = -1;
            UsersTable.SelectedItem = null;
        }

        private string GetSelectedRole()
        {
            if (cmbUserRole.SelectedItem == null)
                throw new ArgumentException("Please select a user role.");

            return ((ComboBoxItem)cmbUserRole.SelectedItem).Content.ToString();
        }

        private bool ValidatePasswordMatch()
        {
            if (txtPassword.Password != txtConfirmPassword.Password)
            {
                ShowWarning("Passwords do not match.");
                txtConfirmPassword.Focus();
                return false;
            }
            return true;
        }

        #endregion
        #region Message Box Helpers

        private void ShowSuccess(string message)
        {
            MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowWarning(string message)
        {
            MessageBox.Show(message, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ShowInfo(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion
    }
}