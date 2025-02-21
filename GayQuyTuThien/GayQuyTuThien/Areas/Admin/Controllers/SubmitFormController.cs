using GayQuyTuThien.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GayQuyTuThien.RequestViewModel;

using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace GayQuyTuThien.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("Admin" + "/[controller]")]
	[Authorize]
	public class SubmitFormController : Controller
	{
		private readonly ISubmitFormService _submitFormService;
		private readonly IConfiguration _configuration;
		private IHostingEnvironment _env;
		public SubmitFormController(ISubmitFormService submitFormService, IConfiguration configuration, IHostingEnvironment env)
		{
			_submitFormService = submitFormService;
			_configuration = configuration;
			_env = env;
		}
		#region List
		public async Task<IActionResult> Index(int page, string fromDate = "", string toDate= "", int pageSize = 10)
		{
			if (page == 0)
			{
				page = 1;
			}
			var data = _submitFormService.GetPagingAdminAsync(page, pageSize, fromDate, toDate);
			if(data !=null)
            {
				ViewBag.TotalItemCount = data.TotalItemCount.ToString();
            } else
			{
				ViewBag.TotalItemCount = "Unknow";

            }
           
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
		public async Task<IActionResult> Create(SubmitFormRequest request)
		{
			if (request == null)
			{
				return View();
			}
			if (!ModelState.IsValid) { }
			try
			{
                await _submitFormService.AddASync(request);
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
			var submit = await _submitFormService.GetByIdAsync(id);
			if (submit == null)
			{
				return NotFound();
			}
			SubmitFormUpdateRequest request = new SubmitFormUpdateRequest
			{
				Id = submit.Id,
				Username = submit.Username,
				Gender = submit.Gender
			};
			return View(request);
		}

		[HttpPost]
		[RequestSizeLimit(100_000_000)]
		[Route("edit/{id}")]
		public async Task<IActionResult> Edit(SubmitFormUpdateRequest request)
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
				var result = await _submitFormService.UpdateASync(request);
				if (result == 1)
				{
					TempData["success"] = "Success!";
				}
				else
				{
					TempData["error"] = "Error!";
				}
				return RedirectToAction("Index", "SubmitForm");
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
			var result = await _submitFormService.DeleteSync(id);
			if (result == 1)
			{
				TempData["success"] = "Success!";
			}
			return Json(new { status = result });
		}
		#endregion

	}
}
