using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ThongFastFood_Api.Data;

namespace ThongFastFood_Api.Repositories.UserService
{
    public class UserRoleService : IUserRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly FoodStoreDbContext db;

        public UserRoleService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, FoodStoreDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            db = context;
        }
        public async Task<IEnumerable<IdentityUserRole<string>>> GetAllAsync()
        {
            var allUserRoles = await db.UserRoles.ToListAsync();

            return allUserRoles;
        }


        public async Task<bool> UpdateAsync(IdentityUserRole<string> userRole)
        {
            // Kiểm tra xem RoleId có tồn tại không
            var role = await _roleManager.FindByIdAsync(userRole.RoleId);
            if (role == null)
            {
                return false; // Nếu không tìm thấy vai trò, trả về false
            }

            // Tìm người dùng
            var user = await _userManager.FindByIdAsync(userRole.UserId);
            if (user == null)
            {
                return false; // Nếu không tìm thấy người dùng, trả về false
            }

            // Tìm vai trò cũ của người dùng
            var oldRole = await _userManager.GetRolesAsync(user);
            if (oldRole != null && oldRole.Any())
            {
                // Xóa người dùng khỏi vai trò cũ
                var removeResult = await _userManager.RemoveFromRoleAsync(user, oldRole.First());
                if (!removeResult.Succeeded)
                {
                    return false; // Nếu không xóa được khỏi vai trò cũ, trả về false
                }
            }

            // Thêm người dùng vào vai trò mới
            var addResult = await _userManager.AddToRoleAsync(user, role.Name);
            if (!addResult.Succeeded)
            {
                return false; // Nếu không thêm được vào vai trò mới, trả về false
            }

            return true; // Trả về true nếu cập nhật thành công

        }
    }
}
