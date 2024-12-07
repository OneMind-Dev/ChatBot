using BOs;
using System.Windows;
using System.Windows.Input;

namespace WPF
{
    public partial class ChatWindow : Window
    {
        private User LoggedUser;

        // Constructor that accepts the logged-in user
        public ChatWindow()
        {
            InitializeComponent();
              // Set the passed user to the LoggedUser property
            UpdateUI();  // Update the UI based on the login state
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Method to update the UI based on the login state
        private void UpdateUI()
        {
            if (LoggedUser == null)
            {
                // Show Login button and hide user info
                LoginButton.Visibility = Visibility.Visible;
                UsernamePanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Show username and user info, hide Login button
                LoginButton.Visibility = Visibility.Collapsed;
                UsernamePanel.Visibility = Visibility.Visible;
                UsernameLabel.Content = LoggedUser.Username; // Set the username dynamically
                //UserStatusLabel.Content = LoggedUser.Status; // Set user status dynamically
            }
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Create and show the LoginWindow as a dialog
            MainWindow loginWindow = new MainWindow();
            bool? result = loginWindow.ShowDialog();  // Blocks until LoginWindow is closed

            // If the login is successful, transfer the logged-in user
            if (result == true && loginWindow.LoggedInUser != null)
            {
                LoggedUser = loginWindow.LoggedInUser;  // Set the logged-in user
                UpdateUI();  // Update UI to show user information
            }
        }
        private void UsernamePanelButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle the click event
            UserProfileWindow userProfileWindow = new UserProfileWindow(LoggedUser);
            userProfileWindow.ShowDialog();
            LoggedUser = userProfileWindow.LoggedUser;
            UpdateUI();
        }
    }
}
