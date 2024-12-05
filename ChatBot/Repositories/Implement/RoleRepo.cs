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
    public class RoleRepo : IRoleRepo
    {
        public List<Role> GetAllRoles()
        => RoleDAO.Instance.GetAllRoles();

        public Role GetRoleById(int id)
        => RoleDAO.Instance.GetRoleById(id);

        public bool DeleteRole(int id)
        => RoleDAO.Instance.DeleteRole(id);

        public bool UpdateRole(Role o)
        => RoleDAO.Instance.UpdateRole(o);

        public bool CreateRole(Role o)
        => RoleDAO.Instance.CreateRole(o);
    }
}
