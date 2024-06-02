using Microsoft.AspNetCore.Mvc;

namespace ThongFastFood_Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        public IActionResult Role()
        {
            return View();
        }
        public IActionResult CreateRole()
        {
            return View();
        }
        public IActionResult EditRole()
        {
            return View();
        }
    }
}
