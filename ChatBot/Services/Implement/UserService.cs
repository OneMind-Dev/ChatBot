using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOs;
using Repositories.Implement;
using Repositories.Interface;
using Services.Interface;

namespace Services.Implement
{
    public class UserService : IUserService
    {

        private IUserRepo _repo = null;
        public UserService()
        {
            if (_repo == null)
            {
                _repo = new UserRepo();
            }
        }

        public User LoginUser(string username, string password)
        {
            return _repo.LoginUser(username, password);
        }

        public List<User> GetAllUsers()
        {
            return _repo.GetAllUsers();
        }

        public User GetUserById(int id)
        {
            return _repo.GetUserById(id);
        }

        public bool DeleteUser(int id)
        {
            return _repo.DeleteUser(id);
        }

        public bool UpdateUser(User o)
        {
            return _repo.UpdateUser(o);
        }

        public bool CreateUser(User o)
        {
            return _repo.CreateUser(o);
        }
    }
}
