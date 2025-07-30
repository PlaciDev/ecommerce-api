using EcommerceApi.Models;

namespace EcommerceApi.ViewModel
{
    public class CategoryListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<ProductListViewModel> Products { get; set; } = new List<ProductListViewModel>();

    }
}
