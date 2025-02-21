using GayQuyTuThien.DataContext.Entity;
using GayQuyTuThien.Enums;
using GayQuyTuThien.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static GayQuyTuThien.Constants.Permissions;

namespace GayQuyTuThien.Services
{
    public interface IRoleService
    {
        Task<List<ApplicationRole>> GetAll();
        Task<List<ApplicationRole>> GetAllNoAdmin();
        Task<ApplicationRole?> GetByIdAsync(string id);
        Task<ResultModel<ApplicationRole>> CreateAsync(ApplicationRole model);
        Task<ResultModel<ApplicationRole>> UpdateAsync(ApplicationRole model);
        Task<ResultModel<ApplicationRole>> DeleteAsync(string id);
        Task<IList<Claim>> GetClaimsByRole(string roleId);
        Task AddClaimAsync(string roleId, string permission);
        Task RemoveClaimAsync(string roleId, string permission);
    }
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        public RoleService(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager; 
        }
        public async Task<List<ApplicationRole>> GetAll()
        {
            return await _roleManager.Roles.AsNoTracking().ToListAsync();
        }
        public async Task<List<ApplicationRole>> GetAllNoAdmin()
        {
            return await _roleManager.Roles.Where(t=>t.Name != Roles.Admin.ToString()).AsNoTracking().ToListAsync();
        }
        public async Task<ApplicationRole?> GetByIdAsync(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }
        public async Task<ResultModel<ApplicationRole>> CreateAsync(ApplicationRole model)
        {
            var result = await _roleManager.CreateAsync(model);
            return new ResultModel<ApplicationRole> { IsSuccess = result.Succeeded };

        }
        public async Task<ResultModel<ApplicationRole>> UpdateAsync(ApplicationRole model)
        {
            var result = await _roleManager.UpdateAsync(model);
            return new ResultModel<ApplicationRole> { IsSuccess = result.Succeeded };
        }
        public async Task<ResultModel<ApplicationRole>> DeleteAsync(string id)
        {
            ApplicationRole role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return new ResultModel<ApplicationRole> { IsSuccess = false, ErrorMessenger = "RoleManagement not found" };
            }
            var result = await _roleManager.DeleteAsync(role);
            return new ResultModel<ApplicationRole> { IsSuccess = result.Succeeded };
        }
        public async Task<IList<Claim>> GetClaimsByRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            return await _roleManager.GetClaimsAsync(role);
        }
        public async Task AddClaimAsync(string roleId, string permission)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            await _roleManager.AddClaimAsync(role, new Claim(PermissionConstants.PermissionType, permission));
        }
        public async Task RemoveClaimAsync(string roleId, string permission)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            await _roleManager.RemoveClaimAsync(role, new Claim(PermissionConstants.PermissionType, permission));
        }
    }
}
