using GayQuyTuThien.DataContext.Entity;
using GayQuyTuThien.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GayQuyTuThien.Services
{
	public interface IUserService
	{
		Task<IQueryable<ApplicationUser>> GetAll(string search);
        Task<ApplicationUser?> GetUserByIdAsync(string id, ClaimsPrincipal claimsPrincipal = null);
        Task<ResultModel<ApplicationUser>> CreateAsync(ApplicationUser model, string password);
        Task<ResultModel<ApplicationUser>> UpdateAsync(ApplicationUser model);
        Task<bool> DeleteAsync(string id);

    }
	public class UserService : IUserService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<ApplicationRole> _roleManager;
        public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
			_userManager = userManager;
			_roleManager = roleManager;
		}
        public async Task<IQueryable<ApplicationUser>> GetAll(string search)
		{
            var result = _userManager.Users.Where(t=>t.ApplicationRole.Name != "Admin").Include(x => x.ApplicationRole).OrderByDescending(t => t.CreatedDate).AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                result = result.Where(t => t.FullName.Contains(search) || t.UserName.Contains(search));
            }
            return result;
        }
        
        public async Task<ApplicationUser?> GetUserByIdAsync(string id, ClaimsPrincipal claimsPrincipal = null)
        {
            if (claimsPrincipal != null)
            {
                var currentUser = await _userManager.GetUserAsync(claimsPrincipal);
                if (currentUser != null)
                {
                    id = currentUser.Id;
				}
			}
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<ResultModel<ApplicationUser>> CreateAsync(ApplicationUser model, string password)
        {
            ApplicationUser exists;
            exists = await _userManager.FindByEmailAsync(model.Email);
            if (exists != null)
            {
                return new ResultModel<ApplicationUser> { IsSuccess = false, ErrorMessenger = "Email is alrealy" };
            }
            exists = await _userManager.FindByNameAsync(model.UserName);
            if (exists != null)
            {
                return new ResultModel<ApplicationUser> { IsSuccess = false, ErrorMessenger = "Username is alrealy", Data = model };
            }
            await _userManager.SetEmailAsync(model, model.Email);
            model.CreatedDate = DateTime.Now;
            var result = await _userManager.CreateAsync(model, password);
            if (result.Succeeded)
            {
                var applicationRole = await _roleManager.FindByIdAsync(model.ApplicationRoleId);
                if (applicationRole != null)
                {
                    await _userManager.AddToRoleAsync(model, applicationRole.Name);
                    return new ResultModel<ApplicationUser> {};
                }
            }
            return new ResultModel<ApplicationUser> { IsSuccess = false };
        }

        public async Task<ResultModel<ApplicationUser>> UpdateAsync(ApplicationUser model)
        {
            var result = await _userManager.UpdateAsync(model);
            if (result.Succeeded)
            {
                await _userManager.SetEmailAsync(model, model.Email);
                var applicationRole = await _roleManager.FindByIdAsync(model.ApplicationRoleId);
                if (applicationRole != null)
                {
                    var roles = await _userManager.GetRolesAsync(model);
                    await _userManager.RemoveFromRolesAsync(model, roles);
                    await _userManager.AddToRoleAsync(model, applicationRole.Name);
                    return new ResultModel<ApplicationUser>();
                }
            }
            return new ResultModel<ApplicationUser> { IsSuccess = false };
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
