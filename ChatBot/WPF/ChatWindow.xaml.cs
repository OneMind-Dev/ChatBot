using BOs;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WPF
{
    public partial class ChatWindow : Window
    {
        private readonly User LoggedUser;
        public ObservableCollection<Message> Messages { get; set; }

        public ChatWindow(User loggedUser)
        {
            InitializeComponent();
            LoggedUser = loggedUser ?? throw new ArgumentNullException(nameof(loggedUser));
            Messages = new ObservableCollection<Message>();
            ChatMessages.ItemsSource = Messages;
            UpdateUI();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to UserProfileWindow
            UserProfileWindow userProfileWindow = new UserProfileWindow(LoggedUser);
            userProfileWindow.Show();
        }

        private void UpdateUI()
        {
            UsernameLabel.Text = LoggedUser.Username;
            UserEmailLabel.Text = LoggedUser.Email ?? "No email provided";

            if (!string.IsNullOrWhiteSpace(LoggedUser.Avatar))
            {
                UserAvatar.Source = new BitmapImage(new Uri(LoggedUser.Avatar, UriKind.RelativeOrAbsolute));
                UserAvatar.Visibility = Visibility.Visible;
            }
            else
            {
                UserAvatar.Visibility = Visibility.Collapsed;
            }
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            string userMessage = ChatInputBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(userMessage))
            {
                Messages.Add(new Message
                {
                    Text = userMessage,
                    BackgroundColor = "#5865F2",
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Avatar = null,
                    AvatarVisibility = Visibility.Collapsed
                });

                ChatInputBox.Clear();

                AddAIResponse("I'm here to help!");
            }
        }

        private void AddAIResponse(string aiResponse)
        {
            Messages.Add(new Message
            {
                Text = aiResponse,
                BackgroundColor = "#3C3F41",
                HorizontalAlignment = HorizontalAlignment.Left,
                Avatar = "path_to_ai_avatar.png", // Replace with actual image path
                AvatarVisibility = Visibility.Visible
            });
        }
    }

    public class Message
    {
        public string Text { get; set; }
        public string BackgroundColor { get; set; }
        public HorizontalAlignment HorizontalAlignment { get; set; }
        public string Avatar { get; set; }
        public Visibility AvatarVisibility { get; set; }
    }
}
