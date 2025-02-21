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
    public interface IHtmlContentService
    {
        Task<List<HtmlContentDto>> GetAllAsync();
        Task<HtmlContentDto> GetByIdAsync(string id);
        Task AddASync(HtmlContentRequest request);
        Task<int> UpdateASync(HtmlContentUpdateRequest request);
        Task<int> DeleteSync(string id);
    }
    public class HtmlContentService : IHtmlContentService
    {
        private readonly DataDbContext _context;
        private readonly IMapper _mapper;
        public HtmlContentService(DataDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<HtmlContentDto>> GetAllAsync()
        {
            return _mapper.Map<List<HtmlContentDto>>(await _context.HtmlContent.OrderByDescending(t => t.CreatedOn).ToListAsync());
        }

        public async Task<HtmlContentDto> GetByIdAsync(string id)
        {
            return _mapper.Map<HtmlContentDto>(await _context.HtmlContent.Where(t => t.Id == id).FirstOrDefaultAsync());
        }
        public async Task AddASync(HtmlContentRequest request)
        {
            var content = _mapper.Map<HtmlContent>(request);
			content.Id = Guid.NewGuid().ToString();
			content.CreatedOn = DateTime.Now;
            content.Content = request.Content;
            await _context.HtmlContent.AddAsync(content);
            await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateASync(HtmlContentUpdateRequest request)
        {
            var content = await _context.HtmlContent.FindAsync(request.Id);
            if (content == null)
            {
                return 0;
            }
            if (!string.IsNullOrEmpty(request.Content) )
            {
                content.Content = request.Content;
            }
            _context.HtmlContent.Update(content);
            await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteSync(string id)
        {
            var content = await _context.HtmlContent.FindAsync(id);
            if (content == null)
            {
                return 0;
            }
            _context.HtmlContent.Remove(content);
            await _context.SaveChangesAsync();
            return 1;
        }
    }
}
