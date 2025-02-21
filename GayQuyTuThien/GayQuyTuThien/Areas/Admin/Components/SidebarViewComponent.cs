using GayQuyTuThien.Extensions;
using GayQuyTuThien.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GayQuyTuThien.Areas.Admin.Components
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly IFunctionService _functionService;
        public SidebarViewComponent(IFunctionService functionService)
        {
            _functionService = functionService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roleId = ((ClaimsPrincipal)User).GetSpecificClaim("RoleId");
            var data = await _functionService.GetByPermissionAsync(roleId);
            return View(data);
        }
    }
}
