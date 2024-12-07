using BOs;
using Services.Implement;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF
{
    /// <summary>
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        User curUser;
        private IUserService userService;
        public ChangePasswordWindow(User user)
        {
            InitializeComponent();
            this.curUser = user;
            userService = new UserService();
        }
        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {

            string currentPass = currentPassword.Password.Trim();
            string newPass = newPassword.Password.Trim();
            string confirmPass = confirmPassword.Password.Trim();

            // Kiểm tra mật khẩu hiện tại (giả định, thay bằng kiểm tra thực tế)
            if (currentPass != curUser.Password) // Thay bằng logic thực
            {
                MessageBox.Show("Current password is incorrect.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Kiểm tra mật khẩu mới và xác nhận mật khẩu
            if (string.IsNullOrWhiteSpace(newPass))
            {
                MessageBox.Show("New password cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (newPass != confirmPass)
            {
                MessageBox.Show("New password and confirm password do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            curUser.Password = newPass;
            userService.UpdateUser(curUser);
            MessageBox.Show("Password changed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
