using AutoMapper;
using GayQuyTuThien.DataContext;
using GayQuyTuThien.DataContext.Entity;
using GayQuyTuThien.DTOs;
using GayQuyTuThien.Enums;
using GayQuyTuThien.Models;
using GayQuyTuThien.RequestViewModel;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net.WebSockets;
using System.Security.Claims;
using static GayQuyTuThien.Constants.Permissions;

namespace GayQuyTuThien.Services
{
    public interface IPermissionService
    {
        Task<List<PermissionDto>> GetByRoleAndFunction(string roleId);
        Task<PermissionDto> GetByIdAsync(string roleId, string id);
        Task<ResultModel<PermissionUpdateViewModel>> UpdateAsync(PermissionUpdateViewModel model);
    }
    public class PermissionService : IPermissionService
    {
        private readonly DataDbContext _context;
        private readonly IRoleService _roleService;

        public PermissionService(DataDbContext context, IRoleService roleService)
        {
            _context = context;
            _roleService = roleService;
        }
        public async Task<List<PermissionDto>> GetByRoleAndFunction(string roleId)
        {
            List<PermissionDto> result = new List<PermissionDto>();
            var role = await _roleService.GetByIdAsync(roleId);
            var functions = await _context.Functions.ToListAsync();
            var claims = await _roleService.GetClaimsByRole(roleId);
            var claimsValue = claims.Select(t => t.Value).ToList();

            var data = await (from function in _context.Functions
                              select new PermissionDto
                        {   
                            Id = $"{PermissionConstants.Prefix}.{function.Id}",
                            ApplicationRoleId = role.Id,
                            ApplicationRole = role,
                            FunctionId = function.Id,
                            Function = function,
                            CanRead = claimsValue.Contains($"{PermissionConstants.Prefix}.{function.Id}.{RoleAction.Read}") ? true : false,
                            CanCreate = claimsValue.Contains($"{PermissionConstants.Prefix}.{function.Id}.{RoleAction.Create}") ? true : false,
                            CanUpdate = claimsValue.Contains($"{PermissionConstants.Prefix}.{function.Id}.{RoleAction.Update}") ? true : false,
                            CanDelete = claimsValue.Contains($"{PermissionConstants.Prefix}.{function.Id}.{RoleAction.Delete}") ? true : false
                        }).OrderBy(t => t.Function.SortOrder).ToListAsync();
            return data;
        }
        public async Task<PermissionDto> GetByIdAsync(string roleId, string id)
        {
            var role = await _roleService.GetByIdAsync(roleId);
            var claims = await _roleService.GetClaimsByRole(roleId);
            var claimsValue = claims.Where(t=> t.Value.Contains(id+".")).Select(t => t.Value).ToList();
            return new PermissionDto
            {
                Id = id,
                ApplicationRoleId = role.Id,
                ApplicationRole = role,
                CanRead = claimsValue.Contains($"{id}.{RoleAction.Read}") ? true : false,
                CanCreate = claimsValue.Contains($"{id}.{RoleAction.Create}") ? true : false,
                CanUpdate = claimsValue.Contains($"{id}.{RoleAction.Update}") ? true : false,
                CanDelete = claimsValue.Contains($"{id}.{RoleAction.Delete}") ? true : false

            };
        }
        public async Task<ResultModel<PermissionUpdateViewModel>> UpdateAsync(PermissionUpdateViewModel model)
        {
            var role = await _roleService.GetByIdAsync(model.RoleId);
            if (role == null)
            {
                return new ResultModel<PermissionUpdateViewModel> { IsSuccess = false, ErrorMessenger = "RoleManagement not found" };
            }
            if (role.Name == Roles.Admin.ToString())
            {
                return new ResultModel<PermissionUpdateViewModel> { IsSuccess = false, ErrorMessenger = "RoleManagement is admin" };
            }
            var claims = await _roleService.GetClaimsByRole(role.Id);
            List<Function> functions = _context.Functions.ToList();
            var functionName = model.Id.Replace($"{PermissionConstants.Prefix}.", "");
            // UPDATE CHA => UPDATE CON
            await UpdateByParent(functionName, functions, role, model, claims);
			return new ResultModel<PermissionUpdateViewModel> {};
        }

        private async Task UpdateByParent(string functionName, List<Function> functions, ApplicationRole? role, PermissionUpdateViewModel model, IList<Claim> claims)
        {
			var claimByFunctions = claims.Where(t => t.Value.Contains($"{PermissionConstants.Prefix}.{functionName}.")).ToList();
			// DELTE CLAIMS
			foreach (var item in claimByFunctions)
			{
				await _roleService.RemoveClaimAsync(role.Id, item.Value);
			}
			if (model.CanRead)
			{
				await _roleService.AddClaimAsync(role.Id, $"{PermissionConstants.Prefix}.{functionName}.{RoleAction.Read}");
			}
			if (model.CanCreate)
			{
				await _roleService.AddClaimAsync(role.Id, $"{PermissionConstants.Prefix}.{functionName}.{RoleAction.Create}");
			}
			if (model.CanUpdate)
			{
				await _roleService.AddClaimAsync(role.Id, $"{PermissionConstants.Prefix}.{functionName}.{RoleAction.Update}");
			}
			if (model.CanDelete)
			{
				await _roleService.AddClaimAsync(role.Id, $"{PermissionConstants.Prefix}.{functionName}.{RoleAction.Delete}");
			}
			var functionByNames = functions.Where(t => t.ParentId == functionName).ToList();
			foreach (var function in functionByNames)
            {
                await UpdateByParent(function.Id, functions, role, model, claims);
			}
        }
    }
}
