namespace MiniProject.Models
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public List<Product> Products { get; set; }
        public string Image {  get; set; }
    }
}
