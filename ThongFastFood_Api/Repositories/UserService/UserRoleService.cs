using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;

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
        public async Task<IEnumerable<UserRolesVM>> GetAll()
        {
            var allUserRoles = await db.UserRoles.ToListAsync();
            var userRoleVM = new List<UserRolesVM>();

            foreach (var userRole in allUserRoles)
            {
                var user = await _userManager.FindByIdAsync(userRole.UserId);
                var role = await _roleManager.FindByIdAsync(userRole.RoleId);

                if (user != null && role != null)
                {
                    userRoleVM.Add(new UserRolesVM
                    {
                        UserId = userRole.UserId,
                        UserName = user.FullName, 
                        RoleId = userRole.RoleId,
                        RoleName = role.Name
                    });
                }
            }

            return userRoleVM;
        }

        public async Task<UserRolesVM> GetUserRolesByUserId(string id)
        {
            // Tìm hàng trong bảng dữ liệu UserRoles có UserId tương ứng
            var userRole = await db.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == id);

            if (userRole == null)
            {
                return null; // Trả về null nếu không tìm thấy
            }

            // Lấy RoleId của người dùng từ hàng đã tìm được
            var roleId = userRole.RoleId;

            // Sử dụng RoleId để tìm tên của vai trò trong bảng dữ liệu Roles
            var role = await db.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
            {
                return null; // Trả về null nếu không tìm thấy
            }

            // Trả về thông tin người dùng và vai trò
            return new UserRolesVM
            {
                UserId = userRole.UserId,
                RoleId = role.Id,
                RoleName = role.Name
            };
        }

        public async Task<UserRolesVM> UpdateUserRole(string id, UserRolesVM userRole)
        {
            // Kiểm tra xem RoleId có tồn tại không
            var role = await _roleManager.FindByIdAsync(userRole.RoleId);
            if (role == null)
            {
                return null; // Trả về null nếu không tìm thấy vai trò
            }

            // Tìm người dùng
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null; // Trả về null nếu không tìm thấy người dùng
            }

            // Tìm vai trò cũ của người dùng
            var oldRoles = await _userManager.GetRolesAsync(user);
            if (oldRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, oldRoles);
                if (!removeResult.Succeeded)
                {
                    return null; // Trả về null nếu không thể xóa vai trò cũ
                }
            }

            // Thêm người dùng vào vai trò mới
            var addResult = await _userManager.AddToRoleAsync(user, role.Name);
            if (!addResult.Succeeded)
            {
                return null; // Trả về null nếu không thể thêm người dùng vào vai trò mới
            }

            // Trả về thông tin người dùng và vai trò trong đối tượng UserRolesVM
            return new UserRolesVM
            {
                UserId = user.Id,
                UserName = user.FullName,
                RoleId = role.Id,
                RoleName = role.Name
            };
        }

    }
}
