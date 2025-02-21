using GayQuyTuThien.Models;
using GayQuyTuThien.RequestViewModel;
using GayQuyTuThien.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Drawing.Printing;

namespace GayQuyTuThien.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISubmitFormService _submitFormService;
        private readonly IPictureService _pictureService;
        private readonly IHtmlContentService _contentService;
		private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;
        public HomeController(
			IHtmlContentService contentService,
			ILogger<HomeController> logger,
            IPictureService pictureService,
            ISubmitFormService submitFormService,
            IConfiguration configuration)
        {
            _pictureService = pictureService;
            _submitFormService = submitFormService;
            _configuration = configuration;
            _logger = logger;
            _contentService = contentService;

		}

		public async Task<IActionResult> Index()
		{
            var model = new HomeModel();
			model.Pictures = await _pictureService.GetAllAsync();
            model.PicturesRevert = await _pictureService.GetAllAsyncRevert();
            return View(model);
		}
        [HttpPost]
        public async Task<IActionResult> SubmitForm(SubmitFormRequest request)
        {
            if (request == null)
            {
                return View();
            }
            if (!ModelState.IsValid) { }
            try
            {
                request.Id = Guid.NewGuid().ToString();
                await _submitFormService.AddASync(request);
                var savedData = await _submitFormService.GetByIdAsync(request.Id);
                var randomPostcard = await _pictureService.GetRandomAsync();
				var htmlcontent = await _contentService.GetAllAsync();
				return Json(new { Success = true, data = savedData, randomPost = randomPostcard ,contentHTML = htmlcontent });
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}