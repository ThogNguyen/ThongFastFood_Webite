using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThongFastFood_Api.Models;

namespace ThongFastFood_Api.Repositories.UserService
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRolesVM>> GetAll();
        Task<UserRolesVM> UpdateUserRole(string id, UserRolesVM userRole);
        Task<UserRolesVM> GetUserRolesByUserId(string id);
    }
}
