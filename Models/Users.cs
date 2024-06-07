namespace cstore.Models
{
    public class Users
    {
        public int? Id { get; set; }
        public string? AppUser { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
