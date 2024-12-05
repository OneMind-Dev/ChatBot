using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOs;
using DAOs;
using Repositories.Interface;

namespace Repositories.Implement
{
    public class ConversationRepo : IConversationRepo
    {

        public List<Conversation> GetAllConversations()
        => ConversationDAO.Instance.GetAllConversations();

        public Conversation GetConversationById(int id)
        => ConversationDAO.Instance.GetConversationById(id);

        public bool DeleteConversation(int id)
        => ConversationDAO.Instance.DeleteConversation(id);

        public bool UpdateConversation(Conversation o)
        => ConversationDAO.Instance.UpdateConversation(o);

        public bool CreateConversation(Conversation o)
        => ConversationDAO.Instance.CreateConversation(o);
    }
}
