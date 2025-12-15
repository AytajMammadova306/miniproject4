using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniProject.DAL;
using MiniProject.ViewModels;

namespace MiniProject.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context=context;
        }
        public async Task<IActionResult> Index()
        {
            List<GetProductVM> productVMs = await _context.Products.Select(p => new GetProductVM()
            {
                Name = p.Name,
                Id = p.Id,
                Image = p.Image,
                Price = p.Price,
            }).ToListAsync();
            return View(productVMs);
        }
        public async Task<IActionResult> Details( int? id)
        {
            if (id is null || id < 1)
            {
                return BadRequest();
            }
            DetailsVM? productVM = await _context.Products.Select(p => new DetailsVM()
            {
                Id=p.Id,
                Name = p.Name,
                CategoryName=p.Category.Name,
                Description = p.Description,
                Image = p.Image,
                Price = p.Price,
            }).FirstOrDefaultAsync(d=>d.Id==id);

            if(productVM is null)
            {
                return NotFound();
            }
            productVM.Products = await _context.Products.Where(p=>p.Category.Name==productVM.CategoryName&&p.Id!=productVM.Id).OrderBy(p => p.CreatedAt).Select(p => new GetProductVM()
            {
                Name = p.Name,
                Id = p.Id,
                Image = p.Image,
                Price = p.Price,
            }).ToListAsync();
            return View(productVM);
        }
    }
}
