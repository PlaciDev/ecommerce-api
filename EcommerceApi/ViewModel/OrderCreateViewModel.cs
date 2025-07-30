using EcommerceApi.Models;

namespace EcommerceApi.ViewModel
{
    public class OrderCreateViewModel
    {
        public PaymentMethod PaymentMethod { get; set; }    

        public List<OrderItemCreateViewModel> OrderItems { get; set; } = new List<OrderItemCreateViewModel>();
    }

    public class OrderItemCreateViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

}
