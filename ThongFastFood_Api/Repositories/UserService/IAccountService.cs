using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;
using ThongFastFood_Api.Models.Response;

namespace ThongFastFood_Api.Repositories.UserService
{
    public interface IAccountService
    {
		Task<List<UserVM>> GetUsersAsync();
		Task<UserVM> GetUserByIdAsync(string id);
		Task<ResponseMessage> UpdateUserAsync(string id, UserVM userVM);
		Task<ResponseMessage> DeleteUserAsync(string id);
	}
}
