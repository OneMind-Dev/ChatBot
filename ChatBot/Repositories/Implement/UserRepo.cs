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
    public class UserRepo : IUserRepo
    {
        public User LoginUser(string username, string password)
        => UserDAO.Instance.LoginUser( username, password);
        public List<User> GetAllUsers()
        => UserDAO.Instance.GetAllUsers();

        public User GetUserById(int id)
        => UserDAO.Instance.GetUserById(id);

        public User GetUserByEmail(string email)
       => UserDAO.Instance.GetUserByEmail(email);

        public bool DeleteUser(int id)
        => UserDAO.Instance.DeleteUser(id);

        public bool UpdateUser(User o)
        => UserDAO.Instance.UpdateUser(o);

        public bool CreateUser(User o)
        => UserDAO.Instance.CreateUser(o);
    }
}
