using EcommerceApi.Models;

namespace EcommerceApi.ViewModel
{
    public class ProductListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public CategoryListViewModel Category { get; set; }
        
    }
}
