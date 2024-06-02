using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ThongFastFood_Client.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class RoleManagerController : Controller
    {
        public IActionResult RoleManager()
        {
            return View();
        }
        public IActionResult EditRoleUser()
        {
            return View();
        }
    }
}
