using AutoMapper;
using GayQuyTuThien.Common;
using GayQuyTuThien.DataContext.Entity;
using GayQuyTuThien.DataContext;
using GayQuyTuThien.DTOs;
using GayQuyTuThien.RequestViewModel;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using static GayQuyTuThien.Constants.CommonConstants;

namespace GayQuyTuThien.Services
{
	public interface IPictureService
	{
		/// <summary>
		/// Lấy toàn bộ tin tức
		/// </summary>
		/// <returns></returns>
		Task<List<PictureDto>> GetAllAsync();
        Task<PictureDto> GetRandomAsync();
        Task<List<PictureDto>> GetAllAsyncRevert();
        /// <summary>
        /// Lấy 1 tin tức
        /// </summary>
        /// <returns></returns>
        Task<PictureDto> GetByIdAsync(string id);
		/// <summary>
		/// Lấy toàn bộ tin tức phân trang
		/// </summary>
		/// <returns></returns>
		StaticPagedList<PictureDto> GetPagingAdminAsync(int page, int pageSize);
		/// <summary>
		/// Lấy tin tức mới nhất
		/// </summary>
		/// <returns></returns>
		Task AddASync(PictureRequest request, string userId, string image);
		/// <summary>
		/// Cập nhật tin tức
		/// </summary>
		/// <returns></returns>
		Task<int> UpdateASync(PictureUpdateRequest request);
		/// <summary>
		/// Xóa tin tức
		/// </summary>
		/// <returns></returns>
		Task<int> DeleteSync(string id);
	}
	public class PictureService : IPictureService
	{
		private readonly DataDbContext _context;
		private readonly IMapper _mapper;
		public PictureService(DataDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}
        public async Task<PictureDto> GetRandomAsync()
        {
            var totalCount = await _context.Pictures.CountAsync();
			if (totalCount < 1) return new PictureDto();
            // Sinh một số ngẫu nhiên từ 0 đến (số lượng bản ghi - 1)
            var randomOffset = new Random().Next(0, totalCount - 1);

            // Lấy bản ghi ngẫu nhiên bằng cách sử dụng Skip và Take
            var randomPostcard = await _context.Pictures
                                        .Skip(randomOffset)
                                        .Take(1)
                                        .FirstOrDefaultAsync();
            return _mapper.Map<PictureDto>(randomPostcard);
        }
        public async Task<List<PictureDto>> GetAllAsync()
		{
			return _mapper.Map<List<PictureDto>>(await _context.Pictures.OrderByDescending(t => t.CreatedOn).ToListAsync());
		}
        public async Task<List<PictureDto>> GetAllAsyncRevert()
        {
            return _mapper.Map<List<PictureDto>>(await _context.Pictures.OrderBy(t => t.CreatedOn).ToListAsync());
        }
        public async Task<PictureDto> GetByIdAsync(string id)
		{
			return _mapper.Map<PictureDto>(await _context.Pictures.Where(t => t.Id == id).FirstOrDefaultAsync());
		}
		public StaticPagedList<PictureDto> GetPagingAdminAsync(int page, int pageSize)
		{
			var query = _context.Pictures.AsQueryable();

			var result = query.OrderByDescending(t => t.CreatedOn).Skip(pageSize * (page - 1)).Take(pageSize);
			int totalCount = query.Count();
			return new StaticPagedList<PictureDto>(_mapper.Map<IEnumerable<PictureDto>>(result), page, pageSize, totalCount);
		}


		public async Task AddASync(PictureRequest request, string userId, string image)
		{
			var pic = _mapper.Map<Picture>(request);
			pic.Id = Guid.NewGuid().ToString();
			pic.CreatedOn = DateTime.Now;
			pic.Guid = $"/post/{image}";
			await _context.Pictures.AddAsync(pic);
			await _context.SaveChangesAsync();
		}

		public async Task<int> UpdateASync(PictureUpdateRequest request)
		{
			var pic = await _context.Pictures.FindAsync(request.Id);
			if (pic == null)
			{
				return 0;
			}
			if (!string.IsNullOrEmpty(request.Image) && pic.Guid != $"/post/{request.Image}")
			{
				pic.Guid = $"/post/{request.Image}";
			}
			_context.Pictures.Update(pic);
			await _context.SaveChangesAsync();
			return 1;
		}

		public async Task<int> DeleteSync(string id)
		{
			var pic = await _context.Pictures.FindAsync(id);
			if (pic == null)
			{
				return 0;
			}
			_context.Pictures.Remove(pic);
			await _context.SaveChangesAsync();
			return 1;
		}
	}
}
