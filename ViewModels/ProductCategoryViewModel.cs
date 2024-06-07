using cstore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace cstore.ViewModels
{
    public class ProductCategoryViewModel
    {
        public IList<Product> Products { get; set; }
        public SelectList Categories { get; set; }
        public string SearchCategory { get; set; }
        public string ProductCategory { get; set; }
        public SelectList Prices { get; set; }
        public string SearchPrice { get; set; }
        public IList<Brand> BrandId { get; set; }

    }
}
