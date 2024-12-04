using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOs;

namespace Repositories.Interface
{
    public interface IMessageRepo
    {
        List<Message> GetAllMessages();

        Message GetMessageById(int id);

        bool DeleteMessage(int o);

        bool UpdateMessage(Message o);

        bool CreateMessage(Message o);
    }
}
