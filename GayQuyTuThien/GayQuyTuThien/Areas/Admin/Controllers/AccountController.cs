using GayQuyTuThien.Areas.Admin.Models;
using GayQuyTuThien.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GayQuyTuThien.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin" + "/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
           
        }
        [HttpGet("/login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("/login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _accountService.Login(model);
            if (result != 1)
            {
                ModelState.AddModelError("error", "Login failed");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
		[Route("/logout")]
		public async Task<IActionResult> Logout()
		{
			await _accountService.Logout();
			return RedirectToAction("Login", "Account");
		}
	}
}
