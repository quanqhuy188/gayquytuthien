
using AutoMapper;
using GayQuyTuThien.DataContext;
using GayQuyTuThien.DataContext.Entity;
using GayQuyTuThien.DTOs;
using GayQuyTuThien.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using static GayQuyTuThien.Constants.Permissions;

namespace GayQuyTuThien.Services
{
    public interface IFunctionService
    {
        Task<List<FunctionDto>> GetAllAsync();
        Task<List<FunctionDto>> GetByPermissionAsync(string roleId);
        int GetMaxLevel();
    }
    public class FunctionService : IFunctionService
    {
        private readonly DataDbContext _context;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public FunctionService(DataDbContext context, IRoleService roleService, IMapper mapper)
        {
            _context = context;
            _roleService = roleService;
            _mapper = mapper;
        }
        public async Task<List<FunctionDto>> GetAllAsync()
        {
            return _mapper.Map<List<FunctionDto>>(await _context.Functions.OrderBy(t => t.SortOrder).ToListAsync());
        }
        public async Task<List<FunctionDto>> GetByPermissionAsync(string roleId)
        {
            List<Function> result = new List<Function>();
            var claims = await _roleService.GetClaimsByRole(roleId);
            var claimsValue = claims.Select(t => t.Value).ToList();
            var functions = await _context.Functions.OrderBy(t => t.SortOrder).ToListAsync();
            foreach (var item in functions)
            {
                if (claimsValue.Contains($"{PermissionConstants.Prefix}.{item.Id}.{RoleAction.Read}"))
                {
                    result.Add(item);
                }
            }
            return _mapper.Map<List<FunctionDto>>(result);
        }
        public int GetMaxLevel()
        {
            return _context.Functions.Select(t => t.Level).ToList().Max();
        }
    }
}
