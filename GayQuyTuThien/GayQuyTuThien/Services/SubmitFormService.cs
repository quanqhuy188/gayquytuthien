using AutoMapper;
using GayQuyTuThien.Common;
using GayQuyTuThien.DataContext.Entity;
using GayQuyTuThien.DataContext;
using GayQuyTuThien.DTOs;
using GayQuyTuThien.RequestViewModel;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using static GayQuyTuThien.Constants.CommonConstants;
using System.Globalization;

namespace GayQuyTuThien.Services
{
	public interface ISubmitFormService
	{
		/// <summary>
		/// Lấy toàn bộ tin tức
		/// </summary>
		/// <returns></returns>
		Task<List<SubmitFormDto>> GetAllAsync();
		/// <summary>
		/// Lấy 1 tin tức
		/// </summary>
		/// <returns></returns>
		Task<SubmitFormDto> GetByIdAsync(string id);
		/// <summary>
		/// Lấy toàn bộ tin tức phân trang
		/// </summary>
		/// <returns></returns>
		StaticPagedList<SubmitFormDto> GetPagingAdminAsync(int page, int pageSize,string fromDate,string toDate);
		/// <summary>
		/// Lấy tin tức mới nhất
		/// </summary>
		/// <returns></returns>
		Task AddASync(SubmitFormRequest request);
		/// <summary>
		/// Cập nhật tin tức
		/// </summary>
		/// <returns></returns>
		Task<int> UpdateASync(SubmitFormUpdateRequest request);
		/// <summary>
		/// Xóa tin tức
		/// </summary>
		/// <returns></returns>
		Task<int> DeleteSync(string id);
	}
	public class SubmitFormService : ISubmitFormService
	{
		private readonly DataDbContext _context;
		private readonly IMapper _mapper;
		public SubmitFormService(DataDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<SubmitFormDto>> GetAllAsync()
		{
			return _mapper.Map<List<SubmitFormDto>>(await _context.SubmitForms.OrderByDescending(t => t.CreatedOn).ToListAsync());
		}
		public async Task<SubmitFormDto> GetByIdAsync(string id)
		{
			return _mapper.Map<SubmitFormDto>(await _context.SubmitForms.Where(t => t.Id == id).FirstOrDefaultAsync());
		}
		public StaticPagedList<SubmitFormDto> GetPagingAdminAsync(int page, int pageSize, string fromDate, string toDate)
		{
			var query = _context.SubmitForms.AsQueryable();
            // Thêm điều kiện lọc nếu fromDate được cung cấp
            if (!string.IsNullOrEmpty(fromDate))
            {
                var fromDateValue = DateTime.ParseExact(fromDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                query = query.Where(t => t.CreatedOn.Date >= fromDateValue.Date);
            }

            // Thêm điều kiện lọc nếu toDate được cung cấp
            if (!string.IsNullOrEmpty(toDate))
            {
                var toDateValue = DateTime.ParseExact(toDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                query = query.Where(t => t.CreatedOn.Date <= toDateValue.Date);
            }

            var result = query.OrderByDescending(t => t.CreatedOn).Skip(pageSize * (page - 1)).Take(pageSize);
			int totalCount = query.Count();
			return new StaticPagedList<SubmitFormDto>(_mapper.Map<IEnumerable<SubmitFormDto>>(result), page, pageSize, totalCount);
		}


		public async Task AddASync(SubmitFormRequest request)
		{
			var submit = _mapper.Map<SubmitForm>(request);
			submit.Id = request.Id;
			submit.CreatedOn = DateTime.Now;
			submit.Username = request.Username;
			submit.Gender = request.Gender;

			await _context.SubmitForms.AddAsync(submit);
			await _context.SaveChangesAsync();
		}

		public async Task<int> UpdateASync(SubmitFormUpdateRequest request)
		{
			var submit = await _context.SubmitForms.FindAsync(request.Id);
			if (submit == null)
			{
				return 0;
			}
			submit.Username = request.Username;
			_context.SubmitForms.Update(submit);
			await _context.SaveChangesAsync();
			return 1;
		}

		public async Task<int> DeleteSync(string id)
		{
			var submit = await _context.SubmitForms.FindAsync(id);
			if (submit == null)
			{
				return 0;
			}
			_context.SubmitForms.Remove(submit);
			await _context.SaveChangesAsync();
			return 1;
		}
	}
}
