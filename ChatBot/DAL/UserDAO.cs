using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOs;
using Microsoft.EntityFrameworkCore;

namespace DAOs
{
    public class UserDAO
    {
        private static ChatBotDBcontext _context;

        private static UserDAO instance = null;
        public UserDAO() { _context = new ChatBotDBcontext(); }

        public static UserDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserDAO();
                }
                return instance;
            }
        }

        public User LoginUser(string username, string password)
        {
            return _context.Users.SingleOrDefault(o => o.Username.Equals(username) && o.Password.Equals(password));
        }


        public List<User> GetAllUsers()
        {
            return _context.Users.Include(u => u.Role).ToList();
        }

        public User GetUserById(int id)
        {
            try
            {
                return _context.Users.SingleOrDefault(o => o.UserId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public User GetUserByEmail(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    throw new ArgumentException("Email cannot be null or empty");
                }

                return _context.Users.SingleOrDefault(o => o.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the user: " + ex.Message);
            }
        }


        public bool CreateUser(User o)
        {
            bool isSucess = false;
            try
            {
                User ca = _context.Users.SingleOrDefault(c => c.UserId == o.UserId);
                if (ca == null)
                {
                    _context.Users.Add(o);
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

        public bool UpdateUser(User o)
        {
            bool isSucess = false;
            try
            {
                isSucess = true;
                _context.Users.Update(o);
                _context.SaveChanges();
                _context.Entry(o).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isSucess;
        }

        public bool DeleteUser(int id)
        {
            bool isSucess = false;
            try
            {
                User o = GetUserById(id);
                if (o != null)
                {
                    _context.Users.Remove(o);
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
