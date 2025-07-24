using EcommerceApi.Models;

namespace EcommerceApi.ViewModel
{
    public class CustomerRegisteViewModel
    {
        public UserRegisterViewModel User { get; set; }

        public string PhoneNumber { get; set; }

        public AddressEditViewModel Address { get; set; }
    }
}
