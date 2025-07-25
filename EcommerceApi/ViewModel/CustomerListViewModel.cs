using EcommerceApi.Models;

namespace EcommerceApi.ViewModel
{
    public class CustomerListViewModel
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }

        public UserListViewModel User { get; set; }

        public IEnumerable<AddressListViewModel> Addresses { get; set; } = new List<AddressListViewModel>();
        public ICollection<OrderListViewModel> Orders { get; set; } = new List<OrderListViewModel>();
    }
}
