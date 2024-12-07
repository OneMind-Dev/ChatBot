using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOs;

namespace Services.Interface
{
    public interface IUserService
    {
        User LoginUser(string username, string password);

        List<User> GetAllUsers();

        User GetUserById(int id);

        User GetUserByEmail(string email);

        bool DeleteUser(int o);

        bool UpdateUser(User o);

        bool CreateUser(User o);
    }
}
