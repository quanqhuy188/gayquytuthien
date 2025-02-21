using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GayQuyTuThien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin" + "/[controller]")]
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
