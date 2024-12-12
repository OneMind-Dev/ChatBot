using BOs;
using Services.Implement;
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
        private Conversation _currentConversation;
        private readonly User LoggedUser;
        private MessageService _messageService;
        private ConversationService _conversationService;
        public ObservableCollection<Message> Messages { get; set; }
        public ObservableCollection<Conversation> Conversations { get; set; }

        public ChatWindow(User loggedUser)
        {
            InitializeComponent();
            LoggedUser = loggedUser ?? throw new ArgumentNullException(nameof(loggedUser));
            _messageService = new MessageService(loggedUser);
            _conversationService = new ConversationService(loggedUser);

            UpdateUI();

            Messages = new ObservableCollection<Message>();
            ChatMessages.ItemsSource = Messages;

            Conversations = new ObservableCollection<Conversation>(); // Danh sách hội thoại cũ
            ConversationsListBox.ItemsSource = Conversations; // Liên kết danh sách hội thoại

            LoadUserConversations();
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

        private void LoadUserConversations() //nội dung hiển thị với title tùy mấy ông setup cho nó
        {
            Conversations.Clear();

            var list = _conversationService.GetConversationsByUserId(LoggedUser.UserId);
            foreach (var item in list)
            {
                Conversations.Add(item);
            }
        }

        private void LoadConversation()
        {
            Messages.Clear();

            foreach (var item in _currentConversation?.Messages)
            {
                AddUserRequest(item.UserResquest);
                AddAIResponse(item.ModelResponse);
            }
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
            // dùng dialog để dừng luồng và có thể kiểm tra
            UserProfileWindow userProfileWindow = new UserProfileWindow(LoggedUser);
            userProfileWindow.ShowDialog();
            if (userProfileWindow.LoggedUser == null)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            UpdateUI();
        }

        private void NewChatButton_Click(object sender, RoutedEventArgs e)
        {
            _currentConversation = null;
            Messages.Clear();
        }

        private async void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            string userMessage = ChatInputBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(userMessage))
            {
                AddUserRequest(userMessage);

                ChatInputBox.Clear();

                if (_currentConversation == null)
                {
                    string res = await _messageService.SendMessage(userMessage);
                    AddAIResponse(res);

                    // Refresh Conversations sau khi bắt đầu conversation mới
                    LoadUserConversations();
                }
                else
                {
                    string res = await _messageService.SendMessage(userMessage, _currentConversation.ConversationId);
                    AddAIResponse(res);
                }
            }
        }

        private void ConversationsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ConversationsListBox.SelectedItem is Conversation selectedConversation)
            {
                _currentConversation = selectedConversation;
                LoadConversation();
            }
        }

        private void AddUserRequest(string userMessage)
        {
            Messages.Add(new Message
            {
                Text = userMessage,
                BackgroundColor = "#5865F2",
                HorizontalAlignment = HorizontalAlignment.Right,
                Avatar = LoggedUser.Avatar,
                AvatarVisibility = Visibility.Collapsed
            });
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

        private void TextBlock_EnterSent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessageButton_Click(sender, e);
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
