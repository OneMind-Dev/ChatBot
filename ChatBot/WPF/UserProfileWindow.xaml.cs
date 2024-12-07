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
    /// Interaction logic for UserProfileWindow.xaml
    /// </summary>
    public partial class UserProfileWindow : Window
    {
        public IUserService UserService;
        public User LoggedUser;
        public UserProfileWindow(User user)
        {
            InitializeComponent();
            UserService = new UserService();
            LoggedUser = user;
            DataContext = LoggedUser;

        }
        public void loadUser() 
        {
            DataContext = LoggedUser;
        }
        private bool IsValidEmail(string email)
        {
            // Biểu thức Regex kiểm tra định dạng email
            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, emailRegex);
        }
        private bool IsNumeric(string input)
        {
            return long.TryParse(input, out _); // Chỉ cần kiểm tra có thể chuyển đổi sang số
        }


        private void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            // Lấy giá trị từ các TextBox
            string emailInput = email.Text.Trim();
            string phoneInput = phone.Text.Trim();
            string addressInput = address.Text.Trim();

            // Kiểm tra Address không được rỗng
            if (string.IsNullOrWhiteSpace(addressInput))
            {
                MessageBox.Show("Address cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra định dạng Email
            if (!IsValidEmail(emailInput))
            {
                MessageBox.Show("Invalid email format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra Phone chỉ chứa số
            if (!IsNumeric(phoneInput))
            {
                MessageBox.Show("Phone number must be numeric.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Nếu tất cả hợp lệ, cập nhật thông tin người dùng
                LoggedUser.Email = emailInput;
                LoggedUser.Phone = phoneInput;
                LoggedUser.Address = addressInput;

                // Gọi UserService để lưu thay đổi
                bool isUpdated = UserService.UpdateUser(LoggedUser);
                if (isUpdated)
                {
                    MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to update profile. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Button_LogOut_Click(object sender, RoutedEventArgs e)
        {
            LoggedUser = null;
            this.Close();
        }

        private void Button_ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow changePasswordWindow = new ChangePasswordWindow(LoggedUser);
            changePasswordWindow.ShowDialog();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
