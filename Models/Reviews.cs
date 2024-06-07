namespace cstore.Models
{
    public class Reviews
    {
        public int? Id { get; set; }
        public int? ProductId { get; set; }
        public string? AppUser { get; set; }
        public string? Comment { get; set; }
        public int? Rating { get; set; }
        public Product? Product { get; set; }
    }
}
