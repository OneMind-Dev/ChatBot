using BOs;
using Services.Implement;
using Services.Interface;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            ValidateField(textBox);
            UpdateCreateButtonState();
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox == null) return;

            bool isValid = !string.IsNullOrWhiteSpace(passwordBox.Password);
            passwordValidation.Visibility = isValid ? Visibility.Collapsed : Visibility.Visible;
            UpdateCreateButtonState();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            roleValidation.Visibility = cmbRole.SelectedValue == null ? Visibility.Visible : Visibility.Collapsed;
            UpdateCreateButtonState();
        }

        private void ValidateField(TextBox textBox)
        {
            string validationMessage = "";
            bool isValid = true;

            switch (textBox.Name)
            {
                case "txtUsername":
                    isValid = ValidationHelper.IsValidUsername(textBox.Text);
                    validationMessage = "Username is required";
                    usernameValidation.Visibility = isValid ? Visibility.Collapsed : Visibility.Visible;
                    break;

                case "txtEmail":
                    isValid = ValidationHelper.IsValidEmail(textBox.Text);
                    validationMessage = "Invalid email format";
                    emailValidation.Visibility = isValid ? Visibility.Collapsed : Visibility.Visible;
                    break;

                case "txtPhone":
                    isValid = ValidationHelper.IsValidPhone(textBox.Text);
                    validationMessage = "Phone must be exactly 10 digits";
                    phoneValidation.Visibility = isValid ? Visibility.Collapsed : Visibility.Visible;
                    break;

                case "txtAddress":
                    isValid = ValidationHelper.IsValidAddress(textBox.Text);
                    validationMessage = "Address is required";
                    addressValidation.Visibility = isValid ? Visibility.Collapsed : Visibility.Visible;
                    break;
            }

            textBox.ToolTip = !isValid ? validationMessage : null;
        }

        private void UpdateCreateButtonState()
        {
            btnCreate.IsEnabled = IsFormValid();
        }

        private bool IsFormValid()
        {
            return ValidationHelper.IsValidUsername(txtUsername.Text) &&
                   !string.IsNullOrWhiteSpace(txtPassword.Password) &&
                   ValidationHelper.IsValidEmail(txtEmail.Text) &&
                   ValidationHelper.IsValidPhone(txtPhone.Text) &&
                   ValidationHelper.IsValidAddress(txtAddress.Text) &&
                   cmbRole.SelectedValue != null;
        }

        private void Phone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            if (!IsFormValid()) return;

            var newUser = new User
            {
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Password.Trim(),
                Email = txtEmail.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                RoleId = (int)cmbRole.SelectedValue,
                Avatar = string.Empty
            };

            try
            {
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
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadRoles()
        {
            var roles = _roleService.GetAllRoles();
            cmbRole.ItemsSource = roles;
            cmbRole.DisplayMemberPath = "RoleName";
            cmbRole.SelectedValuePath = "RoleId";

            // Set default role to User (RoleId = 2)
            var defaultRole = roles.FirstOrDefault(r => r.RoleId == 2);
            if (defaultRole != null)
            {
                cmbRole.SelectedValue = defaultRole.RoleId;
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}