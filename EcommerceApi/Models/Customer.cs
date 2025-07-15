namespace EcommerceApi.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public Address Address { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();


    }
}
