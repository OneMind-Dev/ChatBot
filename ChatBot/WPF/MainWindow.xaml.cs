using BOs;
using Services.Implement;
using Services.Interface;
using System;
using System.Windows;

namespace WPF
{
    public partial class MainWindow : Window
    {
        public User LoggedInUser;
        public IUserService UserService;

        public MainWindow()
        {
            InitializeComponent();
            UserService = new UserService();
        }

        // Handle the Cancel button click
        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            txtUser.Clear();
            txtPass.Clear();
        }

        // Handle the Login button click
        private void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPass.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username and password cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Attempt to login the user
                User user = UserService.LoginUser(username, password);

                if (user != null)
                {
                    LoggedInUser = user;
                    MessageBox.Show($"Login successful! Welcome, {user.Username}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Check the user's roleId and navigate accordingly
                    if (user.RoleId == 1)
                    {
                        // Navigate to AdminWindow
                        AdminWindow adminWindow = new AdminWindow();
                        adminWindow.Show();
                    }
                    else if (user.RoleId == 2)
                    {
                        // Navigate to ChatWindow
                        ChatWindow chatWindow = new ChatWindow(LoggedInUser);
                        chatWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid role. Cannot determine which window to open.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    // Close the current login window
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during login: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Open the Register Window
        private void RegisterLink_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }

        // Open the Forget Password Window
        private void ForgetPasswordLink_Click(object sender, RoutedEventArgs e)
        {
            ForgetPasswordWindow forgetPasswordWindow = new ForgetPasswordWindow();
            forgetPasswordWindow.ShowDialog();
        }
    }
}
