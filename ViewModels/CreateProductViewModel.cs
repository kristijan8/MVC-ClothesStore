using Microsoft.AspNetCore.Mvc.Rendering;

namespace cstore.ViewModels
{
    public class CreateProductViewModel
    {

       
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public string? Color { get; set; }
        public int Size { get; set; }
       
        public bool IsAvailable { get; set; }
        
        public int? SelectedBrandId { get; set; }
        public IEnumerable<SelectListItem>? BrandList { get; set; }
     
    }
}
