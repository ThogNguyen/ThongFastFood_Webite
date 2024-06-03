using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using ThongFastFood_Api.Data;

namespace ThongFastFood_Client.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ILogger<RegisterModel> _logger;
		private readonly RoleManager<IdentityRole> _roleManager;

		public RegisterModel(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			ILogger<RegisterModel> logger,
			RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
			_roleManager = roleManager;
		}


		[BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

		public class InputModel
		{
			[Required(ErrorMessage = "Email không được bỏ trống")]
			[EmailAddress(ErrorMessage = "Định dạng email không hợp lệ")]
			public string Email { get; set; }

			[Required(ErrorMessage = "Họ và tên không được bỏ trống")]
			public string FullName { get; set; }

			[Required(ErrorMessage = "Tên đăng nhập không được bỏ trống")]
			public string Username { get; set; }

			[Required(ErrorMessage = "Địa chỉ không được bỏ trống")]
			public string Address { get; set; }

			[Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
			[RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại không hợp lệ")]
			public string Phone { get; set; }

			[Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
			[StringLength(100, ErrorMessage = "Mật khẩu phải từ {2} đến {1} ký tự", MinimumLength = 6)]
			[DataType(DataType.Password)]
			public string Password { get; set; }

            [Required(ErrorMessage = "Xác nhận mật khẩu không được bỏ trống")]
            [DataType(DataType.Password)]
			[Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp")]
			public string ConfirmPassword { get; set; }
		}


		public async Task OnGetAsync(string returnUrl = null)
		{
			//nếu đã đăng nhập sẽ tự về index khi vào register lại
			if (User.Identity.IsAuthenticated)
			{
				Response.Redirect("/");
			}

			ReturnUrl = returnUrl;
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
		}

		
		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl ??= Url.Content("~/");
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser
				{
					UserName = Input.Username,
					Email = Input.Email,
					FullName = Input.FullName,
					PhoneNumber = Input.Phone,
					Address = Input.Address
				};

				// Kiểm tra xem vai trò "User" đã tồn tại chưa
				var userRole = await _roleManager.FindByNameAsync("User");
				if (userRole == null)
				{
					// Nếu vai trò "User" chưa tồn tại, tạo mới
					userRole = new IdentityRole("User");
					var roleResult = await _roleManager.CreateAsync(userRole);
					if (!roleResult.Succeeded)
					{
						foreach (var error in roleResult.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
						return Page();
					}
				}

				// Thêm vai trò "User" cho người dùng
				var result = await _userManager.CreateAsync(user, Input.Password);
				if (result.Succeeded)
				{
					// Gán vai trò cho người dùng
					var roleAssignResult = await _userManager.AddToRoleAsync(user, userRole.Name);
					if (!roleAssignResult.Succeeded)
					{
						foreach (var error in roleAssignResult.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
						return Page();
					}

					_logger.LogInformation("User created a new account with password.");

					if (_userManager.Options.SignIn.RequireConfirmedAccount)
					{
						return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
					}
					else
					{
						await _signInManager.SignInAsync(user, isPersistent: false);
						return LocalRedirect(returnUrl);
					}
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			// If we got this far, something failed, redisplay form
			return Page();
		}

	}
}
