using Microsoft.AspNetCore.Identity;
using ThongFastFood_Api.Models;

namespace ThongFastFood_Api.Repositories.UserService
{
	public interface IAccountService
	{
		public Task<IdentityResult> RegisterAsync(RegisterVM model);
		public Task<string> LoginAsync(LoginVM model);
	}
}
