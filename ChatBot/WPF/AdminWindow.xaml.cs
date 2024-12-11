using BOs;
using Services.Implement;
using Services.Interface;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF
{
    public partial class AdminWindow : Window
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private User? selectedUser;
        private bool isDirty = false;
        private Dictionary<string, string> originalValues = new Dictionary<string, string>();
        public AdminWindow()
        {
            InitializeComponent();
            _userService = new UserService();
            _roleService = new RoleService();
            LoadData();

            btnUpdate.IsEnabled = false;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (selectedUser == null) return;

            var textBox = sender as TextBox;
            if (textBox == null) return;

            ValidateField(textBox);
            CheckDirtyState();
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

            // Set validation tooltip
            textBox.ToolTip = !isValid ? validationMessage : null;
        }

        private void CheckDirtyState()
        {
            if (selectedUser == null) return;

            isDirty = txtUsername.Text != originalValues["Username"] ||
                     txtEmail.Text != originalValues["Email"] ||
                     txtPhone.Text != originalValues["Phone"] ||
                     txtAddress.Text != originalValues["Address"] ||
                     cmbRole.SelectedValue?.ToString() != originalValues["RoleId"];

            // Enable update button only if form is dirty and all fields are valid
            btnUpdate.IsEnabled = isDirty && IsFormValid();
        }

        private bool IsFormValid()
        {
            return ValidationHelper.IsValidUsername(txtUsername.Text) &&
                   ValidationHelper.IsValidEmail(txtEmail.Text) &&
                   ValidationHelper.IsValidPhone(txtPhone.Text) &&
                   ValidationHelper.IsValidAddress(txtAddress.Text) &&
                   cmbRole.SelectedValue != null;
        }

        private void Phone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private void UsersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser = UsersGrid.SelectedItem as User;
            if (selectedUser != null)
            {
                // Store original values
                originalValues["Username"] = selectedUser.Username;
                originalValues["Email"] = selectedUser.Email;
                originalValues["Phone"] = selectedUser.Phone;
                originalValues["Address"] = selectedUser.Address;
                originalValues["RoleId"] = selectedUser.RoleId.ToString();

                // Set form values
                txtUsername.Text = selectedUser.Username;
                txtEmail.Text = selectedUser.Email;
                txtPhone.Text = selectedUser.Phone;
                txtAddress.Text = selectedUser.Address;
                cmbRole.SelectedValue = selectedUser.RoleId;

                isDirty = false;
                btnUpdate.IsEnabled = false;
            }
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
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtAddress.Text = string.Empty;
            cmbRole.SelectedIndex = -1;
            selectedUser = null;
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

        private void cmbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}