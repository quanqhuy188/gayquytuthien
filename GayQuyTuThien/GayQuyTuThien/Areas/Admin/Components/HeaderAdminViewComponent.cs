using Microsoft.AspNetCore.Mvc;

namespace GayQuyTuThien.Areas.Admin.Components
{
    public class HeaderAdminViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
