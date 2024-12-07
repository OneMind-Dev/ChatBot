using BOs;
using Services.Implement;
using Services.Interface;
using System.Windows;
using System.Windows.Controls;

namespace WPF
{
    public partial class AdminWindow : Window
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private User? selectedUser;

        public AdminWindow()
        {
            InitializeComponent();
            _userService = new UserService();
            _roleService = new RoleService();
            LoadData();
        }

        private void LoadData()
        {
            // Load Users
            UsersGrid.ItemsSource = _userService.GetAllUsers();

            // Load Roles for ComboBox
            cmbRole.ItemsSource = _roleService.GetAllRoles();
            cmbRole.DisplayMemberPath = "RoleName";
            cmbRole.SelectedValuePath = "RoleId";
        }

        private void ClearForm()
        {
            txtUsername.Text = string.Empty;
            txtPassword.Password = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtAddress.Text = string.Empty;
            cmbRole.SelectedIndex = -1;
            selectedUser = null;
        }

        private void UsersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser = UsersGrid.SelectedItem as User;
            if (selectedUser != null)
            {
                txtUsername.Text = selectedUser.Username;
                txtEmail.Text = selectedUser.Email;
                txtPhone.Text = selectedUser.Phone;
                txtAddress.Text = selectedUser.Address;
                cmbRole.SelectedValue = selectedUser.RoleId;
            }
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            var createUserWindow = new CreateUserWindow();
            if (createUserWindow.ShowDialog() == true)
            {
                // Refresh the users grid after successful creation
                LoadData();
            }
        }

        private void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Please select a user to update!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            selectedUser.Username = txtUsername.Text;
            if (!string.IsNullOrEmpty(txtPassword.Password))
                selectedUser.Password = txtPassword.Password;
            selectedUser.Email = txtEmail.Text;
            selectedUser.Phone = txtPhone.Text;
            selectedUser.Address = txtAddress.Text;
            selectedUser.RoleId = (int)cmbRole.SelectedValue;

            if (_userService.UpdateUser(selectedUser))
            {
                MessageBox.Show("User updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
                ClearForm();
            }
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Please select a user to delete!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (_userService.DeleteUser(selectedUser.UserId))
                {
                    MessageBox.Show("User deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                    ClearForm();
                }
            }
        }

        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }
    }
}