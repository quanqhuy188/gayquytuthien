using AutoMapper;
using GayQuyTuThien.DTOs;
using GayQuyTuThien.Extensions;
using GayQuyTuThien.RequestViewModel;
using GayQuyTuThien.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security;
using System.Security.Claims;

namespace GayQuyTuThien.Areas.Admin.Controllers
{
    [Authorize]
    [Area(nameof(Admin))]
    [Route(nameof(Admin) + "/[controller]")]
    public class PermissionController : Controller
    {
        private readonly IPermissionService _permissionService;
        private readonly IFunctionService _functionService;
        private readonly IRoleService _roleService;
        private readonly ILogger<PermissionController> _logger;
        private readonly IMapper _mapper;
        public PermissionController(IPermissionService permissionService, IFunctionService functionService, IRoleService roleService, ILogger<PermissionController> logger, IMapper mapper)
        {
            _permissionService = permissionService;
            _functionService = functionService;
            _roleService = roleService;
            _logger = logger;
            _mapper = mapper;

        }
        [Route("/admin/permission/permissions")]
        public async Task<IActionResult> Index(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                roleId = ((ClaimsPrincipal)User).GetSpecificClaim("RoleId");
            }
            var result = await _permissionService.GetByRoleAndFunction(roleId);
            ViewBag.MaxLevel = _functionService.GetMaxLevel();
            ViewBag.Roles = _mapper.Map<List<RoleDto>>(await _roleService.GetAll());
            ViewBag.CurrentRoleId = roleId;
            return View(result);
        }

        [Route("/admin/permission/getbyid")]
        [HttpPost]
        public async Task<IActionResult> GetById(string roleId, string id)
        {
            var data = await _permissionService.GetByIdAsync(roleId, id);
            if (data == null)
            {
                return NotFound();
            }
            return Json(new { status = true, data });
        }

        [HttpPut]
        public async Task<IActionResult> Update(PermissionUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "UPDATE PERMISSION FAILED. VALIDATE";
                return Json(new { status = false });
            }
            try
            {
                var result = await  _permissionService.UpdateAsync(model);
                return Json(new { status = result.IsSuccess, error = result.ErrorMessenger });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Permission Exception");
                TempData["error"] = "UPDATE PERMISSION FAILED";
                return Json(new { status = false });
            }
        }
    }
}
