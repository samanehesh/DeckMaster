using DECKMASTER.Data;
using DECKMASTER.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DECKMASTER.Repositories
{
    public class RoleRepo
    {
        private readonly ApplicationDbContext _db;

        public RoleRepo(ApplicationDbContext db)
        {
            this._db = db;
            CreateInitialRole();
        }

        public IEnumerable<RoleVM> GetAllRoles()
        {
            var roles =
                _db.Roles.Select(r => new RoleVM
                {
                    RoleName = r.Name
                });

            return roles;
        }

        public RoleVM GetRole(string roleName)
        {


            var role = _db.Roles.Where(r => r.Name == roleName).FirstOrDefault();

            if (role != null)
            {
                return new RoleVM() { RoleName = role.Name };
            }
            return null;
        }

        public bool CreateRole(string roleName)
        {
            bool isSuccess = true;

            try
            {
                _db.Roles.Add(new IdentityRole
                {
                    Id = roleName.ToLower(),
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                });
                _db.SaveChanges();
            }
            catch (Exception)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public SelectList GetRoleSelectList()
        {
            var roles = GetAllRoles().Select(r => new
            SelectListItem
            {
                Value = r.RoleName,
                Text = r.RoleName
            });

            var roleSelectList = new SelectList(roles, "Value","Text");
            return roleSelectList;
        }

        public void CreateInitialRole()
        {
            const string ADMIN = "Admin";

            var role = GetRole(ADMIN);

            if (role == null)
            {
                CreateRole(ADMIN);
            }
        }


        public string DeleteRole(string roleId)
        {
            string message = string.Empty;

            try
            {
                var roleToDelete = _db.Roles.Where(u=>u.Name == roleId).FirstOrDefault();
                var count = _db.UserRoles.Count(ur => ur.RoleId.ToLower() == roleId.ToLower());
                if (roleToDelete != null)
                {
                    if (count == 0)
                    {
                        _db.Roles.Remove(roleToDelete);
                        _db.SaveChanges();
                        return $"Role {roleId} deleted successfully."; // Deletion successful

                    }
                    else
                    {
                        return $"Error! {roleId} role is assigned to a user, Can'd be deleted.";
                    }

                }
                else
                {
                    return $"Error! {roleId} Role Can't be deleted"; // Role with the specified ID not found
                }
            }
            catch (Exception)
            {
                return "Error! This role Can't be deleted";
            }

        }
    }
}


