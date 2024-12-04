using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOs;

namespace Services.Interface
{
    public interface IRoleService
    {
        List<Role> GetAllRoles();

        Role GetRoleById(int id);

        bool DeleteRole(int o);

        bool UpdateRole(Role o);

        bool CreateRole(Role o);
    }
}
