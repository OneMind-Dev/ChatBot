using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Implement;
using Repositories.Interface;
using BOs;
using Services.Interface;

namespace Services.Implement
{
    public class MessageService : IMessageService
    {
        private User _user;
        private IMessageRepo _repo = null;
        private IConversationRepo _conversationRepo = null;
        private ChatBotCore _chatbot;

        public MessageService()
        {
            if (_repo == null)
            {
                _repo = new MessageRepo();
            }
            _conversationRepo ??= new ConversationRepo();
            _chatbot ??= new();
        }

        public MessageService(User user)
        {
            _user = user;
            _repo ??= new MessageRepo();
            _conversationRepo ??= new ConversationRepo();
            _chatbot ??= new();
        }

        public List<Message> GetAllMessages()
        {
            return _repo.GetAllMessages();
        }

        public Message GetMessageById(int id)
        {
            return _repo.GetMessageById(id);
        }

        public bool DeleteMessage(int id)
        {
            return _repo.DeleteMessage(id);
        }

        public bool UpdateMessage(Message o)
        {
            return _repo.UpdateMessage(o);
        }

        public bool CreateMessage(Message o)
        {
            return _repo.CreateMessage(o);
        }

        public async Task<string> SendMessage(string message)
        {
            if (_user == null)
            {
                throw new Exception("User not found");
            }

            string res = await _chatbot.SendMessageAsync(message);

            var conversation = new Conversation()
            {
                UserId = _user.UserId,
                ConservationName = message.Length > 11 ? message.Substring(0, 10) : message, // TODO: trty to improve this
                Messages = new List<Message>()
                {
                    new Message()
                    {
                        UserResquest= message,
                        ModelResponse= res,
                    }
                }
            };

            _conversationRepo.CreateConversation(conversation);

            return res;
        }

        public async Task<string> SendMessage(string message, int converesationId)
        {
            if (_user == null)
            {
                throw new Exception("User not found");
            }

            var conversation = _conversationRepo.GetConversationById(converesationId);

            if (conversation == null)
            {
                throw new Exception("Conversation not found");
            }

            _chatbot.History.AddRange(conversation.Messages);

            string res = await _chatbot.SendMessageAsync(message);

            conversation.Messages.Add(new Message()
            {
                UserResquest = message,
                ModelResponse = res,
            });

            _conversationRepo.UpdateConversation(conversation);

            return res;
        }
    }
}
