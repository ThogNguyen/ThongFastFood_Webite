using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ThongFastFood_Api.Data;
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ThongFastFood_Client.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

		public LoginModel(SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }


        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Tên đăng nhập không được bỏ trống")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
            [StringLength(100, ErrorMessage = "Mật khẩu phải từ {2} đến {1} ký tự", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            //nếu đã đăng nhập sẽ tự về index khi vào login lại
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("/");
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
			

			returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
					var user = await _userManager.FindByNameAsync(Input.Username);

					var claims = new List<Claim>
	                {
	                	new Claim(ClaimTypes.NameIdentifier, user.Id),
						new Claim(ClaimTypes.Name, user.UserName),
						new Claim("FullName", user.FullName),
                        // Các claim khác bạn muốn thêm vào đây
                    };

					// Lấy các role của user 
					var userRoles = await _userManager.GetRolesAsync(user);

					// Thêm claim role của role user đó
					foreach (var role in userRoles)
					{
						claims.Add(new Claim(ClaimTypes.Role, role));
					}

					var claimsIdentity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
					var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);  

					await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, claimsPrincipal);

					_logger.LogInformation("Đăng nhập thành công!.");
					return LocalRedirect(returnUrl);
				}
                else
                {
                    ModelState.AddModelError(string.Empty, "*Thông tin đăng nhập không hợp lệ.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
