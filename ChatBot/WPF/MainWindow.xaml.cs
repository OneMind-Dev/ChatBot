using BOs;
using Services.Implement;
using Services.Interface;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public User LoggedInUser;
        public IUserService UserService;
        public MainWindow()
        {
            InitializeComponent();
            UserService = new UserService();
        }

        
        

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Xóa nội dung của các ô nhập
            txtUser.Clear();
            txtPass.Clear();
        }

        private void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPass.Password;

            // Assume UserService.LoginUser() validates the user
            User user = UserService.LoginUser(username, password);

            if (user != null)
            {
                LoggedInUser = user;
                MessageBox.Show($"Login successful! Welcome, {user.Username}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;  // Close the window and indicate successful login
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}