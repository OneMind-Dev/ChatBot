using System;
using System.Windows;
using Microsoft.Win32;
using Services.Implement;
using Services.Interface;
using BOs;

namespace WPF
{
    public partial class RegisterWindow : Window
    {
        private IUserService _userService;

        public RegisterWindow()
        {
            InitializeComponent();
            _userService = new UserService(); // Giả sử bạn đã có lớp UserService để tương tác với cơ sở dữ liệu.
        }

        // Sự kiện khi bấm nút Register
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy thông tin từ UI
            string username = txtUsername.Text;
            string password = txtPassword.Password;
            string email = txtEmail.Text;
            string phone = txtPhone.Text;
            string address = txtAddress.Text;
            string avatar = txtAvatar.Text;  // Đường dẫn đến ảnh đại diện
            int roleId = 2; // Giả sử roleID là 2 cho người dùng bình thường

            // Kiểm tra thông tin hợp lệ
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Tạo đối tượng User
                User newUser = new User
                {
                    Username = username,
                    Password = password,
                    Email = email,
                    Phone = phone,
                    Address = address,
                    Avatar = avatar,
                    RoleId = roleId
                };

                // Gọi service để tạo người dùng mới
                bool success = _userService.CreateUser(newUser);

                if (success)
                {
                    MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close(); // Đóng cửa sổ
                }
                else
                {
                    MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Sự kiện khi bấm nút Cancel
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Xóa tất cả các trường nhập liệu
            txtUsername.Clear();
            txtPassword.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            txtAvatar.Clear();
            imgAvatar.Source = null;
        }

        // Sự kiện khi bấm nút Close
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Sự kiện chọn ảnh đại diện
        private void UploadAvatarButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            if (openFileDialog.ShowDialog() == true)
            {
                txtAvatar.Text = openFileDialog.FileName; // Lưu đường dẫn ảnh vào TextBox
                imgAvatar.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(openFileDialog.FileName)); // Hiển thị ảnh lên Image
            }
        }
    }
}
