using AutoMapper;
using GayQuyTuThien.DataContext.Entity;
using GayQuyTuThien.DTOs;
using GayQuyTuThien.RequestViewModel;
using GayQuyTuThien.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace CoinViet.Areas.Admin.Controllers
{
	[Authorize]
	[Area("admin")]
	[Route("admin" + "/[controller]")]
	public class UserController : Controller
	{
		private readonly IUserService _userService;
		private readonly IRoleService _roleService;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IRoleService roleService, ILogger<UserController> logger, IMapper mapper)
        {
			_userService = userService;
            _roleService = roleService;
            _logger = logger;
            _mapper = mapper;

		}
		[Route("/admin/user/users")]
        public async Task<IActionResult> Index(string search)
		{
            ViewBag.Search = search;
			var data = await _userService.GetAll(search);
            var result = _mapper.Map<IEnumerable<UserDto>>(data);
            return View(result);
		}

        [Route("/admin/user/create")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = _mapper.Map<List<RoleDto>>( await _roleService.GetAllNoAdmin());
            return View();
        }
        [HttpPost]
        [Route("/admin/user/create")]
        public async Task<IActionResult> Create(UserAddViewModel model)
        {
            ViewBag.Roles = _mapper.Map<List<RoleDto>>(await _roleService.GetAllNoAdmin());
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _mapper.Map<ApplicationUser>(model);
                    var result =  await _userService.CreateAsync(user, model.Password);
                    if (!result.IsSuccess)
                    {
                        TempData["error"] = result.ErrorMessenger;
                        return View();
                    }
                    else
                    {
                        TempData["success"] = "CREATE USER SUCCESS";
                        return RedirectToAction("Index", "User");
                    }
                }
                else
                {
                    TempData["error"] = "CREATE USER FAILED";
                    _logger.LogError("ModelState IsValid"); 
                    return View();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create User Exception");
                TempData["error"] = "CREATE USER FAILED";
                return View();
            }
        }
        [HttpGet]
        [Route("/admin/user/edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Roles = _mapper.Map<List<RoleDto>>(await _roleService.GetAllNoAdmin());
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var data = _mapper.Map<UserUpdateViewModel>(user);
            return View(data);
        }

        [HttpPost]
        [Route("/admin/user/edit/{id}")]
        public async Task<IActionResult> Edit(string id, UserUpdateViewModel model)
        {
            ViewBag.Roles = _mapper.Map<List<RoleDto>>(await _roleService.GetAllNoAdmin());
            try
            {
                if (ModelState.IsValid)
                {

                    var user = await _userService.GetUserByIdAsync(id);
                    if (user == null)
                    {
                        return NotFound();
                    }
                    user.FullName = model.FullName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Email = model.Email;
                    user.ApplicationRoleId = model.ApplicationRoleId;
                    var result = await _userService.UpdateAsync(user);
                    if (!result.IsSuccess)
                    {
                        TempData["error"] = result.ErrorMessenger;
                        return View(model);
                    }
                    TempData["success"] = "UPDATE USER SUCCESS";
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    TempData["error"] = "UPDATE USER FAILED";
                    _logger.LogError("ModelState IsValid");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create User Exception");
                TempData["error"] = "UPDATE USER FAILED";
                return View(model);
            }
        }


        [HttpPost]
        [Route("/admin/user/delete")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _userService.DeleteAsync(id);
                return Json(new { status = result });
            }
            catch (Exception ex)
            {
                TempData["error"] = "DELETE USER FAILED";
                _logger.LogError(ex, "Delete User Exception");
                return RedirectToAction("Index", "User");
            }
        }

		[Route("/admin/user/profile")]
		public async Task<IActionResult> Profile ()
		{
            var user = await _userService.GetUserByIdAsync("",HttpContext.User);
            var data = _mapper.Map<UserUpdateViewModel>(user);
            ViewBag.Roles = _mapper.Map<List<RoleDto>>(await _roleService.GetAll());
            return View(data);
		}
	}
}
