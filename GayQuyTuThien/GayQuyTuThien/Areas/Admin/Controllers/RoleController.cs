using AutoMapper;
using GayQuyTuThien.DataContext.Entity;
using GayQuyTuThien.DTOs;
using GayQuyTuThien.RequestViewModel;
using GayQuyTuThien.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GayQuyTuThien.Areas.Admin.Controllers
{
    [Authorize]
    [Area("admin")]
    [Route("admin" + "/[controller]")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;
        private readonly IMapper _mapper;
        public RoleController(IUserService userService, IRoleService roleService, ILogger<RoleController> logger, IMapper mapper)
        {
            _roleService = roleService;
            _logger = logger;
            _mapper = mapper;

        }
        [Route("/admin/role/roles")]
        public async Task<IActionResult> Index()
        {
            var roles = _mapper.Map<IEnumerable<RoleDto>>(await _roleService.GetAll());
            return View(roles);
        }

        [Route("/admin/role/create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [Route("/admin/role/create")]
        public async Task<IActionResult> Create(RoleAddViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var role = _mapper.Map<ApplicationRole>(model);
                    var result = await _roleService.CreateAsync(role);
                    if (!result.IsSuccess)
                    {
                        TempData["error"] = result.ErrorMessenger;
                        ViewBag.Roles = await _roleService.GetAll();
                        return View();
                    }
                    else
                    {
                        TempData["success"] = "CREATE ROLE SUCCESS";
                        return RedirectToAction("Index", "RoleManagement");
                    }
                }
                else
                {
                    TempData["error"] = "CREATE ROLE FAILED";
                    _logger.LogError("ModelState IsValid");
                    return View();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create RoleManagement Exception");
                TempData["error"] = "CREATE ROLE FAILED";
                return View();
            }
        }

        [HttpGet]
        [Route("/admin/role/edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var role = _mapper.Map<RoleViewModel>(await _roleService.GetByIdAsync(id));
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        [HttpPost]
        [Route("/admin/role/edit/{id}")]
        public async Task<IActionResult> Edit(string id, RoleUpdateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var role = await _roleService.GetByIdAsync(id);
                    if (role == null )
                    {
                        TempData["error"] = "RoleManagement not found";
                        return View(model);
                    }
                    role.Name = model.Name;
                    role.Description = model.Description;
                    var result = await _roleService.UpdateAsync(role);
                    if (!result.IsSuccess)
                    {
                        TempData["error"] = result.ErrorMessenger;
                        return View(model);
                    }
                    TempData["success"] = "UPDATE ROLE SUCCESS";
                    return RedirectToAction("Index", "RoleManagement");
                }
                else
                {
                    TempData["error"] = "UPDATE ROLE FAILED";
                    _logger.LogError("ModelState IsValid");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create RoleManagement Exception");
                TempData["error"] = "UPDATE ROLE FAILED";
                return View(model);
            }
        }

        [HttpPost]
        [Route("/admin/role/delete")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _roleService.DeleteAsync(id);
                return Json(new { status = result.IsSuccess });
            }
            catch (Exception ex)
            {
                TempData["error"] = "DELETE ROLE FAILED";
                _logger.LogError(ex, "Delete RoleManagement Exception");
                return RedirectToAction("Index", "RoleManagement");
            }
        }
    }
}
