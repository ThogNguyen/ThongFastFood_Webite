using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThongFastFood_Api.Data;

namespace ThongFastFood_Api.Repositories.UserService
{
	public class RoleService : IRoleService
	{
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly UserManager<ApplicationUser> userManager;

		public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
		{
			this.roleManager = roleManager;
			this.userManager = userManager;
		}

		public async Task<IdentityRole> AddRoleAsync(IdentityRole identityRole)
		{
			if (!await roleManager.RoleExistsAsync(identityRole.Name))
			{
				var result = await roleManager.CreateAsync(new IdentityRole(identityRole.Name));
				if (result.Succeeded)
				{
					return identityRole;
				}
				return identityRole;
			}
			return null;
		}

		public async Task<IdentityRole> DeleteRoleAsync(string roleId)
		{
			var role = await roleManager.FindByIdAsync(roleId);
			if (role != null)
			{
				var result = await roleManager.DeleteAsync(role);
				if (result.Succeeded)
				{
					return role;
				}
			}
			return null;
		}

		public async Task<IdentityRole> GetIdentityRoleAsync(string roleId)
		{
			var role = await roleManager.FindByIdAsync(roleId);
			if (role == null)
			{
				return null;
			}
			return role;
		}

		public List<IdentityRole> GetRoles()
		{
			return roleManager.Roles.ToList();
		}

		public async Task<IdentityRole> UpdateRoleAsync(IdentityRole identityRole)
		{
			var role = await roleManager.FindByIdAsync(identityRole.Id);
			if (role != null)
			{
				role.Name = identityRole.Name;
				var result = await roleManager.UpdateAsync(role);
				if (result.Succeeded)
				{
					return role;
				}
				return null;
			}
			return null;
		}
	}
}
