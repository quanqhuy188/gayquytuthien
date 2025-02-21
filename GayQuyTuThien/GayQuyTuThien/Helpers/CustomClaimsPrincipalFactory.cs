using GayQuyTuThien.DataContext.Entity;
using GayQuyTuThien.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace GayQuyTuThien.Helpers
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        UserManager<ApplicationUser> _userManger;
        RoleManager<ApplicationRole> _roleManager;
        private readonly IPermissionService _permissionService;

        public CustomClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
            IOptions<IdentityOptions> options, IPermissionService permissionService)
            : base(userManager, roleManager, options)
        {
            _userManger = userManager;
            _roleManager = roleManager;
            _permissionService = permissionService;
        }

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            //var roles = await _userManger.GetRolesAsync(user);
            //var role = await _roleManager.FindByIdAsync(user.ApplicationRoleId);
            //var allClaims = await _roleManager.GetClaimsAsync(role);
            //var allClaimsValue = allClaims.Select(t => t.Value).ToList();
            ((ClaimsIdentity)principal.Identity).AddClaims(new[]
            {
                new Claim("Email",user.Email),
                new Claim("FullName",user.FullName ?? string.Empty),
                new Claim("UserName",user.UserName),
                new Claim("PhoneNumber",user.PhoneNumber),
                new Claim("Address",user.Address ?? string.Empty),
                new Claim("Avatar",user.Avatar ?? string.Empty),
                //new Claim("Roles",string.Join(";",roles)),
                new Claim("RoleId", user.ApplicationRoleId),
                //new Claim("Claims",string.Join(";",allClaimsValue)),
                new Claim("Id",user.Id.ToString())
            });
            return principal;
        }
    }
}
