using EcommerceApi.Models;

namespace EcommerceApi.ViewModel
{
    public class AddressListViewModel
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

    }
}
