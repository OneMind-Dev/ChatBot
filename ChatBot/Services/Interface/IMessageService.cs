using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOs;

namespace Services.Interface
{
    public interface IMessageService
    {
        List<Message> GetAllMessages();

        Message GetMessageById(int id);

        bool DeleteMessage(int o);

        bool UpdateMessage(Message o);

        bool CreateMessage(Message o);
    }
}
