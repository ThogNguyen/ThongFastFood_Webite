using Microsoft.AspNetCore.Identity;

namespace ThongFastFood_Api.Repositories.UserService
{
	public interface IRoleService
	{
		List<IdentityRole> GetRoles();
		Task<IdentityRole> AddRoleAsync(IdentityRole identityRole);
		Task<IdentityRole> UpdateRoleAsync(IdentityRole identityRole);
		Task<IdentityRole> DeleteRoleAsync(string roleId);
		Task<IdentityRole> GetIdentityRoleAsync(string id);
	}
}
