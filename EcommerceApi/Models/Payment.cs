namespace EcommerceApi.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public PaymentMethod Method { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }

        public Order Order { get; set; }
    }

    public enum PaymentMethod
    {
        CreditCard,
        DebitCard,
        Pix

    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }

    
}
