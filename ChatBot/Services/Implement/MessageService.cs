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
        private IMessageRepo _repo = null;
        public MessageService()
        {
            if (_repo == null)
            {
                _repo = new MessageRepo();
            }
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
    }
}
