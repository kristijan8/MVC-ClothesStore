namespace cstore.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public string? Color { get; set; }
        public int Size { get; set; }
        public string? ImageURL { get; set; } = "";
        //public string? FrontPage { get; set; } = "";
        public string? DownloadUrl { get; set; } = "";
        public bool IsAvailable { get; set; }
        public Brand? Brand { get; set; }
        public int? BrandId { get; set; }
        public ICollection<Reviews>? Reviews { get; set; }

        public ICollection<Users>? Users { get; set; }
        

    }
}
