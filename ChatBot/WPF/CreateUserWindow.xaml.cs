using BOs;
using Services.Implement;
using Services.Interface;
using System.Windows;

namespace WPF
{
    public partial class CreateUserWindow : Window
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public CreateUserWindow()
        {
            InitializeComponent();
            _userService = new UserService();
            _roleService = new RoleService();
            LoadRoles();
        }

        private void LoadRoles()
        {
            var roles = _roleService.GetAllRoles();
            cmbRole.ItemsSource = roles;
            cmbRole.DisplayMemberPath = "RoleName";
            cmbRole.SelectedValuePath = "RoleId";
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                var newUser = new User
                {
                    Username = txtUsername.Text,
                    Password = txtPassword.Password,
                    Email = txtEmail.Text,
                    Phone = txtPhone.Text,
                    Address = txtAddress.Text,
                    RoleId = (int)cmbRole.SelectedValue,
                    Avatar = string.Empty
                };

                if (_userService.CreateUser(newUser))
                {
                    MessageBox.Show("User created successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Failed to create user.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Username is required.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Password is required.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (cmbRole.SelectedValue == null)
            {
                MessageBox.Show("Please select a role.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}