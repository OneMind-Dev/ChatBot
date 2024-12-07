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
    /// Interaction logic for ForgetPasswordWindow.xaml
    /// </summary>
    public partial class ForgetPasswordWindow : Window
    {
        private IUserService _userService;
        private User _user;  // Biến để lưu thông tin người dùng

        public ForgetPasswordWindow()
        {
            InitializeComponent();
            _userService = new UserService(); // Giả sử bạn đã có lớp UserService để tương tác với cơ sở dữ liệu.
        }

        // Sự kiện khi bấm nút "Find Email"
        private void FindEmailButton_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Please enter your email address.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Kiểm tra nếu email có tồn tại trong cơ sở dữ liệu
                _user = _userService.GetUserByEmail(email);  // Giả sử phương thức GetUserByEmail tồn tại trong IUserService

                if (_user != null)
                {
                    // Email tìm thấy, hiển thị phần nhập mật khẩu
                    PasswordPanel.Visibility = Visibility.Visible;
                    txtEmail.IsEnabled = false;  // Vô hiệu hóa trường nhập email
                    FindEmailButton.Visibility = Visibility.Collapsed;  // Ẩn nút "Find Email"
                    ChangePasswordButton.Visibility = Visibility.Visible;  // Hiển thị nút "Change Password"

                    MessageBox.Show("Email found! Please enter a new password.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Email not found. Please check and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Sự kiện khi bấm nút "Change Password"
        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = txtNewPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;

            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please enter both new password and confirm password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("The passwords do not match. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (_user != null)
                {
                    _user.Password = newPassword;  // Cập nhật mật khẩu mới

                    bool success = _userService.UpdateUser(_user);  // Cập nhật user vào cơ sở dữ liệu

                    if (success)
                    {
                        MessageBox.Show("Password has been updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update the password. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("User not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Sự kiện khi bấm nút "Cancel"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();  // Đóng cửa sổ
        }
    }
}