using MiniProject.Models;
using System.ComponentModel.DataAnnotations;

namespace MiniProject.ViewModels
{
    public class CreateProductVM
    {
        public string Name { get; set; }
        [Required]
        public decimal? Price { get; set; }
        public string Description { get; set; }
        [Required]
        public int ? CategoryId { get; set; }
        public IFormFile Photo { get; set; }
        public List<Category>? Categories { get; set; }

    }
}
