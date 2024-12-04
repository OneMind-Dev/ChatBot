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
    public class MessageRepo : IMessageRepo
    {
        public List<Message> GetAllMessages()
        => MessageDAO.Instance.GetAllMessages();

        public Message GetMessageById(int id)
        => MessageDAO.Instance.GetMessageById(id);

        public bool DeleteMessage(int id)
        => MessageDAO.Instance.DeleteMessage(id);

        public bool UpdateMessage(Message o)
        => MessageDAO.Instance.UpdateMessage(o);

        public bool CreateMessage(Message o)
        => MessageDAO.Instance.CreateMessage(o);
    }
}
