using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniProject.DAL;
using MiniProject.ViewModels;

namespace MiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<GetCategoryAdminVM> categoryVMs = await _context.Categories.Select(c => new GetCategoryAdminVM
            {
                Name = c.Name,
                Id = c.Id,
                Image = c.Image,
                Products = c.Products.Count()
            }).ToListAsync();
            return View(categoryVMs);
        }
    }
}
