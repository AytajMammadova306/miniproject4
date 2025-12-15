using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniProject.DAL;
using MiniProject.ViewModels;

namespace MiniProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<GetProductVM> productVMs = await _context.Products.OrderBy(p=>p.CreatedAt).Select(p => new GetProductVM()
            {
                Name = p.Name,
                Id = p.Id,
                Image = p.Image,
                Price = p.Price,
            }).ToListAsync();
            List<GetCategoryVM> categoryVMs = await _context.Categories.Select(c => new GetCategoryVM()
            {
                Name = c.Name,
                Image = c.Image,
                Products = c.Products.Count
            }).ToListAsync();
            HomeVM homeVM = new HomeVM() 
            { 
                CategoryVMs=categoryVMs,
                ProductVMs= productVMs,
                FeaturedVMs=productVMs.OrderBy(p=>p.Name).ToList()
            };
            return View(homeVM);
        }
    }
}
