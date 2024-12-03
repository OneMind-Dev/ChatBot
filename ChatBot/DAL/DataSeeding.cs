using BOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    internal class DataSeeding
    {
        public static List<User> Users = new List<User>()
        {
            new User()
            {
                UserId = 1,
                Username = "admin",
                Password = "admin",
                Email = "",
                Phone = "",
                Address = "",
                Avatar = "",
                RoleId = 1
            },
            new User() {
                UserId = 2,
                Username = "user",
                Password = "user",
                Email = "",
                Phone = "",
                Address = "",
                Avatar = "",
                RoleId = 2
            },
            new User() {
                UserId = 3,
                Username = "string",
                Password = "string",
                Email = "",
                Phone = "",
                Address = "",
                Avatar = "",
                RoleId = 2,
            },
            new User() {
                UserId = 4,
                Username = "1",
                Password = "1",
                Email = "",
                Phone = "",
                Address = "",
                Avatar = "",
                RoleId = 2,
            },
        };
        public static List<Role> Roles = new List<Role>()
        {
            new Role()
            {
                RoleId = 1,
                RoleName = "Admin"
            },
            new Role()
            {
                RoleId = 2,
                RoleName = "User"
            }
        };
        public static List<Conversation> Conversations = new List<Conversation>()
        {
            new Conversation()
            {
                ConversationId = 1,
                UserId = 2,
                ConservationName = "Conversation 1",
            },
        };

        public static List<Message> Messages = new List<Message>()
        {
            new Message() {
                MessageId = 1,
                ConservationId = 1,
                UserResquest = "Hello",
                ModelResponse = "Hi",
            },
            new Message () {
                MessageId = 2,
                ConservationId = 1,
                UserResquest = "How are you?",
                ModelResponse = "I'm fine, thank you",
            },
            new Message() {
                MessageId = 3,
                ConservationId = 1,
                UserResquest = "What is your name?",
                ModelResponse = "My name is ChatBot",
            },
        };
    }
}
