using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniProject.DAL;
using MiniProject.Models;
using MiniProject.Utilities.Enums;
using MiniProject.Utilities.Extentions;
using MiniProject.ViewModels;

namespace MiniProject.Area.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController :Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context=context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<GetProductAdminVM> productVMs = await _context.Products.Select(p => new GetProductAdminVM()
            {
                Name = p.Name,
                Id = p.Id,
                Image = p.Image,
                Price = p.Price,
                CategoryName=p.Category.Name
            }).ToListAsync();
            return View(productVMs);
        }
        public async Task<IActionResult> Create()
        {
            CreateProductVM productVm = new CreateProductVM()
            {
                Categories=await _context.Categories.ToListAsync()
            };
            return View(productVm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            productVM.Categories = await _context.Categories.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View(productVM);
            }
            if (!productVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError(nameof(CreateProductVM.Photo), "File type is incorrect");
                return View(productVM);
            }
            if (!productVM.Photo.ValidateSize(FileSize.MB, 2))
            {
                ModelState.AddModelError(nameof(CreateProductVM.Photo), "File size is incorrect");
                return View(productVM);
            }
            Category category = productVM.Categories.FirstOrDefault(c => c.Id == productVM.CategoryId);
            if (category is null)
            {
                ModelState.AddModelError(nameof(CreateProductVM.CategoryId), "Category doesnot exist(custom)");
                return View(productVM);
            }
            bool resultName = await _context.Products.AnyAsync(p => p.Name == productVM.Name);
            if (resultName)
            {
                ModelState.AddModelError(nameof(CreateProductVM.Name), "Product Name already exists");
                return View(productVM);
            }

            Product product = new Product
            {
                Name = productVM.Name,
                CategoryId= category.Id,
                CreatedAt= DateTime.Now,
                Description= productVM.Description,
                Image=await productVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img"),
                Price= productVM.Price.Value,
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1)
            {
                return BadRequest();
            }
            Product existed= await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if(existed is null)
            {
                return NotFound();
            }
            UpdateProductVM productVM = new UpdateProductVM()
            {
                Description = existed.Description,
                CategoryId= existed.CategoryId,
                Name= existed.Name,
                Image=existed.Image,
                Price= existed.Price,
                Categories = await _context.Categories.ToListAsync()
            };
            return View(productVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateProductVM productVM)
        {
            Product existed= await _context.Products.FirstOrDefaultAsync(p=>p.Id == id);
            productVM.Categories = await _context.Categories.ToListAsync();
            
            if (existed is null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(productVM);
            }
            if (productVM.Photo is not null)
            {
                if (!productVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(UpdateProductVM.Photo), "File type is incorrect");
                    return View(productVM);
                }
                if (!productVM.Photo.ValidateSize(FileSize.MB, 2))
                {
                    ModelState.AddModelError(nameof(UpdateProductVM.Photo), "File size is incorrect");
                    return View(productVM);
                }
            }
            Category category = productVM.Categories.FirstOrDefault(c => c.Id == productVM.CategoryId);
            if (category is null)
            {
                ModelState.AddModelError(nameof(UpdateProductVM), "Category does not exist");
                return View(productVM);
            }
            bool resultName = await _context.Categories.AnyAsync(p => p.Name == productVM.Name && p.Id != id);
            if (resultName)
            {
                ModelState.AddModelError(nameof(UpdateProductVM.Name), "Product Name already exists");
                return View(productVM);
            }
            if (productVM.Photo is not null)
            {
                string newFileName = await productVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
                existed.Image.DeleteFile(_env.WebRootPath, "assets", "img");
                existed.Image = newFileName;
            }
            existed.Name = productVM.Name;
            existed.Description = productVM.Description;
            existed.Price = productVM.Price.Value;
            existed.CategoryId = productVM.CategoryId.Value;
            existed.Category=category;
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1)
            {
                return BadRequest();
            }
            Product? product= await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product is null)
            {
                return NotFound();
            }
            product.Image.DeleteFile(_env.WebRootPath, "assets", "img");
            _context.Products.Remove(product);
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }
}
