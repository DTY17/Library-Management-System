using System.Windows;
using WPF.Model;
using WPF.Dto;
using WPF.Models;

namespace WPF.User_Control.Auth
{
    public partial class Login : Window
    {
        private readonly AuthModel userModel;

        public Login()
        {
            InitializeComponent();
            userModel = new AuthModel();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = userModel.HashPassword(txtPassword.Password.ToString());

            bool isValid = userModel.ValidateUser(new User
            {
                Username = username,
                PasswordHash = password
            });

            if (isValid)
            {
                MessageBox.Show("Login successful!");
                UserData.Instance().Role = userModel.getRole(username);
                new MainWindow().Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }
    }
}
