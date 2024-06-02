using Microsoft.AspNetCore.Identity;
using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;

namespace ThongFastFood_Api.Repositories.UserService
{
	public class AccountService : IAccountService
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly IConfiguration config;
		private readonly RoleManager<IdentityRole> roleManager;

		public AccountService(UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IConfiguration config, RoleManager<IdentityRole> roleManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.config = config;
			this.roleManager = roleManager;
		}

		public Task<string> LoginAsync(LoginVM model)
		{
			throw new NotImplementedException();
		}

		public async Task<IdentityResult> RegisterAsync(RegisterVM model)
		{
			var existingEmail = await userManager.FindByEmailAsync(model.Email);
			if (existingEmail != null)
			{
				var error = new IdentityError { Description = "Email đã tồn tại" };
				return IdentityResult.Failed(error);
			}

			var user = new ApplicationUser
			{
				FullName = model.FullName,
				UserName = model.Username,
				Email = model.Email,
				Address = model.Address,
				PhoneNumber = model.Phone
			};

			var result = await userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				if (!await roleManager.RoleExistsAsync("User"))
				{
					await roleManager.CreateAsync(new IdentityRole("User"));
				}

				// Assign the Admin role to the new user
				await userManager.AddToRoleAsync(user, "User");
			}
			return result;
		}
	}
}
