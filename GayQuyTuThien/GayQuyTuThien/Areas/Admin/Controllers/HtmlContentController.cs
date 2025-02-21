using GayQuyTuThien.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GayQuyTuThien.Extensions;
using GayQuyTuThien.RequestViewModel;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using X.PagedList;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;


namespace GayQuyTuThien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin" + "/[controller]")]
    [Authorize]
    public class HtmlContentController : Controller
    {
        private readonly IHtmlContentService _contentService;
        private readonly IConfiguration _configuration;
        private IHostingEnvironment _env;
        public HtmlContentController(IHtmlContentService contentService, IConfiguration configuration, IHostingEnvironment env)
        {
            _contentService = contentService;
            _configuration = configuration;
            _env = env;
        }
        #region List
        public async Task<IActionResult> Index()
        {
            var data = await _contentService.GetAllAsync();
            return View(data);
        }
        #endregion
        #region Create
        [Route("create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        [Route("create")]
        public async Task<IActionResult> Create(HtmlContentRequest request)
        {
            if (request == null)
            {
                return View();
            }
            try
            {
                string userId = (User).GetSpecificClaim("UserId");
                await _contentService.AddASync(request);
                TempData["success"] = "Success!";
                return RedirectToAction("Index", "HtmlContent");
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion
        #region Edit
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var cnt = await _contentService.GetByIdAsync(id);
            if (cnt == null)
            {
                return NotFound();
            }
            HtmlContentUpdateRequest request = new HtmlContentUpdateRequest
            {
                Id = cnt.Id,
                Content = cnt.Content
            };
            return View(request);
        }

        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(HtmlContentUpdateRequest request)
        {
            if (request == null)
            {
                return View();
            }
            try
            {
                var result = await _contentService.UpdateASync(request);
                if (result == 1)
                {
                    TempData["success"] = "Success!";
                }
                else
                {
                    TempData["error"] = "Error!";
                }
                return RedirectToAction("Index", "HtmlContent");
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion
        #region Delete
        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _contentService.DeleteSync(id);
            if (result == 1)
            {
                TempData["success"] = "Success!";
            }
            return Json(new { status = result });
        }
        #endregion

    }
}
