using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThongFastFood_Api.Repositories.UserService
{
    public interface IUserRoleService
    {
        Task<IEnumerable<IdentityUserRole<string>>> GetAllAsync();
        Task<bool> UpdateAsync(IdentityUserRole<string> userRole);
    }
}
