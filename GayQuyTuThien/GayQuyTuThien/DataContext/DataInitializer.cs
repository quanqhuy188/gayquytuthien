using GayQuyTuThien.DataContext.Entity;
using GayQuyTuThien.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Security.Claims;
using static GayQuyTuThien.Constants.CommonConstants;
using static GayQuyTuThien.Constants.Permissions;

namespace GayQuyTuThien.DataContext
{
    public static class DataInitializer
    {
        public static async Task SeedRoleAsync(UserManager<ApplicationUser> _userManager, RoleManager<ApplicationRole> _roleManager)
        {
            await _roleManager.CreateAsync(new ApplicationRole { Name = Roles.Admin.ToString() });
            await _roleManager.CreateAsync(new ApplicationRole { Name = Roles.Manager.ToString() });
            await _roleManager.CreateAsync(new ApplicationRole { Name = Roles.User.ToString() });
        }
		public static async Task SeedBasicUserSupperAsync(UserManager<ApplicationUser> _userManager, RoleManager<ApplicationRole> _roleManager)
		{
			var roleManager = await _roleManager.FindByNameAsync(Roles.Admin.ToString());

			var defaultManager = new ApplicationUser
			{
				UserName = "superadmin",
				Email = "superadmin@gmail.com",
				FullName = "Nguyen Quang Huy",
				Description = "SuperAdmin",
				CreatedDate = DateTime.Now,
				Code = "SuperAdmin",
				PhoneNumber = "0363169188",
				EmailConfirmed = true,
				ApplicationRoleId = roleManager.Id,
				LockoutEnabled = false,
			};
			if (_userManager.Users.All(u => u.Id != defaultManager.Id))
			{
				var user = await _userManager.FindByEmailAsync(defaultManager.Email);
				if (user == null)
				{
					try
					{
						await _userManager.CreateAsync(defaultManager, "zxc123!");
						var userManager = await _userManager.FindByNameAsync("superadmin");
						if (userManager != null)
						{
							await _userManager.AddToRoleAsync(userManager, Roles.Admin.ToString());
						}
					}
					catch (Exception ex)
					{
						throw;
					}
				}
			}
		}
		public static async Task SeedBasicUserAsync(UserManager<ApplicationUser> _userManager, RoleManager<ApplicationRole> _roleManager)
        {
            var roleManager = await _roleManager.FindByNameAsync(Roles.Manager.ToString());

            var defaultManager = new ApplicationUser
            {
                UserName = "manager",
                Email = "manager@gmail.com",
                FullName = "manager",
                Description = "Manager",
                CreatedDate = DateTime.Now,
                Code = "Manager",
                PhoneNumber = "1234567890",
                EmailConfirmed = true,
                ApplicationRoleId = roleManager.Id,
                LockoutEnabled = false,
            };
            if (_userManager.Users.All(u => u.Id != defaultManager.Id))
            {
                var user = await _userManager.FindByEmailAsync(defaultManager.Email);
                if (user == null)
                {
                    try
                    {
                        await _userManager.CreateAsync(defaultManager, "Admin@123");
                        var userManager = await _userManager.FindByNameAsync("manager");
                        if (userManager != null)
                        {
                            await _userManager.AddToRoleAsync(userManager, Roles.Manager.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }

            var roleUser = await _roleManager.FindByNameAsync(Roles.User.ToString());
            var defaultUser = new ApplicationUser
            {
                UserName = "user",
                Email = "user@gmail.com",
                FullName = "info_user",
                Description = "User",
                CreatedDate = DateTime.Now,
                Code = "User",
                PhoneNumber = "1234567890",
                EmailConfirmed = true,
                ApplicationRoleId = roleUser.Id,
            };
            if (_userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await _userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await _userManager.CreateAsync(defaultUser, "Admin@123");
                    var userAdd = await _userManager.FindByNameAsync("user");
                    if (userAdd != null)
                    {
                        await _userManager.AddToRoleAsync(userAdd, Roles.User.ToString());
                    }
                }
            }
        }
        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> _userManager, RoleManager<ApplicationRole> _roleManager)
        {
            var roleAdmin = await _roleManager.FindByNameAsync(Roles.Admin.ToString());
            var defaultUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                FullName = "Admin",
                Description = "Admin",
                CreatedDate = DateTime.Now,
                Code = "Admin",
                PhoneNumber = "0886246363",
                EmailConfirmed = true,
                ApplicationRoleId = roleAdmin.Id,
            };
            if (_userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await _userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await _userManager.CreateAsync(defaultUser, "Admin@123");
                    var admin = await _userManager.FindByNameAsync("admin");
                    if (admin != null)
                    {
                        await _userManager.AddToRoleAsync(admin, Roles.User.ToString());
                        await _userManager.AddToRoleAsync(admin, Roles.Manager.ToString());
                        await _userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
                    }

                }
            }
        }


