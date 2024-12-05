using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOs;
using DAL;

namespace DAOs
{
    public class RoleDAO
    {
        private static ChatBotDBcontext _context;
        private static RoleDAO instance = null;
        public RoleDAO() { _context = new ChatBotDBcontext(); }

        public static RoleDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RoleDAO();
                }
                return instance;
            }
        }


        public List<Role> GetAllRoles()
        {
            return _context.Roles.ToList();
        }

        public Role GetRoleById(int id)
        {
            try
            {
                return _context.Roles.SingleOrDefault(o => o.RoleId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public bool CreateRole(Role o)
        {
            bool isSucess = false;
            try
            {
                Role ca = _context.Roles.SingleOrDefault(c => c.RoleId == o.RoleId);
                if (ca == null)
                {
                    _context.Roles.Add(o);
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

        public bool UpdateRole(Role o)
        {
            bool isSucess = false;
            try
            {
                isSucess = true;
                _context.Roles.Update(o);
                _context.SaveChanges();
                _context.Entry(o).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isSucess;
        }

        public bool DeleteRole(int id)
        {
            bool isSucess = false;
            try
            {
                Role o = GetRoleById(id);
                if (o != null)
                {
                    _context.Roles.Remove(o);
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
