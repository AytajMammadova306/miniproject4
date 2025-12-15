namespace MiniProject.ViewModels
{
    public class DetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public string Image {  get; set; }
        public List<GetProductVM> Products { get; set; }

    }
}
