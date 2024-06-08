using Microsoft.EntityFrameworkCore;
using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;
using ThongFastFood_Api.Models.Response;

namespace ThongFastFood_Api.Repositories.UserService
{
	public class AccountService : IAccountService
	{
		private readonly FoodStoreDbContext db;

		public AccountService(FoodStoreDbContext context)
		{
			db = context;
		}

		public async Task<ResponseMessage> DeleteUserAsync(string id)
		{
			var user = await db.Users.FindAsync(id);
			if (user == null)
			{
				return new ResponseMessage { 
					IsSuccess = false, 
					Message = "Không tìm thấy người dùng." 
				};
			}

			db.Users.Remove(user);
			await db.SaveChangesAsync();

			return new ResponseMessage { 
				IsSuccess = true, 
				Message = "Xóa người dùng thành công." 
			};
		}

		public async Task<UserVM> GetUserByIdAsync(string id)
		{
			var user = await db.Users.FindAsync(id);
			if (user == null)
			{
				return null;
			}

			return new UserVM
			{
				Id = user.Id,
				FullName = user.FullName,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				Address = user.Address
			};
		}

		public async Task<List<UserVM>> GetUsersAsync()
		{
			var users = await db.Users.ToListAsync();
			return users.Select(user => new UserVM
			{
				Id = user.Id,
				FullName = user.FullName,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				Address = user.Address
			}).ToList();
		}

		public async Task<ResponseMessage> UpdateUserAsync(string id, UserVM userVM)
		{
			var user = await db.Users.FindAsync(id);
			if (user == null)
			{
				return new ResponseMessage { IsSuccess = false, Message = "Không tìm thấy người dùng." };
			}

			user.FullName = userVM.FullName;
			user.Email = userVM.Email;
			user.PhoneNumber = userVM.PhoneNumber;
			user.Address = userVM.Address;

			await db.SaveChangesAsync();

			return new ResponseMessage { IsSuccess = true, Message = "Cập nhật người dùng thành công." };
		}
	}
}
