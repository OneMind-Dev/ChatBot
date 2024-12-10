using BOs;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WPF
{
    public partial class ChatWindow : Window
    {
        private readonly User LoggedUser;
        public ObservableCollection<Message> Messages { get; set; }
        public ObservableCollection<string> Conversations { get; set; }

        public ChatWindow(User loggedUser)
        {
            InitializeComponent();
            LoggedUser = loggedUser ?? throw new ArgumentNullException(nameof(loggedUser));
            Messages = new ObservableCollection<Message>();
            Conversations = new ObservableCollection<string>(); // Danh sách hội thoại cũ
            ChatMessages.ItemsSource = Messages;
            ConversationsListBox.ItemsSource = Conversations; // Liên kết danh sách hội thoại
            UpdateUI();
            LoadConversations(); // Tải hội thoại cũ
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

        private void LoadConversations() //nội dung hiển thị với title tùy mấy ông setup cho nó
        {
            Conversations.Add("Conversation with Alice");
            Conversations.Add("Meeting Notes");
            Conversations.Add("Project Discussion");
            Conversations.Add("Chat with Bob");
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
                Avatar = "\\Icons\\message.png",
                AvatarVisibility = Visibility.Visible
            });
        }

        private void ConversationsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ConversationsListBox.SelectedItem is string selectedConversation)
            {
                MessageBox.Show($"Loading conversation: {selectedConversation}", "Load Conversation");
            }
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
