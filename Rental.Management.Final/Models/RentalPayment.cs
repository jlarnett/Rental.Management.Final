namespace Rental.Management.Final.Models
{
    public class RentalPayment
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int RentalPropertyId { get; set; }
        public RentalProperty? Property { get; set; }

        public double PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    }
}
