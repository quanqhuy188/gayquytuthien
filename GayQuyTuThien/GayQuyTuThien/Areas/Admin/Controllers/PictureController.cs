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
	public class PictureController : Controller
	{
		private readonly IPictureService _pictureService;
		private readonly IConfiguration _configuration;
		private IHostingEnvironment _env;
		public PictureController(IPictureService pictureService, IConfiguration configuration, IHostingEnvironment env)
		{
			_pictureService = pictureService;
			_configuration = configuration;
			_env = env;
		}
		#region List
		public async Task<IActionResult> Index(int page, int pageSize = 10)
		{
			if (page == 0)
			{
				page = 1;
			}
			var data = _pictureService.GetPagingAdminAsync(page, pageSize);
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
		public async Task<IActionResult> Create(PictureRequest request)
		{
			if (request == null)
			{
				return View();
			}
			if (!ModelState.IsValid)
			{
				return View();
			}
			try
			{
				string userId = (User).GetSpecificClaim("UserId");
				//request.Image = await GetFileNameAsync();
				string image = await GetFileNameAsync();
				await _pictureService.AddASync(request, userId, image);
				TempData["success"] = "Success!";
				return RedirectToAction("Index", "Picture");
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
			var pic = await _pictureService.GetByIdAsync(id);
			if (pic == null)
			{
				return NotFound();
			}
			PictureUpdateRequest request = new PictureUpdateRequest
			{
				Id = pic.Id,
			};
			if (!string.IsNullOrEmpty(pic.Guid) && pic.Guid.Contains("/post/"))
			{
				request.Image = pic.Guid.Replace("/post/", "");
			}
			return View(request);
		}

		[HttpPost]
		[RequestSizeLimit(100_000_000)]
		[Route("edit/{id}")]
		public async Task<IActionResult> Edit( PictureUpdateRequest request)
		{
			if (request == null)
			{
				return View();
			}
            if (!ModelState.IsValid)
            {

            }
            try
			{
				request.Image = await GetFileNameAsync();
				var result = await _pictureService.UpdateASync(request);
				if (result == 1)
				{
					TempData["success"] = "Success!";
				}
				else
				{
					TempData["error"] = "Error!";
				}
				return RedirectToAction("Index", "Picture");
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
			var result = await _pictureService.DeleteSync(id);
			if (result == 1)
			{
				TempData["success"] = "Success!";
			}
			return Json(new { status = result });
		}
		#endregion
		private async Task<string> GetFileNameAsync()
		{
			var files = HttpContext.Request.Form.Files;
			string avatar = string.Empty;
			int fileIdx = 0;
			foreach (var Image in files)
			{
				if (Image != null && Image.Length > 0)
				{
					var file = Image;
					//var uploads = Path.Combine(_env.WebRootPath, "images/account");
					var uploads = _configuration.GetSection("uploadFile").GetValue<string>("Post");
					var uploadFolder = Path.Combine(_env.ContentRootPath, "wwwroot", uploads);
					if (!Directory.Exists(uploadFolder))
					{
						Directory.CreateDirectory(uploadFolder);
					}
					if (file.Length > 0)
					{
						var fileName = ContentDispositionHeaderValue.Parse
							(file.ContentDisposition).FileName;//.Trim('"');
															   // Get file name without extension
						fileName = $"{Path.GetFileNameWithoutExtension(fileName)}-{DateTime.Now.ToString("ddMMyyyyHHmmss")}_{fileIdx}{Path.GetExtension(fileName)}";
						fileName = Regex.Replace(fileName, @"[\""]", "", RegexOptions.None);
						using var fileStream = new FileStream(Path.Combine(uploadFolder, fileName), FileMode.Create);
						await file.CopyToAsync(fileStream);
						avatar = fileName;
						//var extension = Path.GetExtension(fileName);
						//string renameFile = "\"" + Guid.NewGuid() + extension;
					}
					fileIdx++;
				}
			}
			return avatar;
		}

	}
}
