using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOs;

namespace Repositories.Interface
{
    public interface IUserRepo
    {
        User LoginUser(string username, string password);

        List<User> GetAllUsers();

        User GetUserById(int id);

        bool DeleteUser(int o);

        bool UpdateUser(User o);

        bool CreateUser(User o);
    }
}
