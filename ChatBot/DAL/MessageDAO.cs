using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOs;

namespace DAOs
{
    public class MessageDAO
    {
        private static ChatBotDBcontext _context;

        private static MessageDAO instance = null;
        public MessageDAO() { _context = new ChatBotDBcontext(); }

        public static MessageDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MessageDAO();
                }
                return instance;
            }
        }

        public List<Message> GetAllMessages()
        {
            return _context.Messages.ToList();
        }

        public Message GetMessageById(int id)
        {
            try
            {
                return _context.Messages.SingleOrDefault(o => o.MessageId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public bool CreateMessage(Message o)
        {
            bool isSucess = false;
            try
            {
                Message ca = _context.Messages.SingleOrDefault(c => c.MessageId == o.MessageId);
                if (ca == null)
                {
                    _context.Messages.Add(o);
                    _context.SaveChanges();
                    _context.Entry(o).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    isSucess = true;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isSucess;
        }

        public bool UpdateMessage(Message o)
        {
            bool isSucess = false;
            try
            {
                isSucess = true;
                _context.Messages.Update(o);
                _context.SaveChanges();
                _context.Entry(o).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isSucess;
        }

        public bool DeleteMessage(int id)
        {
            bool isSucess = false;
            try
            {
                Message o = GetMessageById(id);
                if (o != null)
                {
                    _context.Messages.Remove(o);
                    _context.SaveChanges();
                    _context.Entry(o).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    isSucess = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isSucess;
        }
    }
}
