namespace EcommerceApi.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }

      
        public User User { get; set; }

        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();


    }
}
