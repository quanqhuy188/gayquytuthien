using GayQuyTuThien.Areas.Admin.Models;
using GayQuyTuThien.DataContext.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;

namespace GayQuyTuThien.Services
{
    public interface IAccountService
    {
        Task<int> Login(LoginViewModel model);
		Task Logout();
	}
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountService( UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }
        public async Task<int> Login(LoginViewModel model)
        {
            ApplicationUser? user = await _userManager.Users.FirstOrDefaultAsync(t => t.UserName == model.UserName);
            if (user == null)
            {
                return 0;
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                return 1;
            }
            return -1;
        }
		public async Task Logout()
		{
			await _signInManager.SignOutAsync();
		}
	}
}
