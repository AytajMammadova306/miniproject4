using Microsoft.EntityFrameworkCore;
using MiniProject.DAL;
using MiniProject.Services.Interfaces;

namespace MiniProject.Services.Implementations
{
    public class LayoutService:ILayoutService
    {
        private readonly AppDbContext _context;
        private readonly HttpContext? _httpcontext;
        public LayoutService(AppDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _httpcontext = accessor.HttpContext;
        }
        public async Task<Dictionary<string, string>> GetSettingAsync()
        {
            Dictionary<string, string> settings = await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);
            return settings;
        }
    }
}
