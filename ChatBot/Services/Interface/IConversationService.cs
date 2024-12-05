using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOs;

namespace Services.Interface
{
    public interface IConversationService
    {
        List<Conversation> GetAllConversations();

        Conversation GetConversationById(int id);

        bool DeleteConversation(int o);

        bool UpdateConversation(Conversation o);

        bool CreateConversation(Conversation o);
    }
}
