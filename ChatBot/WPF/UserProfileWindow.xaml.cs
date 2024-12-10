using BOs;
using Services.Implement;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// 
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible;
        }
    }
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value != Visibility.Visible;
        }
    }
    public partial class UserProfileWindow : Window
    {
        public IUserService UserService;
        public User LoggedUser;
        public UserProfileWindow(User user)
        {
            InitializeComponent();
            UserService = new UserService();
            LoggedUser = user;
            loadUser();

        }
        public void loadUser() 
        {
            UserAvatar.Source = new BitmapImage(new Uri(LoggedUser.Avatar, UriKind.RelativeOrAbsolute));
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
        private void EmailTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            EmailTextBlock.Visibility = Visibility.Collapsed;
            EmailTextBox.Visibility = Visibility.Visible;
            EmailTextBox.Focus();
        }

        private void EmailTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            EmailTextBox.Visibility = Visibility.Collapsed;
            EmailTextBlock.Visibility = Visibility.Visible;
        }

        private void PhoneTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PhoneTextBlock.Visibility = Visibility.Collapsed;
            PhoneTextBox.Visibility = Visibility.Visible;
            PhoneTextBox.Focus();
        }

        private void PhoneTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            PhoneTextBox.Visibility = Visibility.Collapsed;
            PhoneTextBlock.Visibility = Visibility.Visible;
        }
        private void AddressTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AddressTextBox.Visibility = Visibility.Collapsed;
            AddressTextBlock.Visibility = Visibility.Visible;
        }
        private void AddressTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddressTextBlock.Visibility = Visibility.Collapsed;
            AddressTextBox.Visibility = Visibility.Visible;
            AddressTextBox.Focus();
        }

        private void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            // Lấy giá trị từ các TextBox
            string emailInput = EmailTextBox.Text.Trim();
            string phoneInput = PhoneTextBox.Text.Trim();
            string addressInput = AddressTextBox.Text.Trim();

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
