using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOs;
using Microsoft.EntityFrameworkCore;

namespace DAOs
{
    public class ConversationDAO
    {
        private static ChatBotDBcontext _context;

        private static ConversationDAO instance = null;
        public ConversationDAO() { _context = new ChatBotDBcontext(); }

        public static ConversationDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConversationDAO();
                }
                return instance;
            }
        }


        public List<Conversation> GetAllConversations()
        {
            return _context.Conversations.Include(c => c.Messages).ToList();
        }

        public List<Conversation> GetConversationsByUserId(int id)
        {
            return _context.Conversations
                .Where(c => c.UserId == id)
                .Include(c => c.Messages)
                .ToList();
        }

        public Conversation GetConversationById(int id)
        {
            try
            {
                return _context.Conversations
                    .Include(c => c.Messages)
                    .SingleOrDefault(o => o.ConversationId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public bool CreateConversation(Conversation o)
        {
            bool isSucess = false;
            try
            {
                Conversation ca = _context.Conversations.SingleOrDefault(c => c.ConversationId == o.ConversationId);
                if (ca == null)
                {
                    _context.Conversations.Add(o);
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

        public bool UpdateConversation(Conversation o)
        {
            bool isSucess = false;
            try
            {
                isSucess = true;
                _context.Conversations.Update(o);
                _context.SaveChanges();
                _context.Entry(o).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isSucess;
        }

        public bool DeleteConversation(int id)
        {
            bool isSucess = false;
            try
            {
                Conversation o = GetConversationById(id);
                if (o != null)
                {
                    _context.Conversations.Remove(o);
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
