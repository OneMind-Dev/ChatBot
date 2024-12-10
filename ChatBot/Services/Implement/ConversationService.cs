using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Interface;
using BOs;
using Repositories.Implement;
using Services.Interface;

namespace Services.Implement
{
    public class ConversationService : IConversationService
    {
        private IConversationRepo _repo = null;
        private User _user;
        public ConversationService()
        {
            if (_repo == null)
            {
                _repo = new ConversationRepo();
            }
        }

        public ConversationService(User user)
        {
            _user = user;
            _repo ??= new ConversationRepo();
        }

        public List<Conversation> GetAllConversations()
        {
            return _repo.GetAllConversations();
        }

        public List<Conversation> GetConversationsByUserId(int id)
        {
            return _repo.GetConversationsByUserId(id);
        }

        public Conversation GetConversationById(int id)
        {
            return _repo.GetConversationById(id);
        }

        public bool DeleteConversation(int id)
        {
            return _repo.DeleteConversation(id);
        }

        public bool UpdateConversation(Conversation o)
        {
            return _repo.UpdateConversation(o);
        }

        public bool CreateConversation(Conversation o)
        {
            return _repo.CreateConversation(o);
        }

    }
}