        public static async Task SeedMenuAsync(DataDbContext _context, RoleManager<ApplicationRole> _roleManager)
        {
            var menus = await _context.Functions.ToListAsync();
            //if (menus.Count > 0)
            //{
            //    return;
            //}

            List<Function> lstMenu = new List<Function>
                {
                    new Function{ Id =MenuConstants.Dashboard, Name = "Dashboard", ParentId =null, Slug = "/admin/home", SortOrder = 1, Level = 0},
                    new Function{ Id =MenuConstants.UserSystem, Name = "User System", ParentId = null, Slug = "#", SortOrder = 2},
                    new Function{ Id =MenuConstants.RoleMng, Name = "RoleMng", ParentId =  MenuConstants.UserSystem, Slug = "/admin/role/roles", SortOrder = 3,Level = 1},
                    new Function{ Id =MenuConstants.UserMng, Name = "UserMng", ParentId =  MenuConstants.UserSystem, Slug = "/admin/user/users", SortOrder = 4,Level = 1},
                    new Function{ Id =MenuConstants.PermissionMng, Name = "Permissions", ParentId =  MenuConstants.UserSystem, Slug = "/admin/permission/permissions", SortOrder = 9, Level = 1},
					new Function{ Id =MenuConstants.Manager, Name = "Quản lý", ParentId = null, Slug = "#", SortOrder = 5},
					new Function{ Id =MenuConstants.Pictures, Name = "Danh sách hình ảnh", ParentId = MenuConstants.Manager, Slug = "/admin/picture", SortOrder = 6,Level = 1},
					new Function{ Id =MenuConstants.SharedLists, Name = "Quản lý chia sẻ", ParentId = MenuConstants.Manager, Slug = "/admin/submitform", SortOrder = 7,Level = 1},
                    new Function{ Id =MenuConstants.HtmlContent, Name = "Nội dung chia sẻ", ParentId = MenuConstants.Manager, Slug = "/admin/htmlcontent", SortOrder = 8,Level = 1},
                };

            // INSERT
            var insertIds = lstMenu.Select(t => t.Id).Except(menus.Select(t => t.Id)).ToList();
            List<Function> inserts = lstMenu.Where(t => insertIds.Contains(t.Id)).ToList();
            await InsertMenu(_context, _roleManager, inserts);

            // UPADATE
            await UpdateMenu(_context, lstMenu);
            //// DELETE
            var deleteIds = menus.Select(t => t.Id).Except(lstMenu.Select(t => t.Id)).ToList();
            List<Function> deletes = menus.Where(t => deleteIds.Contains(t.Id)).ToList();
            await DeleteMenu(_context, _roleManager, deletes);
        }

        #region Action Menu
        private static async Task InsertMenu(DataDbContext _context, RoleManager<ApplicationRole> _roleManager, List<Function> models)
        {
            try
            {
                await _context.Functions.AddRangeAsync(models);
                await _context.SaveChangesAsync();
                await SeedPermissionAdminAndManagerAsync(_context, _roleManager, models);
            }
            catch (Exception ex)
            {

                throw;
            }
         
        }
        private static async Task UpdateMenu(DataDbContext _context, List<Function> models)
        {
            foreach (var item in models)
            {
                Function? function = await _context.Functions.FirstOrDefaultAsync(t => t.Id == item.Id);
                if (function == null)
                    continue;
                function.Name = item.Name;
                function.Slug = item.Slug;
                function.ParentId = item.ParentId;
                function.SortOrder = item.SortOrder;
                _context.Functions.Update(function);
            }
            await _context.SaveChangesAsync();
        }
        private static async Task DeleteMenu(DataDbContext _context, RoleManager<ApplicationRole> _roleManager, List<Function> models)
        {
            foreach (var item in models)
            {
                Function? function = await _context.Functions.FirstOrDefaultAsync(t => t.Id == item.Id);
                if (function == null)
                    continue;
                _context.Functions.Remove(function);
            }
            await _context.SaveChangesAsync();
            await RemovePermissionAsync(_context, _roleManager, models);
        }
        #endregion

        #region Permission
        private static async Task SeedPermissionAdminAndManagerAsync(DataDbContext _context, RoleManager<ApplicationRole> _roleManager, List<Function> functions)
        {
            var roleAdmin = await _roleManager.FindByNameAsync(Roles.Admin.ToString());
            foreach (var function in functions)
            {
                var allClaimAdmins = await _roleManager.GetClaimsAsync(roleAdmin);
                if (!allClaimAdmins.Where(t => t.Value.Contains($"{PermissionConstants.Prefix}.{function.Id}.")).Any())
                {
                    await _roleManager.SeedClaimsForRole(roleAdmin.Name, function.Id);
                }
            }
        }

        private static async Task RemovePermissionAsync(DataDbContext _context, RoleManager<ApplicationRole> _roleManager, List<Function> functions)
        {
            foreach (var function in functions)
            {
                foreach (Roles scope in (Roles[])Enum.GetValues(typeof(Roles)))
                {
                    var role = await _roleManager.FindByNameAsync(scope.ToString());
                    var claims = await _roleManager.GetClaimsAsync(role);
                    var claimsByFunction = claims.Where(t => t.Value.Contains($"{PermissionConstants.Prefix}.{function.Id}."));
                    foreach (var item in claimsByFunction)
                    {
                        await _roleManager.RemoveClaimAsync(role, item);
                    }
                }
            }
        }

        private async static Task SeedClaimsForRole(this RoleManager<ApplicationRole> roleManager, string roleName, string module)
        {
            var adminRole = await roleManager.FindByNameAsync(roleName);
            await roleManager.AddPermissionClaim(adminRole, module);
        }
        public static async Task AddPermissionClaim(this RoleManager<ApplicationRole> roleManager, ApplicationRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            List<string> allPermissions;

            if (role.Name == Roles.Admin.ToString())
            {
                allPermissions = GeneratePermissionsForModuleAdmin(module);
            }
            else
            {
                allPermissions = GeneratePermissionsForModuleManager(module);
            }
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == PermissionConstants.PermissionType && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim(PermissionConstants.PermissionType, permission));
                }
            }
        }
        #endregion
    }
}
